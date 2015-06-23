using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

	public GameObject Worker;
	public GameObject Grass;
    public GameObject Base;
	public GameObject Predator;
	public GameObject theGame;

	public Text sheepCountDisplay;
	public Text babySheepCountDisplay;
	public Text totalSheepCountDisplay;
	public Text wolfCountDisplay;
	public Text foodCountDisplay;
	public Text totalCountDisplay;

	public Text sheepAgeCountDisplay;
	public Text sheepMatureCountDisplay;
	public Text sheepStarveCountDisplay;

	public int spread;
	public int foodTotal;
	public int startingSheep;
	public float foodPercentage;

	private foodBehavior fb;
	private Vector3 startingPosition;

	private universalScripts u = universalScripts.getInstance();

	private void initializeVariables()
	{
		u.wolves = 0;
		u.sheep = 0;
		u.foodCount = 0;
		u.total = 0;
		
		u.babySheep = 0;
		u.percentFemale = .50;
		
		u.sheepStarve =0;
		u.sheepMature =0;
		u.sheepAge =0;
	}

	private void initializeDisplayVariables()
	{
		u.scount = sheepCountDisplay;
		u.babySheepCount = babySheepCountDisplay;
		u.totalSheepCount = totalSheepCountDisplay;
		
		u.fcount = foodCountDisplay;
		u.tcount = totalCountDisplay;
		u.wcount = wolfCountDisplay;
		
		u.ageSheepCount= sheepAgeCountDisplay;
		u.matureSheepCount = sheepMatureCountDisplay;
		u.starveSheepCount = sheepStarveCountDisplay;
	}

	private void initializeFoodGrid()
	{
		//Number of grass patches to explore = foodTotal^2
		foodTotal = u.getPlatformSize(); //(e.g. 50 x 50matrix)
		foodPercentage = .1f; //percent of explored grid units to have a grass patch 
		spread = Mathf.RoundToInt(u.getPlatformSize() / 2);
		
		
		for (int i = 0; i < foodTotal; i++)
			for (int j = 0; j < foodTotal; j++)
		{
			if (Random.value<foodPercentage)
			{
				//startingPosition = new Vector3((Random.value * spread)-spread/2*1f, .05f, (Random.value * spread)-spread/2 *1f);
				startingPosition = new Vector3(i-spread, .1f, j-spread);
				GameObject grass = Instantiate<GameObject>(Grass);
				grass.transform.position = startingPosition;
				fb = grass .GetComponent("foodBehavior")as foodBehavior;
				fb.foodValue = Random.value * 50f;
				fb.gridXPos = i;
				fb.gridZPos = j;
				u.setGridValue(i, j, fb.foodValue);
				u.foodCount++;
			}
		}
		u.updateCountText ("Food");
	}

	private void initializeGatherers()
	{
		//SHEEP
		GameObject newGatherer;
		gathererScript gsWorker;
		startingSheep = 12; //must be at least 2
		
		for (int k = 0; k<startingSheep; k++) {
			startingPosition = new Vector3 (Random.value*20-10, .5f, Random.value*20-10 );        
			newGatherer = Instantiate<GameObject> (Worker);
			newGatherer.transform.position = startingPosition;
			gsWorker = newGatherer.GetComponent ("gathererScript") as gathererScript;
			gsWorker.assignGender();
			//guarantee one male and one female
			if (k == 0) gsWorker.isFemale = true;
			if (k == 1) gsWorker.isFemale = false;
			gsWorker.genderColor ();
			gsWorker.setAdult ();
			u.sheep++;
			u.updateCountText ("Sheep");
		}
	}

	private void initializePredators()
	{
		//PREDATOR
		//float startX = (float)(Random.value * u.getPlatformSize () - .5 * u.getPlatformSize ());
		//float startZ = (float)(Random.value * u.getPlatformSize () - .5 * u.getPlatformSize ());		
		float startX = (float)(u.getPlatformSize ()/2);
		float startZ = (float)(u.getPlatformSize ()/2);
		startingPosition = new Vector3(startX, 1f, startZ);        
		Instantiate(Predator, startingPosition, Quaternion.identity);
		u.wolves++;
		u.updateCountText ("Wolf");
	}



    // Use this for initialization
    void Start()
	{
		universalScripts u = universalScripts.getInstance ();
		initializeVariables ();
		initializeDisplayVariables ();
		initializeFoodGrid ();
		initializeGatherers ();
		initializePredators ();
	
	}
	
	// Update is called once per frame
	void Update () {
		u.incrementTicker ();
		if (Input.GetKeyDown("escape"))
			Application.Quit();
		if (Input.GetKeyDown ("space"))
			u.isPaused = !u.isPaused;	

	}
}
