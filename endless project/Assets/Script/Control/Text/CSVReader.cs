using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    // csv 파일 위치
    private string file = @"Assets\Resources\dialogue.csv";
    // csv 파일을 줄별로 정리할 list
    private List<string> lines = new List<string>();
    private StreamReader reader;

    void Awake()
    {
        reader = new StreamReader(File.OpenRead(file));
        fileRead();
    }

    private void fileRead()
    {
        while (!reader.EndOfStream)
            lines.Add(reader.ReadLine());
    }

    public List<string> getLines() { return lines; }
}
