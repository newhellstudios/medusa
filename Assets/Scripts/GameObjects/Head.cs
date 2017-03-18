using UnityEngine;
using System;
using System.Collections;

public class Head : MovingObject {

    public static Head instance = null;
    private bool timeToDie = false;
    private bool deathCommandIssued = false;
    private float offsetXHorizontal = .17F;
    private float offsetYHorizontal = .2F;
    private float sizeXHorizontal = .6F;
    private float sizeYHorizontal = .41F;
    private float offsetXVertical = 0.0F;
    private float offsetYVertical = .05F;
    private float sizeXVertical = .35F;
    private float sizeYVertical = .9F;
    private float destroyDelay = .6f;
    private OrientationToVectorDictionary orientationToOffset;
    private OrientationToVectorDictionary orientationToSize;
    public AudioClip explode;
    public AudioClip explodeHero;
    public AudioClip startMove;
    public AudioClip movingSound;
    private readonly static float INITIAL_SPEED = 9f;
    private float startClipLength;
    public AudioClip explode2;

    private void Awake() {
        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }
        startClipLength = startMove.length;
        StartCoroutine( LoopingNoise() );
    }

    private IEnumerator LoopingNoise() {
        yield return new WaitForSeconds( startClipLength );
        transform.GetComponent<AudioSource>().clip = movingSound;
        transform.GetComponent<AudioSource>().loop = true;
        transform.GetComponent<AudioSource>().Play();
    }

    protected override void Start() {
        base.Start();
        GameManager.instance.RegisterHead( this );
        if ( GameManager.instance.GetMedusaLight() != null ) {
            GameManager.instance.GetMedusaLight().transform.parent = transform;
        }
        moveSpeed = 10;
        moving = true;

        orientationToOffset = new OrientationToVectorDictionary(
            new Vector2( offsetXVertical, offsetYVertical ),
            new Vector2( offsetXVertical, offsetYVertical ),
            new Vector2( -offsetXHorizontal, offsetYHorizontal ),
            new Vector2( offsetXHorizontal, offsetYHorizontal ) );

        orientationToSize = new OrientationToVectorDictionary(
            new Vector2( sizeXVertical, sizeYVertical ),
            new Vector2( sizeXVertical, sizeYVertical ),
            new Vector2( sizeXHorizontal, sizeYHorizontal ),
            new Vector2( sizeXHorizontal, sizeYHorizontal ) );

        CurrentOrientation = GameManager.instance.GetEnemy().GetCurrentOrientation();
        SetBoxColliderSize( orientationToSize.orientationToVectorDictionary[CurrentOrientation] );
        SetBoxColliderOffset( orientationToOffset.orientationToVectorDictionary[CurrentOrientation] );
    }

    private void Update() {
        if ( transform.GetComponent<AudioSource>().isPlaying ) {
            transform.GetComponent<AudioSource>().volume = ( 1f - ( Mathf.Abs( Vector3.Distance( GameManager.instance.GetHero().transform.position, transform.position ) ) ) / 10f );
        }

        if ( timeToDie && !deathCommandIssued ) {
            transform.GetComponent<AudioSource>().Stop();
            StartCoroutine( DestroyHeadAfterDelay( destroyDelay ) );
            DeathCommandHasBeenIssued();
        }
    }

    protected override void FixedUpdate() {
        if ( !MainLoader.ArcadeMode ) {
            moveSpeed = INITIAL_SPEED + ( ( .25f ) * ( 5 - GameManager.instance.GetEnemy().Health ) );
        }
        base.FixedUpdate();
    }

    IEnumerator DestroyHeadAfterDelay( float delayTime ) {
        yield return new WaitForSeconds( delayTime );
        Destroy( gameObject );
    }

    private void SetBoxColliderSize( Vector2 size ) { boxCollider.size = size; }
    private void SetBoxColliderOffset( Vector2 offset ) { boxCollider.offset = offset; }
    private void ResetTimeToDie() { timeToDie = false; }

    private void OnCollisionEnter2D( Collision2D col ) {
        if ( col.gameObject.tag != "Enemy" && !timeToDie ) {
            if ( col.gameObject.tag != "Hero" ) {
                GameObject.FindWithTag( "HeadSounds" ).GetComponent<AudioSource>().PlayOneShot( explode, GameObject.FindWithTag( "HeadSounds" ).GetComponent<AudioSource>().volume - ( .05f * GetDistanceFromHero() ) );
                GameObject.FindWithTag( "HeadSounds" ).GetComponent<AudioSource>().PlayOneShot( explode2, ( GameObject.FindWithTag( "HeadSounds" ).GetComponent<AudioSource>().volume / 2 ) - ( .05f * GetDistanceFromHero() ) );
            }

            else {
                GameObject.FindWithTag( "HeadSounds" ).GetComponent<AudioSource>().PlayOneShot( explodeHero, .6f );
            }
            Destroy( boxCollider );
            destroying = true;
            Stop();
            timeToDie = true;
        }
    }

    protected override void Move() {
        rb2D.velocity = VectorOrientationDictionary.orientationToVDirDictionary[CurrentOrientation] * moveSpeed;
    }

    private void DeathCommandHasBeenIssued() {
        deathCommandIssued = true;
    }

    protected override void SetAnimation() {
        DisableAnimationBooleans();
        if ( moving ) { animator.SetBool( movingString, true ); }
        else if ( destroying ) { animator.SetBool( destroyingString, true ); }
        SetAnimationOrientation();
    }
}