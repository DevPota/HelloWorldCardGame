using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Animator fadeInOut;
    [SerializeField] GameObject[] preObjs;
    [SerializeField] TextMeshProUGUI playedTimeText;
    [SerializeField] TextMeshProUGUI cardPlayedText;
    [SerializeField] TextMeshProUGUI score;

    Queue<GameObject> objQueue = new Queue<GameObject>();

    private void Start()
    {
        Core.I.stage[Core.I.stageIndex] = true;

        playedTimeText.text = GameManager.I.time.ToString("N2") + "√ ";
        cardPlayedText.text = GameManager.I.cardPlayed + "»∏";

        int calculatedScore = 10000 - 
            (int)
            (Mathf.Clamp((GameManager.I.time * 100) - 3200, 0, 5000) + 
            Mathf.Clamp((GameManager.I.cardPlayed * 100) - 2800, 0, 5000));

        score.text          = "<color=yellow>SCORE</color>\n" + calculatedScore;

        fadeInOut.SetTrigger("FadeOut");

        foreach(GameObject obj in preObjs)
        {
            objQueue.Enqueue(obj);
        }

        GetComponent<PlayableDirector>().Play();
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

        if(Core.I.stageIndex == 3)
        {
            Core.I.stageIndex = 0;
        }
        else
        {
            Core.I.stageIndex++;
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
        if (Core.I.stageIndex == 3)
        {
            Core.I.stageIndex = 0;
        }
        else
        {
            Core.I.stageIndex++;
        }

        GameManager.I.StageInit(Core.I.stageIndex);
        Core.I.UnloadLastAdditiveScene();
    }

    void ToCredit()
    {

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
}
