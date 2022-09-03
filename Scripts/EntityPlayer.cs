using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : Entity
{
    public GameObject abilityClamp;
    private GameObject deployedAbility;

    public void UseAbility()
    {
        if (deployedAbility != null)
        {
            //Already an ability in use
            return;
        }

        //Allows for a current ability to be used here
        GameObject clamp = Instantiate(abilityClamp, this.transform.position, Quaternion.identity);
        clamp.GetComponent<EntityClamp>().Cast(this);

        deployedAbility = clamp;
    }
}
