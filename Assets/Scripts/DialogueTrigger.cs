using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool mustFinish = false;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        //set must finish condition
        dialogue.mustFinish = this.mustFinish;

        //start the dialogue in the dialogue manager
        DialogueManager.instance.StartDialogue(dialogue);
    }
}
