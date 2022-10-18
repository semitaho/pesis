using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osumakohdat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var childLength  = transform.childCount;
        for (int i = 0; i < childLength; i++)
        {
            var childTransform = transform.GetChild(i);
            childTransform.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
