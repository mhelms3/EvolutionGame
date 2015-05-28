using UnityEngine;
using System.Collections;

public class rotatePill : MonoBehaviour {

	public Transform pillTransform;
	
	// Update is called once per frame
	void Update () {
		//rotate z, x, y
		pillTransform.Rotate(1f, 0.0f, 0.0f);
		
	}
}