using UnityEngine;
using System.Collections;

public class RenderTextUp : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        if ( renderer != null ) {
            renderer.sortingOrder = 2000;
            renderer.sortingLayerName = "Units";
        }
    }

}
