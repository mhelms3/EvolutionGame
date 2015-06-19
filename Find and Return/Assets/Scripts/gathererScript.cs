﻿using UnityEngine;
using System.Collections;
//using movementScripts;

public class gathererScript : moveBehaviors {

	public GameObject AdultWorker;    
	public GameObject BabySheep;    
	private bool eatFlag = false;
	private bool returnFlag = false;
	private foodBehavior eatTarget;
	private baseBehavior baseTarget;
	private float harvestSpeed;
	private int tick = 0;
	//private GameObject mateTarget;
	private GameObject nearBase;
	private GameObject target;




	Vector3 predatorLocation = Vector3.zero;
	private universalScripts u = universalScripts.getInstance();



	// Use this for initialization
	void Awake () {
		//move stuff
		unitType = "Sheep";
		harvestSpeed = 0.1f * u.multiX;
		senseDistance = 3;
		speed = 4f * u.multiX;
		maxWanderSteps = (int)(30/u.multiX);
		if (maxWanderSteps < 2)
			maxWanderSteps = 2;
		maxRunAwaySteps = 300;

		wanderTarget = setNewCourse (transform.position);	//sets WanderTarget relative to where this object was instantiated	

		nearBase = findClosestObject("Base");
		target = findClosestObject ("Food");

		//alive stuff
		maximumHealth = 5;
		currentHealth = 5;
		currentAge = 0;
		maximumAge = 25;
		rateOfAge = .01f* u.multiX;
		currentResources = 2; 
		maximumResources = 12; 
		resourceRequirement = .02f * u.multiX;
		//resourceRequirement = 2f;
		currentCapacity = 0;
		maximumCapacity = 4;

		//childhood behavior
		isBaby = true;
		maturityAge = 4;

		//mating behavior
		lengthOfPregnancy = 999f; 
		increasedConsumption = 999f; 

	

	}

	public void setAdult()
	{

		senseDistance = 6;
		speed = 8f* u.multiX;
		maxRunAwaySteps = 200;		
			
		//alive stuff
		maximumHealth = 20;
		currentHealth = 20;
		currentResources = 12; 
		maximumResources = 24; 
		//resourceRequirement = .4f;
		resourceRequirement = .06f* u.multiX;
		currentCapacity = 0;
		maximumCapacity = 6;
		isBaby = false;
		maturityAge = 999;
		//mating behavior
		lengthOfPregnancy = 90f; 
		increasedConsumption = 2.2f; 

	}


	public void genderColor ()
	{

		Color32 pinkColor = new Color32(255,125,125,255);
		Color32 blueColor = new Color32(0,75,255,255);
		Renderer rend = gameObject.GetComponent<Renderer>();
		if(isFemale)
			rend.material.color = pinkColor;
		else
			rend.material.color = blueColor;
	}


	private void makeAdult ()
	{
		Vector3 startingPosition = transform.position;  
		bool tempGender = isFemale;
		GameObject newAdultWorker = (GameObject) Instantiate (AdultWorker, startingPosition, Quaternion.identity);
		//Instantiate (AdultWorker, startingPosition, Quaternion.identity);
		gathererScript gsWorker = newAdultWorker.GetComponent ("gathererScript") as gathererScript;
		gsWorker.setAdult ();
		gsWorker.isFemale = tempGender;
		gsWorker.genderColor();
		u.sheep++;
		u.sheepMature++;
		u.updateCountText ("Sheep");
		u.updateCountText ("Mature");
		//currentHealth = -99f;
		//healthCheck ();
		killFlag = true;
	}
		

	private void eatObject (GameObject food)
	{		
		eatTarget = food.GetComponent("foodBehavior")as foodBehavior;
		eatTarget.foodValue -= harvestSpeed;
		currentCapacity += harvestSpeed;
		if (eatTarget.foodValue < 1) 
		{
			eatTarget.killFlag = true;
		}
		//else
			//eatTarget.changeColor ();
	}

	private GameObject predatorCheck()
	{
		GameObject nearPredator = findClosestObject("Predator");
		if ((nearPredator != null) && (getTargetDistance (nearPredator.transform.position) < senseDistance))
			return(nearPredator);
		else
			return(null);
	}

	private void mateCheck()
	{

		if ((currentHealth > .8 * maximumHealth) && (currentResources > .8 * maximumResources) && (!isBaby))
			wantsToMate = true;

		if ((currentHealth < .6 * maximumHealth) || (currentResources < .4 * maximumResources))
			wantsToMate = false;

	}



	private void dropMateResources (GameObject mate)
	{
		gathererScript gathererTarget = mate.GetComponent ("gathererScript") as gathererScript;
		gathererTarget.currentResources = gathererTarget.currentResources - 12;
	
	}

	private void spawnBaby ()
	{
		Vector3 momPosition = transform.position;
		//GameObject newAdultWorker = (GameObject) Instantiate (AdultWorker, startingPosition, Quaternion.identity);
		GameObject newBabySheep = (GameObject)Instantiate(BabySheep, momPosition, Quaternion.identity);
		gathererScript gsWorker = newBabySheep.GetComponent ("gathererScript") as gathererScript;
		gsWorker.assignGender();
		gsWorker.genderColor ();
		u.babySheep++;
		u.updateCountText("Sheep");

		dropMateResources(mateTarget);
		currentResources = currentResources - 12;
		decoupleMates(mateTarget);

	}

	private void matingBehavior ()
	{
		float distanceToMate = 0;
		if (hasMate)
		{
			if (mateTarget == null)
			{
				Debug.Log ("ERROR: hasMate true, mateTarget == null)");
				getFood();
			}
			else
			{
				distanceToMate = getTargetDistance (mateTarget.transform.position);
				if (distanceToMate < .25) 
				{		
					spawnBaby();
				} 
				else 			
					moveToTarget (mateTarget.transform.position);
				
			}
		}
		
		
		else if (mateTarget != null) 
		{
			Debug.Log ("ERROR: hasMate False, but mateTarget not null");
			hasMate = true;
			getFood ();
		}
		else
		{
			GameObject tempMateTarget = findClosestMate ("Gatherer", isFemale);
			if (tempMateTarget != null) 
			{
				distanceToMate = getTargetDistance (tempMateTarget.transform.position);
				if (distanceToMate < senseDistance)
					coupleMates(tempMateTarget);
				else
					getFood ();
			}
			else
				getFood ();
		}
	
	}


	private void gotoBase()
	{
		if (nearBase==null)
			nearBase = findClosestObject("Base");
		else
		{
			distanceToTarget = getTargetDistance(nearBase.transform.position);
			if (distanceToTarget < .5)
			{
				baseTarget = nearBase.GetComponent("baseBehavior")as baseBehavior;
				baseTarget.resourceFood += currentCapacity;
				currentCapacity = 0;
				returnFlag = false;
			}
			else if (distanceToTarget > senseDistance)
			{
				nearBase = findClosestObject("Base");
				moveToTarget(nearBase.transform.position);
			}
			else
				moveToTarget(nearBase.transform.position);
		}
	}


	private void getFood()
	{

		float distanceToFood = 0;
		if (u.foodCount < 1)
			Application.Quit ();

		//if not hungry, then wander for a bit
		else if (isHungry == false) 
		{
			isWandering = true;
			wander ();
		}
		//if no food target, then get one
		else if (target == null)
			target = findClosestObject("Food");
		
		else 
		{
			//if just finished wandering, find a nearby food target
			if (wanderFood = true)
			{
				target = findClosestObject("Food");
				wanderFood = false;
			}

			distanceToFood = getTargetDistance(target.transform.position);
			if (distanceToFood < .25)
			{		
				eatObject(target);
				bounceObject();
			}
			else
			{
				if(distanceToFood < senseDistance)
					moveToTarget(target.transform.position);
				else
				{
					target = findClosestObject("Food");
					isWandering = true;
					wander(); 

				}
			}
			/*
			if (currentCapacity >= maximumCapacity)
			{
				returnFlag = true;
				groundObject();
			}
			*/
		}

	}

	void Update () 
	{


		ageCreature (1);

		if ((currentAge > maturityAge) && (isBaby) && (!killFlag)) {
			makeAdult ();
		}

		consumeResources (1);
		eatFood ();
		hungryCheck ();
		starvingCheck ();
		healthCheck ();
		mateCheck ();

		//add predator check and run away from predator
		GameObject predatorTarget = predatorCheck ();


		//BEHAVIOR PRIORITY: 1. run away from predator (if afraid), 2. wander(if wandering), 3. mate (if wantsToMate), 4. eat (if hungry), 5. initiate wander

		if (predatorTarget != null && afraidFlag==false) 
		{
			//Debug.Log ("PREDATOR FOUND");
			afraidFlag = true;
			predatorLocation = predatorTarget.transform.position;
		}

		if (afraidFlag) {
			//Debug.Log ("PLoc x,z:" + predatorLocation.x + "," + predatorLocation.z);
			runAway (predatorLocation);
		}


		/*
		else if (returnFlag)
			gotoBase();
		*/
		else if (isWandering)
			wander ();
		else if (wantsToMate) 
			matingBehavior();
		else 
			getFood ();
	}
}