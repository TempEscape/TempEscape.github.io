using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Journal", menuName = "Inventory/Item/Journal")]
public class Journal : InventoryItem
{
    #region Singleton
    public static Journal instance;

    private void OnEnable()
    {
        pageTracker = 0;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("there is already an instance of journal, but you are trying to create another");
        }
        
        
    }

    #endregion

    public class Character
    {
        public string content;
        public string name;
        public Dictionary<Dialogue, string> dialogues;
        public int characterListIndex;
        public int pages;

        public Character(string name, int index)
        {
            this.name = name;
            characterListIndex = index;
            dialogues = new Dictionary<Dialogue, string>();
            pages = 0;
        }
    }

    Dictionary<string, Character> characters;               //dictionary for quick searches
    List<Character> characterList;

    GameObject journal;
    GameObject leftPage;
    GameObject rightPage;
    GameObject prevPageButton;
    GameObject nextPageButton;
    CanvasRenderer[] journalRend;

    TextMeshProUGUI leftPageContent;
    TextMeshProUGUI rightPageContent;
    TextMeshProUGUI leftPageNum;
    TextMeshProUGUI rightPageNum;

    int pageTracker = 0;
    public int totalPages { private set; get; }
    bool open;

    public override void AddedToPlayer()
    {
        totalPages = 0;                                     //instantiate total pages
        characters = new Dictionary<string, Character>();   //instantiate character dictionary
        characterList = new List<Character>();              //instantiate characterList
        open = false;                                       //journal always starts closed

        //get references to journal and pages
        journal = GameObject.Find("Journal");
        leftPage = journal.transform.Find("LeftPage").gameObject;
        rightPage = journal.transform.Find("RightPage").gameObject;

        //get leftpage references
        leftPageContent = leftPage.transform.Find("Content").GetComponent<TextMeshProUGUI>();
        leftPageNum = leftPage.transform.Find("Num").GetComponent<TextMeshProUGUI>();

        //get right page references
        rightPageContent = rightPage.transform.Find("Content").GetComponent<TextMeshProUGUI>();
        rightPageNum = rightPage.transform.Find("Num").GetComponent<TextMeshProUGUI>();


        //get buttons
        prevPageButton = journal.transform.Find("PrevPageButton").gameObject;
        nextPageButton = journal.transform.Find("NextPageButton").gameObject;

        //hide the journal
        journalRend = journal.GetComponentsInChildren<CanvasRenderer>() as CanvasRenderer[];        //get canvas renders for objects in journal
        HideJournal();
    }

    //hide the journal
    void HideJournal()
    {
        //deactivate the buttons
        prevPageButton.SetActive(false);
        nextPageButton.SetActive(false);
        
        //make the canvas renderers completely transparent
        foreach (CanvasRenderer renderer in journalRend)
        {
            renderer.SetAlpha(0f);
            //check consequences of not changing raycast blocking
        }
    }

    //show the journal
    void ShowJournal()
    {
        //activate the buttons
        prevPageButton.SetActive(true);
        nextPageButton.SetActive(true);

        //make the canvas renderers completely opeque
        foreach (CanvasRenderer renderer in journalRend)
        {
            renderer.SetAlpha(1f);
        }
    }

    //add dialogue
    public void AddDialogue(Dialogue dialogue)
    {
        string charName = dialogue.name;                                        //get the name of character

        //check if character is already in journal, if not add it
        if (!characters.ContainsKey(charName))
        {
            Character newCharacter = new Character(charName, characters.Count);
            AddCharacter(newCharacter);
        }

        Character currCharacter = characters[charName];                         //get reference to character

        //see if this dialogue already exists, if it doesn't add it
        if (StartDialogue(currCharacter, dialogue))
        {
            //add the dialogue to the characters journal entry
            foreach (string paragraph in dialogue.sentences)
            {
                AddParagraph(paragraph, currCharacter);
            }
        }
    }

    //Attempt to start recording the dialogue and return false if this dialogue already exists
    bool StartDialogue(Character character, Dialogue dialogue)
    {
        //check if dialogue already exists in journal, if it doesn't add it
        if (!character.dialogues.ContainsKey(dialogue))
        {
            character.content += "convo_" + character.dialogues.Keys.Count;
            character.dialogues.Add(dialogue, "convo_" + character.dialogues.Keys.Count);
            return true;
        }

        return false;
    }

    void AddParagraph(string paragraph, Character character)
    {
        character.content += "\n\t" + paragraph;                                //indent new paragraph

        SetPageCount(character);                                                //update page count
    }

    public void SetPageCount(Character character)
    {
        //get number of pages before new pages value is calculated
        int prevPages = character.pages;

        //count the number of pages
        leftPageContent.text = character.content;
        int currPages = leftPageContent.GetTextInfo(leftPageContent.text).pageCount;
        character.pages = currPages;

        //calculate and set total pages
        totalPages += (currPages - prevPages);
    }

    //add a new character to the journal
    void AddCharacter(Character character)
    {
        characterList.Add(character);
        characters.Add(character.name, character);
        character.content += "<b>" + character.name + "</b>\n";     //add the name to content in bold
    }

    //open the journal
    public void Open()
    {

        if (totalPages == 0)
        {
            //journal is empty, so show the empty journal and return
            ShowJournal();
            return;
        }

        Character currCharacter =  null;

        //always look for the left page
        if (pageTracker%2 != 0)
        {
            //page is on the right
            pageTracker -= 1;
        }

        int pageNum = 0;
        int listIndex = 0;
        //given the page number determine the page content
        while(pageNum <= pageTracker)
        {
            currCharacter = characterList[listIndex];                           //loop through the characters
            pageNum += currCharacter.pages;                                     //count the pages that the current character occupies
            listIndex++;
        }

        //if character was correctly determined, display the pages
        if (currCharacter != null)
        {
            
            leftPageContent.text = currCharacter.content;                       //set the content for the left page

            int pgNum_L =pageTracker -  (pageNum - currCharacter.pages) + 1;    //get the left page number in the TMP element (add one to acount for the fact that TMP pages are not indexed)
            int pgNum_R = pgNum_L + 1;                                          //the right page is always going to be the next page
            Debug.Log("L: " + pgNum_L + ", R: " + pgNum_R);
            leftPageContent.pageToDisplay = pgNum_L;                            //display the correct page of text (from TMP) for the left journal page

            leftPageNum.text = pageTracker.ToString();                          //set the page numbers (actual numbers displayed in UI)
            rightPageNum.text = (pageTracker + 1).ToString();

            if(currCharacter.pages - pgNum_L > 0)                               //if there is another page display it
            {
                Debug.Log("display the next page");
                rightPageContent.text = currCharacter.content;
                rightPageContent.pageToDisplay = pgNum_R;
            } else if (listIndex < characterList.Count)                         //if there is not another page for this character, but there is another character
            {
                Debug.Log("display the next character");
                rightPageContent.text = characterList[listIndex].content;
                rightPageContent.pageToDisplay = 1;
            }
            else                                                                //there is not another page, and this is the last character
            {
                Debug.Log("display nothing");
                rightPageContent.text = "";                                     //the right page should be empty
            }


            ShowJournal();                                                      //show the journal
        }
    }

    //what to do when the journal is used
    public override void Use()
    {
        base.Use();
        if (open)                                                               //if the journal is hidden, unhide it
        {
            HideJournal();
        }
        else
        {
            Open();
        }
        open = !open;                                                           //update whether journal is open or not
    }

    //go to the next page
    public void NextPage()
    {
        pageTracker += 2;                                                       //go to the next set of pages

        //check if last page has been reached
        if(pageTracker >= totalPages)
        {
            pageTracker = totalPages - 1;
            Debug.Log("You have reached the last page");
        }

        Open();                                                                 //open the journal
    }

    //go to the previous page
    public void PrevPage()
    {
        pageTracker -= 2;                                                       //go to the previous set of pages

        //check if the first page has been reached
        if (pageTracker < 0)
        {
            pageTracker = 0;
            Debug.Log("This is the first page");
        }

        Open();                                                                 //open the journal
    }
    
}
