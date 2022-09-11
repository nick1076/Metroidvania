using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    public bool enable = false;
    public float destroyTime = 0.1f;

    private void Update()
    {
        if (enable)
        {
            StartCoroutine(WaitDestroy());
        }
    }

    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

}
