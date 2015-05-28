using UnityEngine;
using System.Collections;

public class baseBehavior : MonoBehaviour {

	public float resourceFood;
	public float newGathererAmount;
	public float foodIncrement;
	public GameObject Worker;

	// Use this for initialization
	void Start () {
	
		resourceFood = 0;
		if (newGathererAmount == 0)
			newGathererAmount = 10;
	}

	private void spawnNewGatherer(){
		Vector3 newStartingPosition = new Vector3 (Random.value * 5, 0.5f, Random.value * 5);
		Instantiate(Worker, newStartingPosition, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		resourceFood += foodIncrement;

		if (resourceFood > newGathererAmount) 
		{
			spawnNewGatherer();
			resourceFood -= newGathererAmount;
		}
	}
}
