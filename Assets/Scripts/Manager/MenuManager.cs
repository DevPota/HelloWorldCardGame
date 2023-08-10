using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Animator fadeInOutAnim;
    [SerializeField] Animator doubleAnim;
    [SerializeField] GameObject subTitle;
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;

    [SerializeField] Image    selectImg;
    [SerializeField] Sprite[] selectSprites;
    [SerializeField] TextMeshProUGUI selectNameText;

    string[] stageNames = new string[3] {"±èÇØÂù", "±è°­Çö", "±è¹Î¼®"};

    bool stageSelected  = false;
    int  stageIndex     = 0;

    private void Start()
    {
        Core.I.OnUIInput();

        SoundManager.I.PlayBGM(Define.BGM_MENU);
        fadeInOutAnim.SetTrigger("FadeOut");
    }

    public void OnClickStart()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        left.SetActive(false);
        subTitle.SetActive(false);
        right.SetActive(true);
    }

    public void OnClickExit()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        Core.I.OffUIInput();
        fadeInOutAnim.SetTrigger("FadeIn");

        Invoke("ToExit", 1.0f);
    }

    public void OnClickReturn()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        doubleAnim.SetTrigger("Reset");
        doubleAnim.ResetTrigger("Reset");
        stageSelected = false;
        right.SetActive(false);
        left.SetActive(true);
        subTitle.SetActive(true);
    }

    public void OnClickStageSelect()
    {
        if(stageSelected == false)
        {
            SoundManager.I.PlaySFX(Define.UI_BUTTON_SELECT_FIRST_SFX);
            doubleAnim.SetTrigger("First");
            stageSelected = true;
        }
        else
        {
            SoundManager.I.PlaySFX(Define.UI_BUTTON_SELECT_SECOND_SFX);
            Core.I.OffUIInput();
            doubleAnim.SetTrigger("Second");
            fadeInOutAnim.SetTrigger("FadeIn");
            Core.I.stageIndex = stageIndex;

            Invoke("ToGameScene", 1.0f);
        }
    }

    public void OnClickSelectArrow(bool _isLeft)
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);

        if (_isLeft == true)
        {
            stageIndex--;

            if(stageIndex < 0)
            {
                stageIndex = selectSprites.Length-1;
            }
        }
        else
        {
            stageIndex++;

            if(stageIndex == selectSprites.Length)
            {
                stageIndex = 0;
            }
        }

        selectImg.sprite = selectSprites[stageIndex];
        selectNameText.text = stageNames[stageIndex];
    }

    void ToGameScene()
    {
        Core.I.LoadScene(Define.SCENE_GAME_STR);
    }

    void ToExit()
    {
        Application.Quit();
    }
}
