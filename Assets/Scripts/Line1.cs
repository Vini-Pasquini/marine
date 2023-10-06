using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line1 : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float lineLength = 5f; // Comprimento da linha

    void Start()
    {
        // Obtenha o componente Line Renderer
        lineRenderer = GetComponent<LineRenderer>();

        // Certifique-se de que o Line Renderer tenha pelo menos 2 posições definidas
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Defina a primeira posição da linha como a posição do objeto
        lineRenderer.SetPosition(0, transform.position);

        // Calcule a segunda posição da linha na frente do objeto
        Vector3 lineEnd = transform.position + transform.forward * lineLength;

        // Defina a segunda posição da linha
        lineRenderer.SetPosition(1, lineEnd);
    }
}
