using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Animator fadeInOut;
    [SerializeField] GameObject[] preObjs;
    [SerializeField] TextMeshProUGUI playedTimeText;
    [SerializeField] TextMeshProUGUI cardPlayedText;
    [SerializeField] TextMeshProUGUI score;

    [SerializeField] Sprite[] rtanSprites;
    [SerializeField] Image rtanImage;
    [SerializeField] TextMeshProUGUI rtanText;

    float playedTime = GameManager.I.time;
    int   cardPlayed = GameManager.I.cardPlayed;
    int   calculatedScore = 10000 -
                          (int)
                          (Mathf.Clamp((GameManager.I.time * 100) - 3200, 0, 5000) +
                           Mathf.Clamp((GameManager.I.cardPlayed * 100) - 2800, 0, 5000));

    Queue<GameObject> objQueue = new Queue<GameObject>();

    private void Start()
    {
        Core.I.stage[Core.I.stageIndex] = true;

        score.text          = "<color=yellow>SCORE</color>\n" + calculatedScore;

        if(calculatedScore == 10000)
        {
            rtanImage.sprite = rtanSprites[3];
            rtanText.text = "어떻게 이런 점수가?";
        }
        else if (calculatedScore >= 7500)
        {
            rtanImage.sprite = rtanSprites[2];
            rtanText.text = "꽤 하는구만";
        }
        else if (calculatedScore >= 5000)
        {
            rtanImage.sprite = rtanSprites[1];
            rtanText.text = "나쁘지 않군";
        }
        else
        {
            rtanImage.sprite = rtanSprites[0];
            rtanText.text = "좀 더\n노력해라";
        }

        fadeInOut.SetTrigger("FadeOut");

        foreach(GameObject obj in preObjs)
        {
            objQueue.Enqueue(obj);
        }

        GetComponent<PlayableDirector>().Play();
    }

    public void PlayPlayedTimeText()
    {
        StartCoroutine(NumberAnimation(playedTimeText, playedTime, "초", true));
    }

    public void PlayCardPlayedText()
    {
        StartCoroutine(NumberAnimation(cardPlayedText, cardPlayed, "회", false));
    }

    public void OnClickRetry()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        fadeInOut.SetTrigger("FadeIn");
        Invoke("Retry", 1.0f);
    }

    public void OnClickNext()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        fadeInOut.SetTrigger("FadeIn");

        for(int i = 0; i < Core.I.stage.Length; i++)
        {
            if(Core.I.stage[i] == false)
            {
                Invoke("Next", 1.0f);
                return;
            }
        }

        Invoke("ToCredit", 1.0f);
    }

    public void OnClickToMenu()
    {
        SoundManager.I.PlaySFX(Define.UI_BUTTON_CLICK_SFX);
        fadeInOut.SetTrigger("FadeIn");
        Invoke("ToMenu", 1.0f);
    }

    void Retry()
    {
        GameManager.I.StageInit(Core.I.stageIndex);
        GameUIManager.I.ToggleGameOverUI();
        GameUIManager.I.FadeOut();
        Core.I.UnloadLastAdditiveScene();
    }

    void Next()
    {
        if (Core.I.stageIndex == 2)
        {
            Core.I.stageIndex = 0;
        }
        else
        {
            Core.I.stageIndex++;
        }

        GameManager.I.StageInit(Core.I.stageIndex);
        GameUIManager.I.ToggleGameOverUI();
        GameUIManager.I.FadeOut();
        Core.I.UnloadLastAdditiveScene();
    }

    void ToCredit()
    {
        Core.I.LoadScene(Define.SCENE_CREDIT_STR);
        Core.I.UnloadLastAdditiveScene();
    }

    void ToMenu()
    {
        Core.I.LoadScene(Define.SCENE_MENU_STR);
        Core.I.UnloadLastAdditiveScene();
    }

    public void ActiveObjFromQueue()
    {
        if(objQueue.Count != 0)
        {
            objQueue.Dequeue().SetActive(true);
        }
    }

    IEnumerator NumberAnimation(TextMeshProUGUI _text, float _value, string _lastText, bool _isFloat)
    {
        float timePassed = 0.0f;
        float duration = 1.5f;

        float startValue = 0.0f;

        while(timePassed <= duration)
        {
            float t = timePassed / duration;

            float calculatedValue = Mathf.Lerp(startValue, _value,t);
            if(_isFloat == true)
            {
                _text.text = calculatedValue.ToString("N2") + _lastText;
            }
            else
            {
                _text.text = (int)calculatedValue + _lastText;
            }

            timePassed += Time.deltaTime;

            yield return null;
        }

        if(_isFloat == false)
        {
            _text.text = cardPlayed + _lastText;
        }

        yield break;
    }
}
