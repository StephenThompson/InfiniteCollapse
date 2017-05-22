using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Author: Nigel Munro
*/

public static class ScoreStorer{

    public const int SCORE_COUNT = 4;
    static int[] scores = new int[SCORE_COUNT];

    static bool clearScores = false;

    static ScoreStorer()
    {
        Init();
    }
    

    // Use this for initialization
    public static void Init () {
        if (clearScores)
        {
            PlayerPrefs.DeleteAll();
        }

        // Load up Scores
        for (int i = 0; i < SCORE_COUNT; i++)
		{
			if (PlayerPrefs.HasKey(i + "SCORE"))
			{
				scores[i] = PlayerPrefs.GetInt(i + "SCORE");
			}
			else
			{
				scores[i] = 0;
                //PlayerPrefs.SetInt(i + "SCORE", scores[i]);
            }
			//scores[i] = -i * 2;
		}
        //PlayerPrefs.Save();
        //LogScores();
        //AddScore(200);
        //AddScore(100);
        //AddScore(300);
        //AddScore(4);
        //LogScores();


    }

    public static int[] GetScores()
    {
        return scores;
    }

    static void SaveScores()
	{
		for (int i = 0; i < SCORE_COUNT; i++)
		{
			PlayerPrefs.SetInt(i + "SCORE", scores[i]);
		}
        PlayerPrefs.Save();
    }

    // Call when the game has ended with new score
    public static void AddScore(int score)
	{
        //Debug.Log("Adding score " + score);
        for (int i = 0; i < SCORE_COUNT; i++)
		{
			if (score > scores[i])
			{
				for (int j = SCORE_COUNT - 1; j > i; j--)
				{
                    scores[j] = scores[j-1];
                    
                }
				scores[i] = score;
                SaveScores();
                return;
			}
		}
        
    }

    static void LogScores () {
		for (int i = 0; i < SCORE_COUNT; i++)
		{
			Debug.Log(scores[i]);
		}
		
	}
	
}
