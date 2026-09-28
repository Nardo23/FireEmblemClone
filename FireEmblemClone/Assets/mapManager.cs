using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class mapManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoundsInt bounds = ourTileMap.cellBounds;
        tileInfo = new Dictionary<TileBase, TileData>();
        foreach(var tileData in tileTypes)
        {
            foreach (var tile in tileData.tiles)
            {
                tileInfo.Add(tile, tileData);
            }
        }
        Debug.Log(ourTileMap.WorldToCell(testCharacter.transform.position));
        getActualRange(ourTileMap.WorldToCell(testCharacter.transform.position));
    }
    public GameObject testCharacter;
    public Tilemap ourTileMap;
    Dictionary<TileBase, TileData> tileInfo;
    [SerializeField]
    List<TileData> tileTypes;

   public List<Vector3Int> getNeighborTiles(Vector3 startingLocation) // returns the positions of the neighbouring tiles of a given tile
   {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        //top
        Vector3Int locationToCheck = new Vector3Int((int)startingLocation.x, (int)startingLocation.y+1,0);

        if(ourTileMap.GetTile(locationToCheck)!= null)
        {
            neighbors.Add(locationToCheck);
        }
        //bottom
        locationToCheck = new Vector3Int((int)startingLocation.x, (int)startingLocation.y -1, 0);

        if (ourTileMap.GetTile(locationToCheck) != null)
        {
            neighbors.Add(locationToCheck);
        }
        //right
        locationToCheck = new Vector3Int((int)startingLocation.x+1, (int)startingLocation.y, 0);

        if (ourTileMap.GetTile(locationToCheck) != null)
        {
            neighbors.Add(locationToCheck);
        }
        //left
        locationToCheck = new Vector3Int((int)startingLocation.x - 1, (int)startingLocation.y, 0);

        if (ourTileMap.GetTile(locationToCheck) != null)
        {
            neighbors.Add(locationToCheck);
        }
        Debug.Log("neighbor Length: " + neighbors.Count);
        return neighbors;
   }
    public List<Vector3Int> getNeighborsMulti(List<Vector3Int> tiles)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        foreach(Vector3Int v in tiles)
        {
            Vector3Int locationToCheck = new Vector3Int((int)v.x, (int)v.y + 1, 0);

            if (ourTileMap.GetTile(locationToCheck) != null)
            {
                neighbors.Add(locationToCheck);
            }
            //bottom
            locationToCheck = new Vector3Int((int)v.x, (int)v.y - 1, 0);

            if (ourTileMap.GetTile(locationToCheck) != null)
            {
                neighbors.Add(locationToCheck);
            }
            //right
            locationToCheck = new Vector3Int((int)v.x + 1, (int)v.y, 0);

            if (ourTileMap.GetTile(locationToCheck) != null)
            {
                neighbors.Add(locationToCheck);
            }
            //left
            locationToCheck = new Vector3Int((int)v.x - 1, (int)v.y, 0);

            if (ourTileMap.GetTile(locationToCheck) != null)
            {
                neighbors.Add(locationToCheck);
            }
        }

        return neighbors;
    }


    List<Vector3Int> inRangeTiles;


    void rangeFinder(Vector3Int startingTile, int range) // this function finds all tiles within range but ignores terrain type. useful for certain attacks
    {
        int stepCount = 0;

        inRangeTiles.Add(startingTile);
        var tileForPreviousStep = new List<Vector3Int>();
        tileForPreviousStep.Add(startingTile);

        while(stepCount < range)
        {
            var surroundingTiles = new List<Vector3Int>();
            foreach (Vector3Int item in tileForPreviousStep)
            {
                surroundingTiles.AddRange(getNeighborTiles(item));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;

        }

        inRangeTiles = inRangeTiles.Distinct().ToList();

    }
    Dictionary<Vector3Int, float> inRangeDictionary;

    int tempRange =5;
    float tempForestCost =2;
    float tempWallCost =99;
    float tempWaterCost =3;

    List<Vector3Int> tilesSoFar;
    List<Vector3Int> tilesForThisRound = new List<Vector3Int>();
    void getActualRange(Vector3Int startingTile) // this one is suppose to account for terain
    {
        inRangeDictionary = new Dictionary<Vector3Int, float>();
        inRangeDictionary.Add(startingTile, tempRange); // dictionary that stores tiles that are in range and how much movement you have left when you get there

        List<Vector3Int> neighbors = getNeighborTiles(startingTile);
        tilesSoFar = addToInRange(neighbors, tempRange);
        Debug.Log("tilesSoFar: "+tilesSoFar.Count);
        float i = 0;
        float prevI = 99;
        bool foundThisRound =true;
        while (foundThisRound)
        {
            i = 0;
            foundThisRound = false;
                tilesForThisRound.Clear();
            
            foreach(Vector3Int v in tilesSoFar)
            {
                Debug.Log("dictioary value: " + inRangeDictionary[v]);
                if (inRangeDictionary[v] > i&& inRangeDictionary[v]<prevI)
                {
                    foundThisRound = true;
                    i = inRangeDictionary[v];
                }
            }
            Debug.Log("current i: " + i);
            if (!foundThisRound)
                break;
            Debug.Log("highestvalue: " + i);
            foreach(Vector3Int v in tilesSoFar)
            {
                if (inRangeDictionary[v] == i)
                {
                    tilesForThisRound.Add(v);
                    //tilesSoFar.Remove(v);
                }
            }
            tilesForThisRound = addToInRange(getNeighborsMulti(tilesForThisRound), i);
            foreach (Vector3Int v in tilesForThisRound)
            {
                tilesSoFar.Add(v);
            }

            prevI = i;
        }
        showRange();
    }
    public GameObject highlightPrefab;
    //public List<GameObject> highlights;
    void showRange()
    {
        foreach (KeyValuePair<Vector3Int, float> t in inRangeDictionary)
        {

            if (t.Value >= 0)
            {
                GameObject ob = Instantiate(highlightPrefab);
                ob.transform.position = new Vector3(t.Key.x, t.Key.y, 0);
                ob.GetComponent<overlayTile>().value = t.Value;
                //highlights.Add
            }

        }

    }

    List<Vector3Int> addToInRange(List<Vector3Int> tilesToCheck, float moveLeft)
    {
        var tilesAddedThisPass = new List<Vector3Int>();
        TileBase currentTile;
        foreach (Vector3Int v3 in tilesToCheck) //start by searching adding each neighbor of the starting tile
        {
            currentTile = ourTileMap.GetTile(v3);
            float cost = 1;
            if (tileInfo.ContainsKey(currentTile))
            {
                cost = moveLeft - checkTileCost(tileInfo[currentTile].terrainType);

            }
            else // if tile doesnt have data, treat it as ground
            {
                cost = moveLeft - 1;
            }
            if (cost >= 0)// add neighbor to dictionary if value remaining isnt negative
            {
                if (inRangeDictionary.ContainsKey(v3))
                {
                    if (cost > inRangeDictionary[v3])  //change existing value of tile if higher value is found
                    {
                        inRangeDictionary[v3] = cost;
                        tilesAddedThisPass.Add(v3);
                    }
                        
                }
                else
                {
                    inRangeDictionary.Add(v3, cost);
                    tilesAddedThisPass.Add(v3);
                }
                
            }
            // now I have to check the neighbor's neighbors 

            //once every new neighbor's neighbors is not added to the dictionary(either negative or a higher vaule already exists) end the function
        }
        return tilesAddedThisPass;
    }


    float checkTileCost(string terrainType)
    {

        if (terrainType == "Wall")
            return tempWallCost;
        else if (terrainType == "Forest"|| terrainType == "Pillar")
            return tempForestCost;
        else if (terrainType == "Water")
            return tempWaterCost;
        else if (terrainType == "Ground")
            return 1;
        else
        {
            Debug.LogWarning("Terrain type not found: " + terrainType);
            return 1;
        }
        
    }

}
