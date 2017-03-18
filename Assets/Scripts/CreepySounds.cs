using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepySounds : MonoBehaviour {

    public AudioClip creepy1;
    public AudioClip creepy2;
    public AudioClip creepy3;
    public AudioClip creepy4;

    public AudioSource efxSource;
    public static CreepySounds instance = null;
    private static readonly float VOLUME = 1f;
    private List<AudioClip> clips;

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
        clips = new List<AudioClip>();
    }

    private void BuildList() {

        clips.Add( creepy1 );
        clips.Add( creepy2 );
        clips.Add( creepy3 );
        clips.Add( creepy4 );
    }

    IEnumerator PlayRandomClipInTime() {

        while ( true ) {
            yield return new WaitForSeconds( Random.Range( 10, 15 ) );
            PlayRandomClipAndRemove();
        }
    }

    private void PlayRandomClipAndRemove() {
        if ( clips.Count == 0 ) {
            BuildList();
        }
        int clipToRemove = Random.Range( 0, clips.Count - 1 );
        efxSource.PlayOneShot( clips[clipToRemove], .8f );
        clips.RemoveAt( clipToRemove );
    }


    // Use this for initialization
    void Start() {
        StartCoroutine( PlayRandomClipInTime() );
    }

    // Update is called once per frame
    void Update() {

    }
}
