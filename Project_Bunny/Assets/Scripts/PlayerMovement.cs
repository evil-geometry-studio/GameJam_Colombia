using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandInput;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    public static PlayerMovement Instance { get => instance; }

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
    [HideInInspector]
    public float forceOnTime = 0f;

    //Bandera para indicar cuando se puede incrementar la fuerza que se aplicará
    bool hitIncrement;

    //Fuerza que se suma cada tiempo que se mantiene presionado el mouse
    public float forceAddedOnTime = 3;

    [HideInInspector]
    public bool canMoveToGap = false; //Saber cuando se puede mover el jugador hacia la brecha

    [Header("State of the Inputs")]
    public CommandInputs.StateInput curStateInputs;

    [Header("Gasp Spawn")]
    public Transform gapSpawnPos; //El lugar por donde saldra la brecha
    Vector2 gapDirection; //La dirección de la brecha
    public GameObject gapPrefab; //La brecha a spawnear
    bool canCreateGap = true; //Saber cuando puede crear una brecha
    GameObject newGap; //Referecnia a la brecha creada
    float distancePlayerGap = 0f; //La distancia entre el jugador y la brecha

    // Start is called before the first frame update
    void Start()
    {
        //Asigna la instancia de esta clase, para el singleton
        instance = this;

        //Se obtiene el componente RigidBody2D
        rbd2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        #region  PlayerMovement
        if (CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.GoRight, curStateInputs))
        {
            directionMove = 1;
        }
        else if (CommandInputs.GetKeyButtonUp(CommandInputs.TheKeysButtons.GoRight, curStateInputs))
        {
            directionMove = 0;
        }

        if (CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.GoLeft, curStateInputs))
        {
            directionMove = -1;
        }
        else if (CommandInputs.GetKeyButtonUp(CommandInputs.TheKeysButtons.GoLeft, curStateInputs))
        {
            directionMove = 0;
        }
        #endregion

        #region Intercation with objects

        if (CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.InteractionObjs, curStateInputs))
        {
            //Esto para dar click en objetos que tengan un collider en 2D,
            RaycastHit2D hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (hit.collider != null)
            {
                Debug.Log("Interactuo con el objeto: " + hit.collider.name);
            }
        }
        #endregion

        #region GapDirection
        //Si el jugador puede crear una brecha
        if (canCreateGap)
        {
            //Obtener la posicion del mouse, conviertiendo coordenadas de pantalla a coordenadas de mundo
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //La posicion actual del spawn de brechas
            Vector2 gapPos = gapSpawnPos.position;
            //Obtenemos la direccion hacia donde queremos apuntar en base a la posicion del mouse
            gapDirection = mousePos - gapPos;
            //Asignamos la dirección hacia donde se quiere apuntar al objeto de spawn
            gapSpawnPos.right = gapDirection;
        }
        else //if (distancePlayerGap > 1f)
        {
            Vector2 gapPos = gapSpawnPos.position;
            Vector2 curGapPos = gapPosition;
            gapDirection = curGapPos - gapPos;

            gapSpawnPos.right = gapDirection;
        }
        #endregion

        #region GapCreation
        if (CommandInputs.GetKeyButtonDown(CommandInputs.TheKeysButtons.CreateGap, curStateInputs) && canCreateGap)
        {
            //Debug.Log(mainCamera.ScreenPointToRay(Input.mousePosition));
            gapPosition = mainCamera.ScreenPointToRay(Input.mousePosition).origin; //guardamos la posición del click del mouse
            gapPosition.z = 0f; //Ponemos a cero el eje Z, de esta forma evitaremos errores al momento de sacar distancias entre vectores
            hitIncrement = true; //Podemos asumir que el jugador quizá dejara el btón presionado para incrementar la fuerza
            canCreateGap = false; //Ya no sera posible crear otra brecha, hasta que se destruya la actual
        }

        if (CommandInputs.GetKeyButton(CommandInputs.TheKeysButtons.CreateGap, curStateInputs) && hitIncrement == true && newGap == null)
        {
            forceOnTime += Time.deltaTime * forceAddedOnTime; //Añade fuerza a lo largo del tiempo, mientras se mantiene presionado el botón designado

            if (forceOnTime > 10f) //Si llega a 10, ya no puede aumentar más la fuerza
            {
                forceOnTime = 10f; //Le ponemos diez como maximo

                newGap = Instantiate(gapPrefab, gapSpawnPos.position, gapSpawnPos.rotation); //Creamos un nueva brecha, dandole una posicion y direccion en base al spawn de brechas, también, se hace una referencia a la brecha
                newGap.GetComponent<Gap>().InitGap(gapPosition, forceOnTime); //Obtenemos su componente Gap para inicializar la brecha
                hitIncrement = false; //Ya no es posible incrementar la fuerza, esto impide la ejecución del if
            }
        }
        else if (CommandInputs.GetKeyButtonUp(CommandInputs.TheKeysButtons.CreateGap, curStateInputs) && hitIncrement == true && newGap == null) //Si se ha liberado la telca
        {
            hitIncrement = false; //Ya no es posible incrementar la fuerza, puede ser que no haya llegado a 10, que sea menor

            if (forceOnTime < 10) //Solo si es menor a 10, tenemos que crear la brecha
            {
                newGap = Instantiate(gapPrefab, gapSpawnPos.position, gapSpawnPos.rotation); //Creamos un nueva brecha, dandole una posicion y direccion en base al spawn de brechas, también, se hace una referencia a la brecha
                newGap.GetComponent<Gap>().InitGap(gapPosition, forceOnTime); //Obtenemos su componente Gap para inicializar la brecha
            }

            forceOnTime = 0f; //Reseteamos la fuerza que se aplico a lo largo del tiempo en que estuvo presionado el boton para el lanzamiento
        }
        #endregion

        #region MoveToGap
        if (canMoveToGap) //Solo será posible moverse a la brecha cuando la misa haya llegado a su destino, para dar la indicación al jugador de moverse
        {
            rbd2D.gravityScale = 0f; //Cuando se esta moviendo no queremos que se aplique gravedad
            transform.position = Vector2.MoveTowards(transform.position, gapPosition, speedMove * 2 * Time.deltaTime); //El jugador se movera a la misma posición que se le dio a la brecha
            distancePlayerGap = Vector3.Distance(transform.position, gapPosition); //Obtenemos la distacnia entre el jugador y la brecha
            if (distancePlayerGap < 1f) //Si es menoer a 1
            {
                rbd2D.gravityScale = 1f; //Volvemos a activar la gravedad, 1 = -9.8f
                canMoveToGap = false; //El jugador ya no puede dirigirse hacia la brecha, pues ya llego
                forceOnTime = 0; //Se resetea la fuerza a lo largo del tiempo, solo para asegurar, ya que entre tanto input a veces no respetaba esto
                Destroy(newGap, 0.5f); //Como tenemos referencia a la brecha que se creo, podemos destruirla una vez que el jugador tiene la misma posición
                if(newGap == null) //Si esta destruida la brecha
                {
                    canCreateGap = true; //El jugador puede crear una nueva brecha
                }
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        //Aplica la velocidad de desplazamiento dependiendo de la dirección
        rbd2D.velocity = new Vector2(directionMove * speedMove, rbd2D.velocity.y);
    }
}
