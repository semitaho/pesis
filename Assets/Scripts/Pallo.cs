using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallo : MonoBehaviour
{

    private bool _hit;

    private Rigidbody rb;

    public bool Hit
    {
        get => _hit;
        set
        {
            this._hit = value;
        }

    }

    // Start is called before the first frame update
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddForce(float force)
    {
        rb.AddForce(Vector3.up * force);
    }




    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    public Rigidbody GetRigidbody()
    {
        return this.rb;
    }




}
