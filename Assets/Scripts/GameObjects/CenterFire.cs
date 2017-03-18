using UnityEngine;
using System.Collections;

public class CenterFire : MonoBehaviour {

    private float maxDistance = 1.6f;
    private float speed = .5f;
    private float number;
    public float torchNumber;
    private bool turnedOn;
    private bool fullRendered;
    private float startFadeInTime;
    private float startFadeOutTime;
    private readonly float FADE_DURATION = 1f;
    private float fullShade = 1f;
    private static readonly float MAX_LIGHT_RANGE = 1.4f;
    public AudioClip fireSpawn;
    public AudioClip fireSnuff;
    private bool fadingOut = false;

    public bool TurnedOn {
        get {
            return turnedOn;
        }

        set {
            turnedOn = value;
        }
    }

    private void OnEnable() {
        fadingOut = false;
        transform.GetComponentInChildren<Light>().enabled = true;
        startFadeInTime = Time.time;
        GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, 0f );
    }

    public void TurnOn() {
        if ( !TurnedOn ) {
            fullRendered = false;
            gameObject.SetActive( true );
            TurnedOn = true;
            GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine( PlayFireSpawnAfterDelay() );
        }
    }

    public void TurnOff() {
        if ( TurnedOn ) {
            fadingOut = true;
            startFadeOutTime = Time.time;
            PlayFireSnuff();
            TurnedOn = false;
            StartCoroutine( DeactivateAfterDelay() );
        }
    }

    private IEnumerator DeactivateAfterDelay() {
        yield return new WaitForSeconds( FADE_DURATION );
        transform.GetComponentInChildren<Light>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        gameObject.SetActive( false );
    }

    private IEnumerator PlayFireSpawnAfterDelay() {
        yield return new WaitForSeconds( .25f );
        if ( !GameObject.Find( "CenterFireSound" ).GetComponent<AudioSource>().isPlaying ) {
            GameObject.Find( "CenterFireSound" ).GetComponent<AudioSource>().volume -= ( ( .2f ) * ( Mathf.Abs( Vector3.Distance( gameObject.transform.position, GameManager.instance.GetHero().transform.position ) ) ) );
            GameObject.Find( "CenterFireSound" ).GetComponent<AudioSource>().PlayOneShot( fireSpawn );
        }
    }

    private void PlayFireSnuff() {

        SnuffAndBurnSounds.instance.PlaySnuff();

        //getcomponent<audiosource>().volume -= ( ( .05f ) * ( mathf.abs( vector3.distance( gameobject.transform.position, gamemanager.instance.gethero().transform.position ) ) ) );
        //getcomponent<audiosource>().playoneshot( firesnuff );
    }

    void Start() {
        GetComponent<SpriteRenderer>().enabled = false;
        GameManager.instance.RegisterCenterFire( this );
        gameObject.SetActive( false );
    }

    void Update() {
        if ( fadingOut ) {
            FadeOut();
            return;
        }
        if ( TurnedOn && !fullRendered && GetComponent<SpriteRenderer>().color.a != fullShade ) {
            FadeIn();
        }
        else if ( TurnedOn && !fullRendered ) {
            fullRendered = true;
        }
        if ( fullRendered && TurnedOn ) {
            transform.GetComponentInChildren<Light>().range = PingPong( Time.time * speed, 1.4f, maxDistance );
        }
        if ( TurnedOn && !SnuffAndBurnSounds.instance.efxSource.isPlaying ) {
            SnuffAndBurnSounds.instance.PlayBurning();
        }

    }

    private void LateUpdate() {
        GetComponent<SpriteRenderer>().sortingOrder = ( Mathf.RoundToInt( ( transform.position.y - 1f ) * 100f ) * -1 ) - 55;
    }

    private void FadeIn() {
        float t = ( Time.time - startFadeInTime ) / FADE_DURATION;
        GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, fullShade, t ) );
        transform.GetComponentInChildren<Light>().range = Mathf.SmoothStep( 0f, MAX_LIGHT_RANGE, t );
        // GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, fullShade, t ) );
    }

    private void FadeOut() {
        float t = ( Time.time - startFadeOutTime ) / FADE_DURATION;
        GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( fullShade, 0f, t ) );
        transform.GetComponentInChildren<Light>().range = Mathf.SmoothStep( MAX_LIGHT_RANGE, 0f, t );
        // GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, fullShade, t ) );
    }

    private float PingPong( float aValue, float aMin, float aMax ) {
        return Mathf.PingPong( aValue, aMax - aMin ) + aMin;
    }
}
