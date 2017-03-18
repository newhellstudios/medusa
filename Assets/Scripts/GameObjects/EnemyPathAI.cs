using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathAI : MonoBehaviour {

    public static EnemyPathAI instance;

    //The point to move to
    public Vector3 target;

    private Seeker seeker;

    //The calculated path
    public Path path;

    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    public void Start() {

        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }

        seeker = GetComponent<Seeker>();

        //Start a new path to the targetPosition, return the result to the OnPathComplete function

         InvokeRepeating( "RunPath", 0f, .25f );
    }


    private void RunPath() {
        target = GameManager.instance.GetEnemy().CurrentTarget;
        seeker.StartPath( transform.position, target, OnPathComplete );
    }

    public void OnPathComplete( Path p ) {
        if ( !p.error ) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }

    public void RunUpdate() {
        if ( path == null ) {
            //We have no path to move after yet
            return;
        }

        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        try {
            if ( Vector2.Distance( transform.position, path.vectorPath[currentWaypoint] ) < nextWaypointDistance ) {
                currentWaypoint++;
            }
        }
        catch (ArgumentOutOfRangeException e) {
            Debug.Log(e);
            Debug.Log( "Shit, currentwaypoint is " + currentWaypoint + " but size of path.vectorPath.Count is " + path.vectorPath.Count );
        }

        if ( currentWaypoint >= path.vectorPath.Count ) {
            return;
        }

        //Direction to the next waypoint
        Vector2 dir2 = ( path.vectorPath[currentWaypoint] - transform.position ).normalized;
        float newX = 0f;
        float newY = 0f;
        if (Mathf.Abs(dir2.x) > Mathf.Abs( dir2.y ) ) {
            newX = Mathf.Sign( dir2.x );
            newY = 0f;
        }
        else {
            newX = 0f;
            newY = Mathf.Sign( dir2.y );
        }
        Vector2 newDir = new Vector2( newX, newY );
        try {
            GameManager.instance.GetEnemy().CurrentOrientation
                = GameManager.instance.GetEnemy().VectorOrientationDictionary.vDirToOrientationDictionary[newDir];

        }
        catch ( KeyNotFoundException knf ) {
           GameManager.instance.GetEnemy().CurrentOrientation = OrientationEnum.DOWN;
        }

    }
}