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
    private List<Decoration> _points;
    private List<DecorationMovement> activeDecorations;
    
    [SerializeField] private PathNames _pathNames;

    public PathNames PathNames => _pathNames;
    public List<DecorationMovement> ActiveDecorations => activeDecorations;

    private void Awake()
    {
        activeDecorations = new List<DecorationMovement>();
    }

    public GameObject DisablePath()
    {
        gameObject.SetActive(false);
        return gameObject;
    }

    public void AddActiveDecoration(DecorationMovement decoration)
    {
        activeDecorations.Add(decoration);
    }

    public void RemovePoint(int position, int layer)
    {
        for (int i = 0; i < _points.Count; i++)
        {
            if (position.Equals(_points[i].Position))
            {
                _points[i].Occupation = true;
                _points[i].Layer = layer;
            }
        }
    }

    public void Init(int range)
    {
        _points = new List<Decoration>();
        for (int i = -range; i <= range; i++)
        {
            _points.Add(new Decoration(i, false, 0));
        }
    }

    public void ClearDecorations()
    {
        _points.Clear();
        activeDecorations.Clear();
    }

    public int GetPoint()
    {
        return _points[Random.Range(0, _points.Count)].Position;
    }

    public bool GetOccupation(int position)
    {
        return _points.Find(x => x.Position == position).Occupation;
    }
    
    public int GetLayer(int position)
    {
        return _points.Find(x => x.Position == position).Layer;
    }
    
}

public enum DecorationNames
{
    NONE, LILYPAD, LOGS, STONE, TREE, TRAIN, VEHICLE_PURPLE, VEHICLE_RED, VEHICLE_YELLOW
}
