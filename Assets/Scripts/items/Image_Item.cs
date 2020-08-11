using UnityEngine;
using System.Collections;

public class Image_Item : Item
{
    public float openTweenTime;
    public float closeTweenTime;
    public float slerpSpeed = .1f;      //1 -> immediate , 1>>slerpSpeed -> lag
    //NOTE: when slerp speed is .01 you can see some fun effects of delay

    Vector3 initialDir;                 //the initial direction to the mouse when player pressed left mouse btn
    Vector3 currentDir;                 //the current direction to the mouse when the player is pressing the mouse btn

    Quaternion lastRotation = Quaternion.identity; 

    //TODO: give the player the ability to rotate the items
    public override void Interact()
    {
        //base.Interact();
        if (!clueOpen)
        {
            ShowClue();
        }
        else
        {
            CloseClue();
        }
    }

    public override void ShowClue()
    {
        gameController.PauseCurrentPlayer();

        //base.ShowClue();
        //if there is an interact animation play it
        if (interactAnimator != null)
        {
            PlayAnimation();
        }
        else
        {
            TweenOpen();
        }
    }

    private void TweenOpen() {
        //set scale to something small
        clueObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        //unhide
        clueObj.SetActive(true);

        //tween sprite from small to full size
        LeanTween.scale(clueObj, new Vector3(1f, 1f, 1f), openTweenTime).setEase(LeanTweenType.animationCurve).setOnComplete(LT_OpenComplete);
    }

    void LT_OpenComplete()
    {
        clueOpen = true;
    }

    public void Update()
    {
        
        //rotate the clue
        if (clueOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //get initial direction to the mouse button when the player presses the left mouse button
                initialDir = (Input.mousePosition - clueObj.transform.position).normalized;
            }
            if (Input.GetMouseButton(0))
            {
                //get the direction to the mouse button while the player is holding the left mouse button 
                currentDir = (Input.mousePosition - clueObj.transform.position).normalized;
                //get the rotation between the initial mouse position and the current mouse position
                Quaternion change = Quaternion.FromToRotation(initialDir, currentDir);
                //add the initial rotation of the clue
                Quaternion temp = Quaternion.Euler(change.eulerAngles + lastRotation.eulerAngles);

                //rotate the clue
                //clueObj.transform.rotation = Quaternion.Slerp(clueObj.transform.rotation,temp, slerpSpeed);
                clueObj.transform.rotation = Quaternion.Slerp(clueObj.transform.rotation, temp, slerpSpeed);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //store the rotation of the clue when the player releases the left mouse button
                lastRotation = clueObj.transform.rotation;
            }
        }
    }


    void UpdateRotation()
    {
        //rotate the rotation of the sprite by the difference in current mouse rotaion and intitial mouse roation
        clueObj.transform.rotation = Quaternion.FromToRotation(initialDir, currentDir);
    }

    public override void CloseClue()
    {
        //base.CloseClue();

        //tween sprite from small to full size
        //TODO: replace this with specific animation if there is one for the close vs open
        if (interactAnimator != null)
        {
            PlayAnimation();
        }
        else
        {
            LeanTween.scale(clueObj, new Vector3(.01f, .01f, .01f), closeTweenTime).setEase(LeanTweenType.animationCurve).setOnComplete(LTComplete);
        }
    }

    void CompleteClose()
    {
        gameController.ResumeCurrentPlayer();


        clueOpen = false;
    }

    public override void PlayAnimation()
    {
        if (!clueOpen)
        {
            StartCoroutine("PlayAnimOpen", interactAnimator.GetNextAnimatorClipInfo(0).Length);
            if (interactClip != null)
            {
                PlaySound(interactClip);
            }
            TweenOpen();
        }
        else
        {
            LeanTween.scale(clueObj, new Vector3(.01f, .01f, .01f), closeTweenTime).setEase(LeanTweenType.animationCurve).setOnComplete(LTComplete);
            StartCoroutine("PlayAnimClose", interactAnimator.GetNextAnimatorStateInfo(0).length);
            if (interactClip != null)
            {
                PlaySound(interactClip);
            }

        }
    }

    IEnumerator PlayAnimOpen(float time)
    {
        interactAnimator.SetTrigger("play");
        yield return new WaitForSeconds(time);
        TweenOpen();
        //StopAudio();
    }

    IEnumerator PlayAnimClose(float time)
    {
        interactAnimator.SetTrigger("play");
        yield return new WaitForSeconds(time);
    }

    private void PlaySound(AudioClip clip)
    {
        //play clip at clue
        AudioSource.PlayClipAtPoint(clip, clueObj.transform.position);
    }

    void LTComplete()
    {
        //hide this clue
        clueObj.SetActive(false);

        //reset rotation of clue
        clueObj.transform.rotation = Quaternion.identity;
        lastRotation = Quaternion.identity;

        CompleteClose();

    }
}
