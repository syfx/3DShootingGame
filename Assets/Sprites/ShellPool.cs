using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellPool : MonoBehaviour {

    private static ShellPool instance;
    public static ShellPool Instance
    {
        get
        {
            return instance;
        }
    }
    
    [Tooltip("子弹预制")] public GameObject shellPrefab;
    [Tooltip("取出的子弹是否是激活后的")] public bool getIsEnable;
    public Transform shellPool;

    private List<GameObject> shellList = new List<GameObject>();

	void Awake () {
        instance = this;
    }
	
    //放入一个子弹
    public void PutInShell(GameObject shell)
    {
        shell.GetComponent<Rigidbody>().Sleep();                //休眠钢体
        shell.SetActive(false);      
        shellList.Add(shell);
    }

    //取出一个子弹
    public GameObject TakeOutShell()
    {
        GameObject shell = null;
        if (shellList.Count <= 0)
            shell = InstantiateShell();
        else
        {
            shell = shellList[0];
            shellList.RemoveAt(0);
        }
        shell.SetActive(getIsEnable);
        return shell;
    }

    //实例化子弹
    private GameObject InstantiateShell()
    {
        if (shellPrefab == null)
            return null;

        GameObject go = Instantiate(shellPrefab, shellPool);
        return go;
    }
}
