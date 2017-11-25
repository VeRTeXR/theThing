using UnityEngine;
using System.Collections;

public class BombBlock : MonoBehaviour {

	public int hp = 2; 
	public int totalParts;
	public GameObject bodyPart;


	void Update () 
	{	
		if(hp == 0)
		{
			Explode();
		}

	}


	void OnCollisionEnter2D(Collision2D c)
	{
//		Debug.Log(c);
		if (c.gameObject.CompareTag("Player")) 
		{
			hp = hp - 1;

			//Player.hp = Player.hp-1;

		}
		if (c.gameObject.CompareTag("PlayerBullet")){
			hp = hp - 1;
			c.gameObject.SetActive(false);
		}
		else 
		{
			//being normal meibe????
			//unless that obj is scrap then make it reactive
			
		}

	}


    void Explode()
    {
		Destroy(gameObject);
        for (int i = 0; i < totalParts; i++)
        {
            GameObject b = Instantiate(bodyPart, transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(-40, 40));
            b.GetComponent<Rigidbody2D>().AddForce(Vector3.up * Random.Range(-60, 60));
        }

    }
}


