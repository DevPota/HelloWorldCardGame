using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] Animator   fadeInOut;
    [SerializeField] TextMeshProUGUI stageName;
    [SerializeField] FlickTextObject cardMatch;
    [SerializeField] OXDisplay oXDisplay;

    Dictionary<string, string> matchTextPool = new Dictionary<string, string>();

    public static GameUIManager I;

    private void Awake()
    {
        if(I == null)
        {
            I = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FadeOut();
        Core.I.OnUIInput();

        TextAsset txtAsset = Resources.Load<TextAsset>("CardMatchTexts");

        string[] lines = txtAsset.text.Split('\n');
        
        for(int i = 0; i < lines.Length; i++)
        {
            string[] splitedString = lines[i].Split(',');

            if(splitedString.Length == 2)
            {
                matchTextPool.Add(splitedString[0], splitedString[1]);
            }
        }
    }

    string GetMatchText(string _name)
    {
        string opt = "";

        matchTextPool.TryGetValue(_name, out opt);

        return opt;
    }
    
    public void ShowMatchText(string _name)
    {
        cardMatch.gameObject.SetActive(true);
        string name = GetMatchText(_name);

        if (name == "" || name == null)
        {
            cardMatch.ShowDisplay(_name);
        }
        else
        {
            cardMatch.ShowDisplay(name);
        }
    }

    public void ShowOXDisplay(bool _isCorrect)
    {
        oXDisplay.gameObject.SetActive(true);
        oXDisplay.ShowDisplay(_isCorrect);
    }

    public void SetStageName(int _index)
    {
        switch(_index)
        {
            case 0:
                stageName.text = "스테이지\n<color=white>김해찬</color>";
                break;
            case 1:
                stageName.text = "스테이지\n<color=white>김강현</color>";
                break;
            case 2:
                stageName.text = "스테이지\n<color=white>김민석</color>";
                break;
            default:
                stageName.text = "스테이지\n<color=white>?</color>";
                break;
        }
    }

    public void FadeIn()
    {
        fadeInOut.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        fadeInOut.SetTrigger("FadeOut");
    }

    public void ToggleGameOverUI()
    {
        if (gameOverUI.activeSelf == true)
        {
            gameOverUI.SetActive(false);
        }
        else
        {
            gameOverUI.SetActive(true);
        }
    }

    void ToggleMenuUI()
    {
        SoundManager.I.LowPassBGM();

        if (menuUI.activeSelf == true)
        {
            menuUI.SetActive(false);
        }
        else
        {
            menuUI.SetActive(true);
        }
    }
    void ToMainMenu()
    {
        SoundManager.I.LowPassBGM();
        Core.I.LoadScene(Define.SCENE_MENU_STR);
    }

    public void OnClickMenuButton()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_GAME_MENU_SFX);
        ToggleMenuUI();
    }

    public void OnClickResumeButton()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        ToggleMenuUI();
    }

    public void OnClickRetryButton()
    {
        Core.I.OffUIInput();
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        fadeInOut.SetTrigger("FadeIn");
        Invoke("ToggleMenuUI", 1.5f);
        Invoke("FadeOut", 2.0f);
        GameManager.I.StageInit(Core.I.stageIndex);
        Core.I.OnUIInput();
    }

    public void OnClickToMenuButton()
    {
        Core.I.OffUIInput();
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        fadeInOut.SetTrigger("FadeIn");
        Invoke("ToMainMenu", 1.5f);
    }
}
