using UnityEngine;
using System.Collections;

public class gameui : MonoBehaviour {

	public GameObject buildingtest;
	// Use this for initialization
	void Start () {
		Screen.SetResolution (720, 1280, true);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void buildclicktest()
	{
		GetComponent<gamebuild> ().setting (buildingtest, new Vector2 (2, 3));
	}
}
