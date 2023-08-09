using UnityEngine;
using TMPro;

public class FlickTextObject : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    float duration = 0.0f;
    float limit = 2.0f;

    public void ShowDisplay(string _text)
    {
        text.text = _text;
        duration = 0.0f;
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
