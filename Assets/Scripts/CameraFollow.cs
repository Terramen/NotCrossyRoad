using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour
{
    private bool _gameStarted;
    [SerializeField] private Transform _followingObject;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;
    private Vector3 velocity = Vector3.down;
    [SerializeField] private Camera _camera;
    private Vector3 _movePosition;
    [SerializeField] private float _deadPointZ;
    [SerializeField] private float _forwardTeleportPointZ;
    private int pathRemovePointZ;
    [SerializeField] private MapBuilder _mapBuilder;
    
    private void Awake()
    {
        _movePosition = _offset;
        _camera = GetComponent<Camera>();
    }
    
    private void LateUpdate()
    {
        
        //_movePosition += new Vector3(0, 0, 0.0025f);
        _movePosition += new Vector3(0, 0, 0.02f);
        transform.position = Vector3.SmoothDamp(transform.position, _movePosition, ref velocity, _speed);
        
        //Debug.Log($"TR: {transform.position.z - _offset.z}");

        if (transform.position.z - _offset.z  > pathRemovePointZ + _deadPointZ + 4)
        {
            _mapBuilder.RemovePath(pathRemovePointZ++);
        }

        if (transform.position.z - _followingObject.position.z - _offset.z > _deadPointZ)
        {
            //Dead();
            SceneManager.LoadScene("SampleScene");
        }

        if (_followingObject.position.z - transform.position.z > _forwardTeleportPointZ)
        {
            _movePosition = new Vector3(_movePosition.x, _movePosition.y, _followingObject.position.z + _offset.z);
        }
    }

    public void MoveCameraByPlayer(float xChange, float zChange)
    {
        _movePosition = new Vector3(transform.position.x + xChange, transform.position.y, transform.position.z + zChange);
    }

    public IEnumerator MoveCameraByDecoration(Transform tr)
    {
        while (tr.parent != null)
        {
            _movePosition = new Vector3(tr.position.x + _offset.x, _movePosition.y, _movePosition.z);
            yield return null;
        }
    }
    /*public void MoveCameraByDecoration(Transform tr)
    {
        _movePosition = new Vector3(tr.position.x, tr.position.y, tr.position.z) + _offset;
    }*/
    
}
