using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    gameObject.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    gameObject.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                }
    }
    
    
}
