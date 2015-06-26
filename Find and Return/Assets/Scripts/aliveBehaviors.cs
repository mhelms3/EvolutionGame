using UnityEngine;
using System.Collections;

public class aliveBehaviors : MonoBehaviour {

	public string unitType;

	//current and maximum health - death if 0
	public float currentHealth;
	public float maximumHealth;
	public float percentFemale;

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
	public GameObject mateTarget;

	//for mating behaviors
	public float lengthOfPregnancy; 
	public float currentPregnancy =0;
	public int  numberOfChildren =1; 
	public float increasedConsumption;



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

	public void mateCheck()
	{
		
		if ((currentHealth > .8 * maximumHealth) && (currentResources > .3 * maximumResources) && (!isBaby) && (!isPregnant))
			wantsToMate = true;
		
		if ((currentHealth < .6 * maximumHealth) || (currentResources < .4 * maximumResources))
			wantsToMate = false;
		
	}



	public void assignGender(float percentFemale)
	{
		float genderFemale = Random.value;
		if (genderFemale < percentFemale)
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
		if (currentResources  < (maximumResources*0.65f)) 
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

		if (currentResources < 0) 
		{
			currentHealth = currentHealth -(maximumHealth / 40);
			isStarving = true;
		} 
		else
			isStarving = false;
	}

	public void healthCheck(){
		if (currentHealth <= 0) {
			killFlag = true;
			u.sheepStarve++;
			u.updateCountText ("Starve");
		} else if ((currentHealth < maximumHealth) && (!isStarving))
			currentHealth += maximumHealth * .05f;


	}

	public void coupleMates(GameObject mate)
	{
		hasMate = true;
		mateTarget = mate;
		aliveBehaviors aliveTarget = mate.GetComponent ("aliveBehaviors") as aliveBehaviors;
		aliveTarget.hasMate = true;
		aliveTarget.mateTarget = gameObject;
	}
	
	public void decoupleMates (GameObject mate)
	{
		hasMate = false;
		wantsToMate = false;
		mateTarget = null;
		aliveBehaviors aliveTarget = mate.GetComponent ("aliveBehaviors") as aliveBehaviors;
		aliveTarget.hasMate = false;
		aliveTarget.mateTarget = null;
		aliveTarget.wantsToMate = false;
	}

	public void killUnit()
	{
		if (hasMate)
			decoupleMates (mateTarget);

		if (unitType == "Sheep") {
			if (isBaby)
				u.babySheep--;
			else
				u.sheep--;
			u.updateCountText ("Sheep");
			DestroyImmediate (gameObject);
		} else if (unitType == "Wolf") {
			u.wolves--;
			u.updateCountText ("Wolf");
			DestroyImmediate (gameObject);
		}
			
	}


	public void LateUpdate () {
		if(killFlag)
			killUnit();

	}

}
