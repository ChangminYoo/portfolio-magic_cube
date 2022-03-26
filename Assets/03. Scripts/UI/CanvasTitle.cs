using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTitle : MonoBehaviour
{
    SceneLoadManager sceneLoader;
    Button StartButton;
    Button SettingButton;

    Transform popupSetting;
    Button settingExit;

    void Start()
    {
        sceneLoader = SceneLoadManager.Instance;
        
        StartButton = transform.Find("StartButton").GetComponent<Button>();
        StartButton.onClick.AddListener(GameStart);

        SettingButton = transform.Find("SettingButton").GetComponent<Button>();
        SettingButton.onClick.AddListener(SettingOpen);

        popupSetting = transform.Find("PopupSettingMenu");
        settingExit = popupSetting.Find("ExitButton").GetComponent<Button>();
        settingExit.onClick.AddListener(SettingClose);
        popupSetting.gameObject.SetActive(false);
    }

    void GameStart()
    {
        sceneLoader.LoadScene(sceneLoader.SelectScene);
        StartButton.onClick.RemoveAllListeners();
    }

    void SettingOpen()
    {
        popupSetting.gameObject.SetActive(true);
    }

    void SettingClose()
    {
        popupSetting.gameObject.SetActive(false);
    }

}
