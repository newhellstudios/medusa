using UnityEngine;
using System.Collections;

public class GrabberLight : MonoBehaviour {

    private bool needsLightup = false;
    private bool needsReduction = false;
    private static readonly float MAX_RANGE = 60f;
    private static readonly float DURATION_INCREASE = .5f;
    private static readonly float DURATION_DECREASE = .7f;
    public static GrabberLight instance = null;
    private Light fireLight;
    private float reduceStartTime;
    private float increaseStartTime;

    // Use this for initialization
    void Start() {
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
        fireLight = transform.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update() {
        if ( !needsReduction && fireLight.range == MAX_RANGE ) {
            reduceStartTime = Time.time;
            needsLightup = false;
            needsReduction = true;
        }
        if ( needsLightup ) {
            float tUp = ( Time.time - increaseStartTime ) / DURATION_INCREASE;
            fireLight.range = Mathf.SmoothStep( 0f, MAX_RANGE, tUp );
        }
        if ( needsReduction ) {
            float tDown = ( Time.time - reduceStartTime ) / DURATION_DECREASE;
            fireLight.range = Mathf.SmoothStep( MAX_RANGE, 0f, tDown );
        }

    }

    public void LightUp() {
        increaseStartTime = Time.time;
        needsLightup = true;
        needsReduction = false;
    }
}
