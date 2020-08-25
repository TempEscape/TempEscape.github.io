using UnityEngine;


public class JournalUI : MonoBehaviour
{
    public void PrevPage()
    {
        if (Journal.instance != null)
        {
            Journal.instance.PrevPage();
        }
        else
        {
            Debug.LogWarning("There is no journal instance");
        }
    }

    public void NextPage()
    {
        if(Journal.instance != null)
        {
            Journal.instance.NextPage();
        }
        else
        {
            Debug.LogWarning("There is no journal instance");
        }
    }
}
