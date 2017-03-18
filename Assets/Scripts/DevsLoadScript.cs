using UnityEngine;
using System.Collections;

public class DevsLoadScript : MonoBehaviour {

    // Use this for initialization
    void Start() {
        //StartCoroutine( WaitToResurrect() );
        //StartCoroutine( WaitToLoad() );
        // AutoFade.LoadLevel( "main", 6, 1, Color.black );
    }

    // Update is called once per frame
    void Update() {
        if ( Input.anyKey ) {
            AutoFade.LoadLevel( "main", .5f, .5f, Color.black );
        }
    }
}
