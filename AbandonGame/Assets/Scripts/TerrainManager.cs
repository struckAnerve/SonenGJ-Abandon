using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour {

    public GameObject verticalTile;
    public GameObject horizontalTile;
    public GameObject cornerTile;
    public GameObject centerTile;

    public GameObject player;

    float tileSize = 500F;

    // Use this for initialization
    void Start () {
        //centerTile.transform.position = verticalTile.transform.position + new Vector3(0, 0, tileSize);

        MoveTilesUp();
        MoveTilesDown();

        MoveTilesRight();
        MoveTilesLeft();

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = player.transform.position;
        Vector3 centerPos = centerTile.transform.position;
        Vector3 relativePlayerPos = playerPos - centerPos;

        float hTs = 0.5f * tileSize; // help variable (half of tileSize)
        
        if (relativePlayerPos.x <= tileSize && relativePlayerPos.z <= tileSize)
        {
            //player is on centerTile
            if (relativePlayerPos.z < hTs)
            {
                MoveTilesDown();
            }
            if (relativePlayerPos.x < hTs)
            {
                MoveTilesLeft();
            }
        } 
        else if (relativePlayerPos.x <= tileSize && relativePlayerPos.z > tileSize)
        {
            //player is on verticalTile
            if (relativePlayerPos.z > tileSize + hTs)
            {
                MoveTilesUp();
            }
            if (relativePlayerPos.x < hTs)
            {
                MoveTilesLeft();
            }
        }
        else if (relativePlayerPos.x > tileSize && relativePlayerPos.z <= tileSize)
        {
            //player is on horisontalTile
            if (relativePlayerPos.z < hTs)
            {
                MoveTilesDown();
            }
            if (relativePlayerPos.x > tileSize + hTs)
            {
                MoveTilesLeft();
            }
        }
        else if (relativePlayerPos.x > tileSize && relativePlayerPos.z > tileSize)
        {
            //player is on cornerTile
            if (relativePlayerPos.z > tileSize + hTs)
            {
                MoveTilesUp();
            }
            if (relativePlayerPos.x > tileSize + hTs)
            {
                MoveTilesLeft();
            }
        }

    }

    void MoveTilesUp()
    {
        // swap centerTile and verticalTile
        GameObject tempTile = centerTile;
        centerTile = verticalTile;
        verticalTile = tempTile;
        
        // move the new verticalTile 
        verticalTile.transform.position   = centerTile.transform.position + new Vector3(0, 0, tileSize);

        // swap cornerTile and horizontalTile
        tempTile = horizontalTile;
        horizontalTile = cornerTile;
        cornerTile = tempTile;

        // move the new cornerTile 
        cornerTile.transform.position = horizontalTile.transform.position + new Vector3(0, 0, tileSize);
    }
    void MoveTilesDown()
    {
        // swap centerTile and verticalTile
        GameObject tempTile = centerTile;
        centerTile = verticalTile;
        verticalTile = tempTile;

        // move the new verticalTile 
        centerTile.transform.position = verticalTile.transform.position - new Vector3(0, 0, tileSize);

        // swap cornerTile and horizontalTile
        tempTile = horizontalTile;
        horizontalTile = cornerTile;
        cornerTile = tempTile;

        // move the new horizontalTile
        horizontalTile.transform.position = cornerTile.transform.position - new Vector3(0, 0, tileSize);
    }
    void MoveTilesRight()
    {
        // swap verticalTile and cornerTile
        GameObject tempTile = verticalTile;
        verticalTile = cornerTile;
        cornerTile = tempTile;

        // move the new cornerTile
        cornerTile.transform.position = verticalTile.transform.position + new Vector3(tileSize, 0, 0);

        // swap cornerTile and verticalTile
        tempTile = centerTile;
        centerTile = horizontalTile;
        horizontalTile = tempTile;

        // move the new centerTile
        horizontalTile.transform.position = centerTile.transform.position + new Vector3(tileSize, 0, 0);
    }

    void MoveTilesLeft()
    {
        // swap verticalTile and cornerTile
        GameObject tempTile = verticalTile;
        verticalTile = cornerTile;
        cornerTile = tempTile;

        // move the new cornerTile
        verticalTile.transform.position = cornerTile.transform.position - new Vector3(tileSize, 0, 0);

        // swap cornerTile and verticalTile
        tempTile = centerTile;
        centerTile = horizontalTile;
        horizontalTile = tempTile;

        // move the new centerTile
        centerTile.transform.position = horizontalTile.transform.position - new Vector3(tileSize, 0, 0);
    }

}
