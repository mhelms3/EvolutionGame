using UnityEngine;
using System.Collections;

public class predatorBehavior : moveBehaviors {


	private bool chaseFlag = false;
	private GameObject chaseTarget;



	// Use this for initialization

	void Start () {

		int senseDistance;
		float speed = 6f;
		int maxWanderSteps = 40;
		Vector3 wanderTarget = new Vector3(20f,1f,20f);
		maxRunAwaySteps = 5;
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
