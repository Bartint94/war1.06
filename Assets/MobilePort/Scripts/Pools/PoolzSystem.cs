using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
public interface ISpawnable
{
    
    void Init(Vector3 position, Quaternion rotation, GameObject owner,Inventory inventory = null);
}
public class PoolzSystem : NetworkBehaviour
{
    public List<Poolz> poolz = new List<Poolz>();
    public static PoolzSystem instance;
    Inventory inventory;
    private void Awake()
    {
        instance = this;
        foreach (var spawner in poolz)
        {
            Init(spawner);
        }
        inventory = GetComponent<Inventory>();
    }
    private void Start()
    {
    }
    public GameObject SpawnNob(GameObject prefab, Vector3 position, Quaternion rotation, GameObject parent, Inventory inventory = null)
    {
        var ar = Instantiate(prefab, position, rotation); //NetworkManager.GetPooledInstantiated(prefab, true)
        //base.Spawn(ar);

       // ActivateNobObserver(ar, position, rotation, parent, inventory);
        ar.GetComponent<ISpawnable>().Init(position, rotation, parent, inventory);
        //ar.GetComponent<ISpawnable>().Init(aim.position, aim.rotation, transform);
        //GameObject ar = Instantiate(arrow.gameObject, aim.position, aim.rotation);
        //base.Spawn(ar, base.Owner);
        //SpawnProjectile();
        
        return ar;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnNobServer(GameObject prefab, Vector3 position, Quaternion rotation, GameObject parent,Inventory inventory = null)
    {
        var ar = Instantiate(prefab, position, rotation); //NetworkManager.GetPooledInstantiated(prefab, true)
        base.Spawn(ar);
        
         ar.GetComponent<ISpawnable>().Init(position, rotation, parent, inventory);
        //ar.GetComponent<ISpawnable>().Init(aim.position, aim.rotation, transform);
        //GameObject ar = Instantiate(arrow.gameObject, aim.position, aim.rotation);
        //base.Spawn(ar, base.Owner);
        //SpawnProjectile();
        ActivateNobObserver(ar, position, rotation, parent, inventory);
    }

    [ObserversRpc(BufferLast = true)]
    void ActivateNobObserver(GameObject ob, Vector3 position, Quaternion rotation, GameObject parent, Inventory inventory)
    {
        if(ob == null) { return; }
       // var ar = Instantiate(ob, position, rotation);
        ob.SetActive(true);
        ob.GetComponent<ISpawnable>().Init(position, rotation, parent, inventory);

        //GameObject ar = Instantiate(arrow.gameObject,aim.position,aim.rotation);
        // base.Spawn(ar,base.Owner);
    }
    void Init(Poolz spawner)
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
         }, false, 5, 5);
    }
    GameObject spawn;
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, GameObject owner = null)
    {
        var pool = poolz.Find(p => p.prefab.name == prefab.name);

        if (pool != null)
        {
            var p = pool.Spawn(position, rotation, owner);
            return p;
        }
        else
        {
            Debug.Log("pools dont have this object");
            return null;
        }
    }
    void Show()
    {
        ShowServer(spawn);
    }

    [ServerRpc(RequireOwnership = false)]
    void ShowServer(GameObject ob)
    {
        ob.SetActive(true);
        ShowObserver(ob);
        //ServerManager.Spawn(ob);
    }
    [ObserversRpc]
    void ShowObserver(GameObject ob)
    {
        ob.SetActive(true);
    }

    public void Release(GameObject prefab)
    {
        var name = prefab.name.Substring(0, prefab.name.Length-7);
        var pool = poolz.Find(p => p.prefab.name == name);

        if (pool != null)
        {
            pool.Release(prefab);
        }
        else
        {

            Debug.Log($"pools dont have this object {name}");
        }
    }

}

[System.Serializable]
public class Poolz
{

    public GameObject prefab;
    public ObjectPool<GameObject> _pool;
    [SerializeField] private int spawnAmount;

    public static Spawner instance;

    public GameObject Spawn(Vector3 position, Quaternion rotation, GameObject owner)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            var spawn = _pool.Get();// position + Vector3.up *2 + Random.insideUnitSphere * 7;
            spawn.GetComponent<ISpawnable>().Init(position, rotation, owner);
            return spawn;
        }
        return null;
    }
    public void Release(GameObject blood)
    {
        _pool.Release(blood);
    }

}