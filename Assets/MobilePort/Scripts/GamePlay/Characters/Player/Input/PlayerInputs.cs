using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerInputs : NetworkBehaviour
{
    public bool isMobile;
    public bool isJumpStarted;
    public bool isJumpPerformed;
    public bool isJumpCanceled;

    public bool isAttackStarted;

    public bool isSprintPerformed;

    public bool isDodge;

    public bool isBlock;



    public float verticalL;
    public float vertical;

    public float horizontalL;
    public float horizontal;



    public float mouseX;
    public float mouseY;


    public bool isMoved;

    public bool isSimpleSteering;
    Button hit;
    Button jump;
    Button dash;
    FloatingJoystick joy;
    public float speed;


    MobileInputAccess mobileInput;

    Hud hud;

    private void OnDisable()
    {
        if (hit == null) return;
        hit.onClick.RemoveAllListeners();
        jump.onClick.RemoveAllListeners();
        dash.onClick.RemoveAllListeners();
    }

    public override void OnStartClient()
    {
       
    
    
        if(IsOwner)      
        hud = Hud.instance;
        if (hud != null)
            hud.Init(this);
        //isMobile = true;

        if (isMobile && hud != null)
        {
            mobileInput = hud.MobileInputAccess;
            mobileInput.OpenInputCanvas();
            mobileInput.Initialize(out hit, out jump, out dash, out joy);
            hit.onClick.AddListener(Hit);
            jump.onClick.AddListener(Jump);
            dash.onClick.AddListener(Dashr);
           
        }
    }
    
    public void UpdateInputs()
    {
        if (!isMobile)
        {
            PCInput();

        }
        else
        {
            if (isSimpleSteering)
            {
                MobileInputSimplified();
            }
            else
            {
                MobileInputAiming();
            }
        }
    }

    void MobileInputAiming()
    {
        vertical = joy.Vertical;
        //mouseX = joy.Horizontal;
        horizontal = joy.Horizontal;

        if (Input.touchCount == 0) { return; }
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2)
            {

            }
            else
            {
                if (touch.phase == UnityEngine.TouchPhase.Moved)
                {
                    mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
                    mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
                    // mouseY = touch.deltaPosition.y * Time.deltaTime;
                    //mouseX = touch.deltaPosition.x * Time.deltaTime;

                }
                else
                {
                    mouseY = 0f;
                    mouseX = 0f;
                }
            }
        }
    }
    void MobileInputSimplified()
    {
        vertical = joy.Vertical;
        mouseX = joy.Horizontal;
        //horizontal = joy.Horizontal;

        if (Input.touchCount == 0) { return; }
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2)
            {

            }
            else
            {
                if (touch.phase == UnityEngine.TouchPhase.Moved)
                {
                    mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
                   // mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
                    // mouseY = touch.deltaPosition.y * Time.deltaTime;
                    //mouseX = touch.deltaPosition.x * Time.deltaTime;

                }
                else
                {
                    mouseY = 0f;
                    // mouseX = 0f;
                }
            }
        }

    }
    void PCInput()
    {
        isJumpStarted = UnityEngine.Input.GetKeyDown(KeyCode.Space);
        isJumpPerformed = UnityEngine.Input.GetKey(KeyCode.Space);
        isJumpCanceled = UnityEngine.Input.GetKeyUp(KeyCode.Space);

        isAttackStarted = UnityEngine.Input.GetKeyDown(KeyCode.Mouse0);

        isSprintPerformed = UnityEngine.Input.GetKey(KeyCode.LeftShift);

        isDodge = UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt);

        isBlock = Input.GetKeyDown(KeyCode.Mouse1);

        vertical = UnityEngine.Input.GetAxisRaw("Vertical");
        horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");


        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
    }


        void Hit()
        {
            isAttackStarted = true;
        }
        void Jump()
        {
            isJumpStarted = true;
        Debug.Log("jump listener");
        }
        void Dashr()
        {
            isDodge = true;
        Debug.Log("dash listener");
        }

    }

