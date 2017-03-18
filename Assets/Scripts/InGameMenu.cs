using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour {

    private int selectedIndex = 0;
    private Dictionary<String, Vector3> menuOptions;
    public GameObject selector;
    private float CHANGE_POSITION_VALUE = .112f;
    public static InGameMenu instance;

    private bool optionSelected;

    private bool menuVisible;

    private GameObject textHolder;
    private bool canSwitchAgain;

    public bool MenuVisible {
        get {
            return menuVisible;
        }

        set {
            menuVisible = value;
        }
    }

    private void Awake() {
        if ( instance == null ) { instance = this; }
        else if ( instance != this ) { Destroy( gameObject ); }
        // Array of menu item control names.
        menuOptions = new Dictionary<String, Vector3>();

        float anchorMinYLeft = selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMin.y;
        float anchorMaxYLeft = selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMax.y;

        float anchorMinYRight = selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMin.y;
        float anchorMaxYRight = selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMax.y;


        menuOptions.Add( "Quit to Main Menu", new Vector3( anchorMinYLeft, anchorMaxYLeft ) );
        menuOptions.Add( "Quit to Desktop", new Vector3( anchorMinYLeft, anchorMaxYLeft - CHANGE_POSITION_VALUE ) );

        // Default selected menu item (in this case, Tutorial).

    }

    private void Start() {
        optionSelected = false;
        textHolder = GameObject.Find( "TextHolder" );
        DisableMenu();
    }

    private void DisableMenu() {
        selector.SetActive( false );
        textHolder.SetActive( false );
    }

    private void EnableMenu() {
        selector.SetActive( true );
        textHolder.SetActive( true );
    }

    private void Update() {
        if ( Input.GetButtonDown( "Esc" ) ) {
            if ( MenuVisible ) {
                DisableMenu();
                MenuVisible = false;
            }
            else {
                EnableMenu();
                MenuVisible = true;
            }
        }

        if ( MenuVisible ) {
            if ( Input.GetAxisRaw( "Vertical" ) < .1f && Input.GetAxisRaw( "Vertical" ) > -.1f ) {
                canSwitchAgain = true;
            }


            if ( canSwitchAgain && Input.GetAxisRaw( "Vertical" ) > .1f ) {
                canSwitchAgain = false;
                menuSelection( "up" );
                GetComponent<AudioSource>().Play();
            }

            else if ( canSwitchAgain && Input.GetAxisRaw( "Vertical" ) < -.1f ) {
                canSwitchAgain = false;
                menuSelection( "down" );
                GetComponent<AudioSource>().Play();
            }

            else if ( !optionSelected && Input.GetButton( "attack" ) ) {
                optionSelected = true;
                switch ( selectedIndex ) {
                    case 0:
                        MusicManager.instance.VictorySong();
                        AutoFade.LoadLevel( "main", 1, 1, Color.black );
                        break;
                    case 1:
                        Application.Quit();
                        break;
                }

            }

            selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMin = new Vector2( selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMin.x, menuOptions.ElementAt( selectedIndex ).Value.x );
            selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMax = new Vector2( selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMax.x, menuOptions.ElementAt( selectedIndex ).Value.y );
            selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMin = new Vector2( selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMin.x, menuOptions.ElementAt( selectedIndex ).Value.x );
            selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMax = new Vector2( selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMax.x, menuOptions.ElementAt( selectedIndex ).Value.y );
        }
    }





    private void menuSelection( String direction ) {
        if ( direction == "up" ) {
            if ( selectedIndex == 0 ) {
                selectedIndex = menuOptions.Count - 1;
            }
            else {
                selectedIndex -= 1;
            }
        }

        if ( direction == "down" ) {
            if ( selectedIndex == menuOptions.Count - 1 ) {
                selectedIndex = 0;
            }
            else {
                selectedIndex += 1;
            }
        }

    }


}