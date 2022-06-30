using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject deathMenuUI;

    public PlayerHealth playerHp;

    void Start()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (playerHp.currentHealth <= 0 && GameIsPaused == false)
        {
            GameIsPaused = true;
            deathMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else if(playerHp.currentHealth >= 0 && GameIsPaused == true)
        {
            Resume();
        }
    }


    public void Resume()
    {
        deathMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Restart()
    {
        deathMenuUI.SetActive(false);
        GameIsPaused=false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("CombatTest");
    }

    public void LoadMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
