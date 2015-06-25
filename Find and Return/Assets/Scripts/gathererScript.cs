using UnityEngine;
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
	private GameObject nearBase;
	private GameObject target;


	Vector3 predatorLocation = Vector3.zero;
	private universalScripts u = universalScripts.getInstance();



	// Use this for initialization
	void Awake () {
		//move stuff
		unitType = "Sheep";
		harvestSpeed = 0.3f * u.multiX;
		senseDistance = 3;
		speed = 4f * u.multiX;
		maxWanderSteps = (int)(30/u.multiX);
		if (maxWanderSteps < 2)
			maxWanderSteps = 2;
		maxRunAwaySteps = (int)(150/u.multiX);

		wanderTarget = setNewCourse (transform.position);	//sets WanderTarget relative to where this object was instantiated	

		nearBase = findClosestObjectWithinX ("Base", senseDistance);
		target = findClosestObjectWithinX ("Food", senseDistance);

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
		speed = 6f* u.multiX;
		maxRunAwaySteps = (int)(100/u.multiX);		
			
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
		lengthOfPregnancy = 20f; 
		increasedConsumption = 2.2f; 
		target = findClosestObjectWithinX ("Food", senseDistance);

	}


	public void genderColor ()
	{

		Color32 pinkColor = new Color32(190,190,190,255);
		Color32 blueColor = new Color32(100,135,175,255);
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
		GameObject nearPredator = findClosestObjectWithinX("Predator", senseDistance);
		if ((nearPredator != null) && (getTargetDistance (nearPredator.transform.position) < senseDistance))
			return(nearPredator);
		else
			return(null);
	}





	private void dropMateResources (GameObject mate)
	{
		gathererScript gathererTarget = mate.GetComponent ("gathererScript") as gathererScript;
		gathererTarget.currentResources = .25f*gathererTarget.currentResources;
	
	}

	private void pregnantMate (GameObject mate)
	{
		gathererScript gathererTarget = mate.GetComponent ("gathererScript") as gathererScript;
		gathererTarget.currentResources = .25f*gathererTarget.currentResources;
		gathererTarget.isPregnant = true;
		
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
		currentResources = .25f*currentResources;
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
			GameObject tempMateTarget = findClosestMateWithinX ("Gatherer", senseDistance, isFemale);
			if (tempMateTarget != null) 
				coupleMates(tempMateTarget);
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

	private void incTargetCount(GameObject t)
	{
		foodBehavior fb;
		if (t != null) {
			fb = t.GetComponent ("foodBehavior")as foodBehavior;
			u.incGridEaters (fb.gridXPos, fb.gridZPos);
		}
	}

	private void decTargetCount(GameObject t)
	{
		foodBehavior fb;
		if (t != null) {
			fb = t.GetComponent ("foodBehavior")as foodBehavior;
			u.decGridEaters (fb.gridXPos, fb.gridZPos);
		}
	}

	private void getFood()
	{

		float distanceToFood = 0;

		if (target == null) {
			target = findClosestObjectWithinX ("Food", senseDistance);
			incTargetCount(target);
		}
		//if just finished wandering or running away, get a new target
		else if (travelFood) {	
			decTargetCount(target);
			target = findClosestObjectWithinX ("Food", senseDistance);
			incTargetCount(target);
			travelFood = false;
		}



		if (u.foodCount < 1)
			Application.Quit ();

		//if not hungry, then wander for a bit
		else if (isHungry == false) {
			isWandering = true;
			wander ();
		} 
		else 
		{
			//if too far from target
			if (target == null) 
			{
				isWandering = true;
				wander();
			}
			else
			{	//if close, eat
				distanceToFood = getTargetDistance(target.transform.position);
				if (distanceToFood < .25)
				{		
					eatObject(target);
					bounceObject();
				}
				//if not close, move
				else
					moveToTarget(target.transform.position);
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

	void Update () 
	{

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

			//add predator check and run away from predator
			GameObject predatorTarget = predatorCheck ();


			//BEHAVIOR PRIORITY: 1. run away from predator (if afraid), 2. wander(if wandering), 3. mate (if wantsToMate), 4. eat (if hungry), 5. initiate wander

			if (predatorTarget != null && afraidFlag == false) {
				//Debug.Log ("PREDATOR FOUND");
				afraidFlag = true;
				groundObject(.5f);
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
				matingBehavior ();
			else 
				getFood ();
		}
	}
}