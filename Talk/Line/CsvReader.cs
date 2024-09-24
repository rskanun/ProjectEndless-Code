using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class CsvFile
{
    public string fileName;
    public string[] lines;
}

public class CsvReader
{
    // Select 객체 내 optionsLineNum을 위한 Dictionary
    private Stack<Select> selectStack = new Stack<Select>();

    public static List<CsvFile> ReadFiles(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            // 폴더 내 CSV 파일을 읽어 리턴
            List<CsvFile> files = new List<CsvFile>();
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

            foreach (string filePath in csvFiles)
            {
                if (File.Exists(filePath))
                {
                    CsvFile file = new CsvFile();

                    file.fileName = Path.GetFileName(filePath);
                    file.lines = ReadLines(filePath);

                    files.Add(file);
                }
            }

            return files;
        }

        return null;
    }

    private static string[] ReadLines(string path)
    {
        List<string> lines = new List<string>();
        StreamReader sr = new StreamReader(File.OpenRead(path));

        string str;
        while ((str = sr.ReadLine()) != null)
        {
            // 앞부분 공백 및 주석 제거
            str = RemoveComment(str.TrimStart());

            // 리턴값에 추가
            lines.Add(str);
        }

        return lines.ToArray();
    }

    private static string RemoveComment(string str)
    {
        int commentIndex = str.IndexOf('#');

        // 주석이 존재하는 경우
        if (commentIndex >= 0)
        {
            // 주석 문자(#)가 나오기 이전 값들을 리턴
            return str.Substring(0, commentIndex);
        }

        // 주석이 없으면 그냥 전달
        return str;
    }
}
