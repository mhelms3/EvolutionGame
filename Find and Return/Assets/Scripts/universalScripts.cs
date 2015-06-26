using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;



public class universalScripts {

	private float[,] grassGrid;
	private int[,] grassGridEaters;
	//private int gridSize;
	private int platformSize;
	private int ticker;
	private DateTime startTime;
	private int currentTime;
	public int foodCount;
	public int wolves;

	public int sheep;
	public int babySheep;
	public double percentFemale;

	public int sheepStarve;
	public int sheepMature;
	public int sheepAge;

	public int total;
	public Text wcount;
	public Text scount;
	public Text babySheepCount;
	public Text totalSheepCount;
	public Text ageSheepCount;
	public Text matureSheepCount;
	public Text starveSheepCount;
	public Text tcount;
	public Text fcount;

	public bool isPaused = false;
	public float multiX = 3;



	public int convertPosToGrid(float p)
	{
		int fullPlat = getPlatformSize ();
		int halfPlat = Mathf.RoundToInt (fullPlat / 2);
		int GridPos = Mathf.RoundToInt(p) + halfPlat;
		return(pullBackGrid (GridPos, fullPlat));
	}

	public void updateCountText(string countType)
	{
		switch (countType)
		{
			case "Wolf":
				wcount.text = wolves.ToString ();
				break;
			case "Sheep":
				scount.text = sheep.ToString ();
				babySheepCount.text = babySheep.ToString ();
				totalSheepCount.text = (babySheep+sheep).ToString ();
				break;
			case "Food":
				fcount.text = foodCount.ToString ();
				break;
			case "Starve":
				starveSheepCount.text = sheepStarve.ToString ();
				break;
			case "Age":
				ageSheepCount.text = sheepAge.ToString ();
				break;
			case "Mature":
				matureSheepCount.text = sheepMature.ToString ();
				break;
			default:			
				break;
		}

		int total = wolves + sheep + foodCount;
		tcount.text = total.ToString ();
	}



	//singleton constructor
	private static universalScripts INSTANCE = new universalScripts();

	private void initializeGrassGrid(){
		for (int i = 0; i<platformSize; i++)
			for (int k =0; k<platformSize; k++)
				grassGrid [i, k] = 0.0f;
		//Debug.Log ("GRID INIT COMPLETE");
	}

	private void initializeGrassGridEaters(){
		for (int i = 0; i<platformSize; i++)
			for (int k =0; k<platformSize; k++)
				grassGridEaters [i, k] = 0;
		//Debug.Log ("GRID INIT COMPLETE");
	}

	//private contructor
	private universalScripts() {
		//gridSize = 35;
		platformSize = 50;
		grassGrid = new float[platformSize, platformSize];
		grassGridEaters = new int[platformSize, platformSize];
		initializeGrassGrid ();
		initializeGrassGridEaters ();
		ticker = 0;
		startTime = System.DateTime.Now;
	}

	//return instance
	public static universalScripts getInstance() {
		return INSTANCE;
	}


	//API gets and sets
	public float[,] getGrid() {
		return grassGrid;
	}

	public int pullBackGrid (int n, int edge)
	{
		if (n >edge)
			return (edge);
		else if (n < 0)
			return(0);
		else
			return(n);
	}

	public float pullBackPosition (float n, float edge)
	{
		if (n > edge)
			return (.95f*edge);
		else if (n < -edge)
			return(-.95f*edge);
		else
			return(n);
	}

	public float getGridValue(int i, int j) {
		if ((i < platformSize) && (j < platformSize)) {
			return grassGrid [i, j];
		} else
			return -1f;
	}

	public void setGridValue(int i, int j, float gridValue) {
		if ((i < platformSize) && (j < platformSize)) 
			grassGrid [i, j] = gridValue;
	}

	public void incGridEaters (int i, int j)
	{
		if ((i < platformSize) && (j < platformSize)) 
			grassGridEaters [i, j]++;
	}

	public void decGridEaters (int i, int j)
	{	
		if ((i < platformSize) && (j < platformSize)) 
			grassGridEaters [i, j]--;
	}

	public int getGridEaters (int i, int j)
	{
		if ((i < platformSize) && (j < platformSize)) 
			return grassGridEaters [i, j];
		else
			return (-1);
	}

	public void setGridEaters (int i, int j, int v)
	{
		if ((i < platformSize) && (j < platformSize)) 
			grassGridEaters [i, j] = v;
	}

	public void incrementTicker(){
		ticker++;
	}

	public int getTicker(){
		return (ticker);
	}

	public int getPlatformSize(){
		return (platformSize);
	}


}
