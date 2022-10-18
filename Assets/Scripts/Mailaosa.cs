using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailaosa : MonoBehaviour
{

    [SerializeField] private float voimakkuusZ;
    [SerializeField] private float voimakkuusY;
    [SerializeField] private float voimakkuusX;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        var pallo = other.GetComponent<Pallo>();
        if (pallo.Hit) return;
        Debug.Log("nyt osui: "+ gameObject.transform.rotation);
    
        var rigidB = pallo.GetRigidbody();
        rigidB.AddForce(new Vector3(gameObject.transform.rotation.x * 100,1 * voimakkuusY,1 * voimakkuusZ), ForceMode.VelocityChange);
        pallo.Hit = true;

    }
}
