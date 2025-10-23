using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class FinalCutSceneController : MonoBehaviour
{
    public static FinalCutSceneController Instance { get; private set; }

    [SerializeField] private PlayableDirector cutsceneDirector;

    private bool cutscenePlayed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCutscene()
    {
        if (cutscenePlayed)
        {
            Debug.LogWarning("[FinalCutSceneController] Cutscene already played!");
            return;
        }

        cutscenePlayed = true;

        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            StartCoroutine(WaitForCutsceneEnd());
        }
        else
        {
            Debug.LogWarning("[FinalCutSceneController] No PlayableDirector assigned!");
            EndCutscene();
        }
    }

    private IEnumerator WaitForCutsceneEnd()
    {
        while (cutsceneDirector.state == PlayState.Playing)
        {
            yield return null;
        }

        EndCutscene();
    }

    private void EndCutscene()
    {
        ResetGameProgress();
        SceneManager.LoadScene("Menu");
    }

    private void ResetGameProgress()
    {
        string saveLocation = System.IO.Path.Combine(Application.persistentDataPath, "saveData.json");
        if (System.IO.File.Exists(saveLocation))
        {
            System.IO.File.Delete(saveLocation);
            Debug.Log("[FinalCutSceneController] Save file deleted. Game progress reset.");
        }
    }
}