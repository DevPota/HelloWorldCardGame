using UnityEngine;

public class OXDisplay : MonoBehaviour
{
    [SerializeField] GameObject[] oxObjs;

    float duration = 0.0f;
    float limit    = 1.0f;

    public void ShowDisplay(bool _isCorrect)
    {
        duration = 0.0f;

        if (_isCorrect == true)
        {
            oxObjs[1].SetActive(false);
            oxObjs[0].SetActive(true);
        }
        else
        {
            oxObjs[0].SetActive(false);
            oxObjs[1].SetActive(true);
        }
    }

    private void Update()
    {
        duration += Time.deltaTime;

        if(limit <= duration)
        {
            gameObject.SetActive(false);
        }
    }
}
