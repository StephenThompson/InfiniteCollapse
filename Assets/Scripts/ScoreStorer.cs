using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

*/

public class ScoreStorer : MonoBehaviour {

	const int SCORE_COUNT = 10;
	int[] scores;

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
		//AddScore(1);
		//LogScores();

		
	}

	void SaveScores()
	{
		for (int i = 0; i < SCORE_COUNT; i++)
		{
			PlayerPrefs.SetInt(i + "SCORE", scores[i]);
		}
	}

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
