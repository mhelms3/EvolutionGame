using UnityEngine;
using System.Collections;

public class createGatherer : MonoBehaviour {

	// Use this for initialization
	
	public GameObject SphereThing;
	// Use this for initialization
	void Start () {
		
		Vector3 startingPosition = new Vector3(1,1,1);
		Debug.Log("Here");
		Instantiate(SphereThing);
		Instantiate(SphereThing, startingPosition, Quaternion.identity);
		
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
