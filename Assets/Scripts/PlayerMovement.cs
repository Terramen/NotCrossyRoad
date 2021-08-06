using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    private bool _canMoveRight = true;
    private bool _canMoveLeft = true;
    

    
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private LayerMask _blockableLayer;
    [SerializeField] private LayerMask _deadlyLayer;
    [SerializeField] private LayerMask _transportLayer;

    [Header("Jump")]
    [SerializeField] private int _jumpPoints;
    [SerializeField] private float _animationTime;
    private bool _blockMovement;

    [Header("Camera")]
    [SerializeField] private CameraFollow _camera;

    [Header("Score")]
    [SerializeField] private ScoreCounter _scoreCounter;

    public int StepsBack { get; set; }
    
    private void OnEnable()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= SwipeDetector_OnSwipe;
    }
    
    /*void Update()
    {
        if (!_blockMovement)
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
                if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                {
                    _scoreCounter.SetScore(1);
                    if (_mapBuilder.CurrentPaths[Mathf.RoundToInt( transform.position.z + 1)]
                        .GetOccupation(Mathf.RoundToInt(transform.position.x)) &&
                        ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                        .CurrentPaths[Mathf.RoundToInt(transform.position.z + 1)]
                        .GetLayer(Mathf.RoundToInt(transform.position.x))))
                    {
                        Move(0,0);
                    }
                    else
                    {
                        Move(0, 1);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                {
                    _scoreCounter.SetScore(-1);
                    if (_mapBuilder.CurrentPaths[Mathf.RoundToInt(transform.position.z - 1)]
                        .GetOccupation(Mathf.RoundToInt(transform.position.x)) &&
                        ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                        .CurrentPaths[Mathf.RoundToInt(transform.position.z - 1)]
                        .GetLayer(Mathf.RoundToInt(transform.position.x))))
                    {
                        Move(0,0);
                    }
                    else
                    {
                        Move(0, -1);
                    }
                }
                StepsBack++;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (_canMoveLeft)
                {
                    _canMoveRight = true;
                    if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                    {
                        if (transform.position.x != -Map.BORDER)
                        {
                            if (_mapBuilder.CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                .GetOccupation(Mathf.RoundToInt(transform.position.x) - 1)
                                && ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                                    .CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                    .GetLayer(Mathf.RoundToInt(transform.position.x) - 1)))
                                /*&& _mapBuilder
                                .CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                .GetLayer(Mathf.RoundToInt(transform.position.x) - 1).Equals(_blockableLayer)#1#
                            {
                                Move(0,0);
                            }
                            else
                            {
                                Move(-1, 0);
                                _camera.MoveCameraByPlayer(-1, 0);
                            }
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
                    if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                    {
                        if (transform.position.x != Map.BORDER)
                        {
                            if (_mapBuilder.CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                .GetOccupation(Mathf.RoundToInt(transform.position.x) + 1)
                                && ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                                .CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                .GetLayer(Mathf.RoundToInt(transform.position.x) + 1)))
                            {
                                Move(0,0);
                            }
                            else
                            {
                                Move(1, 0);
                                _camera.MoveCameraByPlayer(1, 0);
                            }
                        }
                        else
                        {
                            _canMoveRight = false;
                        }
                    }
                }
            }
        }
    }*/

    private void Move(float xChange, float zChange)
    {
        StartCoroutine(MoveAnimation(transform.position, new Vector3(transform.position.x + xChange,
            transform.position.y ,transform.position.z + zChange)));
        CheckNextStep();
    }

    private void CheckNextStep()
    {
        if (gameObject.transform.parent != null)
        {
            gameObject.transform.parent = null;
        }
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (ExistLayerByLayerMask(_transportLayer, other.gameObject.layer))
        {
            gameObject.transform.parent = other.transform;
            StartCoroutine(_camera.MoveCameraByDecoration(transform));
        }
        if (ExistLayerByLayerMask(_deadlyLayer, other.gameObject.layer) && !ExistLayerByLayerMask(_transportLayer, other.gameObject.layer))
        {
            SceneManager.LoadScene("SampleScene");
        }

        if (ExistLayerByLayerMask(_blockableLayer, other.gameObject.layer))
        {
            Move(0,0);
        }
    }

    private void SwipeDetector_OnSwipe(SwipeData data)
    {
        if (!_blockMovement)
        {
            switch (data.direction)
            {
                case SwipeDirection.UP:
                    if (StepsBack == 0)
                    {
                        _mapBuilder.GenerateRandomTerrain();
                    }
                    else if (StepsBack > 0)
                    {
                        StepsBack--;
                    }
                    if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                    {
                        _scoreCounter.SetScore(1);
                        if (_mapBuilder.CurrentPaths[Mathf.RoundToInt( transform.position.z + 1)]
                                .GetOccupation(Mathf.RoundToInt(transform.position.x)) &&
                            ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                                .CurrentPaths[Mathf.RoundToInt(transform.position.z + 1)]
                                .GetLayer(Mathf.RoundToInt(transform.position.x))))
                        {
                            Move(0,0);
                        }
                        else
                        {
                            Move(0, 1);
                        }
                    }
                    break;
                case SwipeDirection.DOWN:
                    if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                    {
                        _scoreCounter.SetScore(-1);
                        if (_mapBuilder.CurrentPaths[Mathf.RoundToInt(transform.position.z - 1)]
                                .GetOccupation(Mathf.RoundToInt(transform.position.x)) &&
                            ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                                .CurrentPaths[Mathf.RoundToInt(transform.position.z - 1)]
                                .GetLayer(Mathf.RoundToInt(transform.position.x))))
                        {
                            Move(0,0);
                        }
                        else
                        {
                            Move(0, -1);
                        }
                    }
                    StepsBack++;
                    break;
                case SwipeDirection.LEFT:
                    if (_canMoveLeft)
                    {
                        _canMoveRight = true;
                        if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                        {
                            if (transform.position.x != -Map.BORDER)
                            {
                                if (_mapBuilder.CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                        .GetOccupation(Mathf.RoundToInt(transform.position.x) - 1)
                                    && ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                                        .CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                        .GetLayer(Mathf.RoundToInt(transform.position.x) - 1)))
                                    /*&& _mapBuilder
                                    .CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                    .GetLayer(Mathf.RoundToInt(transform.position.x) - 1).Equals(_blockableLayer)*/
                                {
                                    Move(0,0);
                                }
                                else
                                {
                                    Move(-1, 0);
                                    _camera.MoveCameraByPlayer(-1, 0);
                                }
                            }
                            else
                            {
                                _canMoveLeft = false;
                            }
                        }
                    }
                    break;
                case SwipeDirection.RIGHT:
                    if (_canMoveRight)
                    {
                        _canMoveLeft = true;
                        if (transform.position.x <= Map.BORDER && transform.position.x >= -Map.BORDER)
                        {
                            if (transform.position.x != Map.BORDER)
                            {
                                if (_mapBuilder.CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                        .GetOccupation(Mathf.RoundToInt(transform.position.x) + 1)
                                    && ExistLayerByLayerMask(_blockableLayer, _mapBuilder
                                        .CurrentPaths[Mathf.RoundToInt(transform.position.z)]
                                        .GetLayer(Mathf.RoundToInt(transform.position.x) + 1)))
                                {
                                    Move(0,0);
                                }
                                else
                                {
                                    Move(1, 0);
                                    _camera.MoveCameraByPlayer(1, 0);
                                }
                            }
                            else
                            {
                                _canMoveRight = false;
                            }
                        }
                    }
                    break;
          
            }
        }
    }

    private void Control()
    {
        
    }

    private IEnumerator MoveAnimation(Vector3 start, Vector3 end)
    {
        _blockMovement = true;
        float time = 0f;
        //_camera.MoveCameraByPlayer(start, end);
        while (time < _animationTime)
        {
            time += Time.deltaTime * 3;
            transform.position = MathParabola.Parabola(start, end, 1f, time);
            //Vector3.Lerp()
            yield return new WaitForFixedUpdate();
        }
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        _blockMovement = false;
    }

    private bool ExistLayerByLayerMask(LayerMask layerMask, int layer)
    {
        if ((layerMask.value & 1 << layer) > 0)
        {
            return true;
        }

        return false;
    }
    
}