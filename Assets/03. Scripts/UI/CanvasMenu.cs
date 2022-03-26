using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasMenu : MonoBehaviour
{
    Button menuButton;
    Button exitMenuButton;
    Button settingButton;
    Button titleButton;

    Transform menuPanel;
    Slider hpSlider;
    Slider staminaSlider;

    GameEventManager gameEventManager;
    IngameManager ingameManager;

    TextMeshProUGUI hpText;
    TextMeshProUGUI staminaText;

    private void Awake()
    {
        gameEventManager = GameEventManager.Instance;
        ingameManager = IngameManager.Instance;

        menuPanel = transform.Find("MenuPanel");
        menuPanel.gameObject.SetActive(false);

        menuButton = transform.Find("MenuButton").GetComponent<Button>();
        menuButton.onClick.AddListener(MenuOpen);

        exitMenuButton = menuPanel.Find("BackgroundButton").GetComponent<Button>();
        exitMenuButton.onClick.AddListener(ExitMenu);

        Transform buttons = menuPanel.Find("Buttons");
        settingButton = buttons.Find("SettingButton").GetComponent<Button>();
        titleButton = buttons.Find("TitleButton").GetComponent<Button>();
        titleButton.onClick.AddListener(LoadTitle);

        hpSlider = transform.Find("HPSlider").GetComponent<Slider>();
        hpText = transform.Find("HPSlider/Text").GetComponent<TextMeshProUGUI>();
        hpText.text = string.Format("{0} / {1}", ingameManager.PlayerHP, ingameManager.PlayerHP);

        staminaSlider = transform.Find("StaminaSlider").GetComponent<Slider>();
        staminaText = transform.Find("StaminaSlider/Text").GetComponent<TextMeshProUGUI>();
        staminaText.text = string.Format("{0} / {1}", ingameManager.PlayerStamina, ingameManager.PlayerStamina);

        gameEventManager.EventChangeHP += OnEventChangeHp;
        gameEventManager.EventChangeStamina += OnEventChangeStamina;
    }

    private void OnDestroy()
    {
        gameEventManager.EventChangeHP -= OnEventChangeHp;
        gameEventManager.EventChangeStamina -= OnEventChangeStamina;

        menuButton.onClick.RemoveAllListeners();
        exitMenuButton.onClick.RemoveAllListeners();
        titleButton.onClick.RemoveAllListeners();
    }

    void MenuOpen()
    {
        menuPanel.gameObject.SetActive(true);
    }

    void ExitMenu()
    {
        menuPanel.gameObject.SetActive(false);
    }

    void LoadTitle()
    {
        SceneLoadManager.Instance.LoadScene(SceneLoadManager.Instance.TitleScene);
    }

    #region Event
    void OnEventChangeHp(Transform tr, float lostHp)
    {
        if (tr.GetComponent<Player>() != null)
        {
            hpSlider.value -= lostHp / ingameManager.PlayerHP;
            hpText.text = string.Format("{0} / {1}", (int)(hpSlider.value * ingameManager.PlayerHP), ingameManager.PlayerHP);
        }
    }

    void OnEventChangeStamina(float stamina)
    {
        staminaSlider.value = stamina / ingameManager.PlayerStamina;
        staminaText.text = string.Format("{0} / {1}", (int)(stamina), ingameManager.PlayerStamina);
    }
    #endregion
}
