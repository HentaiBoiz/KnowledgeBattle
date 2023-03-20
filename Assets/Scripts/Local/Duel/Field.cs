using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Field 
{
    public Transform[] battleZone = new Transform[3];
    public Transform[] queueZone = new Transform[3];
    public Transform deckZone;
    public Transform handZone;
    public Transform dropZone;
    public Transform cheatZone;

}
