using UnityEngine;
using System.Collections;
using System;

public class Sword : VariableOrientationObject {

    private static Sword instance = null;
    private OrientationToVectorDictionary orientationToOffset;
    private OrientationToVectorDictionary orientationToSize;
    private float offsetXVerticalUp = .03f;
    private float offsetXVerticalDown = .05f;
    private float offsetYVertical = .34f;
    private float offsetXHorizontalRight = .33f;
    private float offsetYHorizontalRight = .22f;
    private float offsetXHorizontalLeft = .21f;
    private float offsetYHorizontalLeft = .15f;
    private float sizeXVertical = .43f;
    private float sizeYVertical = .7f;
    private float sizeXHorizontal = .65f;
    private float sizeYHorizontal = .28f;
    private float triggerCheckDelay = .01f;
    public AudioClip clang;
    public AudioClip clang2;
    public AudioClip slashSound1;
    public AudioClip slashSound2;
    private static float ANIMATION_FRAME_LENGTH = .138f;
    private bool charged;

    public bool Charged {
        get {
            return charged;
        }

        set {
            charged =  value ;
        }
    }

    void Awake() {
        GameManager.instance.RegisterSword( this );
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
    }

    private void LateUpdate() {
        if(Charged) {
            animator.SetBool( "charged", true );
        }
        else {
            animator.SetBool( "charged", false );
        }
        
    }

    protected override void Start() {
        base.Start();

        orientationToOffset = new OrientationToVectorDictionary(
            new Vector2( offsetXVerticalUp, -offsetYVertical ),
            new Vector2( -offsetXVerticalDown, offsetYVertical ),
            new Vector2( offsetXHorizontalLeft, offsetYHorizontalLeft ),
            new Vector2( -offsetXHorizontalRight, offsetYHorizontalRight ) );

        orientationToSize = new OrientationToVectorDictionary(
            new Vector2( sizeXVertical, sizeYVertical ),
            new Vector2( sizeXVertical, sizeYVertical ),
            new Vector2( sizeXHorizontal, sizeYHorizontal ),
            new Vector2( sizeXHorizontal, sizeYHorizontal ) );
        gameObject.SetActive( false );
    }

    private void OnTriggerEnter2D( Collider2D collider ) {
        if ( collider.tag.Equals( "Pillar" ) ) {        
            SoundManager.instance.RandomizeSfx( 0f, clang );
            SoundManager.instance.RandomizeSfx( 0f, clang2 );
            //boxCollider.isTrigger = false;
        }
        else if ( collider.tag.Equals( "Enemy" ) ) {
            GrabberLight.instance.LightUp();
            GameManager.instance.setEnemyHit();
            //StartCoroutine( EnsureColumnWasNotHitFirstAfterDelayThenSetHit( triggerCheckDelay ) );
        }
    }

    private void SetAnimation() {
        SetAnimationOrientation();
    }

    private void SetBoxColliderSize( Vector2 size ) { boxCollider.size = size; }
    private void SetBoxColliderOffset( Vector2 offset ) { boxCollider.offset = offset; }

    public void SetOrientation( OrientationEnum orientation ) {
        this.CurrentOrientation = orientation;
    }

    public void Activate( OrientationEnum orientation, float chargeQuotient ) {
        if ( !GameManager.instance.GetHero().GetDestroying() ) {
            Charged = false;
            if (chargeQuotient >= (ANIMATION_FRAME_LENGTH *5) ) {
                Charged = true;
                GameObject.FindWithTag( "SwordSounds" ).GetComponent<AudioSource>().PlayOneShot( slashSound2, .2f );
            }
            else {
                GameObject.FindWithTag( "SwordSounds" ).GetComponent<AudioSource>().PlayOneShot( slashSound1, .3f );
            }

            boxCollider.isTrigger = true;
            CurrentOrientation = orientation;
            SpriteRenderer.sortingOrder = GameManager.instance.GetHero().SpriteRenderer.sortingOrder;
            SetBoxColliderSize( orientationToSize.orientationToVectorDictionary[CurrentOrientation] );
            SetBoxColliderOffset( orientationToOffset.orientationToVectorDictionary[CurrentOrientation] );
            gameObject.SetActive( true );
            SetAnimation();
        }
    }
}