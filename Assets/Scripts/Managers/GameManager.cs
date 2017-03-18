using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    private Enemy enemy;
    private Head head;
    private Sword sword;
    private Hero hero;
    private Poseidon poseidon;
    public List<Pillar> pillars;
    public List<GameObject> spawnPoints;
    private Athena athena;
    private GameObject medusaLight;
    private GameObject heroLight;
    private GameObject athenaLight;
    private Light heroLightObject;
    private List<CenterFire> centerFires = new List<CenterFire>();
    private bool gameStarted;

    public Hero GetHero() {
        return hero;
    }

    public Enemy GetEnemy() {
        return enemy;
    }

    public Head GetHead() {
        return head;
    }

    public Sword GetSword() {
        return sword;
    }

    public Athena GetAthena() {
        return athena;
    }

    public GameObject GetMedusaLight() {
        return medusaLight;
    }

    public GameObject GetHeroLight() {
        return heroLight;
    }

    public Light GetHeroLightObject() {
        return heroLightObject;
    }

    void Awake() {
        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }
        Time.timeScale = 1f;
        boardScript = GetComponent<BoardManager>();
        InitGame();
        heroLightObject = GetHeroLight().GetComponent<Light>();
    }

    public void SnuffOutCenterFires() {
        foreach ( CenterFire cf in centerFires ) {
            cf.TurnOff();
        }
    }

    public void TurnOnCenterFires() {
        foreach ( CenterFire cf in centerFires ) {
            cf.TurnOn();
        }
    }

    private void CheckMedusaHealthFire() {
        if ( MainLoader.ArcadeMode ) {
            return;
        }
        if ( GetEnemy().Health < 4.00001f ) {
            CenterFire cf = centerFires.Find( item => item.torchNumber == 0 );
            if ( !cf.TurnedOn ) {
                cf.TurnOn();
            }
        }
        if ( GetEnemy().Health < 3.00001f ) {
            CenterFire cf = centerFires.Find( item => item.torchNumber == 1 );
            if ( !cf.TurnedOn ) {
                cf.TurnOn();
            }
        }
        if ( GetEnemy().Health < 2.00001f ) {
            CenterFire cf = centerFires.Find( item => item.torchNumber == 2 );
            if ( !cf.TurnedOn ) {
                cf.TurnOn();
            }
        }
        if ( GetEnemy().Health < 1.00001f ) {
            CenterFire cf = centerFires.Find( item => item.torchNumber == 3 );
            if ( !cf.TurnedOn ) {
                cf.TurnOn();
            }
        }
        if ( GetEnemy().Health <= 0f ) {
            CenterFire cf = centerFires.Find( item => item.torchNumber == 4 );
            if ( !cf.TurnedOn ) {
                cf.TurnOn();
            }
        }
    }

    //listen for cheats
    private void Update() {
        //if ( ( hero.GetDestroying() || enemy.FinalDeath ) && ( Input.GetKey( KeyCode.Tab ) || Input.GetButton( "Fire2" ) ) ) {
        //    RestartLevel();
        //}
        if ( !gameStarted ) {
            if ( !MainLoader.ArcadeMode ) {
                TurnOnCenterFires();
            }
            StartFunctions();
            gameStarted = true;
        }
        CheckMedusaHealthFire();
    }

    private void StartFunctions() {
        GetAthena().EnableAtPointIntro( 13.832f, 14.663f );
        StartCoroutine( GetAthena().SetResurrectingAfterDelay() );
        StartCoroutine( EnablePoseidonAfterDelay() );
    }

    private IEnumerator EnablePoseidonAfterDelay() {
        yield return new WaitForSeconds( 5f );
        Poseidon.instance.EnableAtPoint( 15.3f, 15.001f );
    }


    public void RestartLevel() {
        Application.LoadLevel( Application.loadedLevelName );
    }

    void InitGame() {
        if ( !MusicManager.instance.musicSource.clip.name.Equals( "game_music" ) ) {
            MusicManager.instance.GameSong();
        }
        //Cursor.visible = false;
        boardScript.SetupScene();
        AstarPath.active.Scan();
    }

    public void RegisterEnemy( Enemy enemy ) {
        this.enemy = enemy;
    }

    public void RegisterHead( Head head ) {
        this.head = head;
    }

    public void RegisterSword( Sword sword ) {
        this.sword = sword;
    }

    public void RegisterHero( Hero hero ) {
        this.hero = hero;
    }

    public void RegisterAthena( Athena athena ) {
        this.athena = athena;
    }

    public void RegisterPoseidon( Poseidon poseidon ) {
        this.poseidon = poseidon;
    }

    public void RegisterSpawnPoint( GameObject spawnPoint ) {
        this.spawnPoints.Add( spawnPoint );
    }

    public void RegisterMedusaLight( GameObject medusaLight ) {
        this.medusaLight = medusaLight;
    }

    public void RegisterHeroLight( GameObject heroLight ) {
        this.heroLight = heroLight;
    }

    public void RegisterPillar( Pillar pillar ) {
        this.pillars.Add( pillar );
    }

    public void RegisterCenterFire( CenterFire centerFire ) {
        this.centerFires.Add( centerFire );
    }

    public void RegisterAthenaLight( GameObject athenaLightHolder ) {
        this.athenaLight = athenaLightHolder;
    }

    public void GameOver() { enabled = false; }
    public void SpawnHead( float x, float y ) { boardScript.SpawnHead( x, y ); }
    public void SpawnMedusaLight() { boardScript.SpawnMedusaLight(); }
    public void SpawnPoseidon( float x, float y ) { boardScript.SpawnPoseidon( x, y ); }
    public bool DoesHeadExist() { return ( head ); }
    public bool DoesHeroExist() { return ( hero ); }
    public void setEnemyHit() { enemy.SetHit = true; }

    public void MakeReactivateEnemySoundAfterDelayShell( float delayTime ) {
        StartCoroutine( SoundOffReactivateEnemyAfterDelay( delayTime ) );
    }

    private IEnumerator SoundOffReactivateEnemyAfterDelay( float delayTime ) {
        yield return new WaitForSeconds( delayTime - .5f );
        SoundManager.instance.RandomizeSfx( enemy.GetDistanceFromHero(), enemy.respawn );
    }

}