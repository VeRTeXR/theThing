using UnityEngine;
using System.Collections;

    

public class Billboard : MonoBehaviour {


	public Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	
	}
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
	//transform.LookAt(Vector3.zero);
	transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));
	animator.SetFloat("vertSpeed", Mathf.Abs(input.y));
	animator.SetFloat("speed", Mathf.Abs(input.x));

	if(input.x > 0){
        transform.eulerAngles = new Vector3 (0,180,0);
        //transform.rotation = Quaternion.identity;
	}
    
    if(input.x < 0) {
    	transform.eulerAngles = new Vector3 (0,0,0);
    }

    if(Input.GetAxisRaw("Vertical") > 0){
        
        transform.eulerAngles = new Vector3 (0,180,0);
	}
	if(Input.GetAxisRaw("Vertical") < 0){
        
        transform.eulerAngles = new Vector3 (0,0,0);
	}

	}
}
