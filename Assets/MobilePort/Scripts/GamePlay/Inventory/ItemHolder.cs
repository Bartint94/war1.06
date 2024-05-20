using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ItemHolder : MonoBehaviour
{
    public bool isEquiped;
    public Item item;
    [Range(0f, 1f)]
    protected float weight;

    private void Awake()
    {
            item = GetComponentInChildren<Item>();
        if(item != null )
        {
            isEquiped = true;
        }
    }
 
    public void SetupRig()
    {
            /*
        if (isEquiped)
        {
            inventory = GetComponentInParent<Inventory>();
            if (item.isRigged)
            {
                inventory.rigs.SetRigWeightServer(weight, RigPart.leftArm);
            }
        }
                  
            */
}
    /*
    public void SwitchItem()
    {
        if (isHide)
        {
            item.gameObject.SetActive(true);
            inventory.rigs.SetRigWeightServer(weight, RigPart.leftArm);
            isHide = false;
        }
        else
        {
            item.gameObject.SetActive(false);
            inventory.rigs.SetRigWeightServer(0f, RigPart.leftArm);
            isHide = true;
        }
    }
    */

}
