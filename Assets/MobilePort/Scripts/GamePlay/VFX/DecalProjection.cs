using UnityEngine;

public class DecalProjection : MonoBehaviour
{
    public Texture2D decalTexture;  // Tekstura naklejki
    public SkinnedMeshRenderer targetRenderer;  // Skinned Mesh Renderer celu

    private Material decalMaterial;
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        // SprawdŸ czy Skinned Mesh Renderer ma materia³ do naklejek
        if (targetRenderer == null || targetRenderer.sharedMaterial == null)
        {
            Debug.LogError("Target Skinned Mesh Renderer or its material is not assigned.");
            return;
        }

        // Skopiuj materia³ z celu i ustaw now¹ teksturê naklejki
        decalMaterial = new Material(targetRenderer.sharedMaterial);
        decalMaterial.mainTexture = decalTexture;

        // Utwórz PropertyBlock do przekazywania danych do materia³u
        propertyBlock = new MaterialPropertyBlock();
    }

    private void LateUpdate()
    {
        // Pobierz wierzcho³ki i indeksy z celu
        Mesh targetMesh = targetRenderer.sharedMesh;
        Vector3[] vertices = targetMesh.vertices;
        int[] indices = targetMesh.triangles;

        // Projektuj wierzcho³ki na naklejce
        Vector4[] decaledVertices = new Vector4[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            decaledVertices[i] = transform.InverseTransformPoint(targetRenderer.transform.TransformPoint(vertices[i]));
        }

        // Aktualizuj wierzcho³ki w PropertyBlock
        propertyBlock.SetVectorArray("_DecalVertices", decaledVertices);

        // Przekazanie danych do materia³u
        Graphics.DrawMesh(targetMesh, targetRenderer.transform.position, targetRenderer.transform.rotation, decalMaterial, targetRenderer.gameObject.layer, null, 0, propertyBlock);
    }
}
