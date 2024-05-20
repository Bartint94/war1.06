using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobileInputAccess : MonoBehaviour
{
    [SerializeField] Button hit;
    [SerializeField] Button jump;
    [SerializeField] Button dash;
    [SerializeField] FloatingJoystick joy;
    [SerializeField] TextMeshProUGUI fpsT;
    [SerializeField] GameObject canvasInput;
    float deltaTime;
    public void OpenInputCanvas()
    {
        canvasInput.SetActive(true);
    }
    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsT.text = "FPS: " + Mathf.Ceil(fps);
    }
    public void Initialize(out Button hit, out Button jump, out Button dash, out FloatingJoystick joy)
    {
        hit = this.hit;
        jump = this.jump;
        dash = this.dash;
        joy = this.joy;
    }
}
