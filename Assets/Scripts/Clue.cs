using UnityEngine;
using UnityEngine.UI;
public class Clue : MonoBehaviour
{
    public delegate void OnClueClosed();
    public OnClueClosed onClueClosed;

    public Image image { get; private set; }

    public void SetImage()
    {
        image = gameObject.GetComponentInChildren<Image>();
    }

    public void Update()
    {
        //TODO: replace this with an x button or something so it can be closed in a better way
        if (Input.GetKeyDown(KeyCode.X))
        {
            Close();
        }
    }

    public void Close()
    {
        onClueClosed.Invoke();
    }
}
