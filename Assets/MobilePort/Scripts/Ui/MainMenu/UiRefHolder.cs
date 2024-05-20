using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiRefHolder : MonoBehaviour
{
    public static UiRefHolder instance;
    CustomizeUI _customizeUi;
    public CustomizeUI customizeUi => _customizeUi; 


    InventoryUi _inventoryUi;
    public InventoryUi inventoryUi => _inventoryUi;
    

    [SerializeField] GameObject _loadingScreenCanvas;
    public GameObject loadingScreenCanvas => _loadingScreenCanvas;


    [SerializeField] GameObject _loginScreen;
    public GameObject loginScreen => _loginScreen;

    [SerializeField] GameObject _createAccountPanel;
    public GameObject createAccountPanel => _createAccountPanel;

    private void Awake()
    {
        _customizeUi = GetComponentInChildren<CustomizeUI>();
        _inventoryUi = GetComponentInChildren<InventoryUi>();
        instance = this;
    }
}
