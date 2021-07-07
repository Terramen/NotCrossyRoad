using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecorationMovement : MonoBehaviour
{
    
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _timeDifficultyMultiplier;
    [SerializeField] private float _delay;
    [SerializeField] private bool _multipleDecoration;

    private List<Transform> _childDecorationsTransform;

    public float Speed { get; set; }

    private void Awake()
    {
        Speed = Random.Range(_minSpeed, _maxSpeed);
        if (_multipleDecoration)
        {
            _childDecorationsTransform = new List<Transform>();
            _childDecorationsTransform.AddRange(GetComponentsInChildren<Transform>());
            _childDecorationsTransform.Remove(GetComponent<Transform>());
        }
    }

    public bool MultipleDecoration
    {
        get => _multipleDecoration;
        set => _multipleDecoration = value;
    }

    private void Update()
    {
        if (_multipleDecoration)
        {
            for (int i = 0; i < _childDecorationsTransform.Count; i++)
            {
                Move(_childDecorationsTransform[i]);
            }
        }
        else
        {
            Move(transform);
        }
    }
    
    

    IEnumerator RespawnDelay(int size, Transform decorationTransform)
    {
        yield return new WaitForSeconds(_delay);
        decorationTransform.position = new Vector3( size, decorationTransform.position.y, decorationTransform.position.z);
    }

    private void Move(Transform decorationTransform)
    {
        decorationTransform.position = new Vector3( decorationTransform.position.x + Time.deltaTime * Speed, decorationTransform.position.y, decorationTransform.position.z);
        if (Speed > 0 && decorationTransform.position.x > Map.FieldSize)
        {
            StartCoroutine(RespawnDelay(-Map.FieldSize, decorationTransform));
        }
        if (Speed < 0 && decorationTransform.position.x < -Map.FieldSize)
        {
            StartCoroutine(RespawnDelay(Map.FieldSize, decorationTransform));
        }
    }
}
