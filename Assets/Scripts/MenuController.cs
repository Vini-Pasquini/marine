using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnChangeSceneButtonPress(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }
}
