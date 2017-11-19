using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachFromParent : MonoBehaviour {

	public void DetachFromParentTransform()
	{
		// Detaches the transform from its parent.
		transform.parent = null;
	}
}