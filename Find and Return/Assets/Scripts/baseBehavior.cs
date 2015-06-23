using UnityEngine;
using System.Collections;

public class baseBehavior : MonoBehaviour {

	public float resourceFood;
	public float newGathererAmount;
	public float foodIncrement;
	//public GameObject BabySheep;
	private universalScripts u = universalScripts.getInstance();

	// Use this for initialization
	void Start () {
	
		resourceFood = 0;
		if (newGathererAmount == 0)
			newGathererAmount = 10;
	}

	private void spawnNewGatherer(){
/*		Vector3 newStartingPosition = new Vector3 (Random.value * 5, 0.5f, Random.value * 5);
		GameObject newGatherer = (GameObject)Instantiate(BabySheep, newStartingPosition, Quaternion.identity);
		gathererScript gsWorker = newGatherer.GetComponent ("gathererScript") as gathererScript;
		gsWorker.assignGender();
		gsWorker.genderColor ();
		u.babySheep++;
		u.updateCountText("Sheep");*/
	}

	// Update is called once per frame
	void Update () {
		if (!u.isPaused) {
			resourceFood += foodIncrement;
			if (resourceFood > newGathererAmount) {
				//spawnNewGatherer();
				resourceFood -= newGathererAmount;
			}
		}
	}
}
