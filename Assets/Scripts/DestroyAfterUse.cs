using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterUse : MonoBehaviour {
	public float destroyTime = 0.5f;

	void Start()
	{
		Invoke ("destroySelf", destroyTime);



	}

	private void destroySelf ()
	{
		Destroy (gameObject);
	}
		
}