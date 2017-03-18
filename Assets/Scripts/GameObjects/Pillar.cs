using UnityEngine;
using System.Collections;
using System;

public class Pillar : RenderedObject {

    protected override void Start() {
        base.Start();
        GameManager.instance.RegisterPillar( this );
        SortByYPillar();
        spriteRenderer.sortingOrder -= 64;
    }
}