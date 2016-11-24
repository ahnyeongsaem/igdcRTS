using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour {

	public void SceneChange1()
    {
        SceneManager.LoadScene("main");
    }
    public void SceneChange2()
    {
        SceneManager.LoadScene("main-shop");
    }
    public void SceneChange3()
    {
        SceneManager.LoadScene("main-option");
    }
    public void SceneChange4()
    {
        SceneManager.LoadScene("main-tech");
    }
    public void SceneChange5()
    {
        SceneManager.LoadScene("main-inven");
    }
    public void SceneChange6()
    {
        SceneManager.LoadScene("game");
    }

}
