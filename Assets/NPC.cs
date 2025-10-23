using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private bool playCutsceneOnInteract = false; 
    
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private enum QuestState { NotStarted, InProgress, Completed }
    private QuestState questState = QuestState.NotStarted;

    [SerializeField] private bool completeQuestOnInteract;

    private void Start()
    {
        dialogueUI = DialogueController.Instance; 
    }
    
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (playCutsceneOnInteract)
        {
            PlayFinalCutscene();
            return;
        }

        if (dialogueData == null)
        {
            return;
        }

        if (completeQuestOnInteract && dialogueData.quest != null)
        {
            FinalScene(dialogueData.quest);
            return;
        }

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void PlayFinalCutscene()
    {
        Debug.Log("[NPC] Playing final cutscene...");

        // Optional: Save or clear progress before loading cutscene
        if (SaveController.Instance != null)
        {
            SaveController.Instance.SaveGame();
        }

        // Load your cutscene scene
        SceneManager.LoadScene("Final");
    }

    void StartDialogue()
    {
        SyncQuestState();

        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.InProgress)
        {
            dialogueIndex = dialogueData.questInProgressIndex;
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
        }

        isDialogueActive = true;

        dialogueUI.SetNPCInfo(dialogueData.npcName);
        dialogueUI.ShowDialogueUI(true);

        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (dialogueData.quest == null) return;

        string questID = dialogueData.quest.questID;

        if (QuestController.Instance.IsQuestCompleted(questID) || QuestController.Instance.IsQuestHandedIn(questID))
        {
            questState = QuestState.Completed;
        }
        else if (QuestController.Instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStarted;
        }
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        dialogueUI.ClearChoices();

        if (dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }
        
        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            bool givesQuest = choice.givesQuest[i];
            dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, givesQuest));
        }
    }

    void ChooseOption(int nextIndex, bool givesQuest)
    {
        if (givesQuest)
        {
            QuestController.Instance.AcceptQuest(dialogueData.quest);
            questState = QuestState.InProgress;
            RewardsController.Instance.GiveItemReward(1, 1);    //drops 1st flower
        }
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    public void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        if (questState == QuestState.Completed && !QuestController.Instance.IsQuestHandedIn(dialogueData.quest.questID))
        {
            HandleQuestCompletion(dialogueData.quest);
        }

        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);

    }

    public void HandleQuestCompletion(Quest quest)
    {
        //change sprite to no flower
        RewardsController.Instance.GiveQuestReward(quest);
        QuestController.Instance.HandInQuest(quest.questID);
        SceneManager.LoadScene("ChurchScene");
        SaveController.Instance.SaveGame();
    }
    
    public void FinalScene(Quest quest)
    {
        RewardsController.Instance.GiveQuestReward(quest);
        QuestController.Instance.HandInQuest(quest.questID);
    }
}




