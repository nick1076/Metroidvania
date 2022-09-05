using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThroughPlatform : MonoBehaviour
{

    [SerializeField] private bool canDropThrough;

    public enum playerLocationData
    {
        None,
        Top,
        Bottom
    };

    [SerializeField] private playerLocationData playerLocation;

    [SerializeField] private BoxCollider2D topColl;
    [SerializeField] private BoxCollider2D bottomColl;
    [SerializeField] private BoxCollider2D topCollTrig;
    [SerializeField] private BoxCollider2D bottomCollTrig;

    public void SetPosition(playerLocationData data)
    {
        playerLocation = data;

        if (data == playerLocationData.Bottom)
        {
            topColl.enabled = false;
            bottomColl.enabled = false;
        }
        else if (data == playerLocationData.Top)
        {
            topColl.enabled = true;
            bottomColl.enabled = true;
        }
        else
        {
            topColl.enabled = true;
            bottomColl.enabled = true;
            topCollTrig.enabled = true;
            bottomCollTrig.enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && playerLocation == playerLocationData.Top && canDropThrough)
        {
            topColl.enabled = false;
            bottomColl.enabled = false;
            topCollTrig.enabled = false;
            bottomCollTrig.enabled = false;
        }
    }
}
