using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class Creep : MonoBehaviour {


	public float delay;
	public GameObject bullet;
	public bool canShoot = true;
	public Animator anim;
	public GameObject[] bulletList;
	public float strayFactor;	

	void Start () {
		anim = GetComponent<Animator>();
		//bulletList = Muzzle.bulletSpawn ;
	}
	
	public void Shot (Transform Pos) { // send in the pattern so each gun have differnt pattern maybe??
		var randomNumberX = Random.Range(-strayFactor, strayFactor);
		var randomNumberY = Random.Range(-strayFactor, strayFactor);
		var randomNumberZ = Random.Range(-strayFactor, strayFactor);
		GameObject puller = GameObject.Find ("EnemyBullet_Pool");
		GameObject obj = puller.GetComponent<ObjectPoolingScript>().GetPooledObject();
		obj.transform.position = Pos.position;
		obj.transform.rotation = Pos.rotation;
		obj.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ); //rotating teh shot
		obj.SetActive(true);
		}

	
	public Animator getAnim() {
		return anim;
	}
}
