using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float lifeTime = 3f;
	public float speed = 10.0f; 
	public int dmg = 1;
	public bool bounceable;

	void OnEnable() 
		{	
			//Physics.IgnoreCollision(this.collider, collider);
			GetComponent <Rigidbody2D> ().velocity = transform.up.normalized * speed;
			Invoke ("Destroy", lifeTime);
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
		c.gameObject.GetComponent<Rigidbody2D>().AddForce (bulletdir.normalized * knockbackForce);
		if (bounceable)
		{
			gameObject.GetComponent<Rigidbody2D>().AddForce(bulletdir.normalized);
		}
	}

}



