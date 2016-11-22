using UnityEngine;
using System.Collections;

public class heroclass : MonoBehaviour {

	public delegate void heroskill(int mana);
	public heroskill skill =null;
	// Use this for initialization
	void Start () {
		skill += new heroskill (test1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void test1(int mana)
	{
		Debug.Log ("dfdf");
	}

}
