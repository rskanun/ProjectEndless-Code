using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    // csv 파일 위치
    private string file = @"Assets\Resources\dialogue.csv";

    // csv 파일을 줄별로 정리할 list
    private StreamReader reader;
    private static List<string> lines = new List<string>();
    public static List<string> Lines { get { return lines; } }

    void Awake()
    {
        reader = new StreamReader(File.OpenRead(file));
        fileRead();
    }

    private void fileRead()
    {
        while (!reader.EndOfStream)
        {
            string str = reader.ReadLine();

            if (str.ToCharArray()[0] != '#')
            {
                lines.Add(str);
            }
        }
    }
}
