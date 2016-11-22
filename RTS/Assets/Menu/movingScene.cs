using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class movingScene: MonoBehaviour {

    public float num;

    public void MovingScene(int num)
    {
        SceneManager.LoadScene(num);
    }
}
