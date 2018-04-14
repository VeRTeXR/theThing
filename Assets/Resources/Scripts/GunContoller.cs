using UnityEngine;
using System.Collections;

public class GunContoller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		//random type and gen gun object
		/*
		type: pistol, shotgun, smg, heavy 
		muzzle: pistol 1-4, shotgun 1-2, smg 1-4, heavy
		reciever: low dmg(pistol)
		stock: damp force feedback, reduce spread 

		*/
		//Instantiate();
	}

	void SpawnGun() {

		//Instantiate(gun, Origin.Transform);
		//gun.Transform.SetParent(obj);
	}
	
	void OnTriggerEnter2D(Collider2D c) 
	{
		if (c.CompareTag("Player")) {
			//set active e to pick up UI
			// this.setParent(c);
		} 
	}

}
