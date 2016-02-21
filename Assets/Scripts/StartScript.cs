using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour {
	public UnityEngine.UI.Text highscore;

	// Use this for initialization
	void Start () {
		if(!PlayerPrefs.HasKey("highscore"))
		{
			PlayerPrefs.SetInt("highscore", 0);
		}
		if (!highscore) highscore = GameObject.Find("Highscore").GetComponent<UnityEngine.UI.Text>();
		highscore.text += PlayerPrefs.GetInt("highscore").ToString();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}
	
	// Update is called once per frame
	void PlayGame () {
		SceneManager.LoadScene(1);
	}
}
