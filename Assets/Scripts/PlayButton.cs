using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void CallPlay()
    {
        Invoke("Play", 1f);
    }

    public void Play()
    {
        SceneManager.LoadScene("CombatTest");
    }
}
