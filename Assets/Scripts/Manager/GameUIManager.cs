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
        cardMatch.ShowDisplay(GetMatchText(_name));
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
                stageName.text = "��������\n<color=white>������</color>";
                break;
            case 1:
                stageName.text = "��������\n<color=white>�谭��</color>";
                break;
            case 2:
                stageName.text = "��������\n<color=white>��μ�</color>";
                break;
            default:
                stageName.text = "��������\n<color=white>?</color>";
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
        GameManager.I.isGamePlaying = false;
        SoundManager.I.PlaySFX(Define.UI_BUTTON_GAME_MENU_SFX);
        ToggleMenuUI();
    }

    public void OnClickResumeButton()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        GameManager.I.isGamePlaying = true;
        ToggleMenuUI();
    }

    public void OnClickRetryButton()
    {
        Core.I.OffUIInput();
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        fadeInOut.SetTrigger("FadeIn");
        GameManager.I.Release();
        GameManager.I.StageInit(Core.I.stageIndex);
        Invoke("FadeOut", 1.0f);
        Invoke("ToggleMenuUI", 1.0f);
        GameManager.I.isGamePlaying = true;
        Core.I.OnUIInput();
    }

    public void OnClickToMenuButton()
    {
        Core.I.OffUIInput();
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        fadeInOut.SetTrigger("FadeIn");
        Invoke("ToMainMenu", 1.0f);
    }
}
