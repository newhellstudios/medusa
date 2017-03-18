using UnityEngine;
using System.Collections;

public class FlamesText : MonoBehaviour {

    private float maxDistance = 1.6f;
    private float speed = .5f;
    private bool turnedOn;
    private bool fullRendered;
    private float startFadeInTime;
    private float startFadeOutTime;
    private readonly float FADE_IN_DURATION = 1f;
    private readonly float FADE_DURATION = 2f;
    private readonly float TEXT_MOVE_DURATION = 2f;
    private float fullShade = .8f;
    private bool fadingOut = false;
    private bool movingUp = false;
    private Vector3 startingPosition;
    public static FlamesText instance = null;

    public bool TurnedOn
    {
        get
        {
            return turnedOn;
        }

        set
        {
            turnedOn = value;
        }
    }

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
    }

    private void OnEnable() {
        startingPosition = transform.position;
        fadingOut = false;
        startFadeInTime = Time.time;
        GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, 0f );
    }

    public void TurnOn() {
        if ( !TurnedOn ) {
            fullRendered = false;
            gameObject.SetActive( true );
            TurnedOn = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void TurnOff() {
        if ( TurnedOn ) {
            fadingOut = true;
            startFadeOutTime = Time.time;
            TurnedOn = false;
           // StartCoroutine( DeactivateAfterDelay() );
        }
    }

    private IEnumerator DeactivateAfterDelay() {
        yield return new WaitForSeconds( FADE_DURATION );
        GetComponent<SpriteRenderer>().enabled = false;
        gameObject.SetActive( false );
    }

    void Start() {
        GetComponent<SpriteRenderer>().enabled = false;
        gameObject.SetActive( false );
    }

    void Update() {
        if ( fadingOut ) {
            FadeOut();
            // return;
        }
        if ( TurnedOn && !fullRendered && GetComponent<SpriteRenderer>().color.a != fullShade ) {
            FadeIn();
        }
        else if ( TurnedOn && !fullRendered ) {
            fullRendered = true;
        }
        if ( TurnedOn ) {
            float t = ( Time.time - startFadeInTime ) / TEXT_MOVE_DURATION;
            transform.position = new Vector3( startingPosition.x, Mathf.SmoothStep( startingPosition.y, startingPosition.y + 1.5f, t ) );
        }
        if ( fullRendered && !fadingOut ) {
            startFadeOutTime = Time.time;
            fadingOut = true;
        }

    }

    private void FadeIn() {
        float t = ( Time.time - startFadeInTime ) / FADE_IN_DURATION;
        GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, fullShade, t ) );
    }

    private void FadeOut() {
        float t = ( Time.time - startFadeOutTime ) / FADE_DURATION;
        GetComponent<SpriteRenderer>().color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( fullShade, 0f, t ) );
    }

}
