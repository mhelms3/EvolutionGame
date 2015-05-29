using UnityEngine;
using System.Collections;

public class aliveBehaviors : MonoBehaviour {

	//current and maximum health - death if 0
	public float currentHealth;
	public float maximumHealth;

	//current and maximum age, and rate of aging, death at maximum age
	public float currentAge;
	public float maximumAge;
	public float rateOfAge;

	//current and maximum level of "resource energy" that the creature maintains for its health
	//if current goes below 0, creature begins to starve (takes HP damage)
	public float currentResources; 
	public float maximumResources; 
	public float resourceRequirement;
	public bool isStarving = false;
	public bool isHungry = false;

	//the amount of food that can be stored, above the creatures personal resources
	public float currentCapacity;
	public float maximumCapacity;

	// Use this for initialization
	void Start () {

	}

	public void ageCreature(int factor){
		//age

			currentAge = currentAge + (rateOfAge*factor);
		if (currentAge > maximumAge) 
			currentHealth = 0;
	}

	public bool hungryCheck(){
		if (currentResources  < (maximumResources*0.6f)) 
			return(true);
		else
			return(false);
	}

	public void eatFood(){
		//eat
		isHungry = hungryCheck ();
		if (isHungry) {
			if (currentCapacity > 0) {
				float eatAmount = Mathf.Min (currentCapacity, (maximumResources - currentResources));			
				currentCapacity -= eatAmount;
				currentResources += eatAmount;
			} 
		}
		isHungry = hungryCheck ();
	}

	public void consumeResources(int factor)
	{
		//resource consumption
		currentResources -= (resourceRequirement*factor);
	}

	public void starvingCheck(){

		if (currentResources < 0) {
			currentHealth -= (maximumHealth / 20);
			isStarving = true;
		} else
			isStarving = false;
	}

	public void healthCheck(){
		if (currentHealth <= 0)
			Destroy (gameObject);
	}

	// Update is called once per frame
	public void Update () {

	/*	ageCreature();
		consumeResources();
		eatFood();
		hungryCheck ();
		starvingCheck ();
		healthCheck ();
		*/
	
	}
}
