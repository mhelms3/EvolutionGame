using UnityEngine;
using System.Collections;
//using movementScripts;

public class gathererScript : moveBehaviors {

	private bool eatFlag = false;
	private bool returnFlag = false;
	private foodBehavior eatTarget;
	private baseBehavior baseTarget;
	private float harvestSpeed = 0.04f;
	Vector3 predatorLocation = Vector3.zero;

	// Use this for initialization
	void Start () {
		//move stuff
		senseDistance = 4;
		speed = 5f;
		maxWanderSteps = 30;
		maxRunAwaySteps = 100;

		wanderTarget = setNewCourse (transform.position);	//sets WanderTarget relative to where this object was instantiated	

		//alive stuff
		maximumHealth = 10;
		currentHealth = 10;
		currentAge = 0;
		maximumAge = 20;
		rateOfAge = .002f;
		currentResources = 20; 
		maximumResources = 20; 
		resourceRequirement = .02f;
		currentCapacity = 0;
		maximumCapacity = 5;
	}
	

	
	private void eatObject (GameObject food)
	{		
		eatTarget = food.GetComponent("foodBehavior")as foodBehavior;
		eatTarget.foodValue -= harvestSpeed;
		currentCapacity += harvestSpeed;
		eatTarget.changeColor ();

		if (eatTarget.foodValue < 1) 
		{
			//food.SetActive (false);
			Destroy(food);
		}
	}

	private GameObject predatorCheck()
	{
		GameObject nearPredator = findClosestObject("Predator");
		if ((nearPredator != null) && (getTargetDistance (nearPredator.transform.position) < senseDistance))
			return(nearPredator);
		else
			return(null);
	}

	void Update () {

		ageCreature();
		consumeResources();
		eatFood();
		hungryCheck ();
		starvingCheck ();
		healthCheck ();


		//add predator check and run away from predator
		GameObject predatorTarget = predatorCheck ();
		if (predatorTarget != null && afraidFlag==false) {
			//Debug.Log ("PREDATOR FOUND");
			afraidFlag = true;
			predatorLocation = predatorTarget.transform.position;
		}

		if (afraidFlag) {
			//Debug.Log ("PLoc x,z:" + predatorLocation.x + "," + predatorLocation.z);
			runAway (predatorLocation);
		}
		else if (returnFlag)
		{
			GameObject nearBase = findClosestObject("Base");
			if (nearBase!=null)
			{
				distanceToTarget = getTargetDistance(nearBase.transform.position);
				if (distanceToTarget < .5)
				{
					baseTarget = nearBase.GetComponent("baseBehavior")as baseBehavior;
					baseTarget.resourceFood += currentCapacity;
					currentCapacity = 0;
					returnFlag = false;
				}
				else
					moveToTarget(nearBase.transform.position);
			}
			else
			{	
				Debug.Log ("NULL NEARBASE ERROR");
			}
		}
		else
		{
			GameObject target = findClosestObject("Food");
			if (target!=null)
			{
				distanceToTarget = getTargetDistance(target.transform.position);
				if (distanceToTarget < .25)
				{
					eatObject(target);
					bounceObject();
				}
				else
				{
					if(distanceToTarget < senseDistance)
						moveToTarget(target.transform.position);
					else
						wander(); 
						//add some trail, and check for trail stuff here						
						//add some repulsers for predators and for death spots
				}

				if (currentCapacity >= maximumCapacity)
				{
					returnFlag = true;
					groundObject();
				}
			}
			else
			{
				returnFlag=true;
				Debug.Log ("NULL FOOD ERROR");
			}
		}        
	}
}