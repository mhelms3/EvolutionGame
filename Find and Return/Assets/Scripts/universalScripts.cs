using UnityEngine;
using System.Collections;
using System;



public class universalScripts {

	private float[,] grassGrid;
	private int gridSize;
	private int platformSize;
	private int ticker;
	private DateTime startTime;
	private int currentTime;


	//singleton constructor
	private static universalScripts INSTANCE = new universalScripts();

	private void initializeGrassGrid(){
		for (int i = 0; i<platformSize; i++)
			for (int k =0; k<platformSize; k++)
				grassGrid [i, k] = 0.0f;
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

	public bool setGridValue(int i, int j, float gridValue) {
		if ((i < gridSize) && (j < gridSize)) {
			grassGrid [i, j] = gridValue;
			return true;
		} else
			return false;
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
