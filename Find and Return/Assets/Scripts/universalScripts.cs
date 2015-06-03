using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;



public class universalScripts {

	private float[,] grassGrid;
	private int gridSize;
	private int platformSize;
	private int ticker;
	private DateTime startTime;
	private int currentTime;
	public int foodCount;
	public int wolves;
	public int sheep;
	public int total;
	public Text wcount;
	public Text scount;
	public Text tcount;
	public Text fcount;


	public void updateCountText(string countType)
	{
		switch (countType)
		{
			case "Wolf":
				wcount.text = wolves.ToString ();
				break;
			case "Sheep":
				scount.text = sheep.ToString ();
				break;
			case "Food":
				fcount.text = foodCount.ToString ();
				break;
			default:			
				break;
		}

		int total = wolves + sheep + foodCount;
		tcount.text = total.ToString ();
	}



	public void dumpData(){
	
		Debug.Log ("W"+wcount.text);
		Debug.Log ("S"+scount.text);
		Debug.Log ("F"+fcount.text);

	}

	//singleton constructor
	private static universalScripts INSTANCE = new universalScripts();

	private void initializeGrassGrid(){
		for (int i = 0; i<platformSize; i++)
			for (int k =0; k<platformSize; k++)
				grassGrid [i, k] = 0.0f;
		Debug.Log ("GRID INIT COMPLETE");
	}

	//private contructor
	private universalScripts() {
		gridSize = 50;
		platformSize = 100;
		grassGrid = new float[platformSize, platformSize];
		initializeGrassGrid ();
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

	public void incrementTicker(){
		ticker++;
	}

	public int getTicker(){
		return (ticker);
	}

	public int getGridSize(){
		return (gridSize);
	}

	public int getPlatformSize(){
		return (platformSize);
	}


}
