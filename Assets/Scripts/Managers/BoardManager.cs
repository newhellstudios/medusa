using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    //1 tile padding on each side
    //9*3
    //5*3
    //6*3 = 18
    //20 * 20


    //27 - 1728
    //20 - 1280

    private int columns = 27;
    private int rows = 21;
    private readonly int COLUMNS_PILLARS = 6;
    private readonly int ROWS_PILLARS = 4;


    public GameObject floorTile;                                //Floor tile prefab
    public GameObject pillarTile;                               //Pillar tile prefab
    public GameObject medusaTile;                               //Medusa tile prefab
    public GameObject headTile;
    public GameObject athenaTile;
    public GameObject poseidonTile;
    public GameObject leftWall;
    public GameObject topLeftWallCorner;
    public GameObject topWall;
    public GameObject topRightWallCorner;
    public GameObject rightWall;
    public GameObject bottomRightWallCorner;
    public GameObject bottomWall;
    public GameObject bottomLeftWallCorner;
    public GameObject heroTile;
    public GameObject pointLightTile;
    public GameObject medusaLight;
    public GameObject athenaLight;
    public GameObject snakesTile;
    public GameObject spawnPoint;

    private Transform boardHolder;                              //A variable to store a reference to the transform of our Board object.
    private GameObject heroHolder;                              //A variable to store a reference to the transform of our Hero object.
    private GameObject medusaHolder;
    private GameObject athenaHolder;
    private GameObject poseidonHolder;
    private GameObject medusaLightHolder;
    private GameObject heroLightHolder;
    private GameObject athenaLightHolder;
    private List<GameObject> spawnPointsHolder = new List<GameObject>();

    private GameObject LayoutObjectAtPoint( GameObject tile, float x, float y ) {
        return Instantiate( tile, new Vector3( x, y, 0f ), Quaternion.identity ) as GameObject;
    }

    private GameObject LayoutAthenaAtPoint( GameObject tile, float x, float y ) {
        return Instantiate( tile, new Vector3( x, y, -1f ), Quaternion.identity ) as GameObject;
    }

    private GameObject LayoutPoseidonAtPoint( GameObject tile, float x, float y ) {
        return Instantiate( tile, new Vector3( x, y, -1f ), Quaternion.identity ) as GameObject;
    }

    public void SetupScene() {
        boardHolder = new GameObject( "Board" ).transform;
        SpawnPoints.InitializeList();
        foreach (float[] point in SpawnPoints.spawnPoints) {
            GameObject tempPoint = LayoutObjectAtPoint( spawnPoint , point[0], point[1]);
            spawnPointsHolder.Add ( tempPoint );
            tempPoint.transform.SetParent( boardHolder );
            GameManager.instance.RegisterSpawnPoint( tempPoint );
        }
        SpawnPoseidon(0f, 0f);
        heroHolder = LayoutObjectAtPoint( heroTile, ( 13.9f ), ( 12.501f ) );
        heroLightHolder = SpawnLight( pointLightTile, heroHolder );
        GameManager.instance.RegisterHeroLight( heroLightHolder );
        medusaHolder = LayoutObjectAtPoint( medusaTile, ( 15.3f ), ( 12.501f ) );
    }

    public void SpawnAthena( float x, float y ) {
        athenaHolder = LayoutAthenaAtPoint( athenaTile, x, y );
    }

    public void SpawnPoseidon( float x, float y ) {
        poseidonHolder = LayoutPoseidonAtPoint( poseidonTile, x, y );
    }

    public void SpawnSnakes( float x, float y ) {
        LayoutObjectAtPoint( snakesTile, x, y );
    }

    private GameObject SpawnLight( GameObject lightTile, GameObject parentTile ) {
        GameObject light = LayoutObjectAtPoint( lightTile, parentTile.transform.position.x, parentTile.transform.position.y );
        light.transform.SetParent( parentTile.transform );
        Vector3 temp = light.transform.position;
        temp.z = -1f;
        light.transform.position = temp;
        return light;
    }

    public void SpawnMedusaLight() {
        medusaLightHolder = SpawnLight( medusaLight, medusaHolder );
        GameManager.instance.RegisterMedusaLight( medusaLightHolder );
    }

    public void SpawnHead( float x, float y ) {
        LayoutObjectAtPoint( headTile, x, y );
    }
}