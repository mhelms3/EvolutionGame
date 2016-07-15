using UnityEngine;
using System.Collections;

public class gameBoardScript : MonoBehaviour {

    public GameObject[] myTerrainTypes;
    public GameObject[,] myBoardSquares;

    public GameObject terrainPrototype;
    public GameObject boardSquarePrototype;

    public int boardSizeX = 10;
    public int boardSizeY = 10;

    public int numberOfTerrains = 2;
    public string[] terrainNames = new string[2];
    public int[] terrainValues= {20, 20, 100, 20, 100, 20};

    public GameObject tileHill;
    public GameObject tilePlain;



    //public SpriteRenderer tileSprite;

    float generateResources(float average)
    {
        float newResources = 0;
        newResources = Mathf.RoundToInt(average + Random.Range(-10.0f, 10.0f) + Random.Range(-10.0f, 10.0f) + Random.Range(-10.0f, 10.0f));
        
        if (newResources < 1)
            newResources = 1;
        return (newResources);
    }




    // Use this for initialization
    void Start () {

        //tileSprite = new SpriteRenderer();
        //tileSprite.sprite = Resources.LoadAssetAtPath("Assets/Sprites/tiles.png", Sprite);
        //if (tileSprite.sprite == null)
        //Debug.Log("TS Null");

        string tempName;
        int[] props = new int[3];
        terrainNames[0] = "Plains";
        terrainNames[1] = "Hills";
        TerrainProperties terrainProps;
        TileProperties tileProps;
        SpriteRenderer tileSR, terrainSR;

        


        Debug.Log("Start");

        myTerrainTypes = new GameObject[numberOfTerrains];
        for (int i = 0; i < numberOfTerrains; i++)
        {
            tempName = terrainNames[i];
            props[0] = terrainValues[i];
            props[1] = terrainValues[i+1];
            props[2] = terrainValues[i+2];

            Debug.Log(terrainValues[0]);
            Debug.Log(props[0]);

            myTerrainTypes[i] = Instantiate(terrainPrototype);
            terrainProps = (TerrainProperties)myTerrainTypes[i].GetComponent(typeof(TerrainProperties));
            terrainProps.setProperties(tempName, props[0], props[1], props[2]);

            //terrainProps.tileSprites[0] = Instantiate(Sprit);


        }

        int totalSquares = boardSizeX * boardSizeY;
        int terrainIndex = 0;
        

        myBoardSquares = new GameObject[boardSizeX, boardSizeY];

        for (int j = 0; j< boardSizeX; j++)
            for (int k = 0; k<boardSizeY; k++)
            {
                terrainIndex = ((j + k) % 2);
                //Debug.Log(terrainIndex);
                myBoardSquares[j, k] = Instantiate(boardSquarePrototype);
                myBoardSquares[j, k].transform.Translate(new Vector3(j, k, 0));



                tileProps = (TileProperties)myBoardSquares[j, k].GetComponent(typeof(TileProperties));
                terrainProps = (TerrainProperties)myTerrainTypes[terrainIndex].GetComponent(typeof(TerrainProperties));
                tileProps.squareTerrain = myTerrainTypes[terrainIndex];
                tileProps.wood = generateResources(terrainProps.woodAverage);
                tileProps.stone = generateResources(terrainProps.stoneAverage);
                tileProps.food = generateResources(terrainProps.foodAverage);
                tileProps.name = terrainProps.name;

                tileSR = (SpriteRenderer)myBoardSquares[j, k].GetComponent(typeof(SpriteRenderer));

                if (tileProps.name == "Hills")
                {
                     terrainSR = (SpriteRenderer)tileHill.GetComponent(typeof(SpriteRenderer));                     
                }
                else
                {
                    terrainSR = (SpriteRenderer)tilePlain.GetComponent(typeof(SpriteRenderer));                    
                }
                tileSR.sprite = terrainSR.sprite;



            }



        Debug.Log("Complete");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
