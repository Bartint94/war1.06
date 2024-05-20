using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

 
public class TerrTreeBaker : EditorWindow
{
    float rand;
    public GameObject objectToSpawn; // Obiekt do zespawnowania w miejscu drzewa
    public float sizeMultipiler=1;
    [MenuItem("Tools/Spawn Objects On Trees")]
    static void Init()
    {
        TerrTreeBaker window = (TerrTreeBaker)EditorWindow.GetWindow(typeof(TerrTreeBaker));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Spawn Objects On Trees", EditorStyles.boldLabel);

        objectToSpawn = EditorGUILayout.ObjectField("Object To Spawn", objectToSpawn, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Spawn Objects"))
        {
            SpawnObjects();
        }
    }

    void SpawnObjects()
    {
        // Pobierz wszystkie obiekty Terrain na scenie
        Terrain[] terrains = Terrain.activeTerrains;

        foreach (Terrain terrain in terrains)
        {
            // Pobierz wszystkie drzewa na terenie
            TreeInstance[] trees = terrain.terrainData.treeInstances;

            foreach (TreeInstance tree in trees)
            {
                // Pobierz pozycjê drzewa na terenie
                Vector3 treePosition = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;

                // Zespawnowanie obiektu w miejscu drzewa
                rand = UnityEngine.Random.Range(0f, 360f);
                var spawned = Instantiate(objectToSpawn, treePosition, Quaternion.Euler(0f,rand,0f));

                spawned.transform.localScale = new Vector3(tree.widthScale * 1.2f, tree.heightScale * 1.2f, tree.widthScale * 1.2f);
            }
        }
    }
    
}
#endif