using UnityEngine;
using System.Collections;

public abstract class VariableOrientationObject : RenderedObject {

    private OrientationEnum currentOrientation;
    protected string orientationXString = "orientationX";
    protected string orientationYString = "orientationY";
    private VectorDirectionToOrientationDictionary vectorOrientationDictionary;

    public OrientationEnum CurrentOrientation {
        get {
            return currentOrientation;
        }

        set {
            currentOrientation =  value ;
        }
    }

    public VectorDirectionToOrientationDictionary VectorOrientationDictionary {
        get {
            return vectorOrientationDictionary;
        }

        set {
            vectorOrientationDictionary =  value ;
        }
    }

    public OrientationEnum GetCurrentOrientation() { return CurrentOrientation; }

    protected override void Start() {
        base.Start();
        VectorOrientationDictionary = new VectorDirectionToOrientationDictionary();
    }

    protected void SetAnimationOrientation() {
        animator.SetFloat( orientationXString, VectorOrientationDictionary.orientationToVDirDictionary[CurrentOrientation].x );
        animator.SetFloat( orientationYString, VectorOrientationDictionary.orientationToVDirDictionary[CurrentOrientation].y );
    }
}