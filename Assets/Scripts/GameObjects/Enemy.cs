using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Enemy : MovingObject {

    private float FADE_DURATION = 1.5f;
    private readonly float INTRO_FADE_DURATION = 1.5f;
    private float PEGASUS_LIGHT_DURATION = .25f;
    private float health;
    private Transform target;
    private float moveTimeLimit = .25f;
    private static float ANIMATION_FRAME_LENGTH = .138f;
    private float stillAttackingTime = ANIMATION_FRAME_LENGTH * 4;
    private float timeToSpawnHead = ANIMATION_FRAME_LENGTH * 4;
    private float timeToSpawnPoseidon = ANIMATION_FRAME_LENGTH * 4;
    private float timeUntilLevitate = ANIMATION_FRAME_LENGTH * 19;
    private float timeToBehead = ANIMATION_FRAME_LENGTH * 48;
    private float timeToReactivate = 1.5f;
    private RaycastHit2D raycastHero;
    private float offset = .75f;
    private float raycastOffset = .22f;
    private bool needToAttack;
    private bool attacking;
    private bool movementDirectionSet;
    private bool beheading;
    private bool hit;
    private bool seekingPerseus;
    private bool levitating;
    private Vector3 currentTarget;
    private Vector3 spawnPointTarget;
    private float circleCastRadiusVertical = .20f;
    private float circleCastRadiusHorizontal = .20f;
    private int pillarHeroSnakesWallLayer;
    private int pillarHeroWallLayer;
    private int spawnPointHeroLayer;
    private OrientationToVectorDictionary offsetDictionary;
    private OrientationToVectorDictionary raycastOffsetDictionary;
    private OrientationToFloatDictionary radiusDictionary;
    private string beheadingString = "beheading";
    private Vector2 raycastPositionVector;
    private static readonly float INITIAL_SPEED = 2.0f;
    private static readonly float INITIAL_HEALTH = 5f;
    public static System.Random systemRandom;
    private float athenaYDistance = 1f;
    private float maximumDistanceToHeroForSeeking = 6f;
    private float minimumSpawnPointDistanceForPatrol = 4f;
    private float minimumSpawnPointDistanceForSpawn = 8f;
    private float radiusForSpawnPointCastForPatrol = 8f;
    private float radiusForSpawnPointCastForSpawn = 16f;
    private float frequencyOfFindingPatrolPoint = .33f;
    private BoxCollider2D physCollider;
    private AudioSource audioSource;

    private static float pegasusLightTime;

    private float xDistanceToPlayer;
    private float yDistanceToPlayer;

    private static readonly float MAX_DISTANCE_TO_OBSTACLE = .5f;

    public AudioClip takeHit;
    public AudioClip shoot;
    public AudioClip victory;
    public AudioClip scream;
    public AudioClip splat;
    public AudioClip thud;
    public AudioClip respawn;
    public AudioClip walk;
    public AudioClip walk2;
    public AudioClip walk3;
    public AudioClip walk4;
    public AudioClip walk5;
    public AudioClip walk6;
    public AudioClip rise;
    public AudioClip reanimate;
    public AudioClip pegaSpawn;
    public AudioClip pegakicks;
    public AudioClip pegakicks2;
    public AudioClip pegakicks3;
    public AudioClip pegakicks4;
    private List<AudioClip> pegaKickList;

    private float fadeTime;
    private bool fading;
    private Vector3 hitPosition;
    private bool finalDeath;
    private Light pegasusLight;
    private bool intro;
    private float startTime;
    private bool fadingIn;
    private bool pegasusIsKicking;
    private static readonly float FADE_IN_DELAY = 6f;
    private static readonly float FADE_OUT_DELAY = FADE_IN_DELAY + 1f;
    private static readonly float RE_ENABLE_DELAY_INTRO = FADE_OUT_DELAY + 1f;

    public float distanceForCloseWalk = 1f;
    private List<AudioClip> walkingSounds;

    public bool SetHit { set { hit = value; } }

    public bool SeekingPerseus {
        get {
            return seekingPerseus;
        }

        set {
            seekingPerseus = value;
        }
    }

    public Vector3 CurrentTarget {
        get {
            return currentTarget;
        }

        set {
            currentTarget = value;
        }
    }

    public float Health {
        get {
            return health;
        }

        set {
            health = value;
        }
    }

    public bool FinalDeath {
        get {
            return finalDeath;
        }

        set {
            finalDeath = value;
        }
    }

    private IEnumerator EndOfIntroAfterDelay() {
        yield return new WaitForSeconds( RE_ENABLE_DELAY_INTRO );
        intro = false;
        moving = true;
    }

    void PegausIsKicking() {
        pegasusIsKicking = true;
    }

    protected override void Start() {
        systemRandom = new System.Random();
        base.Start();

        walkingSounds = new List<AudioClip>();
        walkingSounds.Add( walk );
        walkingSounds.Add( walk2 );
        walkingSounds.Add( walk3 );
        walkingSounds.Add( walk4 );
        walkingSounds.Add( walk5 );
        walkingSounds.Add( walk6 );

        pegaKickList = new List<AudioClip>();
        pegaKickList.Add( pegakicks );
        pegaKickList.Add( pegakicks2 );
        pegaKickList.Add( pegakicks3 );
        pegaKickList.Add( pegakicks4 );

        spriteRenderer.color = new Color( 1f, 1f, 1f, 0f );
        intro = true;
        GameManager.instance.RegisterEnemy( this );
        pillarHeroSnakesWallLayer =
            ( 1 << LayerMask.NameToLayer( "Hero" ) ) |
            ( 1 << LayerMask.NameToLayer( "Pillar" ) ) |
            ( 1 << LayerMask.NameToLayer( "Snakes" ) ) |
            ( 1 << LayerMask.NameToLayer( "Wall" ) );
        pillarHeroWallLayer =
            ( 1 << LayerMask.NameToLayer( "Hero" ) ) |
            ( 1 << LayerMask.NameToLayer( "Pillar" ) ) |
            ( 1 << LayerMask.NameToLayer( "Wall" ) );
        spawnPointHeroLayer =
            ( 1 << LayerMask.NameToLayer( "Hero" ) ) |
            ( 1 << LayerMask.NameToLayer( "SpawnPoint" ) );
        raycastPositionVector = new Vector2();
        if ( MainLoader.ArcadeMode ) {
            Health = MainLoader.ProvidedArcadeHealth;
        }
        else {
            Health = 5;
        }

        animationStrings.Add( attackingString );
        animationStrings.Add( beheadingString );
        target = GameObject.FindGameObjectWithTag( "Hero" ).transform;

        offsetDictionary = new OrientationToVectorDictionary(
            new Vector2( 0, 1 * offset ),
            new Vector2( 0, -1 * offset ),
            new Vector2( -1 * offset, 0 ),
            new Vector2( 1 * offset, 0 ) );

        raycastOffsetDictionary = new OrientationToVectorDictionary(
            new Vector2( 0, 1 * offset ),
            new Vector2( 0, 0 ),
            new Vector2( 0, 0 + raycastOffset ),
            new Vector2( 0, 0 + raycastOffset ) );

        radiusDictionary = new OrientationToFloatDictionary(
            circleCastRadiusVertical,
            circleCastRadiusVertical,
            circleCastRadiusHorizontal,
            circleCastRadiusHorizontal );

        moveSpeed = INITIAL_SPEED;
        CurrentOrientation = OrientationEnum.LEFT;
        InvokeRepeating( "GetNewPatrolPoint", 0f, frequencyOfFindingPatrolPoint );
        BoxCollider2D[] colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach ( BoxCollider2D collider in colliders ) {
            if ( collider.gameObject.tag.Equals( "EnemyPhysCollider" ) ) {
                physCollider = collider;
            }
        }
        audioSource = transform.GetComponent<AudioSource>();
        StartCoroutine( FadeInAfterDelay() );
        StartCoroutine( SetupFade( FADE_OUT_DELAY ) );
        StartCoroutine( EnableMedusaAfterDelay( RE_ENABLE_DELAY_INTRO ) );
        StartCoroutine( EndOfIntroAfterDelay() );

    }

    void WalkNoise() {
        //audioSource.volume = 1 - ( ( xDistanceToPlayer + yDistanceToPlayer ) * .1f );
        if ( moving ) {
            audioSource.PlayOneShot( walkingSounds[UnityEngine.Random.Range( 0, ( walkingSounds.Count - 1 ) )] );
        }
    }

    private void UpdateWalkingSound() {
        if ( !finalDeath ) {
            audioSource.volume = .7f - ( ( xDistanceToPlayer + yDistanceToPlayer ) * .1f );
        }
        //if ( moving && !audioSource.isPlaying && ( !destroying && !fading && !beheading && !levitating ) ) {
        //    audioSource.Play();
        //}
        //if ( !moving && !finalDeath ) {
        //    audioSource.Stop();
        //}
        if ( finalDeath && pegasusIsKicking ) {

            audioSource.Stop();
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        raycastPositionVector.Set( transform.position.x + GetRaycastOffsetX(), transform.position.y + GetRaycastOffsetY() );
        Gizmos.DrawSphere( raycastPositionVector, radiusDictionary.orientationToFloatDictionary[CurrentOrientation] );
    }

    private void FadeOut() {
        float t = ( Time.time - fadeTime ) / FADE_DURATION;
        spriteRenderer.color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 1f, 0f, t ) );
    }

    private void PegasusLight() {
        float t = ( Time.time - ( pegasusLightTime + 2f ) ) / PEGASUS_LIGHT_DURATION;
        pegasusLight.range = Mathf.SmoothStep( 0f, 6f, t );
    }

    private void Update() {
        if ( intro ) {
            if ( fadingIn ) {
                FadeIn();
            }
            if ( fading ) {
                FadeOut();
            }
            return;
        }
        UpdateWalkingSound();
        if ( FinalDeath ) {
            PegasusLight();
        }
        if ( fading ) {
            FadeOut();
            return;
        }
        if ( GameManager.instance.GetHero().GetDestroying() || beheading ) {
            Stop();
            return;
        }
        MeasureDistanceToPlayer();
        if ( hit ) {
            TakeHit();
            hit = false;
            return;
        }
        else if ( needToAttack ) {
            Attack();
            return;
        }
        else if ( beheading || attacking ) {
            return;
        }
        PerseusOrPatrol();
        if ( SeekingPerseus ) {
            currentTarget = GameManager.instance.GetHero().transform.position;
        }
        else {
            currentTarget = spawnPointTarget;
        }
        EnemyPathAI.instance.RunUpdate();
        ExecuteRaycasts();
    }

    private IEnumerator FadeInAfterDelay() {
        yield return new WaitForSeconds( FADE_IN_DELAY );
        startTime = Time.time;
        fadingIn = true;
    }

    private void FadeIn() {
        if ( spriteRenderer.color.a != 1f ) {
            float t = ( Time.time - startTime ) / INTRO_FADE_DURATION;
            spriteRenderer.color = new Color( 1f, 1f, 1f, Mathf.SmoothStep( 0f, 1f, t ) );
        }
        else {
            fadingIn = false;
        }

    }

    private void ExecuteRaycasts() {
        RayCastForHero();
    }

    private void GetNewPatrolPoint() {
        if ( !SeekingPerseus && gameObject.activeSelf ) {
            Collider2D[] colliders = CircleCastForPatrolPoints();
            List<Collider2D> listOfColliders = new List<Collider2D>();
            foreach ( Collider2D collider in colliders ) {
                if ( Vector3.Distance( transform.position, collider.transform.position ) > minimumSpawnPointDistanceForPatrol ) {
                    listOfColliders.Add( collider );
                }
            }
            spawnPointTarget = listOfColliders[systemRandom.Next( listOfColliders.Count )].transform.position;
        }
    }

    private Vector3 GetSpawnPoint() {
        Collider2D[] colliders = CircleCastForSpawnPoints();
        List<Collider2D> listOfColliders = new List<Collider2D>();
        foreach ( Collider2D collider in colliders ) {
            if ( Vector3.Distance( target.position, collider.transform.position ) > minimumSpawnPointDistanceForSpawn ) {
                listOfColliders.Add( collider );
            }
        }
        return listOfColliders[systemRandom.Next( listOfColliders.Count )].transform.position;
    }

    private Collider2D[] CircleCastForPatrolPoints() {
        return Physics2D.OverlapCircleAll( transform.position, radiusForSpawnPointCastForPatrol, ( 1 << LayerMask.NameToLayer( "SpawnPoint" ) ) );
    }

    private Collider2D[] CircleCastForSpawnPoints() {
        return Physics2D.OverlapCircleAll( hitPosition, radiusForSpawnPointCastForSpawn, ( 1 << LayerMask.NameToLayer( "SpawnPoint" ) ) );
    }

    private Collider2D OverlapForHero() {
        return Physics2D.OverlapCircle( transform.position, maximumDistanceToHeroForSeeking, ( 1 << LayerMask.NameToLayer( "Hero" ) ) );
    }

    private void PerseusOrPatrol() {
        Collider2D collider = OverlapForHero();
        if ( collider != null ) {
            SeekingPerseus = true;
        }
        else {
            SeekingPerseus = false;
        }
    }

    void PegasusSpawnNoise() {
        GameObject.FindWithTag( "MedusaSounds" ).GetComponent<AudioSource>().PlayOneShot( pegaSpawn, .2f );
    }

    void PegaWingNoise() {
        GameObject.FindWithTag( "MedusaSounds2" ).GetComponent<AudioSource>().PlayOneShot( pegaKickList[UnityEngine.Random.Range( 0, ( pegaKickList.Count - 1 ) )], .5f );
    }

    void ThudNoise() {
        GameObject.FindWithTag( "MedusaSounds3" ).GetComponent<AudioSource>().PlayOneShot( thud );
    }

    private void TakeHit() {
        if ( GameManager.instance.GetMedusaLight() ) {
            Destroy( GameManager.instance.GetMedusaLight().gameObject );
        }
        hitPosition = new Vector3( transform.position.x, transform.position.y, 0f );
        spriteRenderer.material = new Material( Shader.Find( "Sprites/Default" ) );
        Stop();
        GameObject.FindWithTag( "MedusaSounds" ).GetComponent<AudioSource>().PlayOneShot( scream );
        GameObject.FindWithTag( "MedusaSounds2" ).GetComponent<AudioSource>().PlayOneShot( splat );
        Health -= ( 1 * GameManager.instance.GetHero().damageMultiplier );
        beheading = true;
        ResetAttackState();
        boxCollider.enabled = false;
        physCollider.enabled = false;
        if ( Health > 0 ) {
            StartCoroutine( PlayReanimateAfterDelay( 1.25f ) );
            StartCoroutine( PlayLevitateAfterDelay( timeUntilLevitate - .25f ) );
            StartCoroutine( LevitateAfterDelay( timeUntilLevitate ) );
            StartCoroutine( StopLevitateAfterDelay( timeUntilLevitate + 1f ) );
            StartCoroutine( SetupFade( 2f ) );
            StartCoroutine( EnableMedusaAfterDelay( timeUntilLevitate + 1f + 1f ) );
            GameManager.instance.MakeReactivateEnemySoundAfterDelayShell( timeUntilLevitate + 1f + 1f );
            StartCoroutine( EnablePoseidonAfterDelay( timeToSpawnPoseidon ) );
        }
        else {
            if ( !MainLoader.ArcadeMode ) {
                PlayerPrefs.SetInt( "beatStory", 1 );
            }
            FinalDeath = true;
            if ( CreepySounds.instance ) {
                Destroy( CreepySounds.instance );
            }
            MusicManager.instance.VictorySong();
            GameManager.instance.GetHero().EnableVictoryState();
            pegasusLightTime = Time.time;
            pegasusLight = gameObject.GetComponentInChildren( typeof( Light ) ) as Light;
        }
    }

    public void DisappearWhilePerseusStoned() {
        ResetAttackState();
        Stop();
        if ( GameManager.instance.GetMedusaLight() ) {
            Destroy( GameManager.instance.GetMedusaLight().gameObject );
        }
        hitPosition = new Vector3( transform.position.x, transform.position.y, 0f );
        if ( !MainLoader.ArcadeMode ) {
            Health = INITIAL_HEALTH;
        }
        else {
            Health = MainLoader.ProvidedArcadeHealth;
        }
        moveSpeed = INITIAL_SPEED;
        boxCollider.enabled = false;
        physCollider.enabled = false;
        StartCoroutine( SetupFade( 0f ) );
        moving = false;
    }

    public void EnableMedusaNow() {
        StartCoroutine( EnableMedusaAfterDelay( 0f ) );
        GameManager.instance.MakeReactivateEnemySoundAfterDelayShell( 0f + .5f );
    }

    private IEnumerator SetupFade( float delay ) {
        yield return new WaitForSeconds( delay );
        fading = true;
        fadeTime = Time.time;
    }

    private IEnumerator LevitateAfterDelay( float delay ) {
        yield return new WaitForSeconds( delay );
        turnOffSort = true;
        levitating = true;
    }

    private IEnumerator PlayReanimateAfterDelay( float delay ) {
        yield return new WaitForSeconds( delay );
        SoundManager.instance.PlaySfxNoPitching( GetDistanceFromHero(), reanimate );
    }

    private IEnumerator PlayLevitateAfterDelay( float delay ) {
        yield return new WaitForSeconds( delay );
        SoundManager.instance.RandomizeSfx( GetDistanceFromHero(), rise );
    }

    private IEnumerator StopLevitateAfterDelay( float delay ) {
        yield return new WaitForSeconds( delay );
        levitating = false;
    }

    private IEnumerator ResetMovementDirectionSet( float delay ) {
        yield return new WaitForSeconds( delay );
        movementDirectionSet = false;
    }

    private IEnumerator EnablePoseidonAfterDelay( float delay ) {
        yield return new WaitForSeconds( delay );
        Poseidon.instance.EnableAtPoint( transform.position.x, transform.position.y + 2.5f );
    }

    private void SetMovementDirectionSet() {
        movementDirectionSet = true;
        Debug.Log( "Movement Set True" );
    }

    private void ResetAttackState() { needToAttack = false; }

    private void OnCollisionEnter2D( Collision2D collision ) {
        if ( collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Pillar" ) {
            Debug.Log( "I hit a " + collision.gameObject.tag + " while facing " + this.CurrentOrientation + " and my current velocity is " + rb2D.velocity );
        }
    }

    private IEnumerator EnableMedusaAfterDelay( float delay ) {
        yield return new WaitForSeconds( delay );
        EnableMedusa();
    }

    private void EnableMedusa() {
        hit = false;
        if ( GameManager.instance.GetMedusaLight() ) {
            Destroy( GameManager.instance.GetMedusaLight().gameObject );
        }
        transform.position = GetSpawnPoint();
        turnOffSort = false;
        Material mat = Resources.Load( "Darkness", typeof( Material ) ) as Material;
        spriteRenderer.material = mat;
        fading = false;
        spriteRenderer.color = new Color( 1f, 1f, 1f, 1f );
        if ( boxCollider ) { boxCollider.enabled = true; }
        if ( physCollider ) { physCollider.enabled = true; }
        beheading = false;
        attacking = false;
        moving = true;
    }

    private float GetOffsetX() {
        return offsetDictionary.orientationToVectorDictionary[CurrentOrientation].x;
    }

    private float GetOffsetY() {
        return offsetDictionary.orientationToVectorDictionary[CurrentOrientation].y;
    }

    private float GetRaycastOffsetX() {
        return raycastOffsetDictionary.orientationToVectorDictionary[CurrentOrientation].x;
    }

    private float GetRaycastOffsetY() {
        return raycastOffsetDictionary.orientationToVectorDictionary[CurrentOrientation].y;
    }

    private void SetAttack() {
        if ( GameManager.instance.DoesHeadExist() ) { return; }
        Stop();
        GameManager.instance.SpawnMedusaLight();
        attacking = true;
        needToAttack = true;
    }

    private void ToggleSpriteVisible() {
        if ( spriteRenderer.enabled == true ) {
            spriteRenderer.enabled = false;
        }
        else {
            spriteRenderer.enabled = true;
        }
    }

    private void RevertSpeed() {
        moveSpeed = INITIAL_SPEED;
    }

    private void Attack() {
        StartCoroutine( StartMovingAfterAttacking( stillAttackingTime ) );
        StartCoroutine( SpawnHeadAfterDelay( timeToSpawnHead ) );
        ResetAttackState();
        SoundManager.instance.RandomizeSfx( GetDistanceFromHero(), shoot );
    }

    IEnumerator SpawnHeadAfterDelay( float delay ) {
        yield return new WaitForSeconds( delay );
        SpawnHead();
    }

    private void SpawnHead() {
        if ( beheading ) {
            return;
        }
        GameManager.instance.SpawnHead( transform.position.x + GetOffsetX(),
            transform.position.y + GetOffsetY() );
    }

    private IEnumerator StartMovingAfterAttacking( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        ResumeMovement();
    }

    private void ResumeMovement() {
        attacking = false;
        moving = true;
    }

    private void RaycastHeroWithCircleCast() {
        raycastHero = Cast();
        if ( raycastHero.collider != null && raycastHero.collider.gameObject.tag == "Hero" ) {
            SetAttack();
        }
    }

    private RaycastHit2D Cast() {
        raycastPositionVector.Set( transform.position.x + GetRaycastOffsetX(), transform.position.y + GetRaycastOffsetY() );
        return Physics2D.CircleCast( raycastPositionVector, radiusDictionary.orientationToFloatDictionary[CurrentOrientation],
                                             VectorOrientationDictionary.orientationToVDirDictionary[CurrentOrientation],
                                             Mathf.Infinity, pillarHeroWallLayer );
    }

    private void RayCastForHero() {
        if ( !needToAttack ) {
            RaycastHeroWithCircleCast();
        }
    }
    private IEnumerator ToggleSpriteAfterDelay( float delayTime ) {
        yield return new WaitForSeconds( delayTime );
        ToggleSpriteVisible();
    }

    private void MeasureDistanceToPlayer() {
        xDistanceToPlayer = Mathf.Abs( target.position.x - transform.position.x );
        yDistanceToPlayer = Mathf.Abs( target.position.y - transform.position.y );
    }

    protected override void SetAnimation() {
        animator.SetBool( "movingclose", false );
        DisableAnimationBooleans();
        if ( FinalDeath ) {
            if ( GameManager.instance.GetHero().VictoryOrientation == OrientationEnum.LEFT ) {
                transform.localScale = new Vector3( -1f, 1f );
            }
            animator.SetBool( "finaldeath", true );
        }
        if ( beheading ) { SeBeheadingingAnimation(); }
        else if ( needToAttack ) { SetAttackingAnimation(); }
        else if ( moving ) {
            if ( ( xDistanceToPlayer + yDistanceToPlayer ) < distanceForCloseWalk ) {
                SetMovingCloseAnimation();
            }
            else {
                SetMovingAnimation();
            }

        }
        SetAnimationOrientation();
    }

    private void SetAttackingAnimation() {
        animator.SetBool( attackingString, true );
    }

    private void SeBeheadingingAnimation() {
        animator.SetBool( beheadingString, true );
    }

    private void SetMovingAnimation() {
        animator.SetBool( movingString, true );
    }

    private void SetMovingCloseAnimation() {
        animator.SetBool( "movingclose", true );
    }

    protected override void Move() {
        rb2D.velocity = VectorOrientationDictionary.orientationToVDirDictionary[CurrentOrientation] * moveSpeed;
    }

    protected override void FixedUpdate() {
        if ( !MainLoader.ArcadeMode ) {
            moveSpeed = INITIAL_SPEED + ( ( .1f ) * ( 5 - GameManager.instance.GetEnemy().Health ) );
        }
        base.FixedUpdate();
        if ( levitating ) {
            Vector2 levitateVector = new Vector2( Vector2.up.x, Vector2.up.y );
            rb2D.velocity = levitateVector * 10;
        }
    }
}