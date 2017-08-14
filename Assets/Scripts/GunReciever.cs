using UnityEngine;
using System.Collections;

public class GunReciever : MonoBehaviour {

//	public String tyoe;
	public int clipSize;
	public float force; // this should calculate how fast the bullet gonna go && knock back force 
	public int dmg;
	public float firerate;
	

	// Use this for initialization
	void Start () {
		//switch (type){
		// random shit for each type;
		//}
		//clipSize = Random.Range(10,2); // check for type (shotgun, smg, pistol, heavy) 
		//dmg 
	}
	
	// Update is called once per frame
	void Update () {
	
		if(clipSize == 0) {
			//can shoot no more
		}
		
	}
}
