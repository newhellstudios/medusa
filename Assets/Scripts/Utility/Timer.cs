using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    private Text timerLabel;

    private float time;
    private bool timeStopped;
    public static Timer instance;

    public float Time {
        get {
            return time;
        }

        set {
            time =  value ;
        }
    }

    public bool TimeStopped
    {
        get
        {
            return timeStopped;
        }

        set
        {
            timeStopped =  value ;
        }
    }

    public void ResetTimer() {
        Time = 0f;
    }


    public void StopTimer() {
        TimeStopped = true;
    }

    public void StartTimer() {
        ResetTimer();
        TimeStopped = false;
    }

    private void Awake() {
        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }
    }


    private void Start() {
        TimeStopped = true;
        if ( MainLoader.ArcadeMode ) {
            transform.GetComponent<Text>().enabled = true;
        }
        timerLabel = transform.GetComponent<Text>();
    }

    void Update() {
        if ( !TimeStopped ) {
            Time += UnityEngine.Time.deltaTime;
        }

        //else {
        //    if(Time < PlayerPrefs.GetFloat( "Best Time" ) ) {
        //        PlayerPrefs.SetFloat( "Best Time", Time );
        //    }
            
        //}

       // Debug.Log( PlayerPrefs.GetFloat( "Best Time" ) );

        int minutes = (int)Time / 60;  //Divide the guiTime by sixty to get the minutes.
        int seconds = (int)Time % 60; //Use the euclidean division for the seconds.
        int fraction = (int)( Time * 100 ) % 100;

        //update the label value
        timerLabel.text = minutes.ToString( "00" ) + ":" + seconds.ToString( "00" ) + ":" + fraction.ToString( "00" );
        //timerLabel.text += " \n vs.";
        //timerLabel.text += " \n BEST TIME OF: " + ( PlayerPrefs.GetFloat( "Best Time" ) / 60 ).ToString() + ":"
        //                                        + ( PlayerPrefs.GetFloat( "Best Time" ) % 60 ).ToString( "00" ) + ":"
        //                                        + ( ( PlayerPrefs.GetFloat( "Best Time" ) * 100 ) % 100 ).ToString( "00" );
        // timerLabel.text = string.Format( "{0:00} : {1:00} : {2:000}", minutes, seconds, fraction );

        timerLabel.text += " \n ";
        decimal displayHealth = System.Math.Round( (System.Decimal)GameManager.instance.GetEnemy().Health, 2, System.MidpointRounding.AwayFromZero );
        timerLabel.text += " \n" + displayHealth + "/" + PlayerPrefs.GetInt( "ArcadeHealth" ); ;
    }
}