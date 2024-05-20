using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : NetworkBehaviour
{
    public GameObject prefab;
    public ObjectPool<GameObject> _pool;
    [SerializeField] private int spawnAmount;

    public static Spawner instance;

    private void Awake()
    {
        instance = this;
    }
   
    void Init()
    {
        _pool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(prefab);

        },
         blood =>
         {
             blood.gameObject.SetActive(true);
         },
         blood =>
         {
             blood.gameObject.SetActive(false);
         },
         blood =>
         {
             Destroy(blood.gameObject);
         }, false, 100, 100);


    }

    public void Spawn(Vector3 position)
    {
        for(int i = 0; i < spawnAmount; i++) 
        {
            var spawn = _pool.Get();
            spawn.transform.position = spawn.transform.position;// position + Vector3.up *2 + Random.insideUnitSphere * 7;
        }

    }
    public void Release(GameObject blood)
    {
        _pool.Release(blood);
    }

    public void TrunOnfx(GameObject fx, Vector3 pos, Quaternion rot)
    { 
        OnServer(fx, pos, rot);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnServer(GameObject ob, Vector3 pos, Quaternion rot)
    {
        OnObserver(ob, pos, rot);

    }
    [ObserversRpc]
    void OnObserver(GameObject ob, Vector3 pos, Quaternion rot)
    {   
        ob.GetComponentInChildren<ParticleSystem>().Play();
    }
}
public class PoolManager : MonoBehaviour
{
    List<Spawner> spawners = new List<Spawner>();
    private void Start()
    {
        foreach(Spawner spawner in spawners)
        {
           
        }
    }
    void Init(Spawner spawner)
    {
        spawner._pool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(spawner.prefab);

        },
         blood =>
         {
             blood.gameObject.SetActive(true);
         },
         blood =>
         {
             blood.gameObject.SetActive(false);
         },
         blood =>
         {
             Destroy(blood.gameObject);
         }, false, 100, 100);


    }
}

public class TestPool : ScriptableObject
{
    [SerializeField] GameObject spawnObject;
    ObjectPool<GameObject> pool;
    private void Awake()
    {
        
    }
    void InitPool(Object type)
    {
        pool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(spawnObject);

        },
       blood =>
       {
           blood.gameObject.SetActive(true);
       },
       blood =>
       {
           blood.gameObject.SetActive(false);
       },
       blood =>
       {
           Destroy(blood.gameObject);
       }, false, 100, 100);

    }

}
public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> Pools = new List<PooledObjectInfo>();

    GameObject _objectPoolHolder;
    static GameObject _gameObjectsHolder;
    public static int spawnedShootings;
    private void Awake()
    {
        SetupEmptyHolder();
    }
    void SetupEmptyHolder()
    {
        _objectPoolHolder = new GameObject("Pooled Objects");

        _gameObjectsHolder = new GameObject("Game Objects");
        _gameObjectsHolder.transform.SetParent(_objectPoolHolder.transform);

    }
    public static GameObject Spawn(GameObject spawnObject, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = Pools.Find(p => p.objectName == spawnObject.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { objectName = spawnObject.name };
            Pools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(spawnObject, spawnPosition, spawnRotation);

            if (_gameObjectsHolder != null)
            {
                spawnableObject.transform.SetParent(_gameObjectsHolder.transform);
            }
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }
        return spawnableObject;
    }
    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectInfo pool = Pools.Find(pool => pool.objectName == goName);

        if (pool == null)
        {
            Debug.Log($"object: {obj.name} in not pooled");
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
    public static void ReturnAllGameObjects()
    {
        GameObject[] all = _gameObjectsHolder.GetComponentsInChildren<GameObject>();
        foreach (GameObject obj in all)
        {
            ReturnObjectToPool(obj);
        }
    }
}
public class PooledObjectInfo
{
    public string objectName;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}