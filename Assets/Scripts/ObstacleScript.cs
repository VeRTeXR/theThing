using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {

	public int hp; 
	public int totalParts;
	public GameObject bodyPart;
	public bool destructible;
	public Animator anim;

	void Start() {
		anim = GetComponent<Animator>(); 
	}

	void Update () {
		if(hp == 0) {
			Explode();
		}
	}

	void Explode() {
		//anim.SetTrigger("dead");
		//anim ends destroy obj
		Destroy(gameObject); // this.gameobject.setactive(false);
		if(destructible){
        	for (int i = 0; i < totalParts; i++){
            	GameObject b = Instantiate(bodyPart, transform.position, Quaternion.identity) as GameObject;
            	b.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(-40, 40));
            	b.GetComponent<Rigidbody2D>().AddForce(Vector3.up * Random.Range(-60, 60));
        	}
    	}	
    	else {
    		
    	}

	}

	void OnCollisionEnter2D(Collision2D c) {
		if(destructible){
			if(c.gameObject.CompareTag("PlayerBullet")) {
				hp = hp-1;
				//anim.SetTrigger("isAtked");
				c.gameObject.SetActive(false);
			}
		}
		else {
			if(c.gameObject.CompareTag("PlayerBullet")) {
				c.gameObject.SetActive(false);
			}
		}
	}
}
