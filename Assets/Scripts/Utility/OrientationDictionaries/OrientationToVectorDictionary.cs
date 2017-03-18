using System.Collections.Generic;
using UnityEngine;

public class OrientationToVectorDictionary {
    public Dictionary<OrientationEnum, UnityEngine.Vector2> orientationToVectorDictionary;
    public OrientationToVectorDictionary(Vector2 vectorUp, Vector2 vectorDown, Vector2 vectorLeft, Vector2 vectorRight) {
        orientationToVectorDictionary = new Dictionary<OrientationEnum, Vector2>();
        orientationToVectorDictionary.Add(OrientationEnum.UP, vectorUp);
        orientationToVectorDictionary.Add( OrientationEnum.DOWN, vectorDown );
        orientationToVectorDictionary.Add( OrientationEnum.LEFT, vectorLeft );
        orientationToVectorDictionary.Add( OrientationEnum.RIGHT, vectorRight );
    }
}