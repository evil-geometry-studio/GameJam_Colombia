using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speedMove = 5f;

    Rigidbody2D rbd2D;

    int direction = 0;

    public Camera mainCamera;

    public float countTime = 0f;

    bool hitIncrement;

    public float fuerzaBrecha = 3;

    public float lastCountTime;

    // Start is called before the first frame update
    void Start()
    {
        rbd2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = 1;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -1;
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            direction = 0;
        }

        ///Deteccion de mouse

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse izquierdo");

            RaycastHit2D hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (hit.collider != null)
            {
                Debug.Log(hit.transform.position);
                hitIncrement = true;
            }
            else
            {
                hitIncrement = false;
            }
        }
        if (Input.GetMouseButton(0) && hitIncrement == true)
        {
            countTime += Time.deltaTime * fuerzaBrecha;

            if (countTime > 10f)
            {
                countTime = 10f;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastCountTime = countTime;
            countTime = 0f;
            hitIncrement = false;
        }
    }

    private void FixedUpdate()
    {
        rbd2D.velocity = new Vector2(direction * speedMove, rbd2D.velocity.y);
    }
}
