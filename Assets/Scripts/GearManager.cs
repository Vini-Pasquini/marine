using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    public float maxSpeed = 10.0f; // A velocidade máxima da alavanca
    public float minSpeed = 0.0f;  // A velocidade mínima da alavanca

    private void Update()
    {
        //Debug.Log();
    }

    // Função chamada quando o mouse é pressionado na alavanca
    void OnMouseDown()
    {
        Debug.Log("mouse foi pressionado");
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Função chamada quando o mouse é solto
    void OnMouseUp()
    {
        isDragging = false;
        Debug.Log("mouse foi solto");
    }

    // Função chamada a cada quadro enquanto o mouse estiver pressionado
    void OnMouseDrag()
    {
        if (isDragging)
        {
            Debug.Log("mouse esta sendo arrastado");
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            // Limita a posição da alavanca dentro de um intervalo
            newPosition.x = Mathf.Clamp(newPosition.x, minSpeed, maxSpeed);
            transform.position = newPosition;
        }
    }

    // Retorna o valor da velocidade da bala com base na posição da alavanca
    public float GetBulletSpeed()
    {
        // Mapeia a posição da alavanca para o intervalo entre minSpeed e maxSpeed
        return Mathf.InverseLerp(minSpeed, maxSpeed, transform.position.x);
    }
}
