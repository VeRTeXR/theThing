using System.Collections;
using UnityEngine;


public class Turret : EnemyController 
{
	public float Delay;
	public GameObject Bullet;
	public bool CanShoot = true;
	public Animator Anim;
	public GameObject[] BulletList;
	public float StrayFactor;
	public float MaxHp;
	private bool _shootToTheRight;
	private bool _alreadyShot;
	private GameObject _turretBarrel;
	private float _hp;
	private Collider2D _hitbox;

	private void Awake()
	{
		_hp = MaxHp;
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).name == "barrel")
				_turretBarrel = transform.GetChild(i).gameObject;
		}
	}
	 
	public override IEnumerator IsAttacked()
	{
		gameObject.transform.parent.GetComponent<Renderer>().material.SetFloat("_FlashAmount", 1);
		yield return new WaitForSeconds(0.025f);
		gameObject.transform.parent.GetComponent<Renderer>().material.SetFloat("_FlashAmount", 0);
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

	void Update()
	{
		if (_hp <= 0)
		{
			Debug.Log("die");
			gameObject.transform.parent.gameObject.SetActive(false);
		}
	}
	
	public void Shot (Transform Pos) { 
		// send in the pattern so each gun have differnt pattern maybe??
		var randomNumberX = Random.Range(-StrayFactor, StrayFactor);
		var randomNumberY = Random.Range(-StrayFactor, StrayFactor);
		var randomNumberZ = Random.Range(-StrayFactor, StrayFactor);
		GameObject puller = GameObject.Find ("EnemyBullet_Pool");
		GameObject obj = puller.GetComponent<ObjectPoolingScript>().GetPooledObject();
		obj.transform.position = Pos.position;
		obj.transform.rotation = Pos.rotation;
		//obj.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ); //rotating teh shot
		obj.SetActive(true);
	}


	private void OnCollisionEnter2D(Collision2D other)
	{		
		if (other.gameObject.CompareTag("PlayerBullet"))
		{
			_hp = _hp- 1;
			StartCoroutine("IsAttacked");
		}
		if (other.gameObject.CompareTag("Player"))
		{
			_hp = _hp - 1;
			StartCoroutine("IsAttacked");
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
		}
	}
}
