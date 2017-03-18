using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Hero : MovingObject {

    private int hitPoints;
    private bool horizontalMove;
    private bool verticalMove;
    private bool attacking;
    private bool idling;
    private bool charging;
    private float xPos = 0;
    private float yPos = 0;
    private Vector2 movementVector;
    private static float ANIMATION_FRAME_LENGTH = .138f;

    private readonly float INITIAL_LIGHT_RANGE = 3f;
    private static float TIME_BETWEEN_CHARGE_LIGHT = .05f;
    private static float LIGHT_INCREASE = .2f;
    private static float THROB_SPEED = .4f;
    private static float MAX_LIGHT_VALUE = 6;
    private static float MAX_THROB_VALUE = 6.5f;

    private float timeUntilNextLightIncrease = 0f;
    private float attackingWaitTime = ANIMATION_FRAME_LENGTH * 4;
    private float swordDelayTime = ANIMATION_FRAME_LENGTH * 1;
    private float swordDestoryTime = ANIMATION_FRAME_LENGTH * 2;
    private bool throbbingLight;
    private bool throbbingReduce;
    private static readonly float DAMAGE_INITIAL = 1f;
    private static readonly float DAMAGE_DOUBLED = DAMAGE_INITIAL * 2;
    public float damageMultiplier = DAMAGE_INITIAL;
    private int athenaPillarLayer;

    private OrientationToVectorDictionary orientationDic;
    public AudioClip slashClang;
    public AudioClip scream;
    public AudioClip chargeUp;
    public AudioClip pulse;

    public AudioClip walk;
    public AudioClip walk2;
    public AudioClip walk3;
    public AudioClip walk4;
    public AudioClip walk5;
    public AudioClip walk6;

    public AudioClip resurrect;
    public AudioClip resurrect2;
    public AudioClip resurrect3;

    public AudioClip resurrect4;

    private List<AudioClip> walkingSounds;

    public AudioClip land;

    public AudioClip stonage;

    private float chargeTimeMeasure;
    private float chargeQuotient;

    private AudioSource audioSource;
    private float restoreLightTime;
    private string resurrectString;
    private bool smokin;
    private bool resurrecting;
    private bool restoringLight;
    private bool resurrectPressed;
    private bool cantMove;
    private float startTime;
    private static readonly float FADE_DURATION = 1f;
    private bool fadingIn;
    private static readonly float FADE_IN_DELAY = 2f;
    private static readonly float CONTROL_DELAY = FADE_IN_DELAY + 8f;
    private OrientationEnum victoryOrientation;

    public void EnableVictoryState() {
        if ( MainLoader.ArcadeMode ) {
            Timer.instance.StopTimer();
        }
        VictoryOrientation = CurrentOrientation;
        CantMove = true;
        CurrentOrientation = OrientationEnum.DOWN;
        //idling = true;
        // attacking = false;
        moving = false;
        charging = false;
        StartCoroutine( EnableVictoryAnimation() );

    }
    private IEnumerator EnableVictoryAnimation() {
        yield return new WaitForSeconds( .001f );
        animator.SetBool( "victory", true );
    }

    public float ChargeTimeMeasure
    {
        get
        {
            return chargeTimeMeasure;
        }

        set
        {
            chargeTimeMeasure = value;
        }
    }

    public float ChargeQuotient
    {
        get
        {
            return chargeQuotient;
        }

        set
        {
            chargeQuotient = value;
        }
    }

    public bool CantMove
    {
        get
        {
            return cantMove;
        }

        set
        {
            cantMove = value;
        }
    }

    public OrientationEnum VictoryOrientation
    {
        get
        {
            return victoryOrientation;
        }

        set
        {
            victoryOrientation = value;
        }
    }

    private void SetRestoringLightTrue() {
        restoringLight = true;
    }

    private void SetRestoringLightFalse() {
        restoringLight = false;
    }

    //void OnDrawGizmos() {
    //    Gizmos.DrawSphere( transform.position, 3f );
    //}

    protected override void Start() {
        resurrectString = "resurrect";
        base.Start();
        spriteRenderer.color = new Color( 1f, 1f, 1f, 0f );
        GameManager.instance.GetHeroLightObject().range = 0f;
        CantMove = true;
        GameManager.instance.RegisterHero( this );
        athenaPillarLayer =
           ( 1 << LayerMask.NameToLayer( "AthenaCollider" ) );
        animationStrings.Add( attackingString );
        animationStrings.Add( idlingString );
        animationStrings.Add( destroyingString );
        animationStrings.Add( chargingString );
        animationStrings.Add( resurrectString );
        walkingSounds = new List<AudioClip>();
        walkingSounds.Add( walk );
        walkingSounds.Add( walk2 );
        walkingSounds.Add( walk3 );
        walkingSounds.Add( walk4 );
        walkingSounds.Add( walk5 );
        walkingSounds.Add( walk6 );
        //hitPoints = GameManager.instance.playerHitPoints;
        moveSpeed = 5;
        orientationDic = new OrientationToVectorDictionary(
            new Vector2( 0, 1 ),
            new Vector2( 0, -1 ),
            new Vector2( -1, 0 ),
            new Vector2( 1, 0 ) );
        CurrentOrientation = OrientationEnum.RIGHT;
        idling = true;
        audioSource = transform.GetComponent<AudioSource>();
        StartCoroutine( FadeInAfterDelay() );
        StartCoroutine( RegainControlAfterDelay() );
        StartCoroutine( ToggleFlamesAfterDelay() );
    }

    void ThudLand() {
        audioSource.clip = land;
        audioSource.PlayOneShot( land, 1f );
    }

    void ResurrectNoise1() {
        audioSource.PlayOneShot( resurrect, 1f );
    }
    void ResurrectNoise2() {
        audioSource.PlayOneShot( resurrect2, 1f );
    }
    void ResurrectNoise3() {
        audioSource.PlayOneShot( resurrect3, .25f );
    }
    void ResurrectNoise4() {
        audioSource.PlayOneShot( resurrect4, 1f );
    }

    private void DecreaseLight() {
        GameManager.instance.GetHeroLightObject().range = 0f;
    }

    void RestoreLight() {
        SetRestoringLightTrue();
        restoreLightTime = Time.time;
    }

    void Smokin() {
        if ( !resurrectPressed ) {
            smokin = true;
        }
    }

    void PerseusRunNoise() {

        audioSource.PlayOneShot( walkingSounds[UnityEngine.Random.Range( 0, ( walkingSounds.Count - 1 ) )] );
    }

    private void Update() {
        if ( fadingIn ) {
            FadeIn();
        }
        // UpdateWalkingSound();
        if ( smokin ) {
            moving = false;
            if ( !InGameMenu.instance.MenuVisible && Input.GetButton( "attack" ) ) {
                resurrectPressed = true;
                smokin = false;
                GameManager.instance.GetAthena().SetResurrecting();
                return;
            }
        }
        if ( destroying ) {
            moving = false;
            if ( !restoringLight ) {
                DecreaseLight();
                return;
            }
        }
        if ( restoringLight ) {
            moving = false;
            float t = ( Time.time - restoreLightTime ) / .55f;
            GameManager.instance.GetHeroLightObject().range = Mathf.SmoothStep( 0f, INITIAL_LIGHT_RANGE, t );
            return;
        }
        ResetPosition();
        CheckAttacking();
        if ( !attacking && !charging ) { CheckAndSetMovement(); }
        SetOrientation();
        movementVector = new Vector2( xPos, yPos );
    }

    private void UpdateWalkingSound() {
        if ( moving && !audioSource.isPlaying && ( !destroying && !resurrecting && !restoringLight && !smokin ) ) {
            if ( audioSource.clip == null || !audioSource.clip.Equals( walk ) ) {
                audioSource.clip = walk;
            }
            audioSource.Play();
        }
        if ( !moving && audioSource.clip != null && audioSource.clip.Equals( walk ) ) {
            audioSource.Stop();
        }
    }

    private void ResetPosition() {
        xPos = 0;
        yPos = 0;
    }

    protected override void SetAnimation() {
        DisableAnimationBooleans();
        if ( resurrecting ) {
            animator.SetBool( resurrectString, true );
        }
        else if ( attacking ) { SetAttackingAnimation(); }
        else if ( charging ) { SetChargingAnimation(); }
        else if ( moving ) { SetMovingAnimation(); }
        else if ( idling ) { SetIdlingAnimation(); }
        else if ( destroying ) { SetDestroyingAnimation(); }
        SetAnimationOrientation();
    }

    void EndOfReanimate() {
        chargeQuotient = 0;
        RevertLight();
        GameManager.instance.GetEnemy().EnableMedusaNow();
        SetRestoringLightFalse();
        destroying = false;
        resurrecting = false;
        CurrentOrientation = OrientationEnum.DOWN;
        idling = true;
        resurrectPressed = false;
        if ( MainLoader.ArcadeMode ) {
            Timer.instance.StartTimer();
        }

    }

    void EndOfIntro() {
        SetRestoringLightFalse();
        destroying = false;
        resurrecting = false;
        CurrentOrientation = OrientationEnum.RIGHT;
        idling = true;
    }

    private void SetMovingAnimation() {
        animator.SetBool( movingString, true );
    }

    private void SetAttackingAnimation() {
        animator.SetBool( attackingString, true );
    }

    public void SetResurrecting() {
        resurrecting = true;
        destroying = false;
    }

    private void SetIdlingAnimation() {
        animator.SetBool( idlingString, true );
    }

    private void SetDestroyingAnimation() {
        animator.SetBool( destroyingString, true );
    }

    private void SetChargingAnimation() {
        animator.SetBool( chargingString, true );
    }

    private void StopAttacking() {
        attacking = false;
        if ( CantMove ) {
            idling = true;
            CurrentOrientation = OrientationEnum.DOWN;
        }
    }

    private void CheckAndSetMovement() {
        if ( CantMove ) {
            return;
        }
        if ( Input.GetAxisRaw( "Vertical" ) == 0 && Input.GetAxisRaw( "Horizontal" ) == 0 ) {
            xPos = 0;
            yPos = 0;
            idling = true;
            moving = false;
            return;
        }
        if ( InGameMenu.instance.MenuVisible ) {
            return;
        }
        moving = true;
        idling = false;
		if ( (Math.Round(Input.GetAxisRaw( "Vertical" ), 0) != 0) && (Math.Round(Input.GetAxisRaw( "Horizontal" )) != 0) ) {
            if ( horizontalMove ) {
				yPos = (float) Math.Round(Input.GetAxisRaw( "Vertical" ), 0);
            }
            else if ( verticalMove ) {
				xPos = (float) Math.Round(Input.GetAxisRaw( "Horizontal" ), 0);
            }
        }
        else {
			horizontalMove = Math.Round(Input.GetAxisRaw( "Horizontal" ), 0) != 0;
			xPos = (float) Math.Round(Input.GetAxisRaw( "Horizontal" ), 0);
			verticalMove = Math.Round(Input.GetAxisRaw( "Vertical" ), 0) != 0;
			yPos = (float) Math.Round(Input.GetAxisRaw( "Vertical" ), 0);
        }
    }

    private void SetOrientation() {
        if ( movementVector != Vector2.zero ) {
            CurrentOrientation = VectorOrientationDictionary.vDirToOrientationDictionary[movementVector];
        }
    }

    protected override void Move() {
        transform.position += (Vector3)movementVector * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        if ( ( collision.collider.tag.Equals( "EnemyPhysCollider" ) ) && !destroying ) {
            SetDestroying();
        }
        else if ( collision.collider.tag.Equals( "Head" ) && !destroying ) {
            if ( GameManager.instance.GetHead() ) {
                SetDestroying();
            }
        }
    }

    public void SetDestroying() {
        if ( MainLoader.ArcadeMode ) {
            Timer.instance.StopTimer();
            Timer.instance.ResetTimer();
        }

        if ( !MainLoader.ArcadeMode ) {
            GameManager.instance.SnuffOutCenterFires();
        }
        GameManager.instance.GetEnemy().DisappearWhilePerseusStoned();
        DeactivateSword();
        GameObject.Find( "PerseusScreamSound" ).GetComponent<AudioSource>().PlayOneShot( scream, .2f );
        GameObject.FindWithTag( "HeroSounds" ).GetComponent<AudioSource>().PlayOneShot( stonage, .4f );
        //  Destroy(GameManager.instance.GetHeroLightObject());
        destroying = true;
        moving = false;
        attacking = false;
        charging = false;
        idling = false;
        GameManager.instance.GetAthena().EnableAtPointDelay( transform.position.x, transform.position.y + 2.15f );
    }

    void PlayStonage() {

        Debug.Log("played stonage");
    }

    private IEnumerator FadeInAfterDelay() {
        yield return new WaitForSeconds( FADE_IN_DELAY );
        startTime = Time.time;
        animator.SetBool( "intro", true );
        fadingIn = true;
    }

    private IEnumerator RegainControlAfterDelay() {
        yield return new WaitForSeconds( CONTROL_DELAY );
        if ( MainLoader.ArcadeMode ) {
            Timer.instance.StartTimer();
        }
        else {
            FlamesText.instance.TurnOn();
        }
        CantMove = false;
    }

    private IEnumerator ToggleFlamesAfterDelay() {
        yield return new WaitForSeconds( CONTROL_DELAY - 1f );
        if ( MainLoader.ArcadeMode ) {
            GameManager.instance.TurnOnCenterFires();
        }
        else {
            GameManager.instance.SnuffOutCenterFires();
        }
    }

    private void FadeIn() {
        if ( spriteRenderer.color.a != 1f ) {
            float t = ( Time.time - startTime ) / FADE_DURATION;
            spriteRenderer.color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, 1f, t ) );
        }
        else {
            fadingIn = false;
        }

    }

    private void CheckAttacking() {
        if ( CantMove || resurrecting ) {
            return;
        }
        if ( !InGameMenu.instance.MenuVisible && Input.GetButton( "attack" ) && !attacking && !charging ) {
            idling = false;
            moving = false;
            charging = true;
            chargeQuotient = 0;
            chargeTimeMeasure = Time.time;
        }

        if ( Input.GetButton( "attack" ) && !attacking && charging ) {

            IncreaseLight();
        }

        if ( Input.GetButtonUp( "attack" ) && !attacking && charging ) {
            audioSource.Stop();
            attacking = true;
            idling = false;
            charging = false;
            chargeQuotient = ( Time.time - chargeTimeMeasure );
            StartAttackCoroutines();
        }
    }

    private void StartAttackCoroutines() {
        StartCoroutine( StopAttackingAfterWait( attackingWaitTime ) );
        StartCoroutine( ActivateSwordAfterWait( swordDelayTime ) );
        StartCoroutine( DeactivateSwordAfterWait( swordDestoryTime ) );
        StartCoroutine( RevertLightAfterWait( attackingWaitTime - ANIMATION_FRAME_LENGTH ) );
        StartCoroutine( ResetDamageModifierAfterWait( swordDestoryTime ) );
    }

    private IEnumerator StopAttackingAfterWait( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        StopAttacking();
    }

    private IEnumerator DeactivateSwordAfterWait( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        DeactivateSword();
    }

    private void DeactivateSword() {
        GameManager.instance.GetSword().gameObject.SetActive( false );
    }

    private IEnumerator ActivateSwordAfterWait( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        ActivateSword();
    }

    private IEnumerator RevertLightAfterWait( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        RevertLight();
    }

    private IEnumerator ResetDamageModifierAfterWait( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        ResetDamageModifier();
    }

    private void ActivateSword() {
        float xOffset = orientationDic.orientationToVectorDictionary[CurrentOrientation].x;
        float yOffset = orientationDic.orientationToVectorDictionary[CurrentOrientation].y;
        float xPosition = transform.position.x;
        float yPosition = transform.position.y;
        GameManager.instance.GetSword().transform.position = new Vector2( xPosition + xOffset, yPosition + yOffset );
        GameManager.instance.GetSword().Activate( CurrentOrientation, ChargeQuotient );
    }

    void PlayChargingNoise() {
        audioSource.clip = chargeUp;
        audioSource.PlayOneShot( chargeUp );
    }

    private void IncreaseLight() {
        if ( Time.time > timeUntilNextLightIncrease ) {
            timeUntilNextLightIncrease = Time.time + TIME_BETWEEN_CHARGE_LIGHT;
            //if ( !throbbingLight && GameManager.instance.GetHeroLightObject().range > 3.1f && !audioSource.clip.Equals( chargeUp ) ) {
            //    audioSource.clip = chargeUp;
            //    audioSource.PlayOneShot( chargeUp );
            //}
            if ( GameManager.instance.GetHeroLightObject().intensity < MAX_LIGHT_VALUE && GameManager.instance.GetHeroLightObject().range < MAX_LIGHT_VALUE ) {
                GameManager.instance.GetHeroLightObject().intensity += LIGHT_INCREASE;
                GameManager.instance.GetHeroLightObject().range += LIGHT_INCREASE;
                damageMultiplier = DAMAGE_INITIAL + ( ( GameManager.instance.GetHeroLightObject().range - INITIAL_LIGHT_RANGE ) / ( MAX_LIGHT_VALUE - INITIAL_LIGHT_RANGE ) );
            }
            else {
                if ( !throbbingLight ) {
                    audioSource.clip = pulse;
                    audioSource.PlayOneShot( pulse, 1f );
                }
                throbbingLight = true;
                DoubleDamageModifier();
            }
            ThrobLight();
        }
    }

    private void DoubleDamageModifier() {
        damageMultiplier = DAMAGE_DOUBLED;
    }

    private void ResetDamageModifier() {
        damageMultiplier = DAMAGE_INITIAL;
    }

    private void ThrobLight() {
        if ( throbbingLight && throbbingReduce ) {
            GameManager.instance.GetHeroLightObject().intensity -= THROB_SPEED;
            GameManager.instance.GetHeroLightObject().range -= THROB_SPEED;
        }
        else if ( throbbingLight && !throbbingReduce ) {
            GameManager.instance.GetHeroLightObject().intensity += THROB_SPEED;
            GameManager.instance.GetHeroLightObject().range += THROB_SPEED;
        }
        if ( !throbbingReduce && GameManager.instance.GetHeroLightObject().intensity >= MAX_THROB_VALUE && GameManager.instance.GetHeroLightObject().range >= MAX_THROB_VALUE ) {
            throbbingReduce = true;
        }
        if ( throbbingReduce && GameManager.instance.GetHeroLightObject().intensity <= MAX_LIGHT_VALUE && GameManager.instance.GetHeroLightObject().range <= MAX_LIGHT_VALUE ) {
            audioSource.clip = pulse;
            audioSource.PlayOneShot( pulse, 1f );
            throbbingReduce = false;
        }
    }

    private void MaxLight() {
        if ( GameManager.instance.GetHeroLightObject().intensity < MAX_LIGHT_VALUE && GameManager.instance.GetHeroLightObject().intensity < MAX_LIGHT_VALUE ) {
            GameManager.instance.GetHeroLightObject().intensity = MAX_LIGHT_VALUE;
            GameManager.instance.GetHeroLightObject().range = MAX_LIGHT_VALUE;
        }
    }

    private void RevertLight() {
        try {
            GameManager.instance.GetHeroLightObject().intensity = 4;
            GameManager.instance.GetHeroLightObject().range = INITIAL_LIGHT_RANGE;
            throbbingReduce = false;
            throbbingLight = false;
        }
        catch ( NullReferenceException ) {

        }
        catch ( MissingReferenceException ) { }
    }
}