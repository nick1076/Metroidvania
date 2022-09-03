using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EntityConstructor ecPlayer;
    private Entity ePlayer;
    private EntityControl ePlayerController;

    void Start()
    {
        GameObject obj = new GameObject();

        obj.AddComponent<Entity>().Constructer(ecPlayer);
        ePlayer = obj.GetComponent<Entity>();

        obj.AddComponent<EntityControl>().Constructer(ecPlayer, ePlayer, true);
        ePlayerController = obj.GetComponent<EntityControl>();
    }
}
