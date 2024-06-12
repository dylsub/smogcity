using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(ParticleSystem))]
public class PlayerController : MonoBehaviour
{
    /////////////////////////////////////////////
    //Variables and Stuff//
    /////////////////////////////////////////////
    Rigidbody2D rb;
    [SerializeField] float maxSpeed = 200f;
    [SerializeField] float speed = 5f;
    [SerializeField] AreaEffector2D vacuumEffectorTop;
    [SerializeField] AreaEffector2D vacuumEffectorBottom;
    [SerializeField] AreaEffector2D vacuumEffectorBottomBack;
    [SerializeField] AreaEffector2D vacuumEffectorTopBack;
    [SerializeField] ParticleSystem vacuumParticles;
    [SerializeField] ParticleSystem slidingParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] GameObject silhouette;
    [SerializeField] GameObject silhouette2;
    SpriteRenderer sr;
    Shake shake;
    PlayerAnimator pa;
    Camera cam;
    GameObject s1, s2, s3;
    [SerializeField] CinemachineImpulseSource impulseSource1;
    [SerializeField] CinemachineImpulseSource impulseSource2;
    CheckpointManager cpm;
    SoundManager sm;
    AudioSource aus;

    int count = 40;
    int delayCount = 0;
    int dashCount = 0;
    float jumpCount = 0;

    float lastPosX;
    float currPosX;
    float startDashPosX;
    float endDashPosX;

    /////////////////////////////////////////////
    //State Booleans//
    /////////////////////////////////////////////
    bool isGrounded = false;
    bool facingRight = true;
    bool isDashing = false;
    bool isDashReset = true;
    bool isVacuum = false;
    bool isTouchingWall = false;
    bool isBackTouchingWall = false;
    bool isSlideTouching = false;
    bool isWallJumping = false;
    bool isSliding = false;
    bool isOnFallingPlatform = false;
    bool isTalking = false;

    bool dash0done = false;
    bool dash1done = false;
    bool dash2done = false;
    bool dash3done = false;
    bool dash4done = false;

    bool canMove = true;
    bool canRespawn = true;

    ///////////////////////////////////////  //////
    //Start Function//
    /////////////////////////////////////////////
    void Start()
    {
        sr = FindObjectOfType<PlayerAnimator>().GetComponent<SpriteRenderer>();
        cam = FindObjectOfType<Camera>();
        pa = FindObjectOfType<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
        shake = FindObjectOfType<Shake>();
        cpm = FindObjectOfType<CheckpointManager>();
        sm = FindObjectOfType<SoundManager>();
        aus = GetComponent<AudioSource>();
    }

    /////////////////////////////////////////////
    //Fixed Update Function//
    /////////////////////////////////////////////
    void FixedUpdate()
    {
        //counters
        count++;
        dashCount++;
        delayCount++;

        Move();

        // Debug.Log(isBackTouchingWall);

        // Debug.Log("canMove: " + canMove);
        // Debug.Log("isTalking: " + isTalking);
    }
    /////////////////////////////////////////////
    //Update function//
    /////////////////////////////////////////////
    void Update()
    {
        // Debug.Log(isSliding);

        //Determining when the user can jump again
        if (isGrounded && !isDashing)
        {
            isDashReset = true;
        }

        if (isTalking)
        {
            canMove = false;
        }

        if (count >= 3 && count < 30 && !isTalking)
        {
            sr.color = new Vector4(255f, 255f, 255f, 0f);
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0f;
        }
        else if (count == 30 && !isTalking)
        {
            rb.gravityScale = 1f;
            setCanMove(true);
            canRespawn = true;
            transform.position = cpm.getRespawnPosition();
            sr.color = new Vector4(255f, 255f, 255f, 255f);
        }

        Jump();
        Dash();
        DashProcess();
        Vacuum();
        ClampVelocity();

        if ((isGrounded || !canMove) && isSliding)
        {
            isSliding = false;
            pa.setIsSliding(false);
            rb.gravityScale = 1f;
            var sp = slidingParticles.emission;
            sp.rateOverTime = 0f;
            slidingParticles.Clear();
        }
        if (!isDashing && !isGrounded && delayCount >= 1)
        {
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && isTouchingWall && rb.velocity.y < 0.01f && !isSliding && isSlideTouching && canMove)
            {
                // Debug.Log("Started Sliding");
                isSliding = true;
                rb.gravityScale = 0f;
                pa.setIsSliding(true);
                var sp = slidingParticles.emission;
                sp.rateOverTime = 75f;
                slidingParticles.transform.position = new Vector3(slidingParticles.transform.position.x, slidingParticles.transform.position.y, -1);
            }
            else if (isSliding && (!isSlideTouching || !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))))
            {
                isSliding = false;
                pa.setIsSliding(false);
                rb.gravityScale = 1f;
                var sp = slidingParticles.emission;
                sp.rateOverTime = 0f;
                slidingParticles.Clear();
            }
            else if (isSliding && canMove)
            {
                rb.velocity = new Vector2(rb.velocity.x, -1.5f);
                slidingParticles.transform.position = new Vector3(slidingParticles.transform.position.x, slidingParticles.transform.position.y, -1);
            }

            // Debug.Log(isSlideTouching);
            if (isTouchingWall && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && facingRight && !isDashing && canMove)
            {
                sm.playJump();
                isDashing = true;
                RemoveForce(false);
                dashCount = 0;
                rb.AddForce(new Vector2(-550f, 800f));
                Rotate(0f, 180f, 0f);
                isWallJumping = true;
            }
            else if (isTouchingWall && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && !facingRight && !isDashing && canMove)
            {
                sm.playJump();
                isDashing = true;
                RemoveForce(false);
                dashCount = 0;
                rb.AddForce(new Vector2(550f, 800f));
                Rotate(0f, 180f, 0f);
                isWallJumping = true;
            }
            else if (isBackTouchingWall && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && facingRight && !isDashing && canMove)
            {
                sm.playJump();
                isDashing = true;
                RemoveForce(false);
                dashCount = 0;
                rb.AddForce(new Vector2(500f, 750f));
                isWallJumping = true;
            }
            else if (isBackTouchingWall && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && !facingRight && !isDashing && canMove)
            {
                sm.playJump();
                isDashing = true;
                RemoveForce(false);
                dashCount = 0;
                rb.AddForce(new Vector2(-500f, 750f));
                isWallJumping = true;
            }
        }
    }

    /////////////////////////////////////////////
    //Misc Functions//
    /////////////////////////////////////////////

    public void Respawn()
    {
        // RemoveForce(false);
        setCanMove(false);
        deathParticles.Play();
        sm.playDeath();
        count = 0;
        dashCount = 30;
        rb.velocity = new Vector2(0, 0);
        pa.setIsMoving(false);
        rb.angularVelocity = 0f;
        canRespawn = false;
    }

    public void Rotate(float x, float y, float z)
    {
        facingRight = !facingRight;
        transform.Rotate(x, y, z);
    }

    public void RemoveForce(bool afterDash)
    {
        if (afterDash)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
    }

    public void Move()
    {
        //horizontal movement
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        float movex = Input.GetAxisRaw("Horizontal");
        Vector3 movementHorizontal = new Vector3(0, 0, 0);

        lastPosX = currPosX;
        currPosX = transform.position.x;

        if (!isDashing && canMove)
        {
            rb.velocity = new Vector3(movex * speed, rb.velocity.y); //creates velocity vector for side 
            movementHorizontal = new Vector3(movex, 0, 0); //creates movement vector
            rb.AddForce(movementHorizontal * speed); //adds force to rigid2d

            if ((currPosX - lastPosX <= -0.01 || currPosX - lastPosX >= 0.01) && isGrounded)
            {
                pa.setIsMoving(true);
            }
            else
            {
                pa.setIsMoving(false);
            }
        }

        //Direction of Character
        if (move.x > 0 && !facingRight && !isDashing)
        {
            Rotate(0f, 180f, 0f);
        }
        else if (move.x < 0 && facingRight && !isDashing)
        {
            Rotate(0f, 180f, 0f);
        }
    }

    public void Jump()
    {
        Vector2 jumpForce = new Vector2(0f, 15f);
        // Debug.Log(isOnFallingPlatform);
        // if (isOnFallingPlatform && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Z))))
        // {
        //     isOnFallingPlatform = false;
        //     return;
        // }
        if (isGrounded && !isDashing && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && canMove)
        {
            delayCount = 0;
            jumpCount = 0.1f;

            sm.playJump();
            isGrounded = false;
        }
        else if (rb.velocity.y < -0.001f)
        {
            isGrounded = false;
        }
        else if (!isGrounded && !isDashing && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Z)) && canMove && jumpCount > 0)
        {
            jumpCount -= Time.deltaTime;
            rb.velocity = jumpForce;
        }
        else if (!isGrounded && !isDashing && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Z)) && canMove && jumpCount > 0)
        {
            jumpCount -= Time.deltaTime;
            rb.velocity = jumpForce;
        }
        else if (!isGrounded && !isDashing && canMove && jumpCount > 0)
        {
            jumpCount = 0;
        }


        if (rb.velocity.y < -2f && !isSliding)
        {
            pa.setIsFalling(true);
        }
        if (rb.velocity.y >= -2f && !isSliding)
        {
            pa.setIsFalling(false);
        }
    }

    public void Dash()
    {
        Vector2 dashRightForce = new Vector2(900f, 0f);
        Vector2 dashLeftForce = new Vector2(-900f, 0f);
        Vector2 dashRightUpForce = new Vector2(800f, 650f);
        Vector2 dashLeftUpForce = new Vector2(-800f, 650f);
        Vector2 dashUpForce = new Vector2(0, 950f);
        if (delayCount >= 1 && isDashReset && !isDashing && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.J)) && canMove)
        {
            sm.playDash();
            isDashReset = false;
            isDashing = true;
            RemoveForce(false);

            dashCount = 0;
            startDashPosX = transform.position.x;
            if (facingRight && ((Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))))
            {
                isGrounded = false;
                rb.AddForce(dashRightUpForce);
            }
            else if (!facingRight && ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))))
            {
                isGrounded = false;
                rb.AddForce(dashLeftUpForce);
            }
            else if (!facingRight && ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))) && !isGrounded)
            {
                rb.AddForce(-dashRightUpForce);
            }
            else if (facingRight && ((Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))) && !isGrounded)
            {
                rb.AddForce(-dashLeftUpForce);
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                isGrounded = false;
                rb.AddForce(dashUpForce);
            }
            else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !isGrounded)
            {
                rb.AddForce(-1.5f * dashUpForce);
            }
            else if (facingRight)
            {
                isGrounded = false;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
                rb.AddForce(dashRightForce);
            }
            else if (!facingRight)
            {
                isGrounded = false;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
                rb.AddForce(dashLeftForce);
            }
        }
    }

    public void DashProcess()
    {
        if (isDashing)
        {
            GameObject sil;
            if (isWallJumping)
            {
                sil = silhouette2;
            }
            else
            {
                sil = silhouette;
            }

            if (!dash0done)
            {

                isVacuum = false;
                dash0done = true;

                var sp = slidingParticles.emission;
                sp.rateOverTime = 0f;
                slidingParticles.Clear();
                isSliding = false;
                pa.setIsSliding(false);
                CameraShakeManager.instance.CameraShake(impulseSource1);

                s1 = Instantiate<GameObject>(sil, pa.GetPosition() + new Vector3(0f, 0f, 0.5f), Quaternion.identity);
                if (!facingRight)
                {
                    s1.GetComponent<Transform>().Rotate(0, 180, 0);
                }
            }
            if (dashCount > 2 && !dash1done)
            {
                dash1done = true;
                s2 = Instantiate<GameObject>(sil, pa.GetPosition() + new Vector3(0f, 0f, 0.5f), Quaternion.identity);
                if (!facingRight)
                {
                    s2.GetComponent<Transform>().Rotate(0, 180, 0);
                }
            }
            if (dashCount > 4 && !dash2done)
            {
                dash2done = true;
                s3 = Instantiate<GameObject>(sil, pa.GetPosition() + new Vector3(0f, 0f, 0.5f), Quaternion.identity);
                if (!facingRight)
                {
                    s3.GetComponent<Transform>().Rotate(0, 180, 0);
                }
                Destroy(s1);
            }
            if (dashCount > 6 && !dash3done)
            {
                dash3done = true;
            }
            if (dashCount > 7 && !dash4done)
            {
                dash4done = true;
                RemoveForce(false);
                rb.gravityScale = 0f;
                Destroy(s2);
            }
            if (dashCount > 8)
            {
                endDashPosX = transform.position.x;

                // Debug.Log(Mathf.Abs(endDashPosX - startDashPosX));
                RemoveForce(true);
                rb.gravityScale = 1f;

                dash0done = false;
                dash1done = false;
                dash2done = false;
                dash3done = false;
                dash4done = false;
                if (!isWallJumping)
                {
                    isDashReset = false;
                }
                isWallJumping = false;
                isDashing = false;

                Destroy(s3);
            }
        }
    }

    public void Vacuum()
    {
        if ((Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.K)) && canMove)
        {
            if (isSliding || isDashing)
            {
                StopCoroutine("playRumble");
                aus.loop = false;
                aus.Stop();
                isVacuum = false;
                // anim.SetBool("isVacuuming", false);
                pa.setIsVacuum(false);
                vacuumEffectorTop.forceMagnitude = 0f;
                vacuumEffectorTop.forceVariation = 0f;
                vacuumEffectorBottom.forceMagnitude = 0f;
                vacuumEffectorBottom.forceVariation = 0f;
                vacuumEffectorTopBack.forceMagnitude = 0f;
                vacuumEffectorTopBack.forceVariation = 0f;
                vacuumEffectorBottomBack.forceMagnitude = 0f;
                vacuumEffectorBottomBack.forceVariation = 0f;
                var vc = vacuumParticles.emission;
                vc.rateOverTime = 0f;
                return;
            }
            if (!isVacuum)
            {
                isVacuum = true;
                // anim.SetBool("isVacuuming", true);
                pa.setIsVacuum(true);
                var vc = vacuumParticles.emission;
                vc.rateOverTime = 200f;
                aus.loop = true;
                aus.Play();
                StartCoroutine("playRumble");
            }
            if (facingRight)
            {
                vacuumEffectorTop.forceMagnitude = -200f;
                vacuumEffectorTop.forceVariation = 100f;
                vacuumEffectorTop.forceAngle = 20f;
                vacuumEffectorBottom.forceMagnitude = -200f;
                vacuumEffectorBottom.forceVariation = 100f;
                vacuumEffectorBottom.forceAngle = -20f;
                vacuumEffectorTopBack.forceMagnitude = 200f;
                vacuumEffectorTopBack.forceVariation = 100f;
                vacuumEffectorTopBack.forceAngle = -20f;
                vacuumEffectorBottomBack.forceMagnitude = 200f;
                vacuumEffectorBottomBack.forceVariation = 100f;
                vacuumEffectorBottomBack.forceAngle = 20f;

                vacuumParticles.transform.position = new Vector3(vacuumParticles.transform.position.x, vacuumParticles.transform.position.y, 1.1f);
            }
            else
            {
                vacuumEffectorTop.forceMagnitude = 200f;
                vacuumEffectorTop.forceVariation = 100f;
                vacuumEffectorTop.forceAngle = -20f;
                vacuumEffectorBottom.forceMagnitude = 200f;
                vacuumEffectorBottom.forceVariation = 100f;
                vacuumEffectorBottom.forceAngle = 20f;
                vacuumEffectorTopBack.forceMagnitude = -200f;
                vacuumEffectorTopBack.forceVariation = 100f;
                vacuumEffectorTopBack.forceAngle = 20f;
                vacuumEffectorBottomBack.forceMagnitude = -200f;
                vacuumEffectorBottomBack.forceVariation = 100f;
                vacuumEffectorBottomBack.forceAngle = -20f;
                vacuumParticles.transform.position = new Vector3(vacuumParticles.transform.position.x, vacuumParticles.transform.position.y, 1.1f);
            }
        }
        else
        {
            StopCoroutine("playRumble");
            aus.loop = false;
            aus.Stop();
            isVacuum = false;
            // anim.SetBool("isVacuuming", false);
            pa.setIsVacuum(false);
            vacuumEffectorTop.forceMagnitude = 0f;
            vacuumEffectorTop.forceVariation = 0f;
            vacuumEffectorBottom.forceMagnitude = 0f;
            vacuumEffectorBottom.forceVariation = 0f;
            vacuumEffectorTopBack.forceMagnitude = 0f;
            vacuumEffectorTopBack.forceVariation = 0f;
            vacuumEffectorBottomBack.forceMagnitude = 0f;
            vacuumEffectorBottomBack.forceVariation = 0f;
            var vc = vacuumParticles.emission;
            vc.rateOverTime = 0f;
        }
    }

    public IEnumerator playRumble()
    {
        CameraShakeManager.instance.CameraShake(impulseSource2);
        yield return new WaitForSeconds(0.20f);
        StartCoroutine("playRumble");
    }

    public void ClampVelocity()
    {
        if (rb.velocity.y < -maxSpeed && !isDashing)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    /////////////////////////////////////////////
    //Getters and Setters//
    /////////////////////////////////////////////

    public void SetIsGrounded(bool isGround)
    {
        if (!isGround)
        {
            isGrounded = isGround;
            isDashReset = true;
        }
        else if (rb.velocity.y >= -0.01 && rb.velocity.y <= 0.01)
        {
            isGrounded = isGround;
            isDashReset = true;
        }
    }

    public bool GetIsVacuum()
    {
        return isVacuum;
    }

    public void setIsTouchingWall(bool isTouching)
    {
        isTouchingWall = isTouching;
    }

    public void setIsBackTouchingWall(bool isTouching)
    {
        isBackTouchingWall = isTouching;
    }

    public void setIsSlideTouching(bool isTouching)
    {
        isSlideTouching = isTouching;
    }

    public void setIsOnFallingPlatform(bool isTouching)
    {
        isOnFallingPlatform = isTouching;
    }
    public void setCanMove(bool move)
    {
        if (move)
        {
            canMove = move;
        }
        else
        {
            canMove = move;
            dashCount = 30;
            rb.velocity = new Vector2(0, rb.velocity.y);
            pa.setIsMoving(false);
            rb.angularVelocity = 0f;
        }
    }
    public bool getIsTalking()
    {
        return isTalking;
    }
    public void setIsTalking(bool talking)
    {
        isTalking = talking;
    }

    public bool getCanRespawn()
    {
        return canRespawn;
    }


    /////////////////////////////////////////////
    //Event Listeners//
    /////////////////////////////////////////////

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
