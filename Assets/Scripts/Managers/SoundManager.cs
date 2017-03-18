using UnityEngine;
using System.Collections;
public class SoundManager : MonoBehaviour {
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.  
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
    private static readonly float VOLUME = .7f;
    private static readonly float REVERB = .1f;
    private static readonly float AMPLITUDE = .05f;
    private static readonly float REVERB_AMPLITUDE = .10f;


    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
        efxSource.volume = VOLUME;
        efxSource.reverbZoneMix = REVERB;
    }

    public void PlaySingle( AudioClip clip ) {       
        efxSource.PlayOneShot( clip );
    }

    public void RandomizeSfx( float distance, params AudioClip[] clips ) {
        efxSource.volume = VOLUME;
        efxSource.reverbZoneMix = REVERB;
        int randomIndex = Random.Range( 0, clips.Length );  
        float randomPitch = Random.Range( lowPitchRange, highPitchRange );   
        efxSource.pitch = randomPitch;
        efxSource.volume -= (distance * AMPLITUDE );
        efxSource.reverbZoneMix += ( distance * REVERB_AMPLITUDE );
        efxSource.PlayOneShot( clips[randomIndex] );
    }

    public void PlaySfxNoPitching( float distance, AudioClip clip ) {
        efxSource.pitch = 1f;
        efxSource.volume = VOLUME;
        efxSource.reverbZoneMix = REVERB;
     
        efxSource.PlayOneShot( clip );
    }

}