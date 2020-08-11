using UnityEngine;
using System.Collections.Generic;
public class Item : MonoBehaviour
{
    public Sprite sprite { get; private set; }                   //this items static art
    public Animator interactAnimator { get; private set; }     //the animation to be played when this item is interacted with
    public AudioClip interactClip;
    public bool removeAfterUse;             //when true, this item will be remove from the scene after it is interacted with
    public GameObject clueObj;
    public Clue clue { get; private set; }
    public ItemType type;

    //[HideInInspector]
    public bool clueOpen = false;

    bool inRange = false;

    float playerSpeed;

    public GameController gameController { get; private set; }

    public float interactRadius;
    //public Collider2D clickCollider;        //this collider determines the clickable area for this object

    //TODO: check above condition, should probably only be removed once the player addes it to inventory
    //interact  

    //called when object is created
    public void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        interactAnimator = gameObject.GetComponent<Animator>();

        clue = clueObj.GetComponent<Clue>();
        Debug.Log("clue object = " + clue.gameObject.name);
        clue.onClueClosed += CloseClue;


        CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
        collider.radius = interactRadius;

        if(GameController.instance != null)
        {
            gameController = GameController.instance;
        }
    }

    public void Start()
    {
        if(gameController == null)
        {
            gameController = GameController.instance;
        }

        ItemStart();
    }

    public virtual void ItemStart()
    {

    }

    //
    public virtual void Interact()
    {

        Debug.Log("interacted with " + gameObject.name);
        //stop the player motion
        ShowClue();
        if(gameController != null)
        {
            gameController.PauseCurrentPlayer();
        }
        else
        {
            gameController = GameController.instance;
            gameController.PauseCurrentPlayer();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            inRange = true;
            Debug.Log("player in range");
            InvokeRepeating("InteractCheck", 0f, .1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            Debug.Log("player out of range");
            CancelInvoke("InteractCheck");
        }
    }

    private void InteractCheck()
    {
        
        Debug.Log("type: " + type);
        if(inRange && (!clueOpen) )
        {
            Debug.Log("hitting E should interact");
            if (Input.GetKey(KeyCode.E))
            {
                //interact with the item
                Interact();
            }
        }
    }
    
    //method to play animatnion (might not be neccisary)
    public virtual void PlayAnimation()
    {

    }

    //default behavior for showing a clue (this should depend on the type of clue)
    public virtual void ShowClue()
    {
        clueOpen = true;
        //TODO: might be really clean to add some kind of transition heres
        //show clue
        clueObj.SetActive(true);
        //hide this
        gameObject.SetActive(false);
    }

    //this is the default behavior of when the clue is closed, it will depend on the type of clue
    public virtual void CloseClue()
    {
        Debug.Log("close the clue");
        clueOpen = false;

        //hide clue object
        clueObj.SetActive(false);

        //show scene again
        gameObject.SetActive(true);


        if (gameController != null)
        {
            gameController.ResumeCurrentPlayer();
        }
        else
        {
            gameController = GameController.instance;
            gameController.ResumeCurrentPlayer();
        }
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    public enum ItemType {Text, Image, Audio}
}
