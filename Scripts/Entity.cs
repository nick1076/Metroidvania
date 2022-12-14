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
    private List<GameObject> eWalls = new List<GameObject>();

    private ParticleSystem eWalkingParticles;
    public GameObject eParticleOrigin;

    public Transform eAttackOrigin_Up;
    public Transform eAttackOrigin_Front;
    public Transform eAttackOrigin_Back;
    public Transform eAttackOrigin_Down;

    private bool originDirIsRight = false;
    private ParticleSystem eCurrentParticles;

    private float eCurrentGravity;

    private GameObject eCurrentAttack;

    private void Start()
    {
        this.gameObject.name = eAssignedConstructor.name;

        eCHealth = eAssignedConstructor.ecMaxHealth;
        eMHealth = eAssignedConstructor.ecMaxHealth;
        eCurrentGravity = eAssignedConstructor.ecGravity;

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

        if (GetWalls().Count > 0)
        {
            if (newVel.y <= 0)
            {
                newVel.y = eCurrentGravity;
            }
            if (additionalVelocity.y <= 0)
            {
                additionalVelocity.y = eCurrentGravity;
            }
        }

        additionalVelocity = newVel;

        if (GetWalls().Count == 0)
        {
            if (!isGrounded())
            {
                //Add gravity
                additionalVelocity.y += eCurrentGravity;
            }
            else
            {
                if (additionalVelocity.y < 0)
                {
                    additionalVelocity.y = 0;
                }
            }
        }

        newVel += movementVelocity;

        ePhysics.velocity = newVel;

        if (eAssignedConstructor.ecWalkingParticles != null)
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
                            eCurrentParticles = Instantiate(eAssignedConstructor.ecWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform).GetComponent<ParticleSystem>();
                            eCurrentParticles.Play();
                        }

                        if (eCurrentParticles != null)
                        {
                            if (!originDirIsRight)
                            {
                                eCurrentParticles.transform.parent = null;
                                eCurrentParticles.loop = false;
                                eCurrentParticles = null;

                                eCurrentParticles = Instantiate(eAssignedConstructor.ecWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform).GetComponent<ParticleSystem>();
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
                            eCurrentParticles = Instantiate(eAssignedConstructor.ecWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform).GetComponent<ParticleSystem>();
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

                                eCurrentParticles = Instantiate(eAssignedConstructor.ecWalkingParticles, eParticleOrigin.transform.position, eParticleOrigin.transform.rotation, eParticleOrigin.transform).GetComponent<ParticleSystem>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ObjectTags>() != null)
        {
            if (collision.gameObject.GetComponent<ObjectTags>().HasTag("Tag.WallJump") && !eWalls.Contains(collision.gameObject))
            {
                eWalls.Add(collision.gameObject);

                if (this.gameObject.GetComponent<EntityAnimation>() != null)
                {
                    this.gameObject.GetComponent<EntityAnimation>().SetAdditionState(EntityAnimation.EntityStateData.WallSlide);

                    if (eCurrentGravity == eAssignedConstructor.ecGravity)
                    {
                        eCurrentGravity = eAssignedConstructor.ecWallSideGravityMultiplier * eAssignedConstructor.ecGravity;
                        MoveY(0);
                    }
                }
            }
        }

        if (collision.gameObject.GetComponent<ObjectAttack>() != null)
        {
            //Damage Entity
            ObjectAttack attack = collision.gameObject.GetComponent<ObjectAttack>();

            Health(-attack.aDamage);

            if (attack.aEntityPointer != null)
            {
                if (attack.aDir == 3)
                {
                    if (attack.aEntityPointer.additionalVelocity.y < 0)
                    {
                        attack.aEntityPointer.additionalVelocity.y = attack.aPogoForce;
                    }
                    else
                    {
                        attack.aEntityPointer.additionalVelocity.y += attack.aPogoForce;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ObjectTags>() != null)
        {
            if (collision.gameObject.GetComponent<ObjectTags>().HasTag("Tag.WallJump") && eWalls.Contains(collision.gameObject))
            {
                eWalls.Remove(collision.gameObject);

                if (eWalls.Count == 0)
                {
                    if (eCurrentGravity != eAssignedConstructor.ecGravity)
                    {
                        eCurrentGravity = eAssignedConstructor.ecGravity;
                    }
                }
            }
            else
            {
                if (eWalls.Count == 0)
                {
                    if (eCurrentGravity != eAssignedConstructor.ecGravity)
                    {
                        eCurrentGravity = eAssignedConstructor.ecGravity;
                    }
                }
            }
        }
    }

    public void Attack(int dire)
    {
        if (GetWalls().Count == 0)
        {
            if (eCurrentAttack == null)
            {
                if (dire == 0)
                {
                    eCurrentAttack = Instantiate(eAssignedConstructor.ecAttack, eAttackOrigin_Front.transform.position, eAttackOrigin_Front.rotation, eAttackOrigin_Front.transform);

                    ObjectAttack newAttack = eCurrentAttack.GetComponent<ObjectAttack>();
                    newAttack.aDir = dire;
                    newAttack.aEntityPointer = this;
                }
                else if (dire == 1)
                {
                    eCurrentAttack = Instantiate(eAssignedConstructor.ecAttack, eAttackOrigin_Back.transform.position, eAttackOrigin_Back.rotation, eAttackOrigin_Back.transform);

                    ObjectAttack newAttack = eCurrentAttack.GetComponent<ObjectAttack>();
                    newAttack.aDir = dire;
                    newAttack.aEntityPointer = this;
                }
                else if (dire == 2)
                {
                    eCurrentAttack = Instantiate(eAssignedConstructor.ecAttack, eAttackOrigin_Up.transform.position, eAttackOrigin_Up.rotation, eAttackOrigin_Up.transform);

                    ObjectAttack newAttack = eCurrentAttack.GetComponent<ObjectAttack>();
                    newAttack.aDir = dire;
                    newAttack.aEntityPointer = this;
                }
                else if (dire == 3 && !isGrounded())
                {
                    eCurrentAttack = Instantiate(eAssignedConstructor.ecAttack, eAttackOrigin_Down.transform.position, eAttackOrigin_Down.rotation, eAttackOrigin_Down.transform);

                    ObjectAttack newAttack = eCurrentAttack.GetComponent<ObjectAttack>();
                    newAttack.aDir = dire;
                    newAttack.aEntityPointer = this;
                }
            }
        }
    }

    public void Health(float health)
    {
        if (health < 0)
        {
            //Player is being hurt
            eCHealth += health;

            if (eAssignedConstructor.ecHurtFX != null)
            {
                Instantiate(eAssignedConstructor.ecHurtFX, this.transform.position, this.transform.rotation);
            }

            StartCoroutine(ColorPlayer(new Color(1, 0, 0, 1), 0.1f));
        }
        else if (health > 0)
        {
            //Player is being healed
            eCHealth += health;

            if (eAssignedConstructor.ecHealFX != null)
            {
                Instantiate(eAssignedConstructor.ecHealFX, this.transform.position, this.transform.rotation);
            }

            StartCoroutine(ColorPlayer(new Color(0, 1, 0, 1), 0.1f));
        }

        if (eCHealth > eMHealth)
        {
            eCHealth = eMHealth;
        }
        else if (eCHealth < 0)
        {
            eCHealth = 0;
        }
    }

    IEnumerator ColorPlayer(Color color, float time)
    {
        eRenderer.color = color;
        yield return new WaitForSeconds(time);
        eRenderer.color = new Color(1, 1, 1, 1);
    }

    public void MoveX(float xVel)
    {
        if (eVelocityLocked)
        {
            return;
        }

        if (this.gameObject.GetComponent<EntityAnimation>() != null)
        {
            if (this.gameObject.GetComponent<EntityAnimation>().GetLatestState() != "JumpFromWall")
            {
                if (xVel > 0)
                {
                    eRenderer.flipX = false;
                }
                else if (xVel < 0)
                {
                    eRenderer.flipX = true;
                }
            }
            else
            {
                if (xVel > 0)
                {
                    eRenderer.flipX = true;
                }
                else if (xVel < 0)
                {
                    eRenderer.flipX = false;
                }
            }
        }
        else
        {
            if (xVel > 0)
            {
                eRenderer.flipX = false;
            }
            else if (xVel < 0)
            {
                eRenderer.flipX = true;
            }
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

    public List<GameObject> GetWalls()
    {
        return eWalls;
    }

    public void ClearGrounds()
    {
        eGrounds.Clear();
    }

    public void ClearWalls()
    {
        eWalls.Clear();
    }

}
