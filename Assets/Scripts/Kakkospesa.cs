using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Kakkospesa : MonoBehaviour
{

    [SerializeField] LineRenderer lineRenderer;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {

       DrawKakkospesa(new Vector3(250, 0, -58));

    }

    private void DrawKakkospesa(Vector3 point)
    {
        float steps = 400;
        lineRenderer.positionCount = 0;
        lineRenderer.SetPositions(new Vector3[0]);
        lineRenderer.positionCount = ((int)steps);
        Vector3 B = new Vector3(0, 0, 0);
        float radius = 28;
        Vector3 current = point;
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumreferenceProgress = currentStep / steps;
            float currentRadian = circumreferenceProgress * Mathf.PI;
            float zScaled = Mathf.Cos(currentRadian);
            float xScaled = Mathf.Sin(currentRadian);
            
            Vector3 currentPosition = current - new Vector3(xScaled  * radius, 0, zScaled * radius);
            lineRenderer.SetPosition(currentStep, currentPosition);
        }
    }
}
