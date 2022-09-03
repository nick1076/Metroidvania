using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="newEntityConstructor",menuName="[SO] New Entity Constructor")]
public class EntityConstructor : ScriptableObject
{
    [Header("Entity Visuals")]

    public List<Sprite> ecDirectionalSprites = new List<Sprite>();

    [Header("Entity Statistics")]

    public float ecMaxHealth = 20.0f;
    public float ecSpeed = 0.5f;
    public float ecSpeedCapX = 5.0f;
    public float ecJumpPower = 5;
}
