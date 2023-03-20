using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int minScore = 10;
    public int currentScore;

    public Handler Handler;
    // Start is called before the first frame update
    void Start()
    {
        currentScore = minScore;
        Handler.SetMinBar(minScore);
    }


    void TakeScore(int Sc)
    {
        currentScore += Sc;
        //Handler.SetBar(currentScore);
    }
}
