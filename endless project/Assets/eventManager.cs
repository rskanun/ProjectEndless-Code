using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventManager : MonoBehaviour
{
    public void getEvent(string str)
    {
        string[] commands = str.Split(' ');
        string command = str.Split(' ')[0];

        switch (command)
        {
            case "/getDamage":
                break;

            default:
                Debug.Log(command + " is an incorrect command!");
                break;
        }
    }

    void getDamage(string[] commands)
    {

    }
}
