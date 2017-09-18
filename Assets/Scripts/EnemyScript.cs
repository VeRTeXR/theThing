using UnityEngine;
using System.Collections;
using System.Security.Principal;

public class EnemyScript : MonoBehaviour {

	private int enemyHP;
	private float speed;
	public Transform Player;
	public int point = 100;
	private GameObject Explosion;
	private GameObject ExitTest;
	private GameObject[] BulletSpawn;
	private int BulletSpawnSize;
	private float explosionLifetime = 3.0f;
	private int enemyCount;
	public Creep creep;
	public AudioClip explosion;
	public GameObject EnemyBullet;
	public float shotCooldown = 0.3f;
	public float idleTimer;
	public GameObject BodyPart;
	private int _totalBodyPart = 7;
	

	//** mover code move here **//
	public Waypoint[] wayPoints; // create an auto fill obj. for this waypoint list, it should be active filling 
	public bool isCircular;
	public bool inReverse = true;

	private Waypoint currentWaypoint;
	private int currentIndex   = 0;
	private bool isWaiting     = false;
	private float speedStorage = 0;
	//
	
	private bool _isIdling = true;
	private bool _isEngaging = false;


	public virtual IEnumerator Idle()
	{
		_isEngaging = false;

		while (_isIdling)
		{
			Debug.Log("Idling");
			yield return null;
		}

		yield return null;
	}

	public virtual IEnumerator Engage(GameObject go)
	{
		_isIdling = false;
		_isEngaging = true;
		Debug.Log(gameObject.name + "is engaging :: " + go.gameObject.name);
		while (_isEngaging)
		{
			LookAtPlayer();
			yield return new WaitForSeconds(0.3f);
		}
		yield return null;
	}
	
	IEnumerator Start()
	{
		var creep  = gameObject.AddComponent<Creep>();
		creep.delay = 0.3f;
		Player = GameObject.FindWithTag("Player").transform; 
		creep = GetComponent<Creep> (); // adding this shit in a different module then pass the thing here

				if(wayPoints.Length > 0) {
					currentWaypoint = wayPoints[0];
				}
				
				if (creep.canShoot == false) 
				{
					yield break;
				}
				while (true) 
				{
					for (int i = 0; i < transform.childCount; i++) 
					{
						if (transform.GetChild(i).name == "turret")
						{
							Transform shotPos = transform.GetChild(i);
							creep.Shot(shotPos);
						}
					}
					yield return new WaitForSeconds (shotCooldown);
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
		//GetComponent<Rigidbody2D>().velocity = (Player.transform.position - transform.position);
		//GetComponent<Rigidbody2D>().AddForce (gameObject.transform.up * speed); //movement code
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
/*	
	/**
	 * Pause the mover
	 * 
	 #1#
	 void Pause()
	 {
	 	isWaiting = !isWaiting;
	 }



	/**
	 * Move the object towards the selected waypoint
	 * integrate this movement shit into the normal rigidbody2d code
	 #1#
	 private void MoveTowardsWaypoint()
	 {
		// Get the moving objects current position
	 	Vector3 currentPosition = this.transform.position;

		// Get the target waypoints position
	 	Vector3 targetPosition = currentWaypoint.transform.position;

		// If the moving object isn't that close to the waypoint
	 	if(Vector3.Distance(currentPosition, targetPosition) > .1f) {

			// Get the direction and normalize
	 		Vector3 directionOfTravel = targetPosition - currentPosition;
	 		directionOfTravel.Normalize();

			//scale the movement on each axis by the directionOfTravel vector components
	 		this.transform.Translate(
	 			directionOfTravel.x * speed * Time.deltaTime,
	 			directionOfTravel.y * speed * Time.deltaTime,
	 			directionOfTravel.z * speed * Time.deltaTime,
	 			Space.World
	 			);
	 	} else {

			// If the waypoint has a pause amount then wait a bit
	 		if(currentWaypoint.waitSeconds > 0) {
	 			Pause();
	 			Invoke("Pause", currentWaypoint.waitSeconds);
	 		}

			// If the current waypoint has a speed change then change to it
	 		if(currentWaypoint.speedOut > 0) {
	 			speedStorage = speed;
	 			speed = currentWaypoint.speedOut;
	 		} else if(speedStorage != 0) {
	 			speed = speedStorage;
	 			speedStorage = 0;
	 		}

	 		NextWaypoint();
	 	}
	 }





	/**
	 * Work out what the next waypoint is going to be
	 * 
	 #1#
	 private void NextWaypoint()
	 {
	 	if(isCircular) {

	 		if(!inReverse) {
	 			currentIndex = (currentIndex+1 >= wayPoints.Length) ? 0 : currentIndex+1;
	 		} else {
	 			currentIndex = (currentIndex == 0) ? wayPoints.Length-1 : currentIndex-1;
	 		}

	 	} else {

			// If at the start or the end then reverse
	 		if((!inReverse && currentIndex+1 >= wayPoints.Length) || (inReverse && currentIndex == 0)) {
	 			inReverse = !inReverse;
	 		}
	 		currentIndex = (!inReverse) ? currentIndex+1 : currentIndex-1;

	 	}

	 	currentWaypoint = wayPoints[currentIndex];
	 }*/


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

	private void OnTriggerExit2D(Collider2D other)
	{
		_isEngaging = false;
		_isIdling = true;
		StartCoroutine(Idle());
	}

	void OnCollisionEnter2D (Collision2D c){

	    if (c.gameObject.CompareTag("PlayerBullet"))
	    {
		    //animator.SetBool("IsATKED", true); //blink white sprite
		    float force = 20;
		    c.gameObject.SetActive(false);
		    enemyHP -= 1;
		    transform.Translate(-Vector2.up * force * Time.deltaTime);
		    OnExplode();

	    }
	    //TODO::On wall collision, decrease velocity i.e. lerp the shit
    }
}
