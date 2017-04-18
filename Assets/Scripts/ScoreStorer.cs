using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Author: Nigel Munro
*/

public class ScoreStorer : MonoBehaviour {

	const int SCORE_COUNT = 4;
	int[] scores;

    public GameObject score0;
    public GameObject score1;
    public GameObject score2;
    public GameObject score3;

    //public Text score0Text;
    //public Text score1Text;
    //public Text score2Text;
    //public Text score3Text;

    // Use this for initialization
    void Start () {

		// Load up Scores
		scores = new int[SCORE_COUNT];

		for (int i = 0; i < SCORE_COUNT; i++)
		{
			if (PlayerPrefs.HasKey(i + "SCORE"))
			{
				scores[i] = PlayerPrefs.GetInt(i + "SCORE");
			}
			else
			{
				scores[i] = 0;
			}
			//scores[i] = -i * 2;
		}
        //LogScores();
        //AddScore(200);
        //AddScore(100);
        //AddScore(300);
        //AddScore(4);
        //LogScores();


    }

    // Scores canvas must be Active when called!
    public void DisplayScores()
    {
        Text[] textArray = new Text[SCORE_COUNT];
        textArray[0] = score0.GetComponent<Text>();
        textArray[1] = score1.GetComponent<Text>();
        textArray[2] = score2.GetComponent<Text>();
        textArray[3] = score3.GetComponent<Text>();

        for (int i = 0; i < SCORE_COUNT; i++)
        {
            Text text = textArray[i];
            text.text = "" + scores[i];
        }
    }

	void SaveScores()
	{
		for (int i = 0; i < SCORE_COUNT; i++)
		{
			PlayerPrefs.SetInt(i + "SCORE", scores[i]);
		}
	}

    // Call when the game has ended with new score
	void AddScore(int score)
	{
		for (int i = 0; i < SCORE_COUNT; i++)
		{
			if (score > scores[i])
			{
				for (int j = SCORE_COUNT - 1; j > i; j--)
				{
                    scores[j] = scores[j-1];
                    
                }
				scores[i] = score;
				return;
			}
		}
        SaveScores();
    }

	void LogScores () {
		for (int i = 0; i < SCORE_COUNT; i++)
		{
			Debug.Log(scores[i]);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
