using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    float _ogMovementSpeed;

    Rigidbody2D _rb;
    Animator _animator;

    bool _movingRight;
    bool _movingLeft;
    bool _onStairs;

    Vector2 _stairsNormal;
    Vector2 _stairsDir;

    GameObject _lastStair;

    Dictionary<int, GameObject> stairDict = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //cache references to componenets
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        //store the original movement speed
        _ogMovementSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //check player inputs
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _movingRight = true;
        }
        else
        {
            _movingRight = false;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _movingLeft = true;
        }
        else
        {
            _movingLeft = false;
        }

        //get the x component of the players speed and velocity
        float xVel = _rb.velocity.x;
        float xSpeed = Mathf.Abs(xVel);

        //flip the character depending on the direction of their movement
        if (xVel < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xVel > 0)
        {
            transform.localScale = Vector3.one;
        }

        //set the horizontal move speed in the animator
        _animator.SetFloat("HorizontalSpeed", xSpeed);
    }

    private void FixedUpdate()
    {
        if (!_onStairs)
        {
            //set new movement vector
            Vector2 movementVector = new Vector2(0, _rb.velocity.y);

            //check inputs
            if (_movingRight)
            {
                movementVector += Vector2.right * movementSpeed;
            }
            if (_movingLeft)
            {
                movementVector += Vector2.left * movementSpeed;
            }

            //Debug.Log("movementVect = " + movementVector);

            //apply movment vector to the player
            _rb.velocity = movementVector;
        }
        else
        {
            StairMove();
        }
    }

    void StairMove()
    {
        _rb.velocity = _stairsDir * movementSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Stairs") && collider.gameObject != _lastStair)
        { 
            //disable the collider interactions by making the players collider a trigger
            GetComponent<Collider2D>().isTrigger = true;

            EdgeCollider2D edge = collider as EdgeCollider2D;

            //set bool
            _onStairs = true;

            float xScale = collider.gameObject.transform.localScale.x;
            Vector2 goPos = collider.gameObject.transform.position;

            //get the normal vector of the surface

            //First get the closest point to the player in world courdinates
            List<Vector2> points = new List<Vector2>(edge.points);
            float minDist = Mathf.Infinity;
            Vector2 temp = Vector2.zero;
            foreach (Vector2 point in points)
            {
                Vector2 truePoint = new Vector2 (point.x * xScale, point.y) + goPos;
                float dist = ((Vector2)transform.position - truePoint).magnitude;

                if (dist < minDist)
                {
                    minDist = dist;
                    temp = point;
                }
            }
            Vector2 StartPoint = temp;
            points.Remove(StartPoint);

            //endpoint
            Vector2 EndPoint = points[0];
            
            //get the direction as a normalized vector
            _stairsDir = (EndPoint - StartPoint).normalized;
            //flip if the x scale has been flipped
            _stairsDir.x *= xScale;

            Debug.DrawRay(StartPoint, _stairsDir, Color.red);

            //_stairsNormal = Vector2D.Perpendicular(EndPoint - StartPoint).normalized;

            //set the gravity scale to zero, so the player will not slide down the stairs
            _rb.gravityScale = 0;

            int id = collider.gameObject.GetInstanceID();
            if (!stairDict.ContainsKey(id))
            {
                stairDict.Add(id, collider.gameObject);
            }
            //Debug.Log(_stairsDir);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Stairs"))
        {
            //Debug.Log("left stairs");
            //remove current staircase from the dictionary
            int id = collider.gameObject.GetInstanceID();
            if (stairDict.ContainsKey(id))
            {
                _lastStair = stairDict[id];
                stairDict.Remove(id);
            }

            //see if the last removed staircase was the last staircase in the dictionary
            if (stairDict.Count == 0)
            {
                //Debug.Log("collider enabled");
                GetComponent<Collider2D>().isTrigger = false;

                //reset gravity scale
                _rb.gravityScale = 1;

                //reset bool
                _onStairs = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            //reset the stairs when the player has reached the ground
            _lastStair = null;
        }
    }

    public void PausePlayerMovement()
    {
        movementSpeed = 0;
    }

    public void ResumePlayerMovement()
    {
        
        movementSpeed = _ogMovementSpeed;
    }
    
}
