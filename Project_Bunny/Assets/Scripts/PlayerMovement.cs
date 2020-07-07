using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandInput;

public class PlayerMovement : MonoBehaviour
{
    //Velocidad de movimiento del personaje
    public float speedMove = 5f;

    //Referencia al cuerpo rigido del personaje
    Rigidbody2D rbd2D;

    //Direccion de movimiento en base a las teclas presionadas
    int directionMove = 0;

    //Referencia a la camara de juego, hay que arrastrala desde el inspector
    public Camera mainCamera;

    //La posicion a donde se dirigirá el jugador una vez que se haya creado la brecha
    [HideInInspector]
    public Vector3 gapPosition;

    //Fuerza que se aplicará para desplazarse a la brecha
    public float forceOnTime = 0f;

    //Bandera para indicar cuando se puede incrementar la fuerza que se aplicará
    bool hitIncrement;

    //Fuerza que se suma cada tiempo que se mantiene presionado el mouse
    public float forceAdded = 3;

    //La fuerza que se le aplicará al jugador para moverse hacia la brecha
    public float forceAppliedToMove;

    [Header("State of the Inputs")]
    public CommandInputs.StateInput curState;

    // Start is called before the first frame update
    void Start()
    {
        //Se obtiene el componente RigidBody2D
        rbd2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.GoRight, curState))
        {
            directionMove = 1;
        }

        if (CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.GoLeft, curState))
        {
            directionMove = -1;
        }

        if (CommandInputs.GetKeyButtonUp(CommandInputs.TheKeysButtons.GoRight, curState) || 
            CommandInputs.GetKeyButtonUp(CommandInputs.TheKeysButtons.GoLeft, curState))
        {
            directionMove = 0;
        }

        //Interacción con objetos

        if(CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.InteractionObjs, curState))
        {
            //Esto para dar click en objetos que tengan un collider en 2D,
            RaycastHit2D hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (hit.collider != null)
            {
                Debug.Log("Interactuo con el objeto: "+hit.collider.name);
            }
        }

        ///Creación de brecha

        if (CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.CreateGap, curState))
        {
            //Debug.Log(mainCamera.ScreenPointToRay(Input.mousePosition));
            gapPosition = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
            hitIncrement = true;  
        }
        if (CommandInputs.GetKeyButton(CommandInputs.TheKeysButtons.CreateGap, curState) && hitIncrement == true)
        {
            forceOnTime += Time.deltaTime * forceAdded;

            if (forceOnTime > 10f)
            {
                forceOnTime = 10f;
                //Temporal, ya que se tiene que añadir el como se hace la transición, esto lo teletrasnporta de forma inmediata
                transform.position = gapPosition;
                
                hitIncrement = false;
            }
        }
        else if (CommandInputs.GetKeyButtonUp(CommandInputs.TheKeysButtons.CreateGap, curState))
        {
            forceAppliedToMove = forceOnTime;
            hitIncrement = false;
            
            if(forceOnTime < 10)
            {
                //Temporal, ya que se tiene que añadir el como se hace la transición, esto lo teletrasnporta de forma inmediata
                transform.position = gapPosition;
            }
            
            forceOnTime = 0f;
        }
    }

    private void FixedUpdate()
    {
        //Aplica la velocidad de desplazamiento dependiendo de la dirección
        rbd2D.velocity = new Vector2(directionMove * speedMove, rbd2D.velocity.y);
    }
}
