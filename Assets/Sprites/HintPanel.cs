using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintPanel : MonoBehaviour {

    public Text hint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //打开提示
    public void Open()
    {
        gameObject.SetActive(true);
    }

    //关闭提示
    public void Close()
    {
        gameObject.SetActive(false);
    }

    //设置提示信息
    public void SetHint(string text)
    {
        hint.text = text;
    }
}
