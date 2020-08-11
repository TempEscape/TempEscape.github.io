using UnityEngine;
using System.Collections;
public class Text_Item : Item
{
    public float openTweenTime;
    public float closeTweenTime;
    public override void Interact()
    {
        //base.Interact();
        if (!clueOpen)
        {
            //Show the clue
            ShowClue();
        }
        else
        {
            CloseClue();
        }
    }

    public override void ShowClue()
    {
        //hide gameobject
        //gameObject.SetActive(false);

        gameController.PauseCurrentPlayer();

        //if there is an interact animation play it
        if (interactAnimator != null)
        {
            PlayAnimation();
        }
        else
        {
            TweenOpen();
        }

        clueOpen = true;
    }

    private void TweenOpen()
    {
        //set scale to something small
        clueObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        //unhide
        clueObj.SetActive(true);

        //tween sprite from small to full size
        LeanTween.scale(clueObj, new Vector3(1f, 1f, 1f), openTweenTime).setEase(LeanTweenType.animationCurve);
    }

    public override void CloseClue()
    {
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
        if (gameController != null)
        {
            gameController.ResumeCurrentPlayer();
        }

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

    //callback for when leantween completes
    public void LTComplete()
    {
        //hide clue
        clueObj.SetActive(false);

        //Complete Close
        CompleteClose();
    }
}
