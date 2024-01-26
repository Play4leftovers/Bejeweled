using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Jewel : MonoBehaviour
{
    public JewelSO jewelSO;

    void Awake()
    {
        JewelSO[] tempList;
        tempList = Resources.LoadAll("JewelSO", typeof(JewelSO)).Cast<JewelSO>().ToArray();
        jewelSO = tempList[Random.Range(0, tempList.Length)];
        SetGraphics();
    }

    void SetGraphics()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = jewelSO.Sprite;
    }
}