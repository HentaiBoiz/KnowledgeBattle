using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //SingleTon
    public static DialogueManager Instance;
    


    [Header("Dialogue UI")]
    public GameObject dialogueBoxGUI;

    [Header("Choices UI")]
    [SerializeField]
    private Button buttonPrefab = null;
    [SerializeField]
    private TextMeshProUGUI dialogueText = null;
    public GameObject playerChoices;

    public Story currentStory;

    public KeyCode DialogueInput = KeyCode.Space;

    private float letterDelay = 0.01f;

    private Coroutine displayLineCoroutine;

    public bool dialogueActive { get; private set; }
    public bool dialogueGetMission { get; private set; }


    private void Awake()
    {
        Instance = this;
        RemoveChildren();
    }

    void Start()
    {
        dialogueActive = false;
        dialogueBoxGUI.SetActive(false);
    }

    void Update()
    {
        //return right away if dialogue isn't playing 
        if (!dialogueActive)
        {
            return;
        }

        //handle continuing to the next line in the dialogue when submit pressed
        if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown(DialogueInput))
        {
            ContinueStory();
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueActive = true;
        ViewManager.Instance.Show<DialogueView>();

        ContinueStory();
    }

    //Countine talk to NPC
    public void ContinueStory()
    {
        RemoveChildren();
        if (currentStory.canContinue)
        {
            //set text for the current dialogue line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayString(currentStory.Continue()));

            if (currentStory.currentChoices.Count > 0)
            {
                for (int i = 0; i < currentStory.currentChoices.Count; i++)
                {
                    Choice choice = currentStory.currentChoices[i];
                    Button button = CreateChoiceView(choice.text.Trim());

                    // Tell the button what to do when we press it
                    button.onClick.AddListener(delegate
                    {
                        OnClickChoiceButton(choice);
                    });
                }

            }
        }
        else
        {
            StartCoroutine(DropDialogue()); 
        }
    }

    private IEnumerator DropDialogue()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueActive = false;
        dialogueBoxGUI.SetActive(false);
        dialogueText.text = "";
        ViewManager.Instance.Show<PlayerStatusView>();
    }
    public void OnClickChoiceButton (Choice choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex.index);

        ContinueStory();
    } 

    Button CreateChoiceView(string text)
    {
        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(playerChoices.transform, false);

        // Gets the text from the button prefab
        TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
        choiceText.text = text;

        return choice;
    }

    public void RemoveChildren()
    {
        int childCount = playerChoices.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(playerChoices.transform.GetChild(i).gameObject);
        }
    }

    //Animation typing for dialogue chat
    private IEnumerator DisplayString(string stringToDisplay)
    {
        dialogueText.text = "";
        //display each letter one at a time
        foreach (char letter in stringToDisplay.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }

}
