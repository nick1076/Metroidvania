using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityControl : MonoBehaviour
{

    public Entity eMain;
    public EntityConstructor eEnt;

    private bool w;
    private bool a;
    private bool s;
    private bool d;
    private bool e;
    private bool leftClick;
    private bool space;

    private void Update()
    {
        if (eMain == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            w = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            w = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            a = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            a = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            s = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            s = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            d = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            d = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            leftClick = true;

            if (w)
            {
                eMain.Attack(2);
            }
            else if (s)
            {
                eMain.Attack(3);
            }
            else if (d)
            {
                eMain.Attack(0);
            }
            else if (a)
            {
                eMain.Attack(1);
            }

            if (!w && !a && !s && !d)
            {
                if (eMain.GetComponent<SpriteRenderer>().flipX)
                {
                    eMain.Attack(1);
                }
                else
                {
                    eMain.Attack(0);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            leftClick = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            e = true;
            if (eMain.gameObject.GetComponent<EntityPlayer>() != null)
            {
                eMain.gameObject.GetComponent<EntityPlayer>().UseAbility();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            e = false;
        }

        bool jumped = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            space = true;

            if (eMain.isGrounded() || eMain.GetWalls().Count > 0)
            {
                jumped = true;
                eMain.MoveY(eEnt.ecJumpPower);
                eMain.ClearGrounds();
                eMain.ClearWalls();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            space = false;
        }

        if (a)
        {
            eMain.MoveX(-eEnt.ecSpeed);
        }
        if (d)
        {
            eMain.MoveX(eEnt.ecSpeed);
        }

        if (!a && !d)
        {
            eMain.MoveX(0);
        }

        if (!jumped && eMain.isGrounded())
        {
            eMain.MoveY(0);
        }
    }
}
