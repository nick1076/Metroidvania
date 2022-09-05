using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="newEntityConstructor",menuName="[SO] New Entity Constructor")]
public class EntityConstructor : ScriptableObject
{

    [Header("Entity Data - Health")]

    public float ecMaxHealth = 20.0f;

    [Header("Entity Data - Movement")]

    public float ecSpeed = 0.5f;
    public float ecGravity = -5.0f;
    public float ecJumpPower = 5;
    public float ecDeceleration = 0.25f;
    public float ecWallSideGravityMultiplier = 2;

    [Header("Entity Data - Specific Variables")]

    public float ecPower = 50.0f;
    public float ecRange = 5.0f;

}
