using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TexRend : MonoBehaviour
{
    public RenderTexture renderTexture;
    string path = "C:\\Users\\borow\\War\\Assets\\MobilePort\\VFX\\Decals\\dec.png";


    [SerializeField] Camera cam;
    [SerializeField] Shader shader;
    [SerializeField] DecalProjector decal;
  
    public void SaveRenderTextureToFile(string filePath)
    {
        // Create a temporary Texture2D to hold the contents of the render texture
        Texture2D tempTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

        // Read the pixels from the render texture into the temporary Texture2D
        RenderTexture.active = renderTexture;
        tempTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTexture.Apply();

        // Encode the temporary Texture2D into a byte array
        byte[] bytes = tempTexture.EncodeToPNG();

        // Write the byte array to a file
        System.IO.File.WriteAllBytes(filePath, bytes);

        // Clean up resources
        Destroy(tempTexture);

        Debug.Log("Render texture saved to: " + filePath);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(CreateRenderTextureRuntime());
        }
        
    }
    public IEnumerator CreateRenderTextureRuntime()
    {
        RenderTexture render = new RenderTexture(1024, 1024, 32, RenderTextureFormat.ARGB32);
        cam.targetTexture = render;
        
        Texture2D tempTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

        // Read the pixels from the render texture into the temporary Texture2D
        // RenderTexture.active = render;
        yield return new WaitForEndOfFrame();
        RenderTexture.active = render;
        tempTexture.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
        tempTexture.Apply();

        Material tempMaterial = new Material(shader);
        tempMaterial.SetTexture("Base_Map", tempTexture);
        

        decal.material = tempMaterial;  

    }
}
