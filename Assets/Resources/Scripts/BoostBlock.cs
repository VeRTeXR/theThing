﻿using UnityEngine;
using System.Collections;


public class BoostBlock : MonoBehaviour {
	public float boostSpeed = 50;
	private Player player;
	
	void OnCollisionEnter2D(Collision2D c) {
		if(c.gameObject.CompareTag("Player")) 
		{
			player = c.gameObject.GetComponent<Player>();
			player.OnBoostBlock = true;
			player.BoostSpeed = boostSpeed;
		}
	}
}