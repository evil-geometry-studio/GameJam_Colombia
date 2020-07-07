using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    //La posicion a la que se debe mover la brecha, segun la indicación del player
    public Vector3 goThisPosition;
    //A que velocidad se movera, dependerá del player
    public float forceMovement = 0f;

    // Update is called once per frame
    void Update()
    {
        //Obtenemos la distancia entre la brecha y la posicion final
        float distance = Vector3.Distance(transform.position, goThisPosition);

        //Debug.Log(distance);
        if(distance < 1f) //Si es menor a 1
        {
            forceMovement = 0f; //Ya no se debe mover
            PlayerMovement.Instance.canMoveToGap = true; //Le damos la indicación al jugador, mediante el singleton, de que puede moverse hacia la brecha
        }
        else //Si no ha llegado
        {
            transform.Translate(Vector3.right * Time.deltaTime * forceMovement); //Que se siga moviendo
        }
    }

    //Metodo para inicializar la brecha, una vez que se ha creado, interesa la posicón a la que se movera y a que velocidad lo hará
    public void InitGap(Vector3 finalPos, float speed)
    {
        goThisPosition = finalPos; //Posición recibida como parametro
        forceMovement = speed;  //Velocidad recibidad como parametro
    }
}
