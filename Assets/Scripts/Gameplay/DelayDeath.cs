using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDeath : MonoBehaviour {

	public float delay = 0;
	// Use this for initialization
	void Start ()
	{
		Destroy (gameObject, delay);
	}

}
