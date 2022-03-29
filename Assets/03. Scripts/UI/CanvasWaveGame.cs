using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasWaveGame : MonoBehaviour
{
    TextMeshProUGUI timeText;
    TextMeshProUGUI pointText;
    TextMeshProUGUI gameoverText;
    TextMeshProUGUI startText;
    TextMeshProUGUI resultText;

    Transform gameResult;
    Button exitButton;

    float startTime;
    float elapsedTime;
    int totalPoint;
    bool bStart = false;
    bool bEnd = false;

    void Start()
    {
        timeText = transform.Find("TimeText").GetComponent<TextMeshProUGUI>();
        pointText = transform.Find("PointText").GetComponent<TextMeshProUGUI>();
        startText = transform.Find("StartText").GetComponent<TextMeshProUGUI>();
        gameResult = transform.Find("GameResult");
        gameoverText = gameResult.Find("GameOverText").GetComponent<TextMeshProUGUI>();
        resultText = gameResult.Find("ResultText").GetComponent<TextMeshProUGUI>();
        exitButton = gameResult.Find("ExitButton").GetComponent<Button>();

        exitButton.onClick.AddListener(() => SceneLoadManager.Instance.LoadScene(SceneLoadManager.Instance.TitleScene));
        gameResult.gameObject.SetActive(false);

        GameEventManager.instance.EventPointUp += PointUp;
        GameEventManager.instance.EventWaveStart += WaveStartEnd;

        timeText.text = "00 : 00";

        StartCoroutine(WaitStart());
    }

    private void OnDestroy()
    {
        GameEventManager.instance.EventPointUp -= PointUp;
        GameEventManager.instance.EventWaveStart -= WaveStartEnd;
    }

    private void Update()
    {
        if (bEnd || !bStart) return;

        elapsedTime = Time.time - startTime;

        string minutes = string.Format("{0:D2}", ((int)elapsedTime / 60));
        string seconds = string.Format("{0:D2}", ((int)elapsedTime % 60));

        timeText.text = string.Format("{0} : {1}", minutes, seconds);
    }

    void PointUp(int point)
    {
        pointText.text = point.ToString();
        totalPoint = point;
    }

    void WaveStartEnd(bool isStart)
    {
        if (isStart)
        {
            startTime = Time.time;
            bStart = true;
        }
        else
        {
            gameResult.gameObject.SetActive(true);

            if (totalPoint >= IngameManager.Instance.WaveGameClearPoint)
            {
                gameoverText.text = "Game Clear!";
            }
            resultText.text = "Result : " + totalPoint.ToString();

            bEnd = true;
            bStart = false;
        }
    }

    IEnumerator WaitStart()
    {
        WaitForSeconds ws = new WaitForSeconds(1f);
        int n = 3;
        while (n > 0)
        {
            startText.text = n.ToString();
            n--;
            yield return ws;
        }

        startText.text = "Start!";
        yield return ws;
        startText.gameObject.SetActive(false);
    }
}
