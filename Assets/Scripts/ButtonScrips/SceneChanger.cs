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
}
