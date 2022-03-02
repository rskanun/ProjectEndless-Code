using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textManager : MonoBehaviour
{
    Dictionary<int, string[]> textData;

    private void Awake()
    {
        textData = new Dictionary<int, string[]>();
        textList();
    }

    private void textList()
    {
        textData.Add(1, new string[]
        {
            "네가 이곳의 주인공이구나?",
            "그거 알아? \r\n사실 이곳은 가상세계야."
        });

        textData.Add(2, new string[]
        {
            "표지판에 박힌 가시에 찔렸다."
        });
    }

    public string[] getText(int id)
    {
        return textData[id];
    }
}
