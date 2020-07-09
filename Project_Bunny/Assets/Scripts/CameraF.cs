using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraF : MonoBehaviour {

    public Transform target;                    //Objetivo al cual la camara va a seguir
    public float smoothTime = 0.3F;             //Tiempo de suavizado
    private Vector3 velocity = Vector3.zero;    //Velocidad de desplazamiento

    public Vector3 offset;                      //Compensa o agrega unidades a la posicion deseada

    public float yPosRestriction = -3;          //posicion de restriccion de camara cuando cae el jugador al vacio

    void Update()
    {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 1, -10));
        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref velocity, smoothTime);

        //newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, yPosRestriction, Mathf.Infinity), newPos.z);     //Clamp de camara al caer el jugador de una plataforma
    }

}
