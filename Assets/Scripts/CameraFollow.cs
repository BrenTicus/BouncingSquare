using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
	public GameObject dude;
	public GameObject dudePrefab;
	public GameObject startFloors;
	public GameObject floors;
	
	// Update is called once per frame
	void Update () 
	{
		if (dude)
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(dude.transform.position.x + 3, transform.position.y, transform.position.z), 1.0f);
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene(0); }
		}
	}

	void restart()
	{
		Destroy(floors);
		floors = Instantiate(startFloors);
		floors.name = "Floors";

		GameObject boom = GameObject.FindGameObjectWithTag("Particles");
		Destroy(boom);

		dude = (GameObject)Instantiate(dudePrefab, new Vector3(0, 0, 0), Quaternion.identity);
		dude.SendMessage("setFloors", floors);
	}

	void quit()
	{
		SceneManager.LoadScene(0);
	}
}
