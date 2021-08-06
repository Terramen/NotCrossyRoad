using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPooling : MonoBehaviour
{
    private Dictionary<PathNames, Queue<GameObject>> _disabledPath;
    private Dictionary<DecorationNames, Queue<GameObject>> _disabledDecoration;
    // Start is called before the first frame update
    void Awake()
    {
        _disabledPath = new Dictionary<PathNames, Queue<GameObject>>();
        _disabledDecoration = new Dictionary<DecorationNames, Queue<GameObject>>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDisabledPath(PathNames pathName, GameObject path)
    {
        if (!_disabledPath.ContainsKey(pathName))
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(path);
            _disabledPath.Add(pathName, queue);
        }
        else
        {
            _disabledPath[pathName].Enqueue(path);
        }
    }
    
    public void AddDisabledDecoration(DecorationNames decorationName, GameObject decoration)
    {
        if (!_disabledDecoration.ContainsKey(decorationName))
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(decoration);
            _disabledDecoration.Add(decorationName, queue);
        }
        else
        {
            _disabledDecoration[decorationName].Enqueue(decoration);
        }
    }

    public GameObject EnablePath(PathNames pathName, float positionZ)
    {
        GameObject path = _disabledPath[pathName].Dequeue();
        path.SetActive(true);
        path.transform.position = new Vector3(path.transform.position.x, path.transform.position.y, positionZ);
        return path;
    }

    public GameObject EnableDecoration(DecorationNames decorationName, float positionX, float positionZ)
    {
        GameObject decoration = _disabledDecoration[decorationName].Dequeue();
        decoration.SetActive(true);
        decoration.transform.position = new Vector3(positionX, decoration.transform.position.y, positionZ);
        return decoration;
    }

    public void GetPathQueue(PathNames pathName)
    {
        //_disabledPath.
    }

    public bool CheckPathQueue(PathNames pathName)
    {
        if (_disabledPath.ContainsKey(pathName))
        {
            if (_disabledPath[pathName].Count != 0)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool CheckDecorationQueue(DecorationNames decorationName)
    {
        if (_disabledDecoration.ContainsKey(decorationName))
        {
            if (_disabledDecoration[decorationName].Count != 0)
            {
                return true;
            }
        }
        return false;
    }
}
