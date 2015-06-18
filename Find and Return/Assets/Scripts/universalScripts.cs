﻿using UnityEngine;
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

	public float multiX = 20;



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
		Debug.Log ("GRID INIT COMPLETE");
	}

	//private contructor
	private universalScripts() {
		gridSize = 50;
		platformSize = 75;
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
