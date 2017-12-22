using UnityEngine;

public class Explosion : MonoBehaviour {

	public float ExplosionMaxsize = 10f;
	public float ExplosionRate = 1f;
	public float ExplosionTimeout = 0.1f;
	public float CurrentRadius;

	public bool Exploded;
	private CircleCollider2D _explosionRad;

	private void Start() 
	{
		_explosionRad = gameObject.GetComponent<CircleCollider2D>();
	}

	private void Update () 
	{	
		ExplosionTimeout = ExplosionTimeout - Time.deltaTime;
		if(ExplosionTimeout <= 0)
		{
			Exploded = true;
			gameObject.SetActive(false);
		}

	}

	public void FixedUpdate() 
	{
		if(Exploded) {
			if(CurrentRadius < ExplosionMaxsize)
			{
				CurrentRadius += ExplosionRate;
			}
			else 
			{
				Destroy(gameObject);
			}
		
			_explosionRad.radius = CurrentRadius;
		}
	}

	public void OnTriggerEnter2D(Collider2D c)
	{
		if (Exploded)
		{
			if (c.gameObject.GetComponent<Rigidbody2D>() != null)  //&& c.gameObject.CompareTag("Player")
			{
				Vector2 target = c.gameObject.transform.position;
				Vector2 bomb = gameObject.transform.position;
				Vector2 dir = 300f * (target - bomb);
				c.gameObject.GetComponent<Rigidbody2D>().AddForce(dir);
			}
		}

	}

}
