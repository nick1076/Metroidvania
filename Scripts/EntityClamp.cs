using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityClamp : Entity
{
    private Rigidbody2D clampPhysics;
    private LineRenderer clampLine;
    private Entity clampSource;

    public GameObject clampLineOrigin;
    public Material clampMaterial;

    public void Cast(Entity source)
    {
        clampSource = source;
        clampLine = this.gameObject.AddComponent<LineRenderer>();
        clampLine.startWidth = 0.1f;
        clampLine.endWidth = 0.1f;

        clampLine.material = clampMaterial;
    }

    public override void StartPost()
    {
        base.StartPost();
        clampPhysics = GetPhysics();

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float AngleRad = Mathf.Atan2(worldPoint.y - this.transform.position.y, worldPoint.x - this.transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);

        clampPhysics.velocity = this.transform.up * eAssignedConstructor.ecSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tag.Ground" || collision.tag == "Tag.Wall" || collision.tag == "Tag.Ceiling")
        {
            //Pull player towards it
            GameObject temporary = new GameObject();
            temporary.transform.position = clampSource.transform.position;

            float AngleRad = Mathf.Atan2(clampSource.gameObject.transform.position.y - this.transform.position.y, clampSource.gameObject.transform.position.x - this.transform.position.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            temporary.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 270);

            Vector2 vel = temporary.transform.up * eAssignedConstructor.ecPower;

            if (clampSource.GetMovementVelocity().y < 0)
            {
                vel.y =+ Mathf.Abs(clampSource.GetMovementVelocity().y);
            }
            else
            {
                vel.y -= clampSource.GetMovementVelocity().y;
            }

            clampSource.SetAdditionalVelocity(vel);

            Destroy(clampPhysics);

            StartCoroutine(DecayClamp());
        }
    }

    private void FixedUpdate()
    {
        if (clampLine != null && clampSource != null)
        {
            Vector3 pointOne = clampLineOrigin.transform.position;
            pointOne.z = 5;

            Vector3 pointTwo = clampSource.gameObject.transform.position;
            pointTwo.z = 5;

            clampLine.SetPosition(0, pointOne);
            clampLine.SetPosition(1, pointTwo);

            if (Vector2.Distance(pointOne, pointTwo) > eAssignedConstructor.ecRange)
            {
                Destroy(clampPhysics);
                StartCoroutine(DecayClamp());
            }
        }
    }

    IEnumerator DecayClamp()
    {
        float initialWidth = clampLine.startWidth;
        float decayFactor = initialWidth / 100;

        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.0025f);

            clampLine.startWidth = clampLine.startWidth - decayFactor;
            clampLine.endWidth = clampLine.endWidth - decayFactor;
        }

        float scaleDecy = this.transform.localScale.x / 100;

        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.0025f);

            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x - scaleDecy, this.gameObject.transform.localScale.y - scaleDecy, this.gameObject.transform.localScale.z - scaleDecy);
        }

        Destroy(this.gameObject);
    }
}
