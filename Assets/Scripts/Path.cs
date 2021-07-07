using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Path : MonoBehaviour
{
    public int maxDecorationCount;
    public int minDecorationCount;
    private string title;
    private List<Decoration> _points;
    private string decorationType;

    public string DecorationType
    {
        get => decorationType;
        set => decorationType = value;
    }

    private void Awake()
    {
        
        _points = new List<Decoration>();
        for (int i = -Map.FieldSize; i <= Map.FieldSize; i++)
        {
            _points.Add(new Decoration("None", i, false));
        }
    }

    public void RemovePoint(string decorationName, int position)
    {
        for (int i = 0; i < _points.Count; i++)
        {
            if (position.Equals(_points[i].Position))
            {
                _points[i].Occupation = true;
                _points[i].Name = decorationName;
            }
        }
    }

    public int GetPoint()
    {
        return _points[Random.Range(0, _points.Count)].Position;
    }

    public bool CheckPoint(int position)
    {
        return _points.Find(x => x.Position == position).Occupation;
    }
}
