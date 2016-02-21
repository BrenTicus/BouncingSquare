using UnityEngine;
using System.Collections;

public class BoomScript : MonoBehaviour {
	private bool isQuitting;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject);
	}


	void OnDestroy()
	{
		if(!isQuitting)
		{
			GameObject gui = GameObject.Find("GameOver");
			gui.SetActive(true);
		}
	}

	void OnApplicationQuit()
	{
		isQuitting = true;
	}
}
