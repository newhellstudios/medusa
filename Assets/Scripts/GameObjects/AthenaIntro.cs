using UnityEngine;
using System.Collections;

public class AthenaIntro : RenderedObject {

    private float FADE_DURATION = .75f;
    private float FADE_DURATION_OUT = .25f;
    private readonly float INTRO_FADE_DURATION = .25f;
    private float startTime;
    private float fullShade = .5f;
    private bool auraIncreased;
    private readonly static float TIME_BETWEEN_CHARGE_LIGHT = .05f;
    private readonly static float INITIAL_LIGHT_VALUE = 0f;
    private readonly static float MAX_LIGHT_VALUE = 6f;
    private readonly static float IMMEDIATELY = 0f;

    private readonly static float LIGHT_DURATION = 2f;
    public static AthenaIntro instance;
    private bool fullRendered;
    private float fadeOutTime;
    private bool fadingOut;
    private bool startFade;
    public AudioClip staff;
    private bool firstRessurect;

    private void Awake() {
        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }
        base.Start();

    }

    // Use this for initialization
    protected override void Start() {

        gameObject.SetActive( false );

        EnableAtPointIntro( 0f, 1.5f );
    }

    private void OnEnable() {
        startFade = false;
        fullRendered = false;
        fadingOut = false;
        spriteRenderer.color = new Color( 1f, 1f, 1f, 0f );
        //StartCoroutine( DisableSelf( FADE_DURATION_OUT * 2 ) );
        //PlayClipAfterDelay();
    }

    private IEnumerator PlayClipAfterDelay() {
        yield return new WaitForSeconds( .25f );
        GetComponent<AudioSource>().volume -= ( ( .005f ) * ( Mathf.Abs( Vector3.Distance( gameObject.transform.position, GameManager.instance.GetHero().transform.position ) ) ) );
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update() {
        if ( spriteRenderer.color.a != fullShade && !fullRendered && startFade ) {
            FadeIn();
        }
        else {
            if ( spriteRenderer.color.a == fullShade ) {
                fullRendered = true;
            }

            if ( fadingOut ) {
                FadeOut();
            }
        }
    }

    public void SetResurrecting() {
        animator.SetBool( "resurrecting", true );
        //GetComponent<AudioSource>().volume -= ( ( .005f ) * ( Mathf.Abs( Vector3.Distance( gameObject.transform.position, GameManager.instance.GetHero().transform.position ) ) ) );
        GetComponent<AudioSource>().PlayOneShot( staff );
    }

    public IEnumerator SetResurrectingAfterDelay() {
        yield return new WaitForSeconds( FADE_DURATION + INTRO_FADE_DURATION );
        SetResurrecting();
    }

    void SetResurrectingFalse() {
        animator.SetBool( "resurrecting", false );
        fadeOutTime = Time.time;
        fadingOut = true;
        StartCoroutine( DisableSelf( FADE_DURATION_OUT * 3f ) );
    }

    private IEnumerator DelayFade() {
        yield return new WaitForSeconds( 1.4f );
        startTime = Time.time;
        startFade = true;
    }

    private IEnumerator IntroFade() {
        yield return new WaitForSeconds( INTRO_FADE_DURATION );
        startTime = Time.time;
        startFade = true;
    }

    private IEnumerator DisableSelf( float delay ) {
        yield return new WaitForSeconds( delay );
        gameObject.SetActive( false );
    }

    private void FadeIn() {
        float t = ( Time.time - startTime ) / FADE_DURATION;
        spriteRenderer.color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, fullShade, t ) );
    }

    private void FadeOut() {
        float t = ( Time.time - fadeOutTime ) / FADE_DURATION_OUT;
        spriteRenderer.color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( fullShade, 0f, t ) );
    }

    private void ResetFade() {
        spriteRenderer.color = new Color( 1f, 1f, 1f, 0f );
        startTime = Time.time;
    }

    public void EnableAtPointIntro( float x, float y ) {
        transform.position = new Vector3( x, y, -1f );
        gameObject.SetActive( true );
        StartCoroutine( IntroFade() );
    }

    public void EnableAtPointDelay( float x, float y ) {
        transform.position = new Vector3( x, y, -1f );
        gameObject.SetActive( true );
        StartCoroutine( DelayFade() );
    }

}