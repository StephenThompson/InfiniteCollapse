using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Author: Nigel Munro
*/

public class GameStateManager : MonoBehaviour {

    enum States
    {
        MAIN_MENU,
        PLAYING,
        PAUSED,
        SCORES
    }

    public int gameState;
    public int level = 0;
    public int score = 0;

    public GameObject mainMenuCanvas;
    public GameObject playingCanvas;
    public GameObject pausedCanvas;
    public GameObject scoresCanvas;


    void Start () {
        gameState = (int)States.MAIN_MENU;
    }
	
	
	void Update () {
        // We are playing and open the pause menu
        if (Input.GetButton("Cancel") && gameState == (int)States.PLAYING)
        {
            Pause();
            //if (Application.isEditor) {
            //}

        }
        // We are at the score board and want to exit to menu
        if (Input.GetButton("Cancel") && gameState == (int)States.SCORES)
        {
            ScoresToMenu();
        }

        // Pause to Playing
        if (Input.GetButton("Cancel") && gameState == (int)States.PAUSED)
        {
            //Resume();
        }
    }

    public void Play()
    {
        if (Application.isEditor)
        {
            // Application.LoadLevel(1);
            //EditorSceneManager.OpenScene("Scenes/Menus");
            SceneManager.LoadScene(1);
        } else
        {
            SceneManager.LoadScene(1);
        }
        /*
        mainMenuCanvas.SetActive(false);
        playingCanvas.SetActive(true);
        gameState = (int)States.PLAYING;
        Time.timeScale = 1.0f;
        */
    }

    public void Pause()
    {
        gameState = (int)States.PAUSED;
        Time.timeScale = 0.0f;
        pausedCanvas.SetActive(true);
    }

    public void Resume()
    {
        pausedCanvas.SetActive(false);
        playingCanvas.SetActive(true);
        gameState = (int)States.PLAYING;
    }

    public void QuitToMenu()
    {
        playingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        pausedCanvas.SetActive(false);
        gameState = (int)States.MAIN_MENU;
    }

    public void Scores()
    {
        mainMenuCanvas.SetActive(false);
        scoresCanvas.SetActive(true);
        gameState = (int)States.SCORES;
        GetComponent<ScoreStorer>().DisplayScores();
    }

    public void ScoresToMenu()
    {
        scoresCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        gameState = (int)States.MAIN_MENU;
    }
}
