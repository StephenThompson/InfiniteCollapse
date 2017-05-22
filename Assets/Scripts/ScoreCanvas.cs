using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreCanvas : MonoBehaviour {

    public GameObject score0;
    public GameObject score1;
    public GameObject score2;
    public GameObject score3;

    // Use this for initialization
    void Start () {
    }

    // Scores canvas must be Active when called!
    public void DisplayScores()
    {
        Text[] textArray = new Text[ScoreStorer.SCORE_COUNT];
        textArray[0] = score0.GetComponent<Text>();
        textArray[1] = score1.GetComponent<Text>();
        textArray[2] = score2.GetComponent<Text>();
        textArray[3] = score3.GetComponent<Text>();

        int[] scores = ScoreStorer.GetScores();

        for (int i = 0; i < ScoreStorer.SCORE_COUNT; i++)
        {
            Text text = textArray[i];
            text.text = "" + scores[i];
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
