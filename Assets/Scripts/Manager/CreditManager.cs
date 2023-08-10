using UnityEngine;
using TMPro;

public class CreditManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skipText;

    float timePassed = 0.0f;
    float duration   = 2.0f;

    float bgmFadeOutTimePassed = 0.0f;
    float bgmFadeOutTime = 30.0f;
    bool fadeOutTrigger = false;

    private void Start()
    {
        SoundManager.I.LowPassBGM();
        SoundManager.I.PlayBGM(Define.BGM_CREDIT);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape) == true)
        {
            timePassed += Time.deltaTime;
            skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, 1);

            if(timePassed >= duration)
            {
                ToMenu();
            }
        }
        else
        {
            skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, skipText.color.a - Time.deltaTime);
            timePassed = 0.0f;
        }

        bgmFadeOutTimePassed += Time.deltaTime;

        if(bgmFadeOutTime <= bgmFadeOutTimePassed && fadeOutTrigger == false)
        {
            fadeOutTrigger = true;
            SoundManager.I.FadeOutBGM();
        }
    }

    public void ToMenu()
    {
        SoundManager.I.LowBassBGMOff();
        Core.I.LoadScene(Define.SCENE_MENU_STR);
    }

    public void LowBase()
    {
        SoundManager.I.LowPassBGM();
    }
}
