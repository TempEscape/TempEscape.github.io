using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: this can be structured much better.
public class NpcInteract : MonoBehaviour
{
    public float radius;

    private bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    public void Update()
    {
        if(canInteract && Input.GetKeyDown(KeyCode.E))
        {
            
            gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }


}
