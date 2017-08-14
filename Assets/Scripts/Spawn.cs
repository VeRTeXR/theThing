using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public GameObject player;
	public GameObject spawn;

	// Use this for initialization
	void Start () {
	
		if(GameObject.FindGameObjectWithTag("Player")==null)
		{		//spawning here what the fuck
			Debug.Log ("shit");
			Instantiate(player, transform.position, transform.rotation);
		}
		else 
		{
			player = GameObject.FindWithTag("Player");
			player.transform.position = transform.position;
		}

	}
	
}
