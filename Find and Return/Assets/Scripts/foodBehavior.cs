using UnityEngine;
using System.Collections;

public class foodBehavior : MonoBehaviour {

	public float foodValue;
	//public Material mat;
	private Color32 green1;
	private Color32 green2;
	private Color32 green3;
	private GameObject thisFood;
	public Renderer rend;
	public float growthRate;

	public float seedRequirement; //minimum threshhold before producing seeds
	public int seedRange;	 	  //range seeds can go
	public int seedNumber;	      //number of seeds produced
	public float seedSurvival;      //percent of seeds that create a new patch
	public float seedCost;	      //cost per seed

	public GameObject Grass;
	private foodBehavior fb;
	private universalScripts u = universalScripts.getInstance();

	// Use this for initialization
	void Start () {

		green1 = new Color32(28,226,19,255);
		green2 = new Color32(19,142,2,255);
		green3 = new Color32(71,53,7,255);
		thisFood = gameObject;
		changeColor ();

		growthRate = .02f;
		seedRange = 3;
		seedNumber = 10;
		seedSurvival = .01f;
		seedCost = seedRange * seedRange * seedNumber * seedSurvival*10;
		seedRequirement = seedCost*3;

	}

	public void changeColor()
	{
		rend = thisFood.GetComponent<Renderer>();
		if (foodValue < 10)
			rend.material.color = green3;
		else if (foodValue < 30)
			rend.material.color = green2;
		else
			rend.material.color = green1;
	}

	public void seedGrass()
	{
		foodValue -= seedCost;
		for (int iSeed = 0; iSeed<seedNumber; iSeed++) {
			if(Random.value<seedSurvival)
			{
				int xPos = Mathf.RoundToInt(transform.position.x)+ Mathf.RoundToInt(Random.value*seedRange*2)-seedRange;
				int zPos = Mathf.RoundToInt(transform.position.z)+ Mathf.RoundToInt(Random.value*seedRange*2)-seedRange;
				int gridPosX = Mathf.RoundToInt (xPos+u.getPlatformSize()/2);
				int gridPosZ = Mathf.RoundToInt (zPos+u.getPlatformSize()/2);
				Debug.Log ("X:"+xPos+" Z:"+ zPos+" gridX:"+gridPosX+" gridZ:"+gridPosZ);
				Debug.Log ("Value@:"+ u.getGridValue(gridPosX, gridPosZ));

				if (u.getGridValue(gridPosX, gridPosZ)==0)
				{
					Vector3 seedPosition = new Vector3(xPos, .1f, zPos);
					GameObject grass = Instantiate<GameObject>(Grass);
					grass.transform.position = seedPosition;
					fb = grass.GetComponent("foodBehavior")as foodBehavior;
					fb.foodValue = .2f;
					u.setGridValue (gridPosX, gridPosZ, fb.foodValue);
				}

			}
		}


	}

	public void grow()
	{
		foodValue += growthRate;
		if (foodValue > seedRequirement)
			seedGrass ();

	}

	// Update is called once per frame
	void Update () {
		grow ();

		if ((u.getTicker () % (1 / growthRate)) < 1.5) {
			changeColor ();
		}
	}
}
