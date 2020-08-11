using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Canvas canvas;
    public Camera cam;

    public Text nameText;
    public Text dialogueText;
    public Text testText;
    public GameObject dialogueBox;
    public float downY;
    public GameObject player;

    public static DialogueManager instance;

    [SerializeField]
    private Vector3 originalPos;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("there is already a dialogue manager, but you are trying to create another one!");
        }


        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        sentences = new Queue<string>();

        //record original position and hide the dialogue box;
        originalPos = dialogueBox.GetComponent<RectTransform>().transform.position;
        //Vector3 screenOrigPos = canvas.worldCamera.WorldToViewportPoint(originalPos);

        //testText.text = screenOrigPos.ToString();

        dialogueBox.gameObject.SetActive(false);    //hide the box on awake
        EndDialogue();  //move to the bottom of the screen
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        ShowDialogue();

        if (player != null)
        {
            Debug.Log("there is a player");
            if (dialogue.mustFinish)
            {
                Debug.Log("player movement paused");
                //player.GetComponent<PlayerMovement>().stopMovement.Invoke();    //stop the player from moving
                GameController.instance.PauseCurrentPlayer();
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (dialogue.mustFinish)
            {
                //player.GetComponent<PlayerMovement>().stopMovement.Invoke();
                GameController.instance.PauseCurrentPlayer();
            }
        }

        nameText.text = dialogue.name;

        Debug.Log("sentences.size = " + dialogue.sentences.Length);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void ShowDialogue()
    {
        dialogueBox.SetActive(true);                                                //show the dialogue box

        //animate dialogue box oout of the screen
        LeanTween.move(dialogueBox, originalPos, 2f).setEase(LeanTweenType.easeInOutQuad);

        ////move the player again (NOTE: this doesn't do anything if the player was already moving)
        //player.GetComponent<PlayerMovement>().startMovement.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        //string testString = "";
        ////test with canvas transform instead
        //Vector3 botScreen = canvas.worldCamera.ScreenToWorldPoint(Vector3.zero);
        //Debug.Log("botScreen.y = " + botScreen.y);
        ////float yMin = canvas.transform.position.y - Screen.height;
        //float yMin = canvas.GetComponent<RectTransform>().rect.yMin;
        //Vector3 targetPos = new Vector3(0, yMin, 0);

        ////Convert from screen to world points
        //targetPos = cam.ScreenToWorldPoint(targetPos);

        ////set x to the same as the current x (we only want the dialogue box to move up and down)
        //targetPos.x = dialogueBox.transform.position.x;
        //botScreen.x = dialogueBox.transform.position.x;

        //testString += "yMin: " + yMin + "\ntargetPos: " + targetPos.ToString();

        //testText.text = targetPos.ToString();

        Vector3 targetPos = dialogueBox.GetComponent<RectTransform>().transform.position;   //get the position of the dialog box (pivot at the top of the box)
        targetPos.y = 0;                                                                    //y of zero is the bottom of the screen/canvas


        //animate dialogue box oout of the screen
        LeanTween.move(dialogueBox, targetPos, 2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(Hide);

        //dialogueBox.gameObject.SetActive(false);                                            //hide the dialogue box

        //move the player again (NOTE: this doesn't do anything if the player was already moving)
        //if (player.GetComponent<PlayerMovement>().startMovement != null)
        //{
        //    //player.GetComponent<PlayerMovement>().startMovement.Invoke();
        //    GameController.instance.ResumeCurrentPlayer();
        //}
        GameController.instance.ResumeCurrentPlayer();
    }

    //this is called as the onComplete function for leantween move when ending dialogue
    private void Hide()
    {
        dialogueBox.SetActive(false); //hide the box
    }
}
