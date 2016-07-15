using UnityEngine;
using System.Collections;

public class foodBehavior : MonoBehaviour {

	public float foodValue;
	public float maxFoodValue;

	public int gridXPos;
	public int gridZPos;

	public float reseedDelay;
	public float reseedTimer;
	public bool isDormant;

	private Color32 green1;
	private Color32 green2;
	private Color32 green3;
	private Color32 seedColor;


	private GameObject thisFood;
	public Renderer rend;
	public float growthRate;


	public float seedRequirement; //minimum threshhold before producing seeds
	public int seedRange;	 	  //range seeds can go
	public int seedNumber;	      //number of seeds produced
	public float seedSurvival;      //percent of seeds that create a new patch
	public float seedCost;	      //cost per seed
	private bool seedFlag = false;
	private int seedCounter = 0;

	public GameObject Grass;
	private foodBehavior fb;
	private universalScripts u = universalScripts.getInstance();
	private int foodTicker = 0;
	public bool killFlag = false;
	private int colorUpdate;


	// Use this for initialization
	void Start () {

		reseedDelay = 100f;
		green1 = new Color32(28,255,19,255);
		green2 = new Color32(30,120,12,255);
		green3 = new Color32(71,30,7,255);
		seedColor = new Color32 (140, 120, 7, 125);
		thisFood = gameObject;

		maxFoodValue = 100;
		growthRate = .14f*u.multiX;
		seedRange = 10;
		seedNumber = 1;
		seedSurvival = 1;
		//seedCost = seedRange * seedNumber * seedSurvival* 15;
		seedCost = 20;
		seedRequirement = seedCost*3;
		colorUpdate = (int)(Random.value * 300);


	}

	public void changeColor()
	{
		rend = thisFood.GetComponent<Renderer>();

		float colorBatcher = (int) (foodValue / maxFoodValue * 10);
		colorBatcher = colorBatcher / 10;

		green2 = Color32.Lerp (green3, green1, colorBatcher);
		rend.material.color = green2;
	}




	public void seedFood()
	{
		foodValue -= seedCost;
		changeColor ();
		for (int iSeed = 0; iSeed<seedNumber; iSeed++) 
		{
			if(Random.value<seedSurvival)
			{
				float xPos = Mathf.RoundToInt((transform.position.x)+ (Random.value*seedRange*2)-seedRange);
				float zPos = Mathf.RoundToInt((transform.position.z)+ (Random.value*seedRange*2)-seedRange);

				xPos = u.pullBackPosition(xPos, u.getPlatformSize ()/2);
				zPos = u.pullBackPosition(zPos, u.getPlatformSize ()/2);

				int tempGridXPos = u.convertPosToGrid(xPos);
				int tempGridZPos = u.convertPosToGrid(zPos);

				if (u.getGridValue(tempGridXPos, tempGridZPos)==0)
				{
					Vector3 seedPosition = new Vector3(xPos, .5f, zPos);
					GameObject grass = Instantiate<GameObject>(Grass);
					grass.transform.position = seedPosition;
					fb = grass.GetComponent("foodBehavior")as foodBehavior;
					fb.foodValue = .2f;
					fb.gridXPos = tempGridXPos;
					fb.gridZPos = tempGridZPos;
					u.setGridValue (tempGridXPos, tempGridZPos, fb.foodValue);
					u.foodCount++;
					u.updateCountText("Food");
				}
			}
		}
	}

	public void grow()
	{
		foodValue += growthRate;
		if (foodValue > seedRequirement || foodValue==maxFoodValue) 
			seedFood();

	}

	public void killPlant()
	{
		if (killFlag) {
			u.setGridValue (gridXPos, gridZPos, 0);
			u.setGridEaters (gridXPos, gridZPos, 0);
			//foodValue = 0;
			//reseedTimer = reseedDelay;
			//killFlag = false;
			//isDormant = true;
			Destroy (gameObject);
			u.foodCount--;
			u.updateCountText ("Food");
		}
	}

	// Update is called once per frame
	void Update () {
		if (!u.isPaused) {
			/*
			if (reseedTimer>1)
				reseedTimer-=Time.deltaTime;
			else
			{
				grow ();
				isDormant = false;
			}
			*/
			grow ();
			foodTicker++;
			//InvokeRepeating ("changeColor", .1f, 15*Time.deltaTime);
			if (foodTicker%300==colorUpdate)			
				changeColor();
		}
	}

	void LateUpdate()
	{
		if (!u.isPaused) {
			killPlant ();

		}
	}

}
