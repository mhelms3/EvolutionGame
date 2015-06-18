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
	public  bool wanderFood = false;

	private universalScripts u = universalScripts.getInstance();
	// Use this for initialization
	void Start () {
	
		sizeOfSpace = (u.getPlatformSize ()/2);
	}

	public void groundObject(){
		Vector3 objPosition = transform.position;
		objPosition.y = .5f;
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
		tempTarget.y = .5f;
		//tempTarget.y = transform.position.y; //adjust the y co-ord to be level, to avoid "sinking"
		transform.position = Vector3.MoveTowards(transform.position, tempTarget, step);
	}
	
	public void moveAwayFromTarget(Vector3 targetPosition)
	{        
		Vector3 tempTarget = targetPosition;
		//tempTarget.y = transform.position.y; //adjust the y co-ord to be level, to avoid "sinking"
		tempTarget.y = .5f;
		float step = -speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, tempTarget, step);
	}
	
	public float pullBack (float n)
	{
		if (n > sizeOfSpace-10)
			return (sizeOfSpace-15);
		else if (n < -sizeOfSpace+10)
			return(-sizeOfSpace+15);
		else
			return(n);
	}
	
	public Vector3 setNewCourse(Vector3 currentTarget)
	{
		//currentTarget.x += (Random.value * 30f) - 15f;
		//currentTarget.z += (Random.value * 30f) - 15f;
		currentTarget.x = pullBack (currentTarget.x+(Random.value * 50f) - 25f);
		currentTarget.z = pullBack (currentTarget.z+(Random.value * 50f) - 25f);
		return (currentTarget);
	}
	
	public void wander()
	{
		wanderSteps++;
		float wanderTargetDistance = getTargetDistance(wanderTarget);
		if (wanderTargetDistance < .25f) 
		{
			wanderTarget = setNewCourse (transform.position);
		} 
		else if (wanderSteps > maxWanderSteps)
		{
			wanderTarget = setNewCourse (transform.position);
			wanderSteps = 0;
			isWandering = false;
			wanderFood = true;
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
		if (runAwaySteps > maxRunAwaySteps || avoidTargetDistance > 12) {
			wanderTarget = setNewCourse (transform.position);
			runAwaySteps = 0;
			afraidFlag = false;
		} else
			moveAwayFromTarget (avoidTarget);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
