using UnityEngine;
using System.Collections;

public class predatorBehavior : moveBehaviors {

	public GameObject AdultPredator;    
	public GameObject BabyPredator;   
	private bool chaseFlag = false;
	public GameObject chaseTarget;
	Vector3 rivalLocation = Vector3.zero;
	private universalScripts u = universalScripts.getInstance();

	// Use this for initialization
	void Awake () {
		percentFemale = .60f;
		unitType = "Wolf";
		senseDistance = 7;
		speed = 6.5f * u.multiX;

		maxWanderSteps = (int)(30/u.multiX);
		if (maxWanderSteps < 2)
			maxWanderSteps = 2;
		maxRunAwaySteps = 300;
		
		wanderTarget = setNewCourse (transform.position);	//sets WanderTarget relative to where this object was instantiated	
		chaseTarget = findClosestObjectWithinX ("Gatherer", senseDistance);
		
		//alive stuff
		maximumHealth = 15;
		currentHealth = 5;
		currentAge = 0;
		maximumAge = 35;
		rateOfAge = .01f* u.multiX;
		currentResources = 15; 
		maximumResources = 15; 
		resourceRequirement = .06f * u.multiX;
		//resourceRequirement = 2f;
		currentCapacity = 0;
		maximumCapacity = 25;
		
		//childhood behavior
		isBaby = true;
		maturityAge = 7;
		
		//mating behavior
		lengthOfPregnancy = 999f; 
		increasedConsumption = 999f; 
	}

	public void setAdult()
	{
		
		senseDistance = 7;
		speed = 8f* u.multiX;
		maxRunAwaySteps = 200;		
		
		//alive stuff
		maximumHealth = 40;
		currentHealth = 40;
		currentResources = 25; 
		maximumResources = 25; 
		//resourceRequirement = .4f;
		resourceRequirement = .06f* u.multiX;
		currentCapacity = 0;
		maximumCapacity = 50;
		isBaby = false;
		maturityAge = 999;
		//mating behavior
		lengthOfPregnancy = 20f; 
		increasedConsumption = 2.2f; 
		chaseTarget = findClosestObjectWithinX ("Gatherer", senseDistance);
		
	}

	public void genderColor ()
	{
		
		Color32 redColor = new Color32(81,75,22,255);
		Color32 blackColor = new Color32(47,34,6,255);
		Renderer rend = gameObject.GetComponent<Renderer>();
		if(isFemale)
			rend.material.color = redColor;
		else
			rend.material.color = blackColor;

	}

	private void makeAdult ()
	{
		Vector3 startingPosition = transform.position;  
		bool tempGender = isFemale;
		GameObject newAdultPredator = (GameObject) Instantiate (AdultPredator, startingPosition, Quaternion.identity);
		//Instantiate (AdultWorker, startingPosition, Quaternion.identity);
		predatorBehavior predB = newAdultPredator.GetComponent ("predatorBehavior") as predatorBehavior;
		predB.setAdult ();
		predB.isFemale = tempGender;
		predB.genderColor();
		
		u.wolves++;
		//u.predatorMature++;
		u.updateCountText ("Wolf");
		//u.updateCountText ("Mature");
		//currentHealth = -99f;
		//healthCheck ();
		killFlag = true;
	}

	private void eatObject ()
	{
		gathererScript gs = chaseTarget.GetComponent ("gathererScript") as gathererScript;
		gs.killFlag = true;
		if (gs.isBaby == true)
			currentCapacity += (10 + gs.currentCapacity + gs.currentResources)*2;
		else
			currentCapacity += (20 + gs.currentCapacity + gs.currentResources)*2;
		if (currentCapacity > maximumCapacity)
			currentCapacity = maximumCapacity;
	}

	private void getFood()
	{
		float distanceToFood = 0;
		
		if (chaseTarget == null) {
			chaseTarget = findClosestObjectWithinX ("Gatherer", senseDistance);
			if(chaseTarget == null)
			{
				isWandering = true;
				wander();
			}

		}
		//if just finished wandering or running away, get a new target
		else if (travelFood) {	
			chaseTarget = findClosestObjectWithinX ("Gatherer", senseDistance);
			travelFood = false;
		}
		else if (isHungry == false) {
			isWandering = true;
			wander ();
		} 
		else if (isHungry == true)
		{
			//if too far from target
			if (chaseTarget == null) 
			{
				isWandering = true;
				wander();
			}
			else
			{	//if close, eat
				distanceToFood = getTargetDistance(chaseTarget.transform.position);
				if (distanceToFood < .25)
				{		
					eatObject();
					//bounceObject();
				}
				//if not close, move
				else
					moveToTarget(chaseTarget.transform.position);
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
	private void dropMateResources (GameObject mate)
	{
		predatorBehavior predTarget = mate.GetComponent ("predatorBehavior") as predatorBehavior;
		predTarget.currentResources = .25f*predTarget.currentResources;
		
	}


	private void spawnBaby ()
	{
		Vector3 momPosition = transform.position;
		//GameObject newAdultWorker = (GameObject) Instantiate (AdultWorker, startingPosition, Quaternion.identity);
		GameObject newBabyWolf = (GameObject)Instantiate(BabyPredator, momPosition, Quaternion.identity);
		predatorBehavior predB = newBabyWolf.GetComponent ("predatorBehavior") as predatorBehavior;
		predB.assignGender(predB.percentFemale);
		predB.genderColor();
		u.wolves++;
		u.updateCountText("Wolf");
		currentResources = .25f*currentResources;


	}

	private void pregnantMate (GameObject mate)
	{
		predatorBehavior predTarget = mate.GetComponent ("predatorBehavior") as predatorBehavior;
		predTarget.currentResources = .25f*predTarget.currentResources;
		predTarget.isPregnant = true;
	}


	public void matingBehavior ()
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
					if(isFemale)					
						isPregnant = true;
					else
						pregnantMate(mateTarget);
					
					dropMateResources(mateTarget);
					currentResources = .25f*currentResources;
					decoupleMates(mateTarget);
					//spawnBaby();
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
			GameObject tempMateTarget = findClosestMateWithinX ("Predator", senseDistance*6, isFemale);
			if (tempMateTarget != null) 
				coupleMates(tempMateTarget);
			else
				getFood ();
		}
		
	}

	private void pregCheck()
	{
		if (isPregnant)
			currentPregnancy++;
		if (currentPregnancy > lengthOfPregnancy) {
			spawnBaby ();
			isPregnant = false;
			currentPregnancy = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!u.isPaused) {
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
			pregCheck ();
	

			if (isWandering)
				wander ();
			else if (wantsToMate) 
				matingBehavior ();
			else 
				getFood ();


		}
	}
}
