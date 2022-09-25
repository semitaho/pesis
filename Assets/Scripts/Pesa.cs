using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pesa : MonoBehaviour
{

    [SerializeField] LineRenderer lineRenderer;

    [SerializeField]  float startAngle;

    [SerializeField] float endAngle;

    [SerializeField] float radius = 26;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {

        DrawPesa(this.transform.localPosition);
        //DrawPesa(new Vector3(250, 0, -58));

    }

    private void DrawPesa(Vector3 start)
    {
        float steps = 400;
        float arcLength = endAngle - startAngle;
        lineRenderer.positionCount = 0;
        lineRenderer.SetPositions(new Vector3[0]);
        lineRenderer.positionCount = ((int)steps);
        float angle = startAngle;
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            angle += arcLength / steps;

            Vector3 addedPos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(currentStep, start + addedPos);
        }
    }
}
