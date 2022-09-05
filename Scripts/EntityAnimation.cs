using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    public Animator eAnimator;
    public Rigidbody2D ePhysics;
    private string eLatestState = "Idle";

    public enum EntityStateData
    {
        None,
        WallSlide
    }

    [SerializeField] private EntityStateData eAdditionalState;

    public EntityStateData GetAnimationState()
    {
        return eAdditionalState;
    }

    public string GetLatestState()
    {
        return eLatestState;
    }

    private void FixedUpdate()
    {
        if (eAdditionalState == EntityStateData.None)
        {
            if (ePhysics.velocity.y > 0)
            {
                //Jumping Up
                Animate("Jump");
                eLatestState = "Jump";
                return;
            }
            else if (ePhysics.velocity.y < 0)
            {
                //Falling
                Animate("Fall");
                eLatestState = "Fall";
                return;
            }

            if (ePhysics.velocity.x > 0 || ePhysics.velocity.x < 0)
            {
                //Walking
                Animate("Walk");
                eLatestState = "Walk";
            }

            if (ePhysics.velocity.x == 0)
            {
                //Not moving
                Animate("Idle");
                eLatestState = "Idle";
            }
        }
        else if (eAdditionalState == EntityStateData.WallSlide)
        {
            if (ePhysics.velocity.y < 0)
            {
                if (eLatestState == "JumpFromWall" && this.gameObject.GetComponent<Entity>().GetWalls().Count == 0 && this.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
                {
                    //Falling after jumping from wall
                    Animate("Fall");
                    eLatestState = "Fall";
                    eAdditionalState = EntityStateData.None;
                    return;
                }

                if (eLatestState == "WallSlide")
                {
                    //Falling after jumping from wall
                    if (this.gameObject.GetComponent<Entity>().GetWalls().Count == 0)
                    {
                        Animate("Fall");
                        eLatestState = "Fall";
                        eAdditionalState = EntityStateData.None;
                        return;
                    }
                }

                //Sliding down a wall
                Animate("WallSlide");
                eLatestState = "WallSlide";
                return;
            }
            else if (ePhysics.velocity.y > 0)
            {
                //Jumping off a wall
                Animate("JumpFromWall");
                eLatestState = "JumpFromWall";
                return;
            }
            else
            {
                //Sliding down a wall
                Animate("WallSlide");
                eLatestState = "WallSlide";
                return;
            }
        }
    }

    private void Animate(string action)
    {
        if (eAnimator != null)
        {
            eAnimator.SetTrigger(action);
        }
    }

    public void SetAdditionState(EntityStateData set)
    {
        eAdditionalState = set;
    }
}
