using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
    public GameObject soundManager;
    public GameObject musicManager;
    public GameObject mainCamera;

    void Awake() {

        if ( GameManager.instance == null ) {
            Instantiate( gameManager );
        }
        if ( MainCamera.instance == null ) {
            Instantiate( mainCamera );
        }
        if ( SoundManager.instance == null ) {
            Instantiate( soundManager );
        }
        if ( MusicManager.instance == null ) {
            Instantiate( musicManager );
        }
    }

}