using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public delegate void StopMovement();
    public StopMovement stopMovement;

    public delegate void StartMovement();
    public StartMovement startMovement;

    public float movementSpeed;
    private float ogMovementSpeed;

    private Rigidbody2D rb;
    private bool moveRight = false;
    private bool moveLeft = false;
    private bool moveUp = false;
    private bool moveDown = false;

    // Start is called before the first frame update
    void Start()
    {
        //get reference to the rigidbody
        rb = gameObject.GetComponent<Rigidbody2D>();
        stopMovement += PausePlayer;
        startMovement += MovePlayer;

        ogMovementSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        

        //Input Checks
        if (Input.GetKey(KeyCode.D))
        {
            moveRight = true;            
        }
        else
        {
            moveRight = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveLeft = true;
        }
        else
        {
            moveLeft = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveUp = true;
        }
        else
        {
            moveUp = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDown = true;
        }
        else
        {
            moveDown = false;
        }
    }

    private void FixedUpdate()
    {
        //reset the movement vector at the begining of every check so player stops when there is no input
        Vector3 movementVect = new Vector3();

        //Movement Checks
        if (moveRight)
        {
            movementVect += Vector3.right;
        }
        if (moveLeft)
        {
            movementVect += Vector3.left;
        }
        if (moveUp)
        {
            movementVect += Vector3.up;
        }
        if (moveDown)
        {
            movementVect += Vector3.down;
        }

        //set velocity to movement velocity scaled by movement speed
        rb.velocity = movementVect * movementSpeed;

    }

    private void PausePlayer()
    {
        movementSpeed = 0;
    }

    private void MovePlayer()
    {
        movementSpeed = ogMovementSpeed;
    }
}
