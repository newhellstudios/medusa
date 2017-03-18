using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static MusicManager instance = null;     //Allows other scripts to call functions from SoundManager.   
    public AudioClip gameSong;
    public AudioClip menuVictorySong;
    private bool fadingOut;

    float audio1Volume = .5f;
    float audio2Volume = 0.0f;
    private bool fadingIn;

    public bool FadingOut {
        get {
            return fadingOut;
        }

        set {
            fadingOut =  value ;
        }
    }

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
        DontDestroyOnLoad( gameObject );
        musicSource.reverbZoneMix = 0f;
        if ( SceneManager.GetActiveScene().name == "main" || SceneManager.GetActiveScene().name == "intro" ) {
            VictorySong();
        }
        else {
            GameSong();
        }
    }

    public IEnumerator SetFadeBackIn() {
        yield return new WaitForSeconds(2f);
        FadingOut = false;
        fadingIn = true;
    }

    void Update() {
        if ( FadingOut ) {
            fadeOut();
        }
        else if (fadingIn) {
            fadeIn();
        }
    }

    void fadeIn() {
        if ( audio1Volume < .5 ) {
            audio1Volume += (float) 0.1 * Time.deltaTime;
            GetComponent<AudioSource>().volume = audio1Volume;
        }
    }

    void fadeOut() {
        if ( audio1Volume > 0.1 ) {
            audio1Volume -= (float)0.1 * Time.deltaTime;
            GetComponent<AudioSource>().volume = audio1Volume;
        }
    }

    public void VictorySong() {
        if ( musicSource.clip == menuVictorySong ) {
            return;
        }
        else {
            musicSource.clip = menuVictorySong;
            musicSource.Play();
        }

    }

    public void GameSong() {
        musicSource.clip = gameSong;
        musicSource.Play();
    }

}