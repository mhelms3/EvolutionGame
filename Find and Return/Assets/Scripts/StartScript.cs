using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

    public GameObject Worker;    
	public GameObject Grass;
    public GameObject Base;
	public GameObject Predator;
	public GameObject theGame;

	public int spread;
	public int foodTotal;
	public int startingSheep;
	public float foodPercentage;

	private foodBehavior fb;
	private Vector3 startingPosition;

	private universalScripts u = universalScripts.getInstance();



    // Use this for initialization
    void Start()
    {
        //Instantiate(Base);
		universalScripts u = universalScripts.getInstance();

		startingSheep = 10;
		for (int k = 0; k<startingSheep; k++) {
			startingPosition = new Vector3 (Random.value*10-5, .5f, Random.value*10-5 );        
			Instantiate (Worker, startingPosition, Quaternion.identity);
		}

		//Predator Starts far away (20+1d20 in both x and z)
		startingPosition = new Vector3(Random.value*20, 1f, Random.value*20);        
		Instantiate(Predator, startingPosition, Quaternion.identity);

		//Number of grass patches to explore = foodTotal^2
		foodTotal = u.getGridSize(); //(e.g. 50 x 50matrix)
		foodPercentage = .25f; //percent of explored grid units to have a grass patch 
		spread = foodTotal / 2;
		int gridOrigin = Mathf.RoundToInt(u.getPlatformSize() / 2 - u.getGridSize()/2);

        for (int i = 0; i < foodTotal; i++)
			for (int j = 0; j < foodTotal; j++)
        {
			if (Random.value<foodPercentage)
			{
	            //startingPosition = new Vector3((Random.value * spread)-spread/2*1f, .05f, (Random.value * spread)-spread/2 *1f);
				startingPosition = new Vector3(i-spread, .1f, j-spread);
	            GameObject grass = Instantiate<GameObject>(Grass);
				grass .transform.position = startingPosition;
				fb = grass .GetComponent("foodBehavior")as foodBehavior;
				fb.foodValue = Random.value * 50f;
				u.setGridValue(i+gridOrigin, j+gridOrigin, fb.foodValue);
				u.foodCount++;

			}
        }
    }
	
	// Update is called once per frame
	void Update () {
		u.incrementTicker ();
		if (Input.GetKey("escape"))
			Application.Quit();
	}
}
