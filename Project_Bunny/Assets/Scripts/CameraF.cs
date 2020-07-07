using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraF : MonoBehaviour {

    public Transform target;                    //Objetivo al cual la camara va a seguir
    public float smoothTime = 0.3F;             //Tiempo de suavizado
    private Vector3 velocity = Vector3.zero;    //Velocidad de desplazamiento

    public Vector3 offset;                      //Compensa o agrega unidades a la posicion deseada

    void Update()
    {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 1, -10));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref velocity, smoothTime);
    }

}
