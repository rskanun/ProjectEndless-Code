using System;
using UnityEngine;

[Serializable]
public class Chapter
{
    [SerializeField]
    private int _chapterNum;
    public int ChapterNum
    {
        get { return _chapterNum; }
        set
        {
            if (value < 0) _chapterNum = 0;
            else _chapterNum = value;
        }
    }

    [SerializeField]
    private int _rootNum;
    public int RootNum
    {
        get { return _rootNum; }
        set
        {
            if (value < 0) _rootNum = 0;
            else _rootNum = value;
        }
    }

    [SerializeField]
    private int _subChapterNum;
    public int SubChapterNum
    {
        get { return _subChapterNum; }
        set
        {
            if (value < 0) _subChapterNum = 0;
            else _subChapterNum = value;
        }
    }

    public Chapter() { }
    public Chapter(int chapterNum)
    {
        ChapterNum = chapterNum;
    }
    public Chapter(int chapterNum, int rootNum)
    {
        ChapterNum = chapterNum;
        RootNum = rootNum;
    }
    public Chapter(int chapterNum, int rootNum, int subChapterNum)
    {
        ChapterNum = chapterNum;
        RootNum = rootNum;
        SubChapterNum = subChapterNum;
    }
}
