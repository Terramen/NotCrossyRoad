using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBuilder : MonoBehaviour
{

    [SerializeField] private int[] _pathWeights;
    [SerializeField] private int _pathGenerateNumber;
    [SerializeField] private GameObject _player;

    [SerializeField] private Path _startPath;
    [SerializeField] private GameObject[] _paths;
    
    [SerializeField] private GameObject[] _vehicles;
    [SerializeField] private GameObject[] _waterDecorations;
    [SerializeField] private GameObject[] _forestDecorations;
    [SerializeField] private GameObject[] _trains;

    [SerializeField] private PrefabPooling _pooling;
    
    private int _percentage;
    private int _chanceSum;
    private int _zPozition;
    private bool _moveDirection;
    private int _randomPositionNumber;
    private bool _isLilyPadCreated;
    private GameObject _pathType;
    private List<Path> _currentPaths;

    private GameObject _currentDecoration;

    public List<Path> CurrentPaths => _currentPaths;
    
    

    void Awake()
    {
        _currentPaths = new List<Path>();
        _startPath.Init(Map.FIELDSIZE);
        _currentPaths.Add(_startPath);
        for (int i = 0; i < _pathWeights.Length; i++)
        {
            _chanceSum += _pathWeights[i];
        }
        for (int i = 0; i < _pathGenerateNumber; i++)
        {
            GenerateRandomTerrain();
        }
    }
    
    private GameObject NextRandomTerrain()
    {
        if (_paths.Length == _pathWeights.Length)
        {
            _percentage = Random.Range(0, _chanceSum);
            for (int i = 0; i < _pathWeights.Length; i++)
            {
                if (_percentage < _pathWeights[i])
                {
                    return _paths[i];
                }
                
                _percentage -= _pathWeights[i];
            }
        }
        
        return null;
    }
    
    public void GenerateRandomTerrain()
    {
        _pathType = NextRandomTerrain();
        Path path = _pathType.GetComponent<Path>();
        //path.PathNames
        
        GameObject terrain;
        if (_pooling.CheckPathQueue(path.PathNames))
        {
            terrain = _pooling.EnablePath(path.PathNames, ++_zPozition);
        }
        else
        {
            terrain = Instantiate(_pathType,new Vector3(_pathType.transform.position.x, _pathType.transform.position.y,
                ++_zPozition), transform.rotation);
        }
        Path terrainPath = terrain.GetComponent<Path>();
        _currentPaths.Add(terrainPath);
        switch (terrainPath.PathNames)
        {
            case PathNames.GRASS:
                terrainPath.Init(Map.FIELDSIZE);
                _isLilyPadCreated = false;
                for (int j = 0; j < _forestDecorations.Length; j++)
                {
                    _currentDecoration = _forestDecorations[j];
                    GenerateDecoration(terrainPath);
                }
                break;
            case PathNames.ROAD:
                terrainPath.Init(Map.FIELDSIZE);
                _isLilyPadCreated = false;
                _currentDecoration = _vehicles[Random.Range(0, _vehicles.Length)];
                GenerateDecoration(terrainPath);
                break;
            case PathNames.WATER:
                terrainPath.Init(Map.BORDER);
                if (!_isLilyPadCreated)
                {
                    _currentDecoration = _waterDecorations[Random.Range(0, _waterDecorations.Length)];
                    if (_currentDecoration.TryGetComponent(out DecorationMovement decorationMovement))
                    {
                        if (decorationMovement.DecorationName.Equals(DecorationNames.LILYPAD))
                        {
                            terrainPath.Init(Map.BORDER);
                            _isLilyPadCreated = true;
                        }
                    }
                    GenerateDecoration(terrainPath);  
                }
                else
                {
                    terrainPath.Init(Map.FIELDSIZE);
                    _currentDecoration = _waterDecorations[Random.Range(1, _waterDecorations.Length)];
                    GenerateDecoration(terrainPath); 
                }
                break;
            case PathNames.RAIL:
                terrainPath.Init(Map.FIELDSIZE);
                _currentDecoration = _trains[Random.Range(0, _trains.Length)];
                GenerateDecoration(terrainPath);
                break;
            case PathNames.NONE:
                break;
        }
    }

    private void GenerateDecoration(Path path)
    {
        int decorationCount = Random.Range(path.minDecorationCount, path.maxDecorationCount);
        
        /*if (_currentDecoration.TryGetComponent(out DecorationMovement currentDecorationMovement))
        {
            decorationName = currentDecorationMovement.DecorationName;
            if (currentDecorationMovement.MultipleDecoration)
            {
                decorationCount = 1;
            }
        }*/
        // ????
        DecorationMovement currentDecorationMovement = _currentDecoration.GetComponent<DecorationMovement>();
        DecorationNames decorationName = currentDecorationMovement.DecorationName;
        if (currentDecorationMovement.MultipleDecoration)
        {
            decorationCount = 1;
        }

        for (int i = 0; i < decorationCount; i++)
        {

            _randomPositionNumber = path.GetPoint();
            if (!path.GetOccupation(_randomPositionNumber))
            {
                GameObject decoration;
                if (_pooling.CheckDecorationQueue(decorationName))
                {
                    decoration = _pooling.EnableDecoration(decorationName, _randomPositionNumber, _zPozition);
                }
                else
                {
                    decoration = Instantiate(_currentDecoration, new Vector3(_randomPositionNumber,
                        _currentDecoration.transform.position.y, _zPozition ), _currentDecoration.transform.rotation);
                }
                
                //decoration.transform.parent = path.transform; // 
                path.RemovePoint(_randomPositionNumber, decoration.layer);
                if (decoration.TryGetComponent(out DecorationMovement decorationMovement))
                {
                    path.AddActiveDecoration(decorationMovement);
                    if (GenerateRandomDirection())
                    {
                        decorationMovement.Speed *= (-1);
                    }
                }
            }
        }
    }

    private bool GenerateRandomDirection()
    {
        return Random.Range(0, 2) > 0;
    }

    public void RemovePath(int pathNumber)
    {
        _pooling.AddDisabledPath(_currentPaths[pathNumber].PathNames, _currentPaths[pathNumber].DisablePath());
        for (int i = 0; i < _currentPaths[pathNumber].ActiveDecorations.Count; i++)
        {
            _pooling.AddDisabledDecoration(_currentPaths[pathNumber].ActiveDecorations[i].DecorationName, _currentPaths[pathNumber].ActiveDecorations[i].DisableDecoration());
        }
        _currentPaths[pathNumber].ClearDecorations();
    }

}

public enum PathNames
{
    NONE,
    GRASS,
    ROAD,
    WATER,
    RAIL
}

