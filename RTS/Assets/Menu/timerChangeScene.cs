using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class timerChangeScene : MonoBehaviour {

    public float timer = 3f;

	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if(timer<=0)
        {
            SceneManager.LoadScene("game");
        }
	}
}
