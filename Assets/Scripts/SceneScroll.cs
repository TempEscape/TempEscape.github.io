using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SceneScroll : MonoBehaviour
{
    public bool smoothScroll;           //whether or not arrows jump from one cam to another, or cam moves smoothely
    public GameObject followPoint;      //the point I am using to move the cam for now
    public int moveSpeed;
    private Rigidbody2D rb;

    public CinemachineVirtualCamera[] cameras;
    int currentCam =  0;

    private bool moveRight = false;
    private bool moveLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = followPoint.GetComponent<Rigidbody2D>();
    }


    public void SwapCamRight()
    {
        //disable current cam
        cameras[currentCam].enabled = false;

        //itterate to next cam
        currentCam = (currentCam + 1) % cameras.Length;
        Debug.Log("current cam: " + currentCam);

        //enable new cam
        cameras[currentCam].enabled = true;
    }

    public void SwapCamLeft()
    {
        //disable current cam
        cameras[currentCam].enabled = false;

        //itterate to next cam
        currentCam--;
        if(currentCam < 0)
        {
            currentCam = cameras.Length - 1;
        }

        Debug.Log("current cam: " + currentCam);

        //enable new cam
        cameras[currentCam].enabled = true;
    }


    //This is only needed for having the cammera smooth scroll in side to side 2D game
    // Update is called once per frame
    //void Update()
    //{
        //this is only for point and click game
        //if (smoothScroll)
        //{
        //    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //    {
        //        moveRight = true;
        //    }
        //    else
        //    {
        //        moveRight = false;
        //    }
        //    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        moveLeft = true;
        //    }
        //    else
        //    {
        //        moveLeft = false;
        //    }

        //}
        //else
        //{
        //    if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        //    {
        //        SwapCamRight();
        //    }else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        //    {
        //        SwapCamLeft();
        //    }
        //}
    //}

    //private void FixedUpdate()
    //{
        //uncomment this and above for the cammera smoooth move to be used again
        //Vector3 velocity = Vector3.zero;

        //if (moveRight)
        //{
        //    velocity += (Vector3.right * moveSpeed);
        //}
        //if (moveLeft)
        //{
        //    velocity += (Vector3.left * moveSpeed);
        //}

        //rb.velocity = velocity;
    //}
}
