using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Graphic : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] UniversalRenderPipelineAsset render;
    void Start()
    {
        slider.value = render.renderScale;
    }

    public void AcceptButton()
    {
        render.renderScale = slider.value;
    }
}
