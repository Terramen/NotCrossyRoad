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

    [SerializeField] private GameObject[] _paths;

    [SerializeField] private GameObject[] _vehicles;
    [SerializeField] private GameObject[] _waterDecorations;
    [SerializeField] private GameObject[] _forestDecorations;
    [SerializeField] private GameObject[] _trains;
    
    private int _percentage;
    private int _chanceSum;
    private int _zPozition;
    private bool _moveDirection;
    private int _randomPositionNumber; 
    private GameObject _pathType;
    private List<Path> _currentPaths;
    
    private GameObject _currentDecoration;
    
    

    void Awake()
    {
        for (int i = 0; i < _pathWeights.Length; i++)
        {
            _chanceSum += _pathWeights[i];
        }
        _currentPaths = new List<Path>();
        for (int i = 0; i < _pathGenerateNumber; i++)
        {
            GenerateRandomTerrain();
        }
    }
    
    private GameObject NextRandomTerrain()
    {
        if (_paths.Length == _pathWeights.Length)
        {
            _percentage = Random.Range(1, _chanceSum);
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
        GameObject terrain = Instantiate(_pathType,new Vector3(_pathType.transform.position.x,_pathType.transform.position.y,++_zPozition), transform.rotation);
        Path terrainPath = terrain.GetComponent<Path>();
        _currentPaths.Add(terrainPath);

        switch (_pathType.name)
            {
                case PathNames.Grass:
                    for (int j = 0; j < _forestDecorations.Length; j++)
                    {
                        _currentDecoration = _forestDecorations[j];
                        GenerateDecoration(terrainPath);
                    }
                    break;
                case PathNames.Road:
                    _currentDecoration = _vehicles[Random.Range(0, _vehicles.Length)];
                    GenerateDecoration(terrainPath);
                    break;
                case PathNames.Water:
                    _currentDecoration = _waterDecorations[Random.Range(0, _waterDecorations.Length)];
                    /*if (_currentDecoration.name.Equals(_currentPaths[_currentPaths.Count - 2].DecorationType))
                    {
                        _currentDecoration = _waterDecorations;
                    }*/
                    GenerateDecoration(terrainPath);
                    break;
                case PathNames.Rail:
                    _currentDecoration = _trains[Random.Range(0, _trains.Length)];
                    GenerateDecoration(terrainPath);
                    break;
            }
    }

    private void GenerateDecoration(Path path)
    {
        GameObject decoration;
        int decorationCount = Random.Range(path.minDecorationCount, path.maxDecorationCount);
        if (_currentDecoration.TryGetComponent(out DecorationMovement currentDecorationMovement))
        {
            if (currentDecorationMovement.MultipleDecoration)
            {
                decorationCount = 1;
            }
        }

        for (int i = 0; i < decorationCount; i++)
        {

            _randomPositionNumber = path.GetPoint();
            if (!path.CheckPoint(_randomPositionNumber))
            {
                path.RemovePoint(_currentDecoration.name, _randomPositionNumber);
                decoration = Instantiate(_currentDecoration, new Vector3(_randomPositionNumber, _currentDecoration.transform.position.y, _zPozition ), _currentDecoration.transform.rotation);
                if (decoration.TryGetComponent(out DecorationMovement decorationMovement))
                {
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

}

