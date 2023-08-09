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
    public bool isGamePlaying = true;
    public int cardPlayed = 0;
    public float time;
    public GameObject firstCard;
    public GameObject secondCard;
    public Vector3 firstCardpos;
    public Vector3 secondCardpos;
    List<GameObject> listcard = new List<GameObject>();

    int correct = 0;

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
            timeTxt.text = time.ToString("N2");

        }

        if (isGamePlaying == true && Input.GetKeyDown(KeyCode.Mouse0) == true)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.gameObject.tag == "Card")
            {
                cardPlayed++;
                hit.collider.gameObject.GetComponent<Card>().openCard();

            }
        }
    }

    public void StageInit(int _stageIndex)
    {
        correct = 0;
        SoundManager.I.PlayBGM(Define.BGM_GAME);
        GameUIManager.I.SetStageName(_stageIndex);

        int[] ks = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        ks = ks.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        char[] cardType = new char[3] { 'h', 'k', 'm' };

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
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
    }

    public void IsMatched()
    {
        string firstCardImage  = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage && firstCardpos != secondCardpos)
        {
            SoundManager.I.PlaySFX(Define.GAME_CARD_MATCHED);
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

            firstCard.GetComponent<Card>().destroyCard();
            secondCard.GetComponent<Card>().destroyCard();
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
            SoundManager.I.PlaySFX(Define.GAME_CARD_FAILED);
            GameUIManager.I.ShowOXDisplay(false);
            firstCard.GetComponent<Card>().closeCard();
            secondCard.GetComponent<Card>().closeCard();
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
        for(int i = 0; i < listcard.Count; i++)
        {
            if(listcard[i] != null)
            {
                Destroy(listcard[i]);
            }
        }

        listcard.Clear();

        time = 0.0f;
        cardPlayed = 0;
        correct = 0;
    }
}
