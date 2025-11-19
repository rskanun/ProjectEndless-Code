using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestAiDisplay : MonoBehaviour
{
    public TextMeshProUGUI textPanel;

    private List<Entity> priorityTargets;
    private List<Entity> selectTargets;

    private void Awake()
    {
        priorityTargets = new List<Entity>();
        selectTargets = new List<Entity>();
    }

    public void SetPriorityTargets(List<Entity> priorityTargets)
    {
        this.priorityTargets = new List<Entity>(priorityTargets);
    }

    public void SetPriorityTarget(Entity target)
    {
        priorityTargets.Clear();

        priorityTargets.Add(target);
    }

    public void SetSelectTarget(Entity target)
    {
        selectTargets.Clear();
        selectTargets.Add(target);

        UpdateInfo();
    }

    public void SetSelectTargets(List<Entity> targets)
    {
        selectTargets = new List<Entity>(targets);

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        string str = "<Priority>\r\n";

        foreach (Entity target in priorityTargets)
        {
            if (selectTargets.Contains(target)) str += $"<color=#FF0000>- {target.Name}</color>\r\n";
            else str += $"- {target.Name}\r\n";
        }

        textPanel.text = str;
    }
}