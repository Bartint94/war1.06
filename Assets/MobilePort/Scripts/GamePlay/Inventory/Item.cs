using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : NetworkBehaviour, ISpawnable
{
   
    public bool isRigged;
    
    public virtual void Init(Inventory inventory)
    {
      
    }

    public void Init(Vector3 tranform, Quaternion rotation, GameObject owner, Inventory inventory)
    {
        Init(inventory);
    }
}
