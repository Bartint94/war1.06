using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateAccount : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TextMeshProUGUI commit;
    [SerializeField] PlayerData playerData;
    
    public void SubmitButton()
    {
        if(nameInputField.text.Length <= 3)
        {
            commit.text = "the name should contain at least 4 characters";
        }
        else if (nameInputField.text.Length > 10)
        {
            commit.text = "the name should contain at most 10 characters";
        }
        else
        {
            var cloud = FindAnyObjectByType<CloudSaveData>();
            cloud.SaveString("playerName", nameInputField.text);
            playerData.playerName = nameInputField.text;
            gameObject.SetActive(false);
        }
    }

}
