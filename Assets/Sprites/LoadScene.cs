using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene {

    public static string sceneName;

    public static void loadScene(string loadSceneName)
    {
        sceneName = loadSceneName;
        SceneManager.LoadScene("LoadScene");
    }
}
