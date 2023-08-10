using UnityEngine;
using TMPro;

public class CreditManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skipText;

    float timePassed = 0.0f;
    float duration   = 2.0f;

    private void Start()
    {

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
    }

    public void ToMenu()
    {
        Core.I.LoadScene(Define.SCENE_MENU_STR);
    }
}
