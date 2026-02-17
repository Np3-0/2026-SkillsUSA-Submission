using System;
using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance {get; set;}
    public bool isMenuOpen = false;
    public GameObject pauseMenu;

    void Awake() {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        } 
        else 
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMenuOpen)
        {
            isMenuOpen = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            isMenuOpen = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void getMenuButtonPressed(string button)
    {
        if (button == "Resume")
        {
            isMenuOpen = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
        else if (button == "Settings")
        {
            Debug.Log("There are no settings.");

        }
        else if (button == "Quit")
        {
            #if UNITY_EDITOR
            
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
            
        }
    }
}
