using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;
using System.Linq;
using System.Text;

[Serializable]
public class CsvFile : IEnumerable<string[]>
{
    private List<string[]> cells;

    public CsvFile()
    {
        cells = new List<string[]>();
    }

    public string[] GetLineCells(int index)
    {
        return cells[index];
    }

    public void AddLineCells(string[] lineCells)
    {
        cells.Add(lineCells);
    }

    public CsvFile MergeLines(CsvFile mergeFile)
    {
        // 현재 파일에 매개변수로 받은 파일 셀 값 합치기
        cells.AddRange(mergeFile.cells);

        // 현재 파일을 결과 값으로 리턴
        return this;
    }

    public IEnumerator<string[]> GetEnumerator()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            yield return cells[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();

        foreach (string[] lineCells in cells)
        {
            strBuilder.AppendLine(string.Join(",", lineCells));
        }

        return strBuilder.ToString();
    }
}

public class CsvReader
{
    public static CsvFile ReadFiles(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            // 폴더 내 CSV 파일을 읽어 리턴
            CsvFile file = new CsvFile();
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

            foreach (string filePath in csvFiles)
            {
                // 파일 읽어오기
                CsvFile readFile = ReadFile(filePath);

                // 읽어온 파일을 하나의 파일로 합치기
                file.MergeLines(readFile);
            }

            return file;
        }

        return null;
    }

    public static CsvFile ReadFile(string path)
    {
        TextAsset textAsset = GetTextAsset(path);

        return ReadFile(textAsset);
    }

    public static CsvFile ReadFile(TextAsset csvFile)
    {
        CsvFile result = new CsvFile();
        string[] lines = csvFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] lineCells = SplitLine(line);

            result.AddLineCells(lineCells);
        }

        return result;
    }

    private static TextAsset GetTextAsset(string path)
    {
        // 파일을 찾을 수 없는 경우
        if (!File.Exists(path))
        {
            Debug.LogWarning($"파일을 찾을 수 없습니다: {path}");
            return null;
        }

        // 파일 확장자가 .csv가 아닐 경우
        if (Path.GetExtension(path).ToLower() != ".csv")
        {
            Debug.LogWarning($"csv가 아닌 잘못된 파일 형식입니다: {path}");
            return null;
        }

        // 파일에서 텍스트 읽기
        string fileContent = File.ReadAllText(path);

        // TextAsset을 동적으로 생성
        TextAsset textAsset = new TextAsset(fileContent);

        return textAsset;
    }

    private static string[] SplitLine(string line)
    {
        // 전달받은 라인의 주석 제거
        string result = RemoveComment(line);

        // CSV 라인을 셀 별로 나누기
        string[] cells = result.Split(',');

        return cells;
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
