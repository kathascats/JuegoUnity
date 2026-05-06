using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRandom : MonoBehaviour
{
    [SerializeField] int candleLocation;
    [SerializeField] GameObject candle1;
    [SerializeField] GameObject candle2;
    [SerializeField] GameObject candle3;

    void Start()
    {
        candleLocation = Random.Range(1, 4);
        if (candleLocation == 1)
        {
            candle1.SetActive(true);
        }
        if (candleLocation == 2)
        {
            candle2.SetActive(true);
        }
        if (candleLocation == 3)
        {
            candle3.SetActive(true);
        }
    }


}

