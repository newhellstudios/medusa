using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainLoader : MonoBehaviour {

    private int selectedIndex = 0;
    private Dictionary<String, Vector3> menuOptions;
    public GameObject selector;
    private float CHANGE_POSITION_VALUE = .13f;
    private static bool arcadeMode;
    private static int providedArcadeHealth = 5;

    private String healthInput;
    private bool healthInputEnabled;
    private GameObject healthInputField;
    private GameObject arcadeExplanation;
    private bool optionSelected;

    private GameObject textHolder;
    private bool arcadeEnabled;
    private string text;
    private string newText;
    private bool canSwitchAgain;

    private List<int> arcadeEnabledHPAmounts = new List<int>();
    private int HP_AMOUNT_1 = 10;

    public AudioClip menuSelect;
    public AudioClip menuChange;

    private Color menuRed;

    public static bool ArcadeMode
    {
        get
        {
            return arcadeMode;
        }

        set
        {
            arcadeMode = value;
        }
    }

    public static int ProvidedArcadeHealth
    {
        get
        {
            return providedArcadeHealth;
        }

        set
        {
            providedArcadeHealth = value;
        }
    }

    public bool HealthInputEnabled
    {
        get
        {
            return healthInputEnabled;
        }

        set
        {
            healthInputEnabled = value;
        }
    }

    public void LoadScene() {
        GetComponent<AudioSource>().clip = menuSelect;
        GetComponent<AudioSource>().Play();
        MusicManager.instance.FadingOut = true;
        StartCoroutine( MusicManager.instance.SetFadeBackIn() );
        AutoFade.LoadLevel( "game", 2.25f, 1, Color.black );
    }

    private void Awake() {
        menuRed = new Color( 195 / 255.0F, 28 / 255.0F, 36 / 255.0F, 1f );
        // Array of menu item control names.
        menuOptions = new Dictionary<String, Vector3>();

        if ( !PlayerPrefs.HasKey( "ArcadeHealth" ) ) {
            PlayerPrefs.SetInt( "ArcadeHealth", 10 );
        }

        if ( !PlayerPrefs.HasKey( "beatStory" ) ) {
            PlayerPrefs.SetInt( "beatStory", 0 );
        }

        float anchorMinYLeft = selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMin.y;
        float anchorMaxYLeft = selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMax.y;

        float anchorMinYRight = selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMin.y;
        float anchorMaxYRight = selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMax.y;


        menuOptions.Add( "Story Mode", new Vector3( anchorMinYLeft, anchorMaxYLeft ) );
        menuOptions.Add( "Arcade Mode", new Vector3( anchorMinYLeft, anchorMaxYLeft - CHANGE_POSITION_VALUE ) );
        menuOptions.Add( "Controls/Tips", new Vector3( anchorMinYLeft, anchorMaxYLeft - ( CHANGE_POSITION_VALUE * 2 ) ) );
        menuOptions.Add( "Support the Developers", new Vector3( anchorMinYLeft, anchorMaxYLeft - ( CHANGE_POSITION_VALUE * 3 ) ) );
        menuOptions.Add( "Quit to Desktop", new Vector3( anchorMinYLeft, anchorMaxYLeft - ( CHANGE_POSITION_VALUE * 4 ) ) );

    }

    private void Start() {
        arcadeEnabled = false;
        healthInputField = GameObject.Find( "HealthInputField" );
        healthInputField.SetActive( false );
        textHolder = GameObject.Find( "TextHolder" );
        if ( PlayerPrefs.GetInt( "beatStory" ) == 0 ) {
            GameObject.Find( "text menu arcade mode" ).GetComponent<SpriteRenderer>().color = Color.gray;
        }
        GameObject.Find( "text menu first blood" ).GetComponent<SpriteRenderer>().color = menuRed;


    }

    void OnGUI() {
        if ( HealthInputEnabled ) {

            text = healthInputField.GetComponent<InputField>().text;
            newText = Regex.Replace( text, "[^\\d]", "" );
            if ( newText != "" ) {
                if ( int.Parse( newText ) < 10 ) {
                    newText = "10";
                }
                else if ( int.Parse( newText ) > 10 ) {
                    newText = "10";
                }
            }
            healthInputField.GetComponent<InputField>().text = newText;
            //healthInputField.GetComponent<InputField>().ActivateInputField();
        }

    }

    private void DisableUI() {
        selector.SetActive( false );
        textHolder.SetActive( false );
    }




    private void Update() {
        if ( AutoFade.Fading ) {
            return;
        }

        if ( HealthInputEnabled ) {
            if ( !arcadeEnabled && Input.GetButtonUp( "attack" ) && newText != "" ) {
                arcadeEnabled = true;
                PlayerPrefs.SetInt( "ArcadeHealth", int.Parse( newText ) );
                providedArcadeHealth = int.Parse( newText );
                LoadScene();
                return;
            }
            return;
        }

		if ( Math.Round(Input.GetAxisRaw( "Vertical" )) < .1f && Math.Round(Input.GetAxisRaw( "Vertical" )) > -.1f ) {
            canSwitchAgain = true;
        }


		if ( canSwitchAgain && Math.Round(Input.GetAxisRaw( "Vertical" )) > .1f ) {
            canSwitchAgain = false;
            menuSelection( "up" );
            GetComponent<AudioSource>().Play();
        }

		else if ( canSwitchAgain && Math.Round(Input.GetAxisRaw( "Vertical" )) < -.1f ) {
            canSwitchAgain = false;
            menuSelection( "down" );
            GetComponent<AudioSource>().Play();
        }

        else if ( !optionSelected && Input.GetButtonUp( "attack" ) ) {
            switch ( selectedIndex ) {
                case 0:
                    optionSelected = true;
                    ArcadeMode = false;
                    LoadScene();
                    break;
                case 1:
                    if ( PlayerPrefs.GetInt( "beatStory" ) == 0 ) {
                        break;
                    }
                    else {
                        optionSelected = true;
                        ArcadeMode = true;
                        //Rect( ( Screen.width - w ) / 2, ( Screen.height - h ) / 2, w, h );
                        healthInputField.GetComponent<RectTransform>().position = new Vector3( ( Screen.width - 100 ) / 2, ( ( Screen.height - 32 ) / 2 ) - ( Screen.height / 4 ) );
                        //healthInputField.GetComponent<RectTransform>().pivot = new Vector2(0f,10);
                        healthInputField.SetActive( true );
                        healthInputField.GetComponent<InputField>().text = PlayerPrefs.GetInt( "ArcadeHealth" ).ToString();
                        arcadeExplanation = GameObject.Find( "ArcadeExplanation" );
                        arcadeExplanation.GetComponent<TextMesh>().text = "NUMBER OF HIT POINTS FOR MEDUSA";
                        arcadeExplanation.GetComponent<TextMesh>().text += "\nCOMPETE ON STEAM LEADERBOARDS!!";
                        arcadeExplanation.GetComponent<RectTransform>().position = new Vector3( 0, 0, -1 );
                        arcadeExplanation.GetComponent<RectTransform>().anchorMax = new Vector2( 0, -.05f );
                        arcadeExplanation.GetComponent<RectTransform>().anchorMin = new Vector2( 0, -.05f );
                        DisableUI();
                        HealthInputEnabled = true;
                        //LoadScene();
                        break;
                    }
                case 2:
                    optionSelected = true;
                    AutoFade.LoadLevel( "manual", 1, 1, Color.black );
                    break;
                case 3:
                    optionSelected = true;
                    AutoFade.LoadLevel( "devs", 1, 1, Color.black );
                    break;
                case 4:
                    optionSelected = true;
                    Application.Quit();
                    break;
            }

        }

        selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMin = new Vector2( selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMin.x, menuOptions.ElementAt( selectedIndex ).Value.x );

        selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMax = new Vector2( selector.transform.GetChild( 0 ).GetComponent<RectTransform>().anchorMax.x, menuOptions.ElementAt( selectedIndex ).Value.y );


        selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMin = new Vector2( selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMin.x, menuOptions.ElementAt( selectedIndex ).Value.x );

        selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMax = new Vector2( selector.transform.GetChild( 1 ).GetComponent<RectTransform>().anchorMax.x, menuOptions.ElementAt( selectedIndex ).Value.y );


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

        switch ( selectedIndex ) {
            case 0:
                ClearColors();
                GameObject.Find( "text menu first blood" ).GetComponent<SpriteRenderer>().color = menuRed;
                break;
            case 1:
                ClearColors();
                if ( PlayerPrefs.GetInt( "beatStory" ) == 0 ) {
                    TipText.instance.EnableText();
                }
                else {
                    GameObject.Find( "text menu arcade mode" ).GetComponent<SpriteRenderer>().color = menuRed;
                }
                break;
            case 2:
                ClearColors();
                GameObject.Find( "text menu controls tips" ).GetComponent<SpriteRenderer>().color = menuRed;
                break;
            case 3:
                ClearColors();
                GameObject.Find( "text menu support the devs" ).GetComponent<SpriteRenderer>().color = menuRed;
                break;
            case 4:
                ClearColors();
                GameObject.Find( "text menu quit to desktop" ).GetComponent<SpriteRenderer>().color = menuRed;
                break;
        }

    }

    private void ClearColors() {
        TipText.instance.DisableText();
        GameObject[] textMenuObjs = GameObject.FindGameObjectsWithTag( "TextMenu" );
        foreach ( GameObject textMenu in textMenuObjs ) {
            textMenu.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if ( PlayerPrefs.GetInt( "beatStory" ) == 1 ) {
            GameObject.Find( "text menu arcade mode" ).GetComponent<SpriteRenderer>().color = Color.white;
        }

    }


}