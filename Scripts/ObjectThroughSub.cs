using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThroughSub : MonoBehaviour
{
    public ObjectThroughPlatform platform;
    public ObjectThroughPlatform.playerLocationData locationType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tag.Player")
        {
            platform.SetPosition(locationType);
        }
    }
}
