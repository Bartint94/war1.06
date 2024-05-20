using CharacterBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    PlayerInputs inputs;

    public Transform rotationRoot;

    [SerializeField] float sensivitityX = .1f;
    [SerializeField] float sensivitityY = .1f;

    public float yRot;

    public float xRot;
    private void Start()
    {
        inputs = GetComponent<PlayerInputs>();  
    }
    public void Look(bool isAim = false)
    {
        yRot += inputs.mouseX * sensivitityX;
        

        xRot -= inputs.mouseY * sensivitityX;

        Quaternion target = Quaternion.Euler(xRot, yRot, 0);

        rotationRoot.localRotation = target;

        //rotationRoot.Rotate(Vector3.up, inputs.mouseX);
        //rotationRoot.Rotate(Vector3.right, inputs.mouseY);
    }
}
  