using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int StepsBack { get; set; }

    private bool _canMoveRight = true;
    private bool _canMoveLeft = true;

    public GameObject mapBuilderGameObject;

    private MapBuilder _mapBuilder;

    void Start()
    {
        _mapBuilder = mapBuilderGameObject.GetComponent<MapBuilder>();
    }


    void Update()
    {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (StepsBack == 0)
                    {
                        _mapBuilder.GenerateRandomTerrain();
                    }
                    else if (StepsBack > 0)
                    {
                        StepsBack--;
                    }
                    if (transform.position.x < Map.Border && transform.position.x > -Map.Border)
                    {
                        Move(transform.position.x, transform.position.y, transform.position.z + 1);
                    }
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (transform.position.x < Map.Border && transform.position.x > -Map.Border)
                    {
                        Move(transform.position.x, transform.position.y, transform.position.z - 1);
                    }
                    StepsBack++;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (_canMoveLeft)
                    {
                        _canMoveRight = true;
                        if (transform.position.x < Map.Border && transform.position.x > -Map.Border)
                        {
                            if (transform.position.x != -Map.Border + 1)
                            {
                                Move(transform.position.x - 1, transform.position.y, transform.position.z);
                            }
                            else
                            {
                                _canMoveLeft = false;
                            }
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (_canMoveRight)
                    {
                        _canMoveLeft = true;
                        if (transform.position.x < Map.Border && transform.position.x > -Map.Border)
                        {
                            if (transform.position.x != Map.Border - 1)
                            {
                                Move(transform.position.x + 1, transform.position.y, transform.position.z);
                            }
                            else
                            {
                                _canMoveRight = false;
                            }
                        }
                    }
                }
    }

    private void Move(float x, float y, float z)
    {
        gameObject.transform.position = new Vector3(x, y, z);
    }
    
    
}
