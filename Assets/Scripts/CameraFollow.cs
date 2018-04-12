using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

		public float DampTime = 0.15f;
		private Vector3 _velocity = Vector3.zero;
		public Transform Target;
		// Update is called once per frame
		void Update ()
		{
			Target = GameObject.FindWithTag("Player").transform;
			if (Target)
			{
				Vector3 point = GetComponent<Camera>().WorldToViewportPoint(Target.position);
				Vector3 delta = Target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
				Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, DampTime);
			}
		}

}
