using UnityEngine;
using System.Collections;

public class TerrainProperties : MonoBehaviour {

    public string name;
    public float woodAverage;
    public float stoneAverage;
    public float foodAverage;

    public Sprite[] tileSprites;

    public void setProperties (string n, float w, float s, float f)
    {
        name = n;
        woodAverage = w;
        stoneAverage = s;
        foodAverage = f;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
