using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiButtons : MonoBehaviour
{
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    protected int buttonId;
    protected int buttonsCount;

    public virtual void LeftButton()
    {
        buttonId--;
       

        if (buttonId == 0)
        {
            leftButton.interactable = false;
        }
        if (!rightButton.interactable)
        {
            rightButton.interactable = true;
        }
    }
    public virtual void RightButton()
    {
        buttonId++;
       

        if (buttonId == buttonsCount)
        {
            rightButton.interactable = false;
        }
        if (!leftButton.interactable)
        {
            leftButton.interactable = true;
        }
    }
    public virtual void InitButtons(int Id, int count)
    {
        leftButton.interactable = true;
        rightButton.interactable = true;

        buttonsCount = count-1;
        buttonId = Id;
        if (buttonId == 0)
        {
            leftButton.interactable = false;
        }
        if (buttonId == buttonsCount)
        {
            rightButton.interactable = false;
        }
    }
}
