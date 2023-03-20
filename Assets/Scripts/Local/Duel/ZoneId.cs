using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZoneId 
{
    public int healthPoint;

    public string[] battleZone = new string[3];
    public bool[] battleZoneAttacked = new bool[3];
    public string[] queueZone = new string[3];
    public List<string> handZone = new List<string>();
    public List<string> dropZone = new List<string>();
    public string cheatZone;
    public List<string> deckZone;

}
