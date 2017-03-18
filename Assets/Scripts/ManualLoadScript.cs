using UnityEngine;
using System.Collections;

public class ManualLoadScript : MonoBehaviour {

    // Use this for initialization
    void Start() {
        //StartCoroutine( WaitToResurrect() );
        //StartCoroutine( WaitToLoad() );
        // AutoFade.LoadLevel( "main", 6, 1, Color.black );
    }

    private IEnumerator WaitToLoad() {
        yield return new WaitForSeconds( 4 );
        AutoFade.LoadLevel( "main", 2, 2, Color.black );
    }

    // Update is called once per frame
    void Update() {
        if ( Input.anyKey ) {
            AutoFade.LoadLevel( "main", .5f, .5f, Color.black );
        }
    }
}
