using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject image;
    public GameObject endTxt;
    public GameObject card;
    float time;
    public static GameManager I;
    public GameObject firstCard;
    public GameObject secondCard;

    void Awake()
    {
        I = this;
    }

    private void Start()
    {
        StageInit(Core.I.stageIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.gameObject.tag == "Card")
            {
                // dDo something
                hit.collider.gameObject.GetComponent<Card>().openCard();

            }
        }
    }

    public void StageInit(int _stageIndex)
    {
        int[] ks = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        ks = ks.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        char[] cardType = new char[3] { 'h', 'k', 'm' };

        Debug.Log(cardType[_stageIndex]);

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
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

     public void isMatched()
     {
        string firstCardImage  = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            firstCard.GetComponent<Card>().destroyCard();
            secondCard.GetComponent<Card>().destroyCard();
        }
        else
        {
            firstCard.GetComponent<Card>().closeCard();
            secondCard.GetComponent<Card>().closeCard();
        }

        firstCard  = null;
        secondCard = null;



    }


}
