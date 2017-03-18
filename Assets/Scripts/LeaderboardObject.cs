using UnityEngine;


public class LeaderboardObject  {
    private string userName;
    private float timeInMilliseconds;
    private float rank;

    public LeaderboardObject( string userName, float timeInMilliseconds, float rank ) {
        this.userName = userName;
        this.timeInMilliseconds = timeInMilliseconds;
        this.rank = rank;
    }

    public string UserName {
        get {
            return userName;
        }

        set {
            userName =  value ;
        }
    }

    public float TimeInMilliseconds {
        get {
            return timeInMilliseconds;
        }

        set {
            timeInMilliseconds =  value ;
        }
    }

    public float Rank {
        get {
            return rank;
        }

        set {
            rank =  value ;
        }
    }
}