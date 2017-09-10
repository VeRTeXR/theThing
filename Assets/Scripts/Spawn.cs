using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public GameObject player;
	public GameObject spawn;

	void Start () 
	{
		if(!GameObject.FindGameObjectWithTag("Player"))
			Instantiate(player, transform.position, transform.rotation);
		else 
		{
			player = GameObject.FindWithTag("Player");
			player.transform.position = transform.position;
		}
	}
}
