using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f; // Velocidad de movimiento con WASD.

    [Header("Rotación con Mouse")]
    public float mouseSensitivity = 100f; // Sensibilidad del mouse.
    public float minY = -90f; // Límite inferior de rotación vertical.
    public float maxY = 90f;  // Límite superior de rotación vertical.

    private float rotationY = 0f; // Rotación vertical acumulada.
    private float rotationX = 0f; // Rotación horizontal acumulada.

    void Update()
    {
        // Movimiento con WASD
        float horizontal = Input.GetAxis("Horizontal"); // A/D o Izquierda/Derecha
        float vertical = Input.GetAxis("Vertical");     // W/S o Adelante/Atrás

        Vector3 direction = (transform.right * horizontal + transform.forward * vertical).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotación con Mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX += mouseX;
        rotationY -= mouseY; // Invertido para rotación natural
        rotationY = Mathf.Clamp(rotationY, minY, maxY); // Restringir rotación vertical

        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0f);
    }
}
