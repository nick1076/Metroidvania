using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityControl : MonoBehaviour
{

    private Entity eMain;
    private EntityConstructor eEnt;

    private bool w;
    private bool a;
    private bool s;
    private bool d;
    private bool space;

    private List<GameObject> eGrounds = new List<GameObject>();

    public void Constructer(EntityConstructor ent, Entity e, bool camera = false)
    {
        eEnt = ent;
        eMain = e;

        if (camera)
        {
            this.gameObject.AddComponent<Camera>();
            Camera cam = this.gameObject.GetComponent<Camera>();
            cam.orthographic = true;
            cam.nearClipPlane = 0;
            cam.orthographicSize = 8;
            cam.backgroundColor = new Color(1, 1, 1);
        }
    }

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            space = true;

            if (eGrounds.Count > 0)
            {
                //Jump
                eMain.MoveY(eEnt.ecJumpPower);
            }
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!eGrounds.Contains(collision.gameObject))
        {
            if (collision.gameObject.tag == "Tag.Ground")
            {
                eGrounds.Add(collision.gameObject);
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
}
