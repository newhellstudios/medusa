using UnityEngine;
using System.Collections;

public abstract class RenderedObject : MonoBehaviour {

    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb2D;
    protected LayerMask blockingLayer;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer {
        get {
            return spriteRenderer;
        }
    }

    protected virtual void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void SortByY () {
        spriteRenderer.sortingOrder = Mathf.RoundToInt( transform.position.y * 100f ) * -1;
    }

    protected void SortByYPillar() {
        spriteRenderer.sortingOrder = Mathf.RoundToInt( (transform.position.y - 1f) * 100f ) * -1;
    }
}