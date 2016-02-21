using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

	void Start()
	{
		gameObject.GetComponent<Animation>().wrapMode = WrapMode.PingPong;
	}
}
