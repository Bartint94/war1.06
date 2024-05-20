using UnityEngine;

public class CombineMeshes : MonoBehaviour
{
    void Start()
    {
        CombineMeshesInChildren();
    }

    void CombineMeshesInChildren()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        Mesh finalMesh = new Mesh();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        finalMesh.CombineMeshes(combineInstances, true);

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = finalMesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial; // Ustaw materia� z pierwszego dziecka (zmie�, je�li potrzebujesz innego podej�cia)

        gameObject.SetActive(true);
    }
}