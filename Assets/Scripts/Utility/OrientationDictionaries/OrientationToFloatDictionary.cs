using System.Collections.Generic;
using UnityEngine;

public class OrientationToFloatDictionary {
    public Dictionary<OrientationEnum, float> orientationToFloatDictionary;
    public OrientationToFloatDictionary( float floatUp, float floatDown, float floatLeft, float floatRight ) {
        orientationToFloatDictionary = new Dictionary<OrientationEnum, float>();
        orientationToFloatDictionary.Add( OrientationEnum.UP, floatUp );
        orientationToFloatDictionary.Add( OrientationEnum.DOWN, floatDown );
        orientationToFloatDictionary.Add( OrientationEnum.LEFT, floatLeft );
        orientationToFloatDictionary.Add( OrientationEnum.RIGHT, floatRight );
    }
}