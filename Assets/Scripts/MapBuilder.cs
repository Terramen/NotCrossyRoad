using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private GameObject[] _terrains; 
    [SerializeField] private int[] _terrainWeights;
    [SerializeField] private int _terrainGenerateNumber;
    
    private int _percentage;
    private int _chanceSum;
    private int _zPozition;

    void Awake()
    {
        for (int i = 0; i < _terrainWeights.Length; i++)
        {
            _chanceSum += _terrainWeights[i];
        }
    }

    void Start()
    {
        GenerateRandomTerrain();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {
            GameObject go = NextRandomTerrain();
            var transformPosition = go.transform.position;
            Instantiate(go,new Vector3(transformPosition.x,transformPosition.y,_zPozition++), transform.rotation);
        }
        
    }
    
    
    private GameObject NextRandomTerrain()
    {
        if (_terrains.Length == _terrainWeights.Length)
        {
            _percentage = Random.Range(1, _chanceSum);
            for (int i = 0; i < _terrainWeights.Length; i++)
            {
                if (_percentage < _terrainWeights[i])
                {
                    Debug.Log(_terrains[i]);
                    return _terrains[i];
                }
                
                _percentage -= _terrainWeights[i];
            }
        }

        return null;
    }
    
    private void GenerateRandomTerrain()
    {
        for (int i = 0; i < _terrainGenerateNumber; i++)
        {
            GameObject go = NextRandomTerrain();
            var transformPosition = go.transform.position;
            Instantiate(go,new Vector3(transformPosition.x,transformPosition.y,_zPozition++), transform.rotation); 
        }
    } 

}
