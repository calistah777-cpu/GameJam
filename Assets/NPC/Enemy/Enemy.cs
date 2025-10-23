using UnityEngine;

public class NPC_FinalCutscene : MonoBehaviour, IInteractable
{
    private bool hasInteracted = false;
    [SerializeField] private FinalCutSceneController cutsceneController; // assign in Inspector

    private void Start()
    {
        // Try to find a FinalCutSceneController automatically if not assigned
        if (cutsceneController == null)
        {
            cutsceneController = FindAnyObjectByType<FinalCutSceneController>();
        }
    }

    public bool CanInteract()
    {
        return !hasInteracted;
    }

    public void Interact()
    {
        if (hasInteracted) return;
        hasInteracted = true;

        if (cutsceneController != null)
        {
            Debug.Log("[NPC_FinalCutscene] Starting final cutscene...");
            cutsceneController.PlayCutscene();
        }
        else
        {
            Debug.LogWarning("[NPC_FinalCutscene] No FinalCutSceneController assigned or found!");
        }
    }
}
