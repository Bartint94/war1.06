using CharacterBehaviour;
using FishNet.Object;
using GameKit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public int health = 100;
    int maxHeath;
    [SerializeField] RectTransform healthBar;
    CharacterManager stateManager;
    private void Awake()
    {
        stateManager = GetComponent<CharacterManager>();
        maxHeath = health;
    }
   
    public void UpdateHealth(int value)
    {

        health += value;
        if(health < 0)
        {
            health = 0;
        }
        float axHeath=  (float)health/(float)maxHeath;
        
        healthBar.SetScale(new Vector3(axHeath,1f,1f));

        
    }
}
