using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainCamera : MonoBehaviour {

    public static MainCamera instance = null;
    private Vector3 offset;

    void Awake() {
        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }
    }

    private void Start() {
        offset = transform.position;
    }

    private void LateUpdate() {
        if ( GameManager.instance.GetHero() ) {
            transform.position = GameManager.instance.GetHero().transform.position + offset;
        }
    }
}