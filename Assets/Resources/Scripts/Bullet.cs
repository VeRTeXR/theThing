using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float LifeTime = 3f;
	public float Speed = 10.0f; 
	public int Dmg = 10;
	public bool Bounceable;

	void OnEnable() 
		{	
			GetComponent <Rigidbody2D> ().velocity = transform.up.normalized * Speed;
			Invoke ("Destroy", LifeTime);
		}

	void OnDisable () {
		CancelInvoke();
	}

	void Destroy () {
		gameObject.SetActive(false);
	}

	void OnTriggerEnter2D (Collider2D c) {
			Vector3 bulletdir = gameObject.transform.forward;
			bulletdir.y = 0;
			float knockbackForce = 1000; //knockback
			c.gameObject.GetComponent<Rigidbody2D>().AddForce(bulletdir.normalized * knockbackForce);
			if (Bounceable)
			{
				gameObject.GetComponent<Rigidbody2D>().AddForce(bulletdir.normalized);
			}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		return;
		
	}
}



