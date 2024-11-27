using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f; // Velocidad de movimiento con WASD.

    [Header("Rotaci�n con Mouse")]
    public float mouseSensitivity = 100f; // Sensibilidad del mouse.
    public float minY = -90f; // L�mite inferior de rotaci�n vertical.
    public float maxY = 90f;  // L�mite superior de rotaci�n vertical.

    private float rotationY = 0f; // Rotaci�n vertical acumulada.
    private float rotationX = 0f; // Rotaci�n horizontal acumulada.

    void Update()
    {
        // Movimiento con WASD
        float horizontal = Input.GetAxis("Horizontal"); // A/D o Izquierda/Derecha
        float vertical = Input.GetAxis("Vertical");     // W/S o Adelante/Atr�s

        Vector3 direction = (transform.right * horizontal + transform.forward * vertical).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotaci�n con Mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX += mouseX;
        rotationY -= mouseY; // Invertido para rotaci�n natural
        rotationY = Mathf.Clamp(rotationY, minY, maxY); // Restringir rotaci�n vertical

        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0f);
    }
}
