using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    public Animator eAnimator;
    public Rigidbody2D ePhysics;

    private void FixedUpdate()
    {
        if (ePhysics.velocity.y > 0)
        {
            //Jumping Up
            Animate("Jump");
            return;
        }
        else if (ePhysics.velocity.y < 0)
        {
            //Falling
            Animate("Fall");
            return;
        }

        if (ePhysics.velocity.x > 0 || ePhysics.velocity.x < 0)
        {
            //Walking
            Animate("Walk");
        }
         
        if (ePhysics.velocity.x == 0)
        {
            //Not moving
            Animate("Idle");
        }
    }

    private void Animate(string action)
    {
        if (eAnimator != null)
        {
            eAnimator.SetTrigger(action);
        }
    }
}
