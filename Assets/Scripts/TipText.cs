using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipText : MonoBehaviour {

    public static TipText instance = null;

    // Use this for initialization
    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
        else if ( instance != this ) {
            Destroy( gameObject );
        }
    }


    void Start() {

        transform.GetComponent<TextMesh>().color = new Color( .5f, .5f, .5f, 1f );
    }

    // Update is called once per frame
    void Update() {

    }

    public void EnableText() {
        transform.GetComponent<MeshRenderer>().enabled = true;
    }

    public void DisableText() {
        transform.GetComponent<MeshRenderer>().enabled = false;
    }
}
