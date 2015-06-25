using UnityEngine;
using System.Collections;

public class moveBehaviors : aliveBehaviors {


	public  int senseDistance;
	public  float distanceToTarget = Mathf.Infinity;
	public  float speed;
	public  bool upFlag = true;
	public  int wanderSteps = 0;
	public  int maxWanderSteps;
	public 	bool isWandering = false;
	public  float sizeOfSpace;
	public  Vector3 wanderTarget;
	public  bool afraidFlag = false;
	public  int maxRunAwaySteps;
	public  int runAwaySteps = 0;
	public  bool travelFood = false;

	private universalScripts u = universalScripts.getInstance();
	// Use this for initialization
	void Start () {
	
		sizeOfSpace = (u.getPlatformSize ()/2);
	}

	public void groundObject(float yCoord){
		float yDelta = transform.position.y - yCoord;
		Vector3 objPosition = new Vector3 (0,-yDelta,0);
		//Debug.Log (objPosition+" "+transform.position);
		transform.Translate (objPosition); 
	}
	
	public void bounceObject()
	{
		float bounceSpeed = 2f;
		Vector3 objPosition = transform.position;
		
		if (upFlag && objPosition.y<1)
		{
			transform.Translate(Vector3.up * Time.deltaTime * bounceSpeed);
		}
		else if (upFlag && objPosition.y >= 1)
		{
			upFlag = false;
		}
		else if (!upFlag && objPosition.y > .51)
		{
			transform.Translate(Vector3.down * Time.deltaTime * bounceSpeed);
		}
		else
		{
			upFlag = true;
		}
		
	}

	public GameObject findClosestMate (string targetTag, bool isFemale)
	{
		GameObject[] gObjs;
		gObjs = GameObject.FindGameObjectsWithTag(targetTag);    
		int returnLength = gObjs.GetLength(0);
		if (returnLength > 0) {
			GameObject closestObject = null;
			float dist = Mathf.Infinity;
			Vector3 thisPosition = transform.position;
			aliveBehaviors aliveTarget;
			
			foreach (GameObject gObj in gObjs) {
				Vector3 diff = gObj.transform.position - thisPosition;
				float curDistance = diff.sqrMagnitude;            
				if (curDistance < dist) 
				{
					aliveTarget = gObj.GetComponent ("aliveBehaviors") as aliveBehaviors;
					//Debug.Log ("Dist:"+curDistance+" X:"+gObj.transform.position.x+" Z:"+gObj.transform.position.z);
					//Debug.Log ("isFemale:"+aliveTarget.isFemale+" toMate:"+aliveTarget.wantsToMate +" iAmGender"+isFemale);
					if ((aliveTarget.isFemale != isFemale) && (aliveTarget.wantsToMate) && (!aliveTarget.hasMate))
					{
						closestObject = gObj;
						dist = curDistance;
						//Debug.Log ("closest Dist:"+dist);
					}
				}
			}        
			return (closestObject);
		} else {
			//Debug.Log ("Null Error:" + targetTag + ":" + returnLength);
			return(null);
		}
	}


	public GameObject findClosestMateWithinX (string targetTag, float distance, bool isFemale)
	{
		GameObject[] gobj;
		Vector3 thisPosition = transform.position;
		Collider[] closeObjects;
		aliveBehaviors aliveTarget;
		closeObjects = Physics.OverlapSphere (thisPosition, distance);
		int numObjects = closeObjects.GetLength(0);

		if (numObjects > 0) {
			GameObject closestObject = null;
			float dist = Mathf.Infinity;
			foreach (Collider closeObj in closeObjects) {
				Vector3 diff = closeObj.transform.position - thisPosition;
				float curDistance = diff.sqrMagnitude;            
				if ((curDistance < dist) && (closeObj.tag == targetTag)) {
					aliveTarget = closeObj.GetComponent ("aliveBehaviors") as aliveBehaviors;
					if ((aliveTarget.isFemale != isFemale) && (aliveTarget.wantsToMate) && (!aliveTarget.hasMate))
					{
						closestObject = closeObj.gameObject;
						dist = curDistance;
					}
				}
			}
			return(closestObject);
		} else
			return(null);
	}


	public GameObject findClosestObjectWithinX (string targetTag, float distance)
	{
		GameObject[] gobj;
		Vector3 thisPosition = transform.position;
		Collider[] closeObjects;
		closeObjects = Physics.OverlapSphere (thisPosition, distance);
		int numObjects = closeObjects.GetLength(0);
		
		if (numObjects > 0) {
			GameObject closestObject = null;
			float dist = Mathf.Infinity;
			foreach (Collider closeObj in closeObjects) {
				Vector3 diff = closeObj.transform.position - thisPosition;
				float curDistance = diff.sqrMagnitude;            
				if ((curDistance < dist) && (closeObj.tag == targetTag)) {
					closestObject = closeObj.gameObject;
					dist = curDistance;
				}
			}
			return(closestObject);
		} else
			return(null);
	}


	public GameObject findClosestObject(string targetTag)
	{
		GameObject[] gObjs;
		gObjs = GameObject.FindGameObjectsWithTag(targetTag);    
		int returnLength = gObjs.GetLength(0);
		if (returnLength > 0) {
			GameObject closestObject = null;
			float dist = Mathf.Infinity;
			Vector3 thisPosition = transform.position;
			
			foreach (GameObject gObj in gObjs) {
				Vector3 diff = gObj.transform.position - thisPosition;
				float curDistance = diff.sqrMagnitude;            
				if (curDistance < dist) {
					closestObject = gObj;
					dist = curDistance;
				}
			}        
			return (closestObject);
		} else {
			//Debug.Log ("Null Error:" + targetTag + ":" + returnLength);
			return(null);
		}
	}

	public float getTargetDistance(Vector3 targetPosition)
	{
		Vector3 targetDiff = targetPosition - transform.position;
		float targetDistance = Mathf.Sqrt (Mathf.Pow (targetDiff.x, 2) + Mathf.Pow (targetDiff.z, 2));
		return targetDistance;
	}
	
	public void moveToTarget(Vector3 targetPosition)
	{        
		Vector3 tempTarget = targetPosition;
		float step = speed * Time.deltaTime;
		if (unitType == "Sheep")
			tempTarget.y = .5f;
		else
			tempTarget.y = 1f;
		//tempTarget.y = transform.position.y; //adjust the y co-ord to be level, to avoid "sinking"
		transform.position = Vector3.MoveTowards(transform.position, tempTarget, step);
	}
	
	private void boundaryPullback(Vector3 p)
	{

	}


	public void moveAwayFromTarget(Vector3 targetPosition)
	{        
		Vector3 tempTarget = targetPosition;
		//tempTarget.y = transform.position.y; //adjust the y co-ord to be level, to avoid "sinking"
		if (unitType == "Sheep")
			tempTarget.y = .5f;
		else
			tempTarget.y = 1f;

		float step = -speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, tempTarget, step);
		boundaryPullback (transform.position);
	}



	
	public Vector3 setNewCourse(Vector3 currentTarget)
	{
		float maxWanderDist = .10f*u.getPlatformSize();
		currentTarget.x += (Random.value * maxWanderDist) - (maxWanderDist / 2);
		currentTarget.z += (Random.value * maxWanderDist) - (maxWanderDist / 2);
		currentTarget.x = u.pullBackPosition (currentTarget.x, u.getPlatformSize()/2);
		currentTarget.z = u.pullBackPosition (currentTarget.z, u.getPlatformSize()/2);
		currentTarget.y = 0.5f;
		return (currentTarget);
	}
	
	public void wander()
	{
		wanderSteps++;
		float wanderTargetDistance = getTargetDistance(wanderTarget);
		if (wanderTargetDistance < .5f) 
		{
			wanderTarget = setNewCourse (transform.position);
		} 
		else if (wanderSteps > maxWanderSteps)
		{
			wanderTarget = setNewCourse (transform.position);
			wanderSteps = 0;
			isWandering = false;
			travelFood = true;
		}
		else 
		{	
			moveToTarget (wanderTarget);
		}
	}
	
	public void runAway(Vector3 avoidTarget)
	{
		runAwaySteps++;
		float avoidTargetDistance = getTargetDistance(avoidTarget);
		//Debug.Log ("PDistace:"+avoidTargetDistance+" TLoc x,z:" + string.Format ("{0:0.00}", avoidTarget.x) + "," + string.Format ("{0:0.00}", avoidTarget.z));
		//Debug.Log ("PDistace:"+avoidTargetDistance+" RLoc x,z:" + string.Format ("{0:0.00}", transform.position.x) + "," + string.Format ("{0:0.00}", transform.position.z));
		if (runAwaySteps > maxRunAwaySteps || avoidTargetDistance > (senseDistance*2)) {
			wanderTarget = setNewCourse (transform.position);
			runAwaySteps = 0;
			afraidFlag = false;
			travelFood = true;
		} else
			moveAwayFromTarget (avoidTarget);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
