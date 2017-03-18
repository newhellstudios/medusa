using UnityEngine;
using System.Collections;

public class Poseidon : RenderedObject {

    public static Poseidon instance;
    private float FADE_DURATION = 1f;
    private float startTime;
    private float startTime2;
    private float fullShade = .5f;
    private bool auraIncreased;
    private bool fullRendered;
    private readonly static float TIME_BETWEEN_CHARGE_LIGHT = .05f;
    private readonly static float MAX_LIGHT_VALUE = 6f;
    private readonly static float IMMEDIATELY = 0f;

    private readonly static float LIGHT_DURATION = 2f;

    private void Awake() {
        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }
        gameObject.SetActive( false );
        base.Start();
        GameManager.instance.RegisterPoseidon( this );
    }

    // Use this for initialization
    protected override void Start() {

    }

    private void OnEnable() {
        fullRendered = false;
        startTime = Time.time;
        spriteRenderer.color = new Color( 1f, 1f, 1f, 0f );
        StartCoroutine( DisableSelf( FADE_DURATION * 2 ) );
    }

    // Update is called once per frame
    void Update() {
        if ( spriteRenderer.color.a != fullShade && !fullRendered ) {
            FadeIn();
        }
        else {
            if ( !fullRendered ) {
                startTime2 = Time.time;
            }
            fullRendered = true;
            FadeOut();
        }
    }

    private IEnumerator DisableSelf( float delay ) {
        yield return new WaitForSeconds( FADE_DURATION * 2 );
        gameObject.SetActive( false );
    }

    private void FadeIn() {
        float t = ( Time.time - startTime ) / FADE_DURATION;       
        spriteRenderer.color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, fullShade, t ) );
    }

    private void FadeOut() {
        float t = ( Time.time - startTime2 ) / FADE_DURATION;       
        spriteRenderer.color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( fullShade, 0f, t ) );
    }

    private void ResetFade() {
        spriteRenderer.color = new Color( 1f, 1f, 1f, 0f );
        startTime = Time.time;
    }

    public void EnableAtPoint( float x, float y ) {
        transform.position = new Vector3( x, y, -1f );
        gameObject.SetActive( true );
    }

}
