using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : EnemyController {

	public float delay;
	public GameObject bullet;
	public bool canShoot = true;
	public Animator anim;
	public GameObject[] bulletList;
	public float strayFactor;
	private bool _shootToTheRight;
	private bool _alreadyShot;
	private GameObject _turretBarrel;

	private void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).name == "barrel")
				_turretBarrel = transform.GetChild(i).gameObject;
		}
	}

	public override IEnumerator Engage(GameObject go)
	{
		var shotDir = Player.transform.position.x - transform.position.x;

		if (shotDir > 0)
		{ 
			_turretBarrel.transform.eulerAngles = new Vector3(0, 0, -90);
			_shootToTheRight = true;
		}
		else
		{
			_turretBarrel.transform.eulerAngles = new Vector3(0, 0, 90);
			_shootToTheRight = false;
		}
		
		Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
		yield return new WaitForSeconds(0.3f);
		if(!_alreadyShot)
		StartCoroutine(Shoot(_turretBarrel.transform,1, _shootToTheRight));
	}

	IEnumerator Shoot(Transform shotPos,float wait,bool shootToTheRight)
	{
		Shot(shotPos);
		_alreadyShot = true;
		yield return new WaitForSeconds(wait);
		_alreadyShot = false;
	} 

	public override IEnumerator Idle()
	{
		IsEngaging = false;

		while (IsIdling)
		{
			yield return null;
		}

		yield return null;
	}
	
	public void Shot (Transform Pos) { // send in the pattern so each gun have differnt pattern maybe??
		
		Debug.LogError(Pos.rotation);
		var randomNumberX = Random.Range(-strayFactor, strayFactor);
		var randomNumberY = Random.Range(-strayFactor, strayFactor);
		var randomNumberZ = Random.Range(-strayFactor, strayFactor);
		GameObject puller = GameObject.Find ("EnemyBullet_Pool");
		GameObject obj = puller.GetComponent<ObjectPoolingScript>().GetPooledObject();
		obj.transform.position = Pos.position;
		obj.transform.rotation = Pos.rotation;
		Debug.LogError("Pos Rotation ::" + Pos.rotation + "Rot::" +obj.transform.rotation);
		//obj.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ); //rotating teh shot
		obj.SetActive(true);
	}

}
