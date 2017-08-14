using UnityEngine;
using System.Collections;

public class Grabbing : MonoBehaviour {


	public bool grabbing;
	public float pickupDistance = 2.0f;
	public Transform Hold;
	RaycastHit2D hit;
	public float throwForce;
	public LayerMask notgrabbing;
	public int raycastTargetLeftOrRight;
	public Rigidbody2D rb;

	void Start() {
		rb = GetComponentInParent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		if(transform.rotation.y > 0)
		{
			raycastTargetLeftOrRight = 1;
		}
		else
		{
			raycastTargetLeftOrRight = -1;
		}

		if(grabbing) 
		{
			hit.collider.gameObject.transform.position = Hold.transform.position;
		}


		if (Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("ddd");
			// Interact
			/*
			Bring up the Dialogue, Pick up guns, Trigger etc...
			*/
			

			if(!grabbing) 
			{
				Physics2D.queriesStartInColliders = false;
				hit = Physics2D.Raycast(transform.position, Vector2.right*raycastTargetLeftOrRight, pickupDistance);
				if(hit.collider!= null && hit.collider.CompareTag ("Gun")) 
				{
					grabbing = true;
				}
			}
				else if(Physics2D.OverlapPoint(Hold.position)) 
				{
					Debug.Log("thr");
					grabbing = false;
					if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
					{
						Debug.Log("add f");
						hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (raycastTargetLeftOrRight,1)*throwForce;
					}
				}	
			}
		}
		void OnDrawGizmos () {
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position+Vector3.right*raycastTargetLeftOrRight*pickupDistance);
		}

	}

