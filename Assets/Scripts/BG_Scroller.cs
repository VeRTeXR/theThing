using UnityEngine;
using System.Collections;

public class BG_Scroller : MonoBehaviour {


	public float scrollSpeed = 1.0f;
	public float tileSizeZ = 1.0f;

	private Vector3 startPosition;

	void Start () {
		startPosition = transform.position;
	}
	
	void Update () {
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
		//transform.position = startPosition + Vector3.right * newPosition;
		if(transform.position.z == tileSizeZ)
		{
			Debug.Log("hit");
			transform.position = startPosition + Vector3.left * newPosition;
		}
		else
		{
			transform.position = startPosition + Vector3.right * newPosition;
		}
	}
}
