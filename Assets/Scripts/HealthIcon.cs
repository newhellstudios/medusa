using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour {

    // Use this for initialization
    void Start() {
        if ( MainLoader.ArcadeMode ) {
            transform.GetComponent<Image>().enabled = true;
            //transform.GetComponent<RectTransform>().position = new Vector3( ( Screen.width / 1.2f ), ( Screen.height / 1.2f ) );
        }

    }

}
