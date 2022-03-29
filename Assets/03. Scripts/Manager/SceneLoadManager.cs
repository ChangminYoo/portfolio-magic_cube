using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoadManager : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField]
    Slider prograssBar;
    [SerializeField]
    TextMeshProUGUI prograssText;

    string currentScene = "Title";

    public string TitleScene => "Title";
    public string SelectScene => "SelectScene";
    public string WaveGameScene => "Game1";
    public string CubeGameScene => "Game2";


    protected static SceneLoadManager instance;
    public static SceneLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SceneLoadManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }

        private set { instance = value; }
    }

    static SceneLoadManager Create()
    {
        var sceneLoader = Resources.Load<SceneLoadManager>("SceneLoader");
        return Instantiate(sceneLoader);
    }

    void Awake()
    {
        canvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();

        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        canvasGroup.gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        StartCoroutine(Load(sceneName));
    }

    IEnumerator Load(string sceneName)
    {
        prograssBar.value = 0f;
        prograssText.text = "0%";
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);       
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if (op.progress >= 0.9f)
            {
                prograssBar.value = 1;
                prograssText.text = "100%";

                op.allowSceneActivation = true;
                currentScene = sceneName;
                yield break;
            }
            else
            {
                prograssBar.value = Mathf.Lerp(prograssBar.value, op.progress, timer);
                if (prograssBar.value >= op.progress)
                {
                    timer = 0f;
                }

                prograssText.text = string.Format("{0}%", (int)(op.progress * 100));
            }
        }

    }

    void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(Fade(false));
        SceneManager.sceneLoaded -= LoadSceneEnd;

        if (scene.name != SelectScene)
        {
            EnGameMode loadSceneGameMode = EnGameMode.None;
            if (scene.name == WaveGameScene)
            {
                loadSceneGameMode = EnGameMode.WaveDefence;
            }
            else if (scene.name == CubeGameScene)
            {
                loadSceneGameMode = EnGameMode.CubeBreak;
            }
            IngameManager.Instance.StartMode(loadSceneGameMode);
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.deltaTime * 2f;
            canvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);
        }

        if (!isFadeIn)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
