using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTags : MonoBehaviour
{

    [SerializeField] private List<string> objTags = new List<string>();

    public bool HasTag(string tag)
    {
        if (objTags.Contains(tag))
        {
            return true;
        }
        else return false;
    }

}
