using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpawnPoints {

    public static readonly float[] SPAWN_TOP_LEFT = { 3, 8 };
    public static readonly float[] SPAWN_TOP_RIGHT = { 12, 8 };
    public static readonly float[] SPAWN_BOTTOM_LEFT = { 3, 4 };
    public static readonly float[] SPAWN_BOTTOM_RIGHT = { 12, 4 };
    private static readonly float LEFTMOST_X = 6.5f;
    private static readonly float BOTTOMMOST_Y = 5f;
    private static readonly float DIFFERENCE_X = 4f;
    private static readonly float DIFFERENCE_Y = 3.5f;
    private static float MINIMUM_DIFFERENCE = 2f;
    public static List<float[]> pointsToSpawn = new List<float[]>();
    public static List<float[]> spawnPoints = new List<float[]>();
    private static readonly float MAX_DISTANCE_SPAWN = 6.5f;
    private static readonly float MINUS_MAX_DISTANCE_SPAWN = 2f;

    private static void ReinitializeList() {
        pointsToSpawn.Clear();
        pointsToSpawn = new List<float[]> {
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y * 2 } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y * 2 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y * 4 }
        };
    }

    public static void InitializeList() {
        spawnPoints.Clear();
        spawnPoints = new List<float[]> {
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y * 2 } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 2, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 3, BOTTOMMOST_Y + DIFFERENCE_Y * 4 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y * 2 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y * 3 } ,
            new float[]{ LEFTMOST_X + DIFFERENCE_X * 4, BOTTOMMOST_Y + DIFFERENCE_Y * 4 }
        };
    }


    public static float[] GetSpawnPointRelative( float[] currentPosition ) {
        ReinitializeList();
        System.Random random = new System.Random();

        List<float> listOfDifferences = new List<float>();

        foreach ( float[] spawnPoint in pointsToSpawn ) {
            float differenceX = Mathf.Abs( GameManager.instance.GetHero().transform.position.x - spawnPoint[0] );
            float differenceY = Mathf.Abs( GameManager.instance.GetHero().transform.position.y - spawnPoint[1] );
            listOfDifferences.Add( differenceX + differenceY );
        }

        for ( int i = 0; i < listOfDifferences.Count; i++ ) {
            if ( listOfDifferences[i] <= listOfDifferences.Min() + MAX_DISTANCE_SPAWN || listOfDifferences[i] >= listOfDifferences.Max() - MINUS_MAX_DISTANCE_SPAWN ) {
                pointsToSpawn[i] = null;
            }
        }

        foreach(float[] point in pointsToSpawn.ToList() ) {
            if (point == null) {
                pointsToSpawn.Remove( point );
            }
        }

        return pointsToSpawn.ElementAt( random.Next( pointsToSpawn.Count ) );
    }

}