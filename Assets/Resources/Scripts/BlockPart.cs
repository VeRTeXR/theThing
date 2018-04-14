using UnityEngine;
using System.Collections;

public class BlockPart : MonoBehaviour {

	public float delay;

	void Update () {
		delay += Time.deltaTime;
		if(delay >= 2.5)
		{
			Destroy(gameObject);
		}
	}
}
