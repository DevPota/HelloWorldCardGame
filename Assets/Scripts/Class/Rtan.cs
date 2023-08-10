using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rtan : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnEnter()
    {
        int ranValue = Random.Range(0, 2);

        if(ranValue == 0)
        {
            SoundManager.I.PlaySFX(Define.UI_RTAN_SUPIRISED_SFX_0);
        }
        else
        {
            SoundManager.I.PlaySFX(Define.UI_RTAN_SUPIRISED_SFX_1);
        }
        anim.SetTrigger("Hover");
    }

    public void OnExit()
    {
        anim.SetTrigger("Reset");
    }

    public void OnClick()
    {
        Core.I.LoadScene(Define.SCENE_CREDIT_STR);
    }
}
