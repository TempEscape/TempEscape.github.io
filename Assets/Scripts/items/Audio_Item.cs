using UnityEngine;
using System.Collections;

public class Audio_Item : Item
{
    public float openTweenTime = .4f;
    public float closeTweenTime = .3f;

    private AudioSource _audioSource;

    private bool _audioIsPlaying;

    public override void ItemStart()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        clue.SetImage();
    }

    public override void Interact()
    {

        //base.Interact();

        
        
        if (!_audioIsPlaying && !clueOpen)
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
        if (!clueOpen)
        {
            clueOpen = true;

            gameController.PauseCurrentPlayer();

            //base.ShowClue();

            //hide gameobject
            //gameObject.SetActive(false);

            //if there is an interact animation play it
            if (interactAnimator != null)
            {
                PlayAnimation();
            }
            else if (clue.image != null)
            {
                //set scale to something small
                clueObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                //unhide
                clueObj.SetActive(true);

                //tween sprite from small to full size
                LeanTween.scale(clueObj, new Vector3(1f, 1f, 1f), openTweenTime).setEase(LeanTweenType.animationCurve);

                PlayAudio();
            }
            else
            {
                PlayAudio();
            }
        }
    }

    public override void CloseClue()
    {
        //if the clue is not already closed, close it (this check is neccisary becuase it can be closed from multiple places
        if (clueOpen)
        {
            
            //base.CloseClue();
            if (interactAnimator != null)
            {
                PlayAnimation();
                CloseComplete();
            }
            else if (clue.image != null)
            {
                LeanTween.scale(clueObj, new Vector3(.01f, .01f, .01f), closeTweenTime).setEase(LeanTweenType.animationCurve).setOnComplete(LTComplete);
            }
            else
            {
                StopAudio();
                CloseComplete();
            }
        }
    }

    void CloseComplete()
    {
        gameController.ResumeCurrentPlayer();
        clueOpen = false;
    }

    public override void PlayAnimation()
    {
        //Play Animation

        StartCoroutine("AnimCoroutine");
        Debug.Log("Play animatiom");
    }

    IEnumerator AnimCoroutine(float time)
    {
        interactAnimator.SetTrigger("play");
        PlayAudio();
        yield return new WaitForSeconds(time);
    }

    public void LTComplete() {
        //stop audio
        StopAudio();

        //hide the clue
        clueObj.SetActive(false);

        //close is now complete
        CloseComplete();
    }

    public void PlayAudio()
    {
        //play the audio sorce
        StartCoroutine("AudioCoroutine", _audioSource.clip.length);
        _audioSource.Play();

        _audioIsPlaying = true;
    }

    IEnumerator AudioCoroutine(float time)
    {
        _audioSource.Play();
        yield return new WaitForSeconds(time);
        _audioIsPlaying = false;
    }

    private void StopAudio()
    {
        _audioSource.Stop();

        _audioIsPlaying = false;
    }
}
