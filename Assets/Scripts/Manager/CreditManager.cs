using UnityEngine;

public class CreditManager : MonoBehaviour
{
    private void Start()
    {

    }

    public void ToMenu()
    {
        Core.I.LoadScene(Define.SCENE_MENU_STR);
    }
}
