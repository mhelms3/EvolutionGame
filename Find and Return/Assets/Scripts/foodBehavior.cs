using UnityEngine;
using System.Collections;

public class foodBehavior : MonoBehaviour {

	public float foodValue;
	public float maxFoodValue;

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

	// Use this for initialization
	void Start () {

		green1 = new Color32(28,255,19,255);
		green2 = new Color32(30,120,12,255);
		green3 = new Color32(71,30,7,255);
		seedColor = new Color32 (140, 120, 7, 125);
		thisFood = gameObject;


		changeColor (false);

		maxFoodValue = 50;
		growthRate = .02f;
		seedRange = 3;
		seedNumber = 1;
		seedSurvival = .1f;
		seedCost = seedRange * seedRange * seedNumber * seedSurvival*10;
		seedRequirement = seedCost*3;

	}

	public void changeColor(bool seedFlag)
	{
		rend = thisFood.GetComponent<Renderer>();

		if (seedFlag)
			seedCounter = 10;
		if (seedCounter > 0) {
			seedCounter--;
			rend.material.color = seedColor;
		} else {
			green2 = Color32.Lerp (green3, green1, (foodValue / maxFoodValue) *Time.deltaTime);
			rend.material.color = green2;
		}
	}

	public void seedFood()
	{
		foodValue -= seedCost;
		changeColor (true);
		for (int iSeed = 0; iSeed<seedNumber; iSeed++) {
			if(Random.value<seedSurvival)
			{
				int xPos = Mathf.RoundToInt(transform.position.x)+ Mathf.RoundToInt(Random.value*seedRange*2)-seedRange;
				int zPos = Mathf.RoundToInt(transform.position.z)+ Mathf.RoundToInt(Random.value*seedRange*2)-seedRange;
				int gridPosX = Mathf.RoundToInt (xPos+u.getPlatformSize()/2);
				int gridPosZ = Mathf.RoundToInt (zPos+u.getPlatformSize()/2);


				if (u.getGridValue(gridPosX, gridPosZ)==0)
				{
					Vector3 seedPosition = new Vector3(xPos, .5f, zPos);
					GameObject grass = Instantiate<GameObject>(Grass);
					grass.transform.position = seedPosition;
					fb = grass.GetComponent("foodBehavior")as foodBehavior;
					fb.foodValue = .2f;
					u.setGridValue (gridPosX, gridPosZ, fb.foodValue);
					u.foodCount++;
				}

			}
		}


	}

	public void grow()
	{
		foodValue += growthRate;
		if (foodValue > seedRequirement || foodValue==maxFoodValue) {
			seedFood();
		}
	}

	// Update is called once per frame
	void Update () {
		grow ();
		changeColor(false);
	}

}
