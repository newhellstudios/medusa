using UnityEngine;
using System.Collections;

public class Poem : MonoBehaviour {

    public float speed = .05f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate( Vector3.up * Time.deltaTime * speed );
	}

}
