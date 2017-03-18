using UnityEngine;
using System.Collections;

public class PreLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AutoFade.LoadLevel( "logo", .5f, 2, Color.black );
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
