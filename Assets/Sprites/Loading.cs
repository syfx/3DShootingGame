using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    public Slider loadSlider;

	// Use this for initialization
	void Start () {
        StartCoroutine(loading(LoadScene.sceneName));
	}
    
    IEnumerator loading(string sceneName)
    {
        AsyncOperation ap = SceneManager.LoadSceneAsync(sceneName);
        float smooth = 0;
        ap.allowSceneActivation = false;

        while (smooth < 1)
        {
            smooth = Mathf.Lerp(smooth, ap.progress + 0.11f, 0.16f);
            loadSlider.value = smooth;
            if (smooth >= 1)
                ap.allowSceneActivation = true;
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }
}
