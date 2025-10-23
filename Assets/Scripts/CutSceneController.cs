using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public PlayableDirector timeline;

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.stopped += OnCutsceneFinished;
        timeline.Play();
    }

    private void OnCutsceneFinished(PlayableDirector obj)
    {
        Debug.Log("[Cutscene] Finished playing, loading CemeteryScene...");
        SceneManager.LoadScene("CemeteryScene");
    }
}
