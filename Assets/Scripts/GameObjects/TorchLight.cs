using UnityEngine;
using System.Collections;

public class TorchLight : MonoBehaviour {

    private float maxDistance;
    private float speed;

	void Start () {
        SetValues();
    }

    private void SetValues() {
        maxDistance = ( Random.Range( 1.35f, 1.4f ) );
        speed = ( Random.Range( .48f, .5f ) );
    }


    private void LateUpdate() {
        transform.parent.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt( transform.position.y * 100f ) * -1;
    }


    void Update () {
        GetComponent<Light>().range = PingPong( Time.time * speed, 1.2f, maxDistance );
    }

    private float PingPong( float aValue, float aMin, float aMax ) {
        return Mathf.PingPong( aValue, aMax - aMin ) + aMin;
    }
}
