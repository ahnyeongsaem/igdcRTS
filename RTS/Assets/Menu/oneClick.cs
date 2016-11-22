using UnityEngine;
using System.Collections;

public class oneClick : MonoBehaviour {

	public void SeceLoad()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
