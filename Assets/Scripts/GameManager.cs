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

    void Start()
    {
        int Stageindex = Core.I.stageIndex;

        int[] ks = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        ks = ks.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        if (Stageindex == 0)
        {
            for (int i = 0; i < 16; i++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("Cards").transform;

                float x = (i / 4) * 1.4f - 2.1f;
                float y = (i % 4) * 1.4f - 3.0f;
                newCard.transform.position = new Vector3(x, y, 0);

                string name = "h_" + ks[i];
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/" + name);

                SpriteRenderer[] sr = newCard.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer element in sr)
                {
                    element.sortingOrder = 2;
                }
            }
        }

        if (Stageindex == 1)
        {
            for (int i = 0; i < 16; i++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("Cards").transform;

                float x = (i / 4) * 1.4f - 2.1f;
                float y = (i % 4) * 1.4f - 3.0f;
                newCard.transform.position = new Vector3(x, y, 0);

                string name = "k_" + ks[i];
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/" + name);
            }

        }

        if (Stageindex == 2)
        {
            for (int i = 0; i < 16; i++)
            {
                GameObject newCard = Instantiate(card);
                newCard.transform.parent = GameObject.Find("Cards").transform;

                float x = (i / 4) * 1.4f - 2.1f;
                float y = (i % 4) * 1.4f - 3.0f;
                newCard.transform.position = new Vector3(x, y, 0);

                string name = "m_" + ks[i];
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/" + name);
            }

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
