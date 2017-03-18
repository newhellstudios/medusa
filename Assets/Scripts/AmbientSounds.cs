using UnityEngine;
using System.Collections;

public class AmbientSounds : MonoBehaviour {

    public AudioClip ambient;
    public AudioSource efxSource;
    public static AmbientSounds instance = null;
    private static readonly float VOLUME = 1f;

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
    }


    // Use this for initialization
    void Start() {
        efxSource.clip = ambient;
        efxSource.Play();
        efxSource.loop = true;
    }

    // Update is called once per frame
    void Update() {

    }
}
