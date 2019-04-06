using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {
	
	
	public  float destroyTime=0.5f;
	public AudioSource audioDie;
	
	// Use this for initialization
	void Start () {
		GameObject mainCamera=GameObject.FindGameObjectsWithTag("MainCamera")[0];
		audioDie.transform.position=mainCamera.transform.position;
		audioDie.Play();
		Invoke("StopAudio",0.3f);
		Destroy(gameObject,destroyTime);
	}
	
	
	void StopAudio(){
		if(audioDie.isPlaying){
			audioDie.Stop();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
