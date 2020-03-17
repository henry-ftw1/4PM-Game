using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_BulletPooler : MonoBehaviour
{
    public static D_BulletPooler current;
    //List of different bullets to spawn. CANNOT BE NULL
    //Put all of the bullets here from the inspector, or can add later in code
    public List<GameObject> BulletObjects;
    //pool of bullets to store in
    List<List<GameObject>> pooledBulletObjects;
    //multiply by list length to store that many bullets
    public int EachBulletPoolAmount = 10;

    // Start is called before the first frame update
    void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        //Initialize a set amount of each enemy
        if (BulletObjects.Count == 0)
        {
            Debug.LogError("BulletObjects list is empty. Fill with Bullets.");
        }
        pooledBulletObjects = new List<List<GameObject>>();
        for (int i = 0; i < BulletObjects.Count; i++)
        {
            pooledBulletObjects.Add(new List<GameObject>());
            for (int k = 0; k < EachBulletPoolAmount; k++)
            {
                GameObject obj = (GameObject)Instantiate(BulletObjects[i]);
                obj.SetActive(false);
                pooledBulletObjects[i].Add(obj);
            }
        }
        //Debug.Log(pooledBulletObjects[0].Count);
    }
    //gets an object from the list pooledObjects.
    public GameObject GetPooledObject(int index)
    {
        for (int i = 0; i < pooledBulletObjects[index].Count; ++i)
        {
            if (pooledBulletObjects[index][i] != null && !pooledBulletObjects[index][i].activeInHierarchy) //&& pooledBulletObjects[index][i].name == string.Format("{0}(Clone)", pooledBulletObjects[index][i].name))
            {
                return pooledBulletObjects[index][i];
            }
        }
        return null;
    }

    /// <summary>
    /// Pools more bullets into the scene when there are not enough bullets
    /// </summary>
    /// <param name="listIndex"></param>
    /// <returns>none</returns>
    public GameObject PoolMoreBullets(int listIndex)
    {
        GameObject obj = (GameObject)Instantiate(BulletObjects[listIndex]);
        obj.SetActive(false);
        pooledBulletObjects[listIndex].Add(obj);
        return obj;
    }
}
