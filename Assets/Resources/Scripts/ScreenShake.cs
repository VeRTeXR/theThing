using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour 
{
	
	Vector3 originalCameraPosition;
	
	public float shakeAmt;
	Camera mainCamera;

	void Start () {
		mainCamera = Camera.main;
	}

	void Update () {
		originalCameraPosition = mainCamera.transform.position;
	}

	void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.CompareTag ("enemyBullet")) {
			InvokeRepeating ("CameraShake", 0, .15f);
			Invoke ("StopShaking", 0.2f);
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "enemyBullet") {
			InvokeRepeating ("CameraShake", 0, .15f);
			Invoke ("StopShaking", 0.2f);
		}
		if (c.gameObject.tag == "Enemy") {
			InvokeRepeating ("CameraShake", 0, .15f);
			Invoke ("StopShaking", 0.2f);
		}
	}

	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = shakeAmt*2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;
			pp.x+= quakeAmt;
			pp.y+= quakeAmt/2;// can also add to x and/or z
			mainCamera.transform.position = pp;
		}
	}
	
	void StopShaking()
	{
		CancelInvoke("CameraShake");
		mainCamera.transform.position = originalCameraPosition;
	}
	
}
