using UnityEngine;
using System.Collections;

public class TileProperties : MonoBehaviour {

    public float wood;
    public float stone;
    public float food;

    public string name;

    public GameObject squareTerrain; // this must be a terrainType object
    public Sprite squareSprite; // this must be a terrainType object

    public void setPropertiest(string n, float w, float s, float f)
    {
        name = n;
        stone = s;
        wood = w;
        food = f;
    }

    public TileProperties (string n, float w, float s, float f)
    {
        setPropertiest(n, w, s, f);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
}
