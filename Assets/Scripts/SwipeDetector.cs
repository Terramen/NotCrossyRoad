using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;

    [SerializeField] private bool _detectSwipeOnlyAfterRelease = false;
    [SerializeField] private float _minDistanceForSwipe;
    
    public static event Action<SwipeData> OnSwipe = delegate {  };

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _fingerUpPosition = touch.position;
                _fingerDownPosition = touch.position;
            }

            if (!_detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                _fingerDownPosition = touch.position;
                DetectSwipe();
                
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (CheckMinimumSwipeDistance())
        {
            if (IsVerticalSwipe())
            {
                var direction = _fingerDownPosition.y - _fingerUpPosition.y > 0
                    ? SwipeDirection.UP
                    : SwipeDirection.DOWN;
                SendSwipe(direction);
            }
            else
            {
                var direction = _fingerDownPosition.x - _fingerUpPosition.x > 0
                    ? SwipeDirection.RIGHT
                    : SwipeDirection.LEFT;
                SendSwipe(direction);
            }
            
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool CheckMinimumSwipeDistance()
    {
        return VerticalMovementDistance() > _minDistanceForSwipe || HorizontalMovementDistance() > _minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Math.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Math.Abs(_fingerDownPosition.x - _fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            direction = direction,
            endPosition = _fingerUpPosition,
            startPosition = _fingerDownPosition
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 startPosition;
    public Vector2 endPosition;
    public SwipeDirection direction;
}

public enum SwipeDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
