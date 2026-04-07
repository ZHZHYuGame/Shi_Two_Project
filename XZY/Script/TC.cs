using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 1.5f, 0);
        if (transform.position.y >= 1500)
        {
            Destroy(gameObject);
        }
    }
}
