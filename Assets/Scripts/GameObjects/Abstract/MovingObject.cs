using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class MovingObject : VariableOrientationObject
{

    protected float moveSpeed;
    protected bool moving;
    protected bool destroying;
    protected bool turnOffSort;

    protected List<string> animationStrings;
    protected string movingString = "moving";
    protected string attackingString = "attacking";
    protected string idlingString = "idling";
    protected string destroyingString = "destroying";
    protected string chargingString = "charging";

    protected abstract void SetAnimation();
    protected abstract void Move();

    public bool GetDestroying()
    {
        return destroying;
    }

    protected virtual void FixedUpdate()
    {
        if (moving)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (!turnOffSort ) {
            SortByY();
        }
        SetAnimation();
    }

    protected override void Start()
    {
        base.Start();
        animationStrings = new List<string>();
        animationStrings.Add(movingString);
        animationStrings.Add(destroyingString);
    }

    protected void DisableAnimationBooleans()
    {
        foreach (string animationString in animationStrings)
        {
            animator.SetBool(animationString, false);
        }
    }

    protected void Stop()
    {
        moving = false;
        Vector2 temp = new Vector2( 0, 0 );
        rb2D.velocity = temp;
    }

    public float GetDistanceFromHero()
    {
        return Math.Abs(Vector2.Distance(transform.position, GameManager.instance.GetHero().transform.position));
    }

}