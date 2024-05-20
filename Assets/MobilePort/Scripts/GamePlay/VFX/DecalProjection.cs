using UnityEngine;

public class DecalProjection : MonoBehaviour
{
    public Texture2D decalTexture;  // Tekstura naklejki
    public SkinnedMeshRenderer targetRenderer;  // Skinned Mesh Renderer celu

    private Material decalMaterial;
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        // Sprawd� czy Skinned Mesh Renderer ma materia� do naklejek
        if (targetRenderer == null || targetRenderer.sharedMaterial == null)
        {
            Debug.LogError("Target Skinned Mesh Renderer or its material is not assigned.");
            return;
        }

        // Skopiuj materia� z celu i ustaw now� tekstur� naklejki
        decalMaterial = new Material(targetRenderer.sharedMaterial);
        decalMaterial.mainTexture = decalTexture;

        // Utw�rz PropertyBlock do przekazywania danych do materia�u
        propertyBlock = new MaterialPropertyBlock();
    }

    private void LateUpdate()
    {
        // Pobierz wierzcho�ki i indeksy z celu
        Mesh targetMesh = targetRenderer.sharedMesh;
        Vector3[] vertices = targetMesh.vertices;
        int[] indices = targetMesh.triangles;

        // Projektuj wierzcho�ki na naklejce
        Vector4[] decaledVertices = new Vector4[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            decaledVertices[i] = transform.InverseTransformPoint(targetRenderer.transform.TransformPoint(vertices[i]));
        }

        // Aktualizuj wierzcho�ki w PropertyBlock
        propertyBlock.SetVectorArray("_DecalVertices", decaledVertices);

        // Przekazanie danych do materia�u
        Graphics.DrawMesh(targetMesh, targetRenderer.transform.position, targetRenderer.transform.rotation, decalMaterial, targetRenderer.gameObject.layer, null, 0, propertyBlock);
    }
}
