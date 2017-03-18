using System.Collections.Generic;
using UnityEngine;

public class VectorDirectionToOrientationDictionary {
    public Dictionary<UnityEngine.Vector2, OrientationEnum> vDirToOrientationDictionary;
    public Dictionary<OrientationEnum, UnityEngine.Vector2> orientationToVDirDictionary;
    public VectorDirectionToOrientationDictionary() {
        vDirToOrientationDictionary = new Dictionary<Vector2, OrientationEnum>();
        vDirToOrientationDictionary.Add( Vector2.up, OrientationEnum.UP );
        vDirToOrientationDictionary.Add( Vector2.down, OrientationEnum.DOWN );
        vDirToOrientationDictionary.Add( Vector2.left, OrientationEnum.LEFT );
        vDirToOrientationDictionary.Add( Vector2.right, OrientationEnum.RIGHT );
        vDirToOrientationDictionary.Add( new Vector2(0,0), OrientationEnum.RIGHT );

        orientationToVDirDictionary = new Dictionary<OrientationEnum, Vector2>();
        orientationToVDirDictionary.Add( OrientationEnum.RIGHT, Vector2.right );
        orientationToVDirDictionary.Add( OrientationEnum.LEFT, Vector2.left );
        orientationToVDirDictionary.Add( OrientationEnum.UP, Vector2.up );
        orientationToVDirDictionary.Add( OrientationEnum.DOWN, Vector2.down );
    }
}