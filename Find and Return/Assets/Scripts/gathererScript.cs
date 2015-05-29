using UnityEngine;
using System.Collections;
//using movementScripts;

public class gathererScript : moveBehaviors {

	private bool eatFlag = false;
	private bool returnFlag = false;
	private foodBehavior eatTarget;
	private baseBehavior baseTarget;
	private float harvestSpeed = 0.1f;
	private int tick = 0;
	public int tickCheck = 29;
	private GameObject nearBase;
	private GameObject target;

	Vector3 predatorLocation = Vector3.zero;
	private universalScripts u = universalScripts.getInstance();


	// Use this for initialization
	void Start () {
		//move stuff
		senseDistance = 4;
		speed = 8f;
		maxWanderSteps = 30;
		maxRunAwaySteps = 100;

		wanderTarget = setNewCourse (transform.position);	//sets WanderTarget relative to where this object was instantiated	

		nearBase = findClosestObject("Base");
		target = findClosestObject ("Food");

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
		if (eatTarget.foodValue < 1) 
		{
			u.foodCount--;
			Destroy(food);
		}
		else
			eatTarget.changeColor (false);
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


		ageCreature (1);
		consumeResources (1);
		eatFood ();
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
		else
		{
			//default behavior = find food (quit application if there is no food remaining)
			if (u.foodCount<1)
				Application.Quit ();

			//if no food target, then get one
			else if (target == null)
				target = findClosestObject("Food");

			else 
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
					{
						target = findClosestObject("Food");
						wander(); 
						//add some trail, and check for trail stuff here						
						//add some repulsers for predators and for death spots
					}
				}

				if (currentCapacity >= maximumCapacity)
				{
					returnFlag = true;
					groundObject();
				}
			}
		}        
	}
}