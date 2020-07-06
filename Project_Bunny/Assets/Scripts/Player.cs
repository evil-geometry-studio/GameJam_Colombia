using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    //Velocidad de Movimiento del Personaje
    public float Speed = 22;

    //Velocidad Maxima a la cual se movera el personaje
    public float MaxSpeed = 5;

    //Poder de salto del personaje
    public float JumpPower=10;
    //Rigidbody del Player
    public Rigidbody2D rb;


    //Se esta tocando el suelo?
    public bool ground;

    //Se puede saltar?
    public bool Jump;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && ground)
        {
            Jump = true;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
           // rb.transform.Translate(-0.02f,0,0);
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            //rb.AddForce(new Vector2(-Speed, 0),0);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
           // rb.transform.Translate(0.02f, 0, 0);
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
        //salto
        if (Jump)
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            Jump = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.LogError("Toy tocando piso");
            ground = true;
        }
        if(collision.tag == "Skewers")
        {
            RestarGame();
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            ground = false;
        }
    }

    public void RestarGame()
    {
        SceneManager.LoadScene(0);
    }
}
