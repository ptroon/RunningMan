using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileManager : MonoBehaviour
{
    public GameObject PlainTile, DangerTile, JumpTile, ScoreTile;

    public enum ETileType {plain, danger, jump, score};     // tile can one of these only.

    // for showing in the Inspector

    public enum ETileComplexity { EASY=1, NORMAL, MEDIUM, HARD, SUPERHARD};      // used to define the complexity of the tile mapping
    public ETileComplexity tileComplexity = ETileComplexity.EASY;                // default tile complexity

    private Dictionary<int, GameObject> tileArray; // List of tiles to be generated.

    private System.Random randomNumber; // random number generator for tile type;
    private int randomInt;              // holds the random number

    private int dangerCounter = 0;      // counter for danger tiles - cannot ever go beyond 2 or there would be 3 danger tiles side-by-side
    private static int MAX_DANGER_TILES = 2;
    private static int TILE_CHUNK = 10;
    private int MAX_LENGTH = 100;

    private float xx = 0, yy = 0;
    private int tileCount = 0; //tileIndex = 0; // overall count for the Dictionary so we can add key/value pairs & the index we use for the highest value


    void Awake()
    {
        tileArray = new Dictionary<int, GameObject>();
        randomNumber = new System.Random();
    }

    public void SetComplexity (ETileComplexity complexity)
    {
        tileComplexity = complexity;
    }


    public void BuildPlatform ()
    {
        xx = 0;
        yy = 0;
        GameObject tObj;
        tileCount = 1;
        dangerCounter = 0;
        CleanList();

        //yy = randomNumber.Next(1, 3);

        // builds the platform of X number of tiles long, with specific types of tiles depending on complexity
        // first build a section of tiles that are plain - so that the character can land on them and get going - too hard if the player drops straight onto a danger tile.
        for (var a = 1; a <= TILE_CHUNK; a++)
        {
            // get the tile instance into a temp variable
            tObj = GetTile(ChooseTileType(1), xx++, yy);
            // save the index into the TileManager key variable so we can look it up with OnCollisionEnter2D
            tObj.GetComponent<TileManager>().key = a;
            tileArray.Add(a, tObj); // draw a plain tile only.
        }

        for (var a=TILE_CHUNK+1; a <= MAX_LENGTH + TILE_CHUNK; a++) // draw the tiles
        {
            randomInt = randomNumber.Next(1, 100);
            // check the tile counter
            if (dangerCounter >= MAX_DANGER_TILES) // no matter what, set to plain tile after danger counter MAX_DANGER_TILES reached.
            {
                randomInt = 1;      // set to a low number so it will definitely be a plain tile
                dangerCounter = 0;  // reset
            }
            tObj = GetTile(ChooseTileType(randomInt), xx++, yy);
            // save the index into the TileManager key variable so we can look it up with OnCollisionEnter2D
            tObj.GetComponent<TileManager>().key = a;
            tileArray.Add(a, tObj); // generate tile object to screen co-ords and also save to the List
            tileCount = a;
        } // LOOP

        // tileIndex = tileCount; // set the index of highest tile to the count
    }

    public void CleanList ()
    {
        GameObject tObj;

        if (tileArray.Count >= 1)
        {
            // Destroy all objects in list and remove the list entry as well - use to initialise the screen by removing all tiles not yet encountered, but player is dead.
            foreach(var key in tileArray.Keys)
            {
                if (tileArray.TryGetValue(key, out tObj))
                {
                    if (tObj)
                    {
                        tObj.GetComponent<BoxCollider2D>().enabled = false;
                        Destroy(tObj);
                    }
                }
            }
            tileArray.Clear();
            Debug.Log("Remaining " + tileArray.Count + " records");
        }
        tileCount = 0;
    }

    public void RemoveTileFromList (GameObject tile)
    {
        if (tile)
        {
            tileArray.Remove(tile.GetComponent<TileManager>().key);
            Destroy(tile);
            // tileCount--;
        }
    }

    public void AppendTileChunk ()
    {
        GameObject tObj;
        if (Time.timeScale == 1f)
        {
            // create a chunk of tiles that will be added to the platform to keep the platform endless..
            if (tileArray.Count <= (MAX_LENGTH - TILE_CHUNK))
            {
                // Debug.Log("Count is " + tileCount);
                for (var a = 1; a <= TILE_CHUNK; a++)
                {
                    tileCount++;
                    // tileIndex++;
                    tObj = GetTile(ChooseTileType(randomNumber.Next(1, 100)), xx++, yy);
                    tObj.GetComponent<TileManager>().key = tileCount;
                    tileArray.Add(tileCount, tObj);
                }
            }
        }
    }

    ETileType ChooseTileType (int tileNumber)
    {
        // determine the tile type by working out complexity and what is nearby.
        // EASY:        tiles are mostly plain with 20% danger tiles - no jumps.
        // NORMAL:      tiles are mostly plain with a few jumps and 20% danger tiles.
        // MEDIUM:      tiles are a mix of plain and jumps with 33% danger tiles.
        // HARD:        tiles are a mix of plain and jumps with <=50% danger tiles.
        // SUPERHARD:   tiles are a mix of plain and jumps with many danger tiles (50%+)
        //

        ETileType vTileType = ETileType.plain;

        switch (tileComplexity)
        {
            case ETileComplexity.EASY:      vTileType = CalculateTileType(tileNumber, 80, 100); break;
            case ETileComplexity.NORMAL:    vTileType = CalculateTileType(tileNumber, 80, 95); break;
            case ETileComplexity.MEDIUM:    vTileType = CalculateTileType(tileNumber, 75, 95); break;
            case ETileComplexity.HARD:      vTileType = CalculateTileType(tileNumber, 66, 90); break;
            case ETileComplexity.SUPERHARD: vTileType = CalculateTileType(tileNumber, 40, 75); break;
        }
        return vTileType;
    }

    private ETileType CalculateTileType(int tileNumber, int maxPlain, int maxDanger)
    {
        ETileType vTileType;

        if (tileNumber >= 0 && tileNumber <= maxPlain)
        {
            vTileType = ETileType.plain;
        }
        else
        if (tileNumber > maxPlain && tileNumber <= maxDanger)
        {
            vTileType = ETileType.danger; dangerCounter++;
        }
        else vTileType = ETileType.jump;
        return vTileType;
    }

    private GameObject GetTile (ETileType eTileType, float x, float y)
    {
        // Depending on the type of tile - instantiate prefabs that refer to each tile type at the location speficifed.
        GameObject gameObject = null;
        int jumpHeight;
        List<GameObject> objectList = new List<GameObject>(); // list of game objects to return back

        if (eTileType == ETileType.plain)   { gameObject = Instantiate(PlainTile, new Vector3(x, y, 0f), transform.rotation);  }
        if (eTileType == ETileType.danger)  { gameObject = Instantiate(DangerTile, new Vector3(x, y, 0f), transform.rotation); }

        if (eTileType == ETileType.jump) // can create a 2+ tile jump obstacle
        {
            jumpHeight = randomNumber.Next(1, 2);
            for (var a = 0; a <= jumpHeight; a++)
            {
                objectList.Add(Instantiate(JumpTile, new Vector3(x, y+a, 0f), transform.rotation)); // first
                if (gameObject)
                {
                    objectList[objectList.Count-1].transform.parent = gameObject.transform; // set new object to parent of previous object
                }
                gameObject = objectList[objectList.Count-1]; // set gameObject to new object
            }
            gameObject = objectList[0];
        }
        return gameObject;
    }

}
