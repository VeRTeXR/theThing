using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolingScript : MonoBehaviour {

	
	public GameObject pooledObject;
	private Transform bulletHolder;
	public int pooledAmount = 30;
	public bool willGrow = true;
	public string poolName;

	public List<GameObject> pooledObjects; 

	

	void Start ()
	{
		bulletHolder = new GameObject ("BulletHolder: "+pooledObject.name).transform;
		pooledObjects = new List<GameObject>();
		for(int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject) Instantiate (pooledObject);
			obj.transform.SetParent (bulletHolder);
			obj.SetActive (false);
			pooledObjects.Add(obj);
		}
	}

	public GameObject GetPooledObject () {
			for(int i = 0; i < pooledObjects.Count; i++) {
				if(!pooledObjects[i].activeInHierarchy) {
					return pooledObjects[i];
				}
			}

			if(willGrow) {
				GameObject obj = (GameObject) Instantiate (pooledObject);
				pooledObjects.Add(obj);
				return obj;
			}

			return null;
	}
	

}