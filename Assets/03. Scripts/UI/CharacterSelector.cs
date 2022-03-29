using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    Transform characters;
    [SerializeField]
    List<Transform> characterList;

    GameObject playerCharacter;

    Transform customizeSetting;
    Transform customizeCostume;
    Transform customizeFace;

    Button leftButton;
    Button rightButton;
    Button selectButton;
    Button startButtton;
    Button backButton;

    TextMeshProUGUI nameText;
    
    SceneLoadManager sceneLoader;
    ClassManager classManager;

    int index = 0;
    int costumeToggleIndex = 0;
    int faceToggleIndex = 0;

    void Start()
    {
        sceneLoader = SceneLoadManager.Instance;
        classManager = ClassManager.Instance;

        leftButton = transform.Find("LeftButton").GetComponent<Button>();
        leftButton.onClick.AddListener(() => ChangeCharacter(true));
        leftButton.gameObject.SetActive(false);

        rightButton = transform.Find("RightButton").GetComponent<Button>();
        rightButton.onClick.AddListener(() => ChangeCharacter(false));

        selectButton = transform.Find("Name").GetComponent<Button>();
        selectButton.onClick.AddListener(() => SetCustomize(true));
        nameText = transform.Find("Name").GetChild(0).GetComponent<TextMeshProUGUI>();
        nameText.text = characterList[index].name;

        startButtton = transform.Find("StartButton").GetComponent<Button>();
        startButtton.onClick.AddListener(SelectComplete);

        customizeSetting = transform.Find("Customize");
        customizeCostume = customizeSetting.Find("Panel/ToggleCostume");
        customizeFace = customizeSetting.Find("Panel/ToggleFace");
        backButton = customizeSetting.Find("Panel/BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(() => SetCustomize(false));

        for (int i = 0; i < customizeCostume.childCount; i++)
        {
            int num = i;
            customizeCostume.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener((n) =>
            { 
                costumeToggleIndex = num;
                SetCostumeCustomize();
            });
        }
        for (int i = 0; i < customizeFace.childCount; i++)
        {
            int num = i;
            customizeFace.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener((n) =>
            {
                faceToggleIndex = num;
                SetFaceCustomize();
            });
        }
        
        playerCharacter = characterList[index].GetChild(0).gameObject;
    }

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    CharacterMotion();
                }
            }
        }
#endif

#if UNITY_ANDRIOD
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = touch.position;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    CharacterMotion();
                }
            }
        }
#endif
    }

    void ChangeCharacter(bool bLeft)
    {
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);

        if (bLeft)
        {
            if (index > 0)
            {
                characterList[index].gameObject.SetActive(false);
                index--;
                characterList[index].gameObject.SetActive(true);
            }

            if (index <= 0)
            {
                leftButton.gameObject.SetActive(false);
            }
        }
        else
        {
            if (index < characters.childCount - 1)
            {
                characterList[index].gameObject.SetActive(false);
                index++;
                characterList[index].gameObject.SetActive(true);
            }

            if (index >= characters.childCount - 1)
            {
                rightButton.gameObject.SetActive(false);
            }
        }

        nameText.text = characterList[index].name;
        SetCostumeCustomize();
        SetFaceCustomize();
    }

    void CharacterMotion()
    {
        Animator animator = characterList[index].GetComponent<Animator>();

        if (animator != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.SetTrigger("WaveHand");
            }
        }
    }

    void SelectComplete()
    {
        classManager.CurrentClass = (ClassType)index;
        classManager.CostumeIndex = costumeToggleIndex;
        classManager.FaceIndex = faceToggleIndex;

        sceneLoader.LoadScene(sceneLoader.WaveGameScene);

        selectButton.onClick.RemoveAllListeners();
        startButtton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();

        selectButton.gameObject.SetActive(false);
        startButtton.gameObject.SetActive(false);
    }

    // --------------------------------------------------------------------------------------------------------------------
    // Customizing

    void SetCustomize(bool bActive)
    {
        transform.Find("Name").gameObject.SetActive(!bActive);
        leftButton.gameObject.SetActive(!bActive);
        rightButton.gameObject.SetActive(!bActive);
        customizeSetting.gameObject.SetActive(bActive);
        startButtton.gameObject.SetActive(bActive);
    }

    void SetCostumeCustomize()
    {
        if (customizeCostume.GetChild(costumeToggleIndex).GetComponent<Toggle>().isOn)
        {
            bool bActive = false;
            for (int i = 0; i < characterList[index].childCount; i++)
            {
                bActive = (i == costumeToggleIndex) ? true : false;
                characterList[index].GetChild(i).gameObject.SetActive(bActive);
                
                playerCharacter = characterList[index].GetChild(i).gameObject;
            }
        }
        SetFaceCustomize();
    }

    void SetFaceCustomize()
    {
        if (customizeFace.GetChild(faceToggleIndex).GetComponent<Toggle>().isOn)
        {
            int activeNum = 0;
            for (int i = 0; i < characterList[index].childCount; i++)
            {
                if (characterList[index].GetChild(i).gameObject.activeInHierarchy)
                {
                    activeNum = i;
                }
            }

            Transform bone = classManager.FindFaceBone(characterList[index].GetChild(activeNum));
            
            for (int i = 0; i < bone.childCount; i++)
            {
                bone.GetChild(i).gameObject.SetActive(false);
            }

            if (faceToggleIndex > 0)
            {
                bone.GetChild(faceToggleIndex - 1).gameObject.SetActive(true);
            }
        }
    }

    
}
