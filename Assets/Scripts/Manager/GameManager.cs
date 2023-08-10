using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeTxt;

    public GameObject image;
    public GameObject endTxt;
    public GameObject card;
    public bool isGamePlaying = false;
    public int cardPlayed = 0;
    public float time;
    public GameObject firstCard = null;
    public GameObject secondCard = null;
    public Vector3 firstCardpos;
    public Vector3 secondCardpos;
    List<GameObject> listcard = new List<GameObject>();

    int correct = 0;
    Coroutine initialCardAnimCorotuine = null;

    public static GameManager I;

    void Awake()
    {
        if(I == null)
        {
            I = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StageInit(Core.I.stageIndex);
    }

    private void Update()
    {
        if (isGamePlaying == true)
        {
            time += Time.deltaTime;

            if(time >= 30)
            {
                timeTxt.text = "<color=red>" + time.ToString("N2") + "</color>";
            }
            else
            {
                timeTxt.text = time.ToString("N2");
            }
        }

        if (isGamePlaying == true && Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.gameObject.tag == "Card")
            {
                if(firstCard != null && 
                   firstCard.name ==
                   hit.collider.gameObject.name)
                {
                    return;
                }
                else
                {
                    cardPlayed++;
                    hit.collider.gameObject.GetComponent<Card>().OpenCard();
                }
            }
        }
    }

    public void StageInit(int _stageIndex)
    {
        Release();

        SoundManager.I.PlayBGM(Define.BGM_GAME);
        GameUIManager.I.SetStageName(_stageIndex);
        timeTxt.text = "0:00";

        int[] ks = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        ks = ks.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        char[] cardType = new char[3] { 'h', 'k', 'm' };

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.gameObject.name = newCard.gameObject.name + "_" + i;
            newCard.SetActive(false);
            listcard.Add(newCard);
            newCard.transform.parent = GameObject.Find("Cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            string name = "" + cardType[_stageIndex] + '_' + ks[i];
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/" + name);
            SpriteRenderer[] sr = newCard.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer element in sr)
            {
                element.sortingOrder = 2;
            }

            newCard.transform.Find("front").gameObject.SetActive(false);
        }

        initialCardAnimCorotuine = StartCoroutine(InitialCardAnim());
    }

    IEnumerator InitialCardAnim()
    {
        int cardInitialAnimNum = Random.Range(0, 2);

        foreach (GameObject cardObj in listcard)
        {
            cardObj.SetActive(true);
            Card card = cardObj.GetComponent<Card>();

            if (cardInitialAnimNum == 0)
            {
                card.PlayAnim(Define.CARD_INIT_ANIM_SPIN);
            }
            else
            {
                card.PlayAnim(Define.CARD_INIT_ANIM_WAVE);
            }

            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return null;
        yield return new WaitForSecondsRealtime(1.0f);

        foreach (GameObject cardObj in listcard)
        {
            Card card = cardObj.GetComponent<Card>();
            card.PlayAnim(Define.CARD_ANIM_RESET);
        }

        isGamePlaying = true;
        GameUIManager.I.ShowMatchText("게임 시작!");
        initialCardAnimCorotuine = null;

        yield break;
    }

    public void IsMatched()
    {
        string firstCardImage  = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage && firstCardpos != secondCardpos)
        {
            SoundManager.I.PlaySFX(Define.GAME_CARD_MATCHED_SFX);
            GameUIManager.I.ShowOXDisplay(true);
            GameUIManager.I.ShowMatchText(firstCardImage);

            for(int i = 0; i < listcard.Count; i++)
            {
                if(listcard[i] == null)
                {
                    continue;
                }

                string tempName = listcard[i].transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

                if (tempName == firstCardImage)
                {
                    listcard[i] = null;
                }
            }

            firstCard.GetComponent<Card>().DestroyCard();
            secondCard.GetComponent<Card>().DestroyCard();

            correct++;

            if (correct == listcard.Count / 2)
            {
                isGamePlaying = false;
                GameUIManager.I.ToggleGameOverUI();

                Invoke("FadeInDelay", 1.0f);
                Invoke("ToResultScene", 2.0f);
            }

        }
        else
        {
            SoundManager.I.PlaySFX(Define.GAME_CARD_FAILED_SFX);
            GameUIManager.I.ShowOXDisplay(false);
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
        }

        firstCard = null;
        secondCard = null;
        firstCardpos = Vector3.zero;
        secondCardpos = Vector3.zero;
    }
    
    void FadeInDelay()
    {
        GameUIManager.I.FadeIn();
    }

    void ToResultScene()
    {
        Core.I.LoadScene(Define.SCENE_RESULT_STR, LoadSceneMode.Additive);
    }

    public void Release()
    {
        if(initialCardAnimCorotuine != null)
        {
            StopCoroutine(initialCardAnimCorotuine);
            initialCardAnimCorotuine = null;
        }

        for(int i = 0; i < listcard.Count; i++)
        {
            if(listcard[i] != null)
            {
                Destroy(listcard[i]);
            }
        }

        listcard.Clear();

        isGamePlaying = false;
        time = 0.0f;
        cardPlayed = 0;
        correct = 0;
    }
}
