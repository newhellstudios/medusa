using UnityEngine;
using System.Collections;

public class LoadMenu : MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine( WaitToLoad() );
        // AutoFade.LoadLevel( "main", 6, 1, Color.black );
    }

    private IEnumerator WaitToLoad() {
        yield return new WaitForSeconds( 3.5f );
        AutoFade.LoadLevel( "intro", 3, 1, Color.black );
    }

    // Update is called once per frame
    void Update() {

    }
}
