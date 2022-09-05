using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{

    [SerializeField] private Animator pAnim;
    [SerializeField] private float pAnimCooldown = 2.0f;

    [SerializeField] private bool pDirectional;
    [SerializeField] private SpriteRenderer pRenderer;

    private bool animating;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (animating)
        {
            return;
        }

        if (pAnim != null)
        {
            if (pDirectional)
            {
                if (collision.GetComponent<Rigidbody2D>() != null)
                {
                    if (collision.GetComponent<Rigidbody2D>().velocity.x > 0)
                    {
                        if (pRenderer != null)
                        {
                            pRenderer.flipX = false;
                        }
                    }
                    else
                    {
                        if (pRenderer != null)
                        {
                            pRenderer.flipX = true;
                        }
                    }
                }
            }

            animating = true;
            pAnim.SetTrigger("Animate");
            StartCoroutine(AnimationCooldown());
        }
    }

    IEnumerator AnimationCooldown()
    {
        yield return new WaitForSeconds(pAnimCooldown);
        animating = false;
    }
}
