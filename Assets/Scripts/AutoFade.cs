// AutoFade.cs
using UnityEngine;
using System.Collections;

public class AutoFade : MonoBehaviour {
    private static AutoFade m_Instance = null;
    private Material m_Material = null;
    private string m_LevelName = "";
    private int m_LevelIndex = 0;
    private bool m_Fading = false;

    private static AutoFade Instance {
        get {
            if ( m_Instance == null ) {
                m_Instance = ( new GameObject( "AutoFade" ) ).AddComponent<AutoFade>();
            }
            return m_Instance;
        }
    }
    public static bool Fading {
        get { return Instance.m_Fading; }
    }

    private void Awake() {
        DontDestroyOnLoad( this );
        m_Instance = this;
        m_Material = Resources.Load<Material>( "Plane_No_zTest" );
#if UNITY_EDITOR
        if ( m_Material == null ) {
            var resDir = new System.IO.DirectoryInfo( System.IO.Path.Combine( Application.dataPath, "Resources" ) );
            if ( !resDir.Exists )
                resDir.Create();
            Shader s = Shader.Find( "Plane/No zTest" );
            if ( s == null ) {
                string shaderText = "Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }";
                string path = System.IO.Path.Combine( resDir.FullName, "Plane_No_zTest.shader" );
                Debug.Log( "Shader missing, create asset: " + path );
                System.IO.File.WriteAllText( path, shaderText );
                UnityEditor.AssetDatabase.Refresh( UnityEditor.ImportAssetOptions.ForceSynchronousImport );
                UnityEditor.AssetDatabase.LoadAssetAtPath<Shader>( "Resources/Plane_No_zTest.shader" );
                s = Shader.Find( "Plane/No zTest" );
            }
            var mat = new Material( s );
            mat.name = "Plane_No_zTest";
            UnityEditor.AssetDatabase.CreateAsset( mat, "Assets/Resources/Plane_No_zTest.mat" );
            m_Material = mat;

        }
#endif
    }

    private void DrawQuad( Color aColor, float aAlpha ) {
        aColor.a = aAlpha;
        m_Material.SetPass( 0 );
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin( GL.QUADS );
        GL.Color( aColor );   // moved here, needs to be inside begin/end
        GL.Vertex3( 0, 0, -1 );
        GL.Vertex3( 0, 1, -1 );
        GL.Vertex3( 1, 1, -1 );
        GL.Vertex3( 1, 0, -1 );
        GL.End();
        GL.PopMatrix();
    }

    private IEnumerator Fade( float aFadeOutTime, float aFadeInTime, Color aColor ) {
        float t = 0.0f;
        while ( t < 1.0f ) {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01( t + Time.deltaTime / aFadeOutTime );
            DrawQuad( aColor, t );
        }
        if ( m_LevelName != "" )
            Application.LoadLevel( m_LevelName );
        else
            Application.LoadLevel( m_LevelIndex );
        while ( t > 0.0f ) {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01( t - Time.deltaTime / aFadeInTime );
            DrawQuad( aColor, t );
        }
        m_Fading = false;
    }
    private void StartFade( float aFadeOutTime, float aFadeInTime, Color aColor ) {
        m_Fading = true;
        StartCoroutine( Fade( aFadeOutTime, aFadeInTime, aColor ) );
    }

    public static void LoadLevel( string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor ) {
        if ( Fading ) return;
        Instance.m_LevelName = aLevelName;
        Instance.StartFade( aFadeOutTime, aFadeInTime, aColor );
    }
    public static void LoadLevel( int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor ) {
        if ( Fading ) return;
        Instance.m_LevelName = "";
        Instance.m_LevelIndex = aLevelIndex;
        Instance.StartFade( aFadeOutTime, aFadeInTime, aColor );
    }
}