using CharacterBehaviour;
using FishNet.Object;
using GameKit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [SerializeField] int health = 100;
    int maxHeath;
    [SerializeField] RectTransform healthBar;
    CharacterStateManager stateManager;
    private void Awake()
    {
        stateManager = GetComponent<CharacterStateManager>();
        maxHeath = health;
    }
   
    public void UpdateHealth(int value)
    {

        health += value;
        float axHeath=  (float)health/(float)maxHeath;
        
        healthBar.SetScale(new Vector3(axHeath,1f,1f));

        if (health <= 0)
        {
            stateManager.SwitchCurrentState(stateManager.dyingState);
        }
    }
}
