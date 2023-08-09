using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void PlayAnim(string _triggerName)
    {
        anim.SetTrigger(_triggerName);
    }

    public bool isFliping()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName(Define.CARD_ANIM_FLIP_0) == true ||
            anim.GetCurrentAnimatorStateInfo(0).IsName(Define.CARD_ANIM_FLIP_1) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OpenCard()
    {
        if(isFliping() == true)
        {
            return;
        }

        int ranFlipNum = Random.Range(0, 2);

        if(ranFlipNum == 0)
        {
            PlayAnim(Define.CARD_ANIM_FLIP_0);
        }
        else
        {
            PlayAnim(Define.CARD_ANIM_FLIP_1);
        }

        if (GameManager.I.firstCard == null)
        {
            GameManager.I.firstCard = gameObject;
            GameManager.I.firstCardpos = gameObject.transform.position;
        }
        else if (GameManager.I.firstCard != null)
        {
            GameManager.I.secondCard = gameObject;
            GameManager.I.secondCardpos = gameObject.transform.position;
            GameManager.I.IsMatched();
        }
    }
    public void DestroyCard()
    {
        PlayAnim(Define.CARD_ANIM_MATCHED);
    }

    public void DestroyAnimEvent()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        PlayAnim(Define.CARD_ANIM_RESET);
    }

    #region Old
    /* 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openCard()
    {
        anim.SetBool("isOpen", true);

        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (GameManager.I.firstCard == null)
        {
            GameManager.I.firstCard = gameObject;
            GameManager.I.firstCardpos = gameObject.transform.position;
        }
        else if (GameManager.I.firstCard != null)
        {
            GameManager.I.secondCard = gameObject;
            GameManager.I.secondCardpos = gameObject.transform.position;
            GameManager.I.IsMatched();
        }
    }
    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 0.5f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 0.5f);
    }

    void closeCardInvoke()
    {
        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
    */
    #endregion
}

