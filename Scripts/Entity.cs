using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public EntityConstructor eAssignedConstructor = null;

    private SpriteRenderer eRenderer;
    private Rigidbody2D ePhysics;
    private BoxCollider2D eCollider;
    private float eCHealth;
    private float eMHealth;
    private bool eVelocityLocked;

    private Vector2 movementVelocity;
    private Vector2 additionalVelocity;

    private List<GameObject> eGrounds = new List<GameObject>();

    public ParticleSystem eWalkingParticles;
    public GameObject eParticleOrigin;
    private bool originDirIsRight = false;
    private ParticleSystem eCurrentParticles;

    private void Start()
    {
        this.gameObject.name = eAssignedConstructor.name;

        eCHealth = eAssignedConstructor.ecMaxHealth;
        eMHealth = eAssignedConstructor.ecMaxHealth;

        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            eRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            eRenderer = GetComponent<SpriteRenderer>();
        }

        if (GetComponent<Rigidbody2D>() == null)
        {
            ePhysics = gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            ePhysics = GetComponent<Rigidbody2D>();
        }

        if (GetComponent<BoxCollider2D>() == null)
        {
            eCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        else
        {
            eCollider = GetComponent<BoxCollider2D>();
        }

        StartPost();
    }

    public virtual void StartPost()
    {

    }

    private void FixedUpdate()
    {
        if (ePhysics == null)
        {
            return;
        }

        Vector2 newVel = new Vector2();

        if (additionalVelocity.x < 0)
        {
            if (additionalVelocity.x + eAssignedConstructor.ecDeceleration > 0)
            {
                newVel.x = 0;
            }
            else
            {
                newVel.x = additionalVelocity.x + eAssignedConstructor.ecDeceleration;
            }
        }
        else if (additionalVelocity.x > 0)
        {
            if (additionalVelocity.x - eAssignedConstructor.ecDeceleration < 0)
            {
                newVel.x = 0;
            }
            else
            {
                newVel.x = additionalVelocity.x - eAssignedConstructor.ecDeceleration;
            }
        }

        if (additionalVelocity.y < 0)
        {
            if (additionalVelocity.y + eAssignedConstructor.ecDeceleration > 0)
            {
                newVel.y = 0;
            }
            else
            {
                newVel.y = additionalVelocity.y + eAssignedConstructor.ecDeceleration;
            }
        }
        else if (additionalVelocity.y > 0)
        {
            if (additionalVelocity.y - eAssignedConstructor.ecDeceleration < 0)
            {
                newVel.y = 0;
            }
            else
            {
                newVel.y = additionalVelocity.y - eAssignedConstructor.ecDeceleration;
            }
        }

        additionalVelocity = newVel;

        if (!isGrounded())
        {
            //Add gravity
            additionalVelocity.y += eAssignedConstructor.ecGravity;
        }
        else
        {
            if (additionalVelocity.y < 0)
            {
                additionalVelocity.y = 0;
            }
        }

        newVel += movementVelocity;

        ePhysics.velocity = newVel;

        if (eWalkingParticles != null)
        {
            if (isGrounded())
            {
                if (newVel.x != 0)
                {
                    if (newVel.x > 0)
                    {
                        if (eCurrentParticles == null && eParticleOrigin != null)
                        {
                            originDirIsRight = true;
                            eCurrentParticles = Instantiate(eWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform);
                            eCurrentParticles.Play();
                        }

                        if (eCurrentParticles != null)
                        {
                            if (!originDirIsRight)
                            {
                                eCurrentParticles.transform.parent = null;
                                eCurrentParticles.loop = false;
                                eCurrentParticles = null;

                                eCurrentParticles = Instantiate(eWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform);
                                eCurrentParticles.Play();
                                originDirIsRight = true;
                            }

                            eCurrentParticles.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                    }
                    else if (newVel.x < 0)
                    {
                        if (eCurrentParticles == null && eParticleOrigin != null)
                        {
                            eCurrentParticles = Instantiate(eWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform);
                            eCurrentParticles.Play();
                            originDirIsRight = false;
                        }

                        if (eCurrentParticles != null)
                        {
                            if (originDirIsRight)
                            {
                                eCurrentParticles.transform.parent = null;
                                eCurrentParticles.loop = false;
                                eCurrentParticles = null;

                                eCurrentParticles = Instantiate(eWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform);
                                eCurrentParticles.Play();
                                originDirIsRight = false;
                            }
                            eCurrentParticles.gameObject.transform.localEulerAngles = new Vector3(0, -180, 170);
                        }
                    }
                }
                else
                {
                    if (eCurrentParticles != null)
                    {
                        eCurrentParticles.transform.parent = null;
                        eCurrentParticles.loop = false;
                        eCurrentParticles = null;
                    }
                }
            }
            else
            {
                if (eCurrentParticles != null)
                {
                    eCurrentParticles.transform.parent = null;
                    eCurrentParticles.loop = false;
                    eCurrentParticles = null;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!eGrounds.Contains(collision.gameObject))
        {
            if (collision.gameObject.tag == "Tag.Ground")
            {
                eGrounds.Add(collision.gameObject);
            }
            if (collision.gameObject.tag == "Tag.Ceiling")
            {
                movementVelocity.y = 0;
                additionalVelocity.y = 0;
            }
            if (collision.gameObject.tag == "Tag.Wall")
            {
                additionalVelocity.x = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (eGrounds.Contains(collision.gameObject))
        {
            eGrounds.Remove(collision.gameObject);
        }
    }

    public void MoveX(float xVel)
    {
        if (eVelocityLocked)
        {
            return;
        }

        if (xVel > 0)
        {
            eRenderer.flipX = false;
        }
        else if (xVel < 0)
        {
            eRenderer.flipX = true;
        }

        movementVelocity = new Vector2(xVel, movementVelocity.y);

        Vector2 combinedVelocity = new Vector2(movementVelocity.x + additionalVelocity.x, movementVelocity.y + additionalVelocity.y);

        ePhysics.velocity = combinedVelocity;
    }

    public void MoveY(float yVel)
    {
        if (eVelocityLocked)
        {
            return;
        }

        movementVelocity = new Vector2(movementVelocity.x, yVel);

        Vector2 combinedVelocity = new Vector2(movementVelocity.x + additionalVelocity.x, movementVelocity.y + additionalVelocity.y);

        ePhysics.velocity = combinedVelocity;
    }

    public void SetAdditionalVelocity(Vector2 vel)
    {
        additionalVelocity = vel;
    }

    public void Damage(float amount)
    {
        eCHealth -= amount;

        if (eCHealth > eMHealth)
        {
            eCHealth = eMHealth;
        }
        else if (eCHealth < 0)
        {
            eCHealth = 0;
        }
    }

    public void LockVelocity()
    {
        eVelocityLocked = true;
    }

    public void UnLockVelocity()
    {
        eVelocityLocked = false;
    }

    public Rigidbody2D GetPhysics()
    {
        return ePhysics;
    }
    public Vector2 GetMovementVelocity()
    {
        return movementVelocity;
    }

    public bool isGrounded()
    {
        if (eGrounds.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearGrounds()
    {
        eGrounds.Clear();
    }

}
