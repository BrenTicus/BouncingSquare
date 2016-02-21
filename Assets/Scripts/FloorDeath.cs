using UnityEngine;
using System.Collections;

public class FloorDeath : MonoBehaviour {
	public GameObject dude;

	void Start()
	{
		dude = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (dude)
		{
			if (dude.transform.position.x - transform.position.x > 8)
			{
				Destroy(gameObject);
			}
		}
	}
}
