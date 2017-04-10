using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Author: Nigel Munro
*/

public class GameStateManager : MonoBehaviour {

    enum States
    {
        MAIN_MENU,
        PLAYING,
        PAUSED
    }

    public int gameState;
    public int level = 0;
    public int score = 0;

    public GameObject mainMenuCanvas;
    public GameObject playingCanvas;
    public GameObject pausedCanvas;


    void Start () {
        gameState = (int)States.MAIN_MENU;
    }
	
	
	void Update () {
        if (Input.GetButton("Cancel") && gameState == (int)States.PLAYING)
        {
            Pause();
            if (Application.isEditor) {
                //QuitToMenu();
                
            }
            else
            {
                //QuitToMenu();
            }

        }
    }

    public void Play()
    {
        mainMenuCanvas.SetActive(false);
        playingCanvas.SetActive(true);
        gameState = (int)States.PLAYING;
        Time.timeScale = 1.0f;
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
}
