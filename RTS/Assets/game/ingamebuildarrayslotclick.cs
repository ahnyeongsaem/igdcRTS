using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ingamebuildarrayslotclick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void ingamebuildslotclick()
    {
        int idx = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        GameObject.Find("Main Camera").GetComponent<gamebuild>().setting(playerinformation.buildinforarray[idx].getgameobjectmodel(), new Vector2(2, 2));
    }
    // Update is called once per frame
    void Update () {
	
	}
}
