using UnityEngine;
using System.Collections;

public class SnuffAndBurnSounds : MonoBehaviour {

    public AudioClip burning;
    public AudioClip snuffing;
    public static SnuffAndBurnSounds instance = null;
    private bool active;
    public AudioSource efxSource;
    private static readonly float VOLUME = 1f;

    public bool Active
    {
        get
        {
            return active;
        }

        set
        {
            active = value;
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

    public void PlaySnuff() {
        if ( !efxSource.clip || ( !efxSource.clip.Equals( snuffing ) ) ) {
            efxSource.clip = snuffing;
            efxSource.loop = false;
            efxSource.Play();
        }
    }

    public void PlayBurning() {
        if ( !efxSource.clip || ( !efxSource.clip.Equals( burning ) ) ) {
            efxSource.clip = burning;
            efxSource.Play();
            efxSource.loop = true;
        }
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float xDistanceToPlayer = Mathf.Abs( GameManager.instance.GetHero().transform.position.x - transform.position.x );
        float yDistanceToPlayer = Mathf.Abs( GameManager.instance.GetHero().transform.position.y - transform.position.y );
        efxSource.volume = .5f - ( ( xDistanceToPlayer + yDistanceToPlayer ) / 22f );
    }
}
