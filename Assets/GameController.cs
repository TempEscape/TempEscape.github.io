using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    #region Singleton

    public static GameController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("there is already a game controller, but you are trying to create another one.");
        }
    }

    #endregion

    public GameObject currentPlayer;
    SlowPlayerMovement slowPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if(currentPlayer == null)
        {
            SetCurrentPlayer();
        }
    }

    //Pause the current player if there is one, or try to find the current player if there is not one
    public void PauseCurrentPlayer()
    {
        if(currentPlayer != null)
        {
            slowPlayer.PausePlayerMovement();
        }
        else
        {
            SetCurrentPlayer();
        }
    }
    //Resume the current player movement if there is one, or try to find the current player if there is not one
    public void ResumeCurrentPlayer()
    {
        if (currentPlayer != null)
        {
            slowPlayer.ResumePlayerMovement();
        }
        else
        {
            SetCurrentPlayer();
        }
    }

    //Set the current player
    void SetCurrentPlayer()
    {
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        slowPlayer = currentPlayer.GetComponent<SlowPlayerMovement>();
    }

    
}
