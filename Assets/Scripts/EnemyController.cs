using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class EnemyController: MonoBehaviour {

	public AudioClip explosion;
	public GameObject EnemyBullet;
	public float shotCooldown = 0.3f;
	public float idleTimer;
	public GameObject BodyPart;
	public Transform Player;
	public int point = 100;
	float accelerationTimeAirborne = 0.2f; 		
	float accelerationTimeGrounded = 0.1f; 	
	
	private int enemyHP = 1;
	private float speed;
	public GameObject Explosion;
	private GameObject ExitTest;
	private GameObject[] BulletSpawn;
	private int BulletSpawnSize;
	private float explosionLifetime = 3.0f;
	private int enemyCount;
	private int _totalBodyPart = 7;
	private float _distanceToGround;
	public bool IsIdling = true;
	public bool IsEngaging;
	private Controller2D _controller2D;
	public Rigidbody2D Rigidbody2D;
	private BoxCollider2D _enemyHitBox;
	private float _gravity;
	public Vector3 Velocity;
	private float _airTime;
	public bool OnBoostBlock;
	private float _velocityXSmoothing = 10f;
	
	
	public virtual IEnumerator Idle()
	{
		IsEngaging = false;

		while (IsIdling)
		{
//			Debug.Log("Idling");
			yield return null;
		}

		yield return null;
	}

	public virtual IEnumerator Engage(GameObject go)
	{
		IsIdling = false;
		IsEngaging = true;
//		Debug.Log(gameObject.name + "is engaging :: " + go.gameObject.name);
		while (IsEngaging)
		{
			LookAtPlayer();
			MoveToPlayer();
			yield return new WaitForSeconds(0.3f);
		}
		yield return null;
	}
	
	void Start()
	{
		Player = GameObject.FindWithTag("Player").transform; 
		Rigidbody2D = GetComponent<Rigidbody2D>();
		_controller2D = GetComponent<Controller2D>();
		_gravity =  -(2 * 5) / Mathf.Pow (0.5f, 2);
	}
	
	void CollisionCheck() 
	{
		if (_controller2D.collisions.above || _controller2D.collisions.below) 
			Velocity.y = 0;
		if (OnBoostBlock) 
			Velocity.y += Velocity.y + 0;
		if(!_controller2D.collisions.below)
		{
			_airTime += Time.deltaTime;
			if(_controller2D.collisions.below) 
				_airTime = 0;
		}
	}
	

	void Awake()
	{
		StartCoroutine(Idle());
	}

	private void LookAtPlayer()
	{
		float z = Mathf.Atan2 ((Player.transform.position.y - transform.position.y),
			          (Player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90; 
		transform.eulerAngles = new Vector3 (0, 0, z);
	}

	private void MoveToPlayer()
	{
//		_rigidbody2D.velocity = (Player.transform.position - transform.position);
//		_rigidbody2D.AddForce (gameObject.transform.up * speed); //movement code
		float targetVelocityX = (Player.transform.position.x - transform.position.x) * 10;
//		Debug.Log("target  x ::: "+ targetVelocityX);
		//Debug.LogError("cunt "+ targetVelocityX);
		Velocity.x = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller2D.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		
//		Debug.Log("erreeee" + Velocity.x);
		
		Velocity.y = _gravity * Time.deltaTime;
		_controller2D.Move(Velocity * Time.deltaTime);
	}
	
	void Update ()
	{
		enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

		if (enemyHP <= 0) 
		{
			// enemy destroyed maybe dropping his weapon??????
			//Instantiate(onHoldWeapon, transform.position, transform.rotation);
			Instantiate(Explosion, transform.position, transform.rotation);
			OnExplode();
			FindObjectOfType<Score> ().AddPoint (point);
			//Destroy(gameObject);
			//thisprefabsetactive.false 
			AudioSource.PlayClipAtPoint(explosion,transform.position);
		}
	}

	void OnExplode() {
		if (enemyCount == 1) {
			Instantiate (ExitTest, transform.position, transform.rotation);	
		}
		for (int i = 0; i < _totalBodyPart; i++)
		{
			GameObject b = Instantiate(BodyPart, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(-400, 400));
            b.GetComponent<Rigidbody2D>().AddForce(Vector3.up * Random.Range(-200, 200));
        }
        FindObjectOfType<Score> ().AddPoint (point);
        Destroy(gameObject);
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("aa");
		if (other.CompareTag("Player"))
		{
			Debug.Log("LLLLL :::" + other.transform.name);
			StartCoroutine(Engage(other.gameObject));
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine(Engage(other.gameObject));
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		IsEngaging = false;
		IsIdling = true;
		StartCoroutine(Idle());
	}

	void OnCollisionEnter2D (Collision2D c){

	    if (c.gameObject.CompareTag("PlayerBullet"))
	    {
		    //animator.SetBool("IsATKED", true); //blink white sprite
		    //float force = 20;
		    c.gameObject.SetActive(false);
		    enemyHP -= 1;
		    //transform.Translate(-Vector2.up * force * Time.deltaTime);
		    OnExplode();
	    }
	    //TODO::On wall collision, decrease velocity i.e. lerp the shit
    }
}
