using UnityEngine;
using System.Collections;

public class IntroLogic : MonoBehaviour {

    private bool skipped = false;

    // Use this for initialization
    void Start() {
        //StartCoroutine( WaitToResurrect() );
        //StartCoroutine( WaitToLoad() );
        // AutoFade.LoadLevel( "main", 6, 1, Color.black );
        skipped = false;
    }

    // Update is called once per frame
    void Update() {
        if ( !AutoFade.Fading && !skipped && ( Input.GetButtonDown( "Esc" ) || Input.GetButtonDown( "attack" ) ) ) {
            skipped = true;
            AthenaIntro.instance.SetResurrecting();
            AutoFade.LoadLevel( "main", 3, .1f, Color.black );
        }
    }

}
