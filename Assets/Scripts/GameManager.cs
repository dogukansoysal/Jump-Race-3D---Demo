﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameConstants;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;    // Singleton pattern **Instance**
    public GameState GameState;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameState = GameState.Menu;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Game Event Management

    public void Play()
    {
        GameState = GameState.Playable;
        UIController.Instance.ShowPanel(1);
    }

    public void Pause()
    {
        // TODO: Add a pause option.
    }

    public void Success()
    {
        GameState = GameState.Menu;
        UIController.Instance.ShowPanel(2);
    }

    public void Fail()
    {
        GameState = GameState.Menu;
        UIController.Instance.ShowPanel(3);
    }

    #endregion
    
    
    
    
    
    
    
    
    

    /// <summary>
    /// All scene changing methods will be listed in here.
    /// </summary>

    #region Scene Management

    // Returns current scene build index
    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    
    // Restart Scene
    public void RestartScene()
    {
        SceneManager.LoadScene(GetCurrentSceneIndex());
    }

    // Open Next Scene. If there is no next scene left, open random.
    public void NextScene()
    {
        if (SceneManager.sceneCountInBuildSettings > GetCurrentSceneIndex() + 1)    // Check max. scene count
        {
            SceneManager.LoadScene(GetCurrentSceneIndex() + 1);
        }
        else
        {
            Debug.Log("Max scene reached. Opening Random scene.");
            RandomScene();
        }
    }

    // Open Random Scene except Tutorial scene
    public void RandomScene()
    {
        SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCountInBuildSettings));    // - DONE - TODO: Set SceneIndex 0 as the Tutorial scene, and ignore it when randomize in the future.
    }


    #endregion
    
    
}
