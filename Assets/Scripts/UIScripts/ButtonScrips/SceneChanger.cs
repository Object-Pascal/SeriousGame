using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public void StartGameScene()
    {
        SceneManager.LoadScene("StartGameScene");
    }

    public void JoinGameScene()
    {
        SceneManager.LoadScene("JoinGameScene");
    }

    public void HomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void RoleSelectScene()
    {
        SceneManager.LoadScene("RoleSelectScene");
    }

    public void PauseScene()
    {
        SceneManager.LoadScene("PauseScene");
    }

    public void SettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}
