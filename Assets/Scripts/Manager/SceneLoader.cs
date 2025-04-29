using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private LoadingBar loadingBar;

    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    private float timer;

    private Dialog dialog;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            dialog = GetComponent<Dialog>();
        }
        else Destroy(gameObject);
    }

    public void LoadScene(int index) {
        StartCoroutine(LoadSceneHandle(index));
    }

    IEnumerator LoadSceneHandle(int index) {
        dialog.Open();
        GameManager.Instance.gameState = GameState.Pause;
        loadingBar.SetValue(0f);
        yield return StartCoroutine(FadeIn());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        while (!asyncOperation.isDone) {
            loadingBar.SetValue(asyncOperation.progress);
            yield return null;
        }

        yield return StartCoroutine(GameManager.Instance.LoadLevel(index));
        loadingBar.SetValue(1f);

        yield return StartCoroutine(FadeOut());

        dialog.Close();
        GameManager.Instance.gameState = GameState.Play;
    }

    IEnumerator FadeIn() {
        timer = 0f;
        while (timer < fadeInTime) {
            timer += Time.fixedDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeInTime);
            yield return null;
        }
    }

    IEnumerator FadeOut() {
        timer = 0f;
        while (timer < fadeOutTime) {
            timer += Time.fixedDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - timer / fadeOutTime);
            yield return null;
        }
    }

    public void SetLoadingBar(float value) {
        loadingBar.SetValue(value);
    }
}
