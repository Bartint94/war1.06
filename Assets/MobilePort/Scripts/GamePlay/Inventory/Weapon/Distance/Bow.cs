using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] Transform aim;
    public override void Init(Inventory inventory)
    {
        base.Init(inventory);
        inventory.SetAim(aim);
    }
}
