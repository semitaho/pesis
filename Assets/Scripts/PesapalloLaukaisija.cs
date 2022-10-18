using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesapalloLaukaisija : MonoBehaviour
{

    [SerializeField] private GameObject pesapalloPrefab;

    [SerializeField]  float minKorkeusPower = 15000f;

    [SerializeField]float maxKorkeusPower = 20000f;




    private float startTime = 0.5f;

    [SerializeField]  float repeatInterval = 3f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBall", startTime, repeatInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnBall()
    {
        float force = Random.Range(minKorkeusPower, maxKorkeusPower);
        var gameObject = Instantiate(pesapalloPrefab, transform.position, Quaternion.identity);
        var pallo  =gameObject.GetComponent<Pallo>();
    
        pallo.AddForce(force);
        pallo.transform.parent = transform;
        Destroy(gameObject, 8f);
    }
}
