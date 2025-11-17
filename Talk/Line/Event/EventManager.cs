using UnityEngine;

public class EventManager : MonoBehaviour
{
    public void GetCommandEvent(string str)
    {
        string[] commands = str.Split(' ');
        string command = commands[0];

        switch (command)
        {
            default:
                Debug.Log(command + " is an incorrect command!");
                break;
        }
    }
}
