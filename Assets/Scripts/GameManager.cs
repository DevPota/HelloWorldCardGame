using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {

        int[] name = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        name = name.OrderBy(item => Random.Range(0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;
            GameObject Newimage = Instantiate(image);
            Newimage.transform.position = new Vector3(x, y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
