﻿using UnityEngine;
using System.Collections;


public class BoostBlock : MonoBehaviour {
	public float boostSpeed = 50;
	Player player;
	float cd;

	void Update () 
	{
//		Debug.Log("c"+cd);
	}
	
	void OnCollisionEnter2D(Collision2D c) {
		if(c.gameObject.CompareTag("Player")) 
		{
			Debug.Log("cuck");
			player = c.gameObject.GetComponent<Player>();
			player.OnBoostBlock = true;
			player.BoostSpeed = boostSpeed;
		}
		else
		{
			///fuck
		}
	}
}