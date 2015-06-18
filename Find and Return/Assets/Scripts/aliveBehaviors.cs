﻿using UnityEngine;
using System.Collections;

public class aliveBehaviors : MonoBehaviour {

	//current and maximum health - death if 0
	public float currentHealth;
	public float maximumHealth;

	//current and maximum age, and rate of aging, death at maximum age
	public float currentAge;
	public float maximumAge;
	public float rateOfAge;
	public float maturityAge;
	public bool isBaby;
	public bool isFemale;

	//for mating behaviors
	public bool wantsToMate = false;
	public bool hasMate = false;
	public bool isPregnant = false;
	//for mating behaviors
	public float lengthOfPregnancy; //set at lower level
	public float increasedConsumption; //set at lower level



	//current and maximum level of "resource energy" that the creature maintains for its health
	//if current goes below 0, creature begins to starve (takes HP damage)
	public float currentResources; 
	public float maximumResources; 
	public float resourceRequirement;
	public bool isStarving = false;
	public bool isHungry = false;
	public bool killFlag = false;

	//the amount of food that can be stored, above the creatures personal resources
	public float currentCapacity;
	public float maximumCapacity;
	private universalScripts u = universalScripts.getInstance();

	// Use this for initialization
	void Start () {

	}

	public void assignGender()
	{
		float genderFemale = Random.value;
		if (genderFemale < u.percentFemale)
			isFemale = true;
		else
			isFemale = false;
	}

	public void ageCreature(int factor){
		//age

			currentAge = currentAge + (rateOfAge*factor);
		if (currentAge > maximumAge) {
			killFlag = true;
			u.sheepAge++;
			u.updateCountText("Age");
		}

	}

	public bool hungryCheck(){
		if (currentResources  < (maximumResources*0.8f)) 
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
	}

	public void consumeResources(int factor)
	{
		//resource consumption
		currentResources -= (resourceRequirement*factor);
	}

	public void starvingCheck(){

		if (currentResources < 0) 
		{
			currentHealth = currentHealth -(maximumHealth / 20);
			isStarving = true;
		} 
		else
			isStarving = false;
	}

	public void healthCheck(){
		if (currentHealth <= 0) {
			killFlag = true;
			u.sheepStarve++;
			u.updateCountText("Starve");
		}

	}

	public void killUnit()
	{
		if (isBaby)
			u.babySheep--;
		else
			u.sheep--;

			u.updateCountText("Sheep");
			DestroyImmediate (gameObject);
	}


	public void LateUpdate () {
		if(killFlag)
			killUnit();

	}

}
