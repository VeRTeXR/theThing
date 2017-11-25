using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float explosion_maxsize = 10f;
	public float explosion_rate = 1f;
	public float explosion_delay = 5f;
	public float cur_rad = 0f;

	public bool exploded = false;
	CircleCollider2D explosion_rad;

	void Start() 
	{
		explosion_rad = gameObject.GetComponent<CircleCollider2D>();
	}

	void Update () 
	{	
		explosion_delay -= Time.deltaTime;
		if(explosion_delay <= 0)
		{
			exploded = true;
		}

	}
	
	void fixedUpdate() 
	{
		if(exploded) {
			if(cur_rad < explosion_maxsize)
			{
				cur_rad += explosion_rate;
			}
			else 
			{
				Destroy(this.gameObject);
			}
		
			explosion_rad.radius = cur_rad;
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		Debug.Log(c);
		
		if (exploded == true)
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
