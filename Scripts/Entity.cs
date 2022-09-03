using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    private EntityConstructor eAssignedConstructor = null;

    private List<Sprite> eDirectionalSprites = new List<Sprite>();
    private SpriteRenderer eRenderer;
    private Rigidbody2D ePhysics;
    private BoxCollider2D eCollider;
    private float eCHealth;
    private float eMHealth;

    public void Constructer(EntityConstructor e)
    {
        eAssignedConstructor = e;
        this.gameObject.name = e.name;

        eDirectionalSprites = e.ecDirectionalSprites;

        eCHealth = e.ecMaxHealth;
        eMHealth = e.ecMaxHealth;

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

        if (GetComponent <BoxCollider2D>() == null)
        {
            eCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        else
        {
            eCollider = GetComponent<BoxCollider2D>();
        }

        ePhysics.gravityScale = 2.5f;
        ePhysics.freezeRotation = true;

        eCollider.size = new Vector2(1, 1);

        eRenderer.sprite = eDirectionalSprites[0];
    }

    private void FixedUpdate()
    {
        if (ePhysics == null)
        {
            return;
        }
    }

    public void MoveX(float xVel)
    {
        if (xVel > 0)
        {
            eRenderer.sprite = eDirectionalSprites[0];
        }
        else if (xVel < 0)
        {
            eRenderer.sprite = eDirectionalSprites[1];
        }

        ePhysics.velocity = new Vector2(xVel, ePhysics.velocity.y);
    }

    public void MoveY(float yVel)
    {
        ePhysics.velocity = new Vector2(ePhysics.velocity.x, yVel);
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

}
