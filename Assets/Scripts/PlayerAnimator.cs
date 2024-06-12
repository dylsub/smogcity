using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void setIsMoving(bool isMoving)
    {
        anim.SetBool("isMoving", isMoving);
    }

    public void setIsVacuum(bool isVacuum)
    {
        anim.SetBool("isVacuuming", isVacuum);
    }

    public void setIsSliding(bool isSlide)
    {
        anim.SetBool("isSliding", isSlide);
    }

    public void setIsFalling(bool isFalling)
    {
        anim.SetBool("isFalling", isFalling);
    }

    public Vector3 GetPosition()
    {
        return transform.position;

    }
}
