using UnityEngine;
using System.Collections;

public class Arena : MonoBehaviour {
    private static readonly float FADE_DURATION = .001f;
    private float startTime;

    // Use this for initialization
    void Start () {
        startTime = Time.time;

    }
	
	// Update is called once per frame
	void Update () {
        float t = ( Time.time - startTime ) / FADE_DURATION;
        transform.GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, 1f, t ) );
    }
}
