using UnityEngine;
using System.Collections;

public class PlayerAim : MonoBehaviour {

	//refactor into muzzle object
	public Controller2D controller;
	public Rigidbody2D rb;
	public float force; 
	public float shootcd; // tracking rate of fire
	public float barrelCooldownTime;
	public float firerate; // tracking rate of fire
	public GameObject [] bulletSpawn; // adding predefined transform into the list
	public int bulletSpawnSize; // get bulletSpawn[]list.Length; send this value to controller for multiple activation?? 
	public int clipSize = 10; // get value from another mag obj.

	public bool overheatAble;
	public bool isOverheat;
	public float heatThreadshold; // this is the limit the gun can take
	public float heatCount; // tracking heat

	public bool jammedAble;
	public bool isJammed = false;
	public float jammedThreadshold; //max time of cd
	public float jammedCooldown; 
	public int jammedIndex; // tweak this for jam rate on guns

	
	IEnumerator attack () // move this shit to each gun nozzle script?
	{
		//sending bulletspawnlist transform instead of a transform.origin, so multiple spread muzzle can be implemented
		//GunReciever.clipSize--;
		//heatCount++; 
		for(int i = 0; i < bulletSpawnSize; i++) 
		{
			controller.Shot(bulletSpawn[i].GetComponent<Transform>());
		}
		yield return new WaitForSeconds (1);
		StopCoroutine("attack");

	}

	void Start() 
	{
		//clipSize = GameObject.FindObjectOfType<GunReciever>().clipSize; //get clipsize from obj
		jammedCooldown = jammedThreadshold;
		controller = GetComponentInParent<Controller2D>();
		rb = GetComponentInParent<Rigidbody2D>();
	}

	void Update () 
	{
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		var mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Quaternion roit = Quaternion.LookRotation (transform.position - mousePosition, Vector3.forward);
		transform.rotation = roit;
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
		//Debug.Log("eA:"+transform.eulerAngles);
		shootcd += Time.deltaTime;

		if(isJammed)
		{
			shootcd = 0;
			Debug.Log("jammed");
			// if it jammed able then it should also have an upside (decision making for the Player)
			jammedCooldown -= Time.deltaTime;
			if(jammedCooldown <= 0)
			{
				isJammed = false;
				shootcd = 0;
				jammedCooldown = jammedThreadshold;
			}
		}

		if(isOverheat && overheatAble) 
		{
			shootcd = 0;

		}

		if(Input.GetMouseButton(0)) 
		{
			
			if(clipSize > 0)
			{
				if(shootcd > firerate)
				{
					StartCoroutine ("attack");
					Vector3 dir = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.up)*Vector3.one;
					//Debug.Log("dir:"+dir);
					transform.root.transform.Translate(dir * force);
					 // this feedback something smh
					shootcd = 0;
					jammedIndex = Random.Range(0,100); // rng god pls help us
					clipSize--; //decrease bullet
					if(jammedIndex > 92 && jammedAble) 
					{
						isJammed = true;
					}
				}
			}
		}
	}

}
