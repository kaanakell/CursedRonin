using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RestartButton();
    }

    public void RestartButton()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R");
            Time.timeScale = 1;
            SceneManager.LoadScene("SampleScene");
        }

    }
}
