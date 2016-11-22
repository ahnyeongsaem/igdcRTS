using UnityEngine;
using System.Collections;

public class gameinitbuildingandunit : MonoBehaviour {

	GameObject mybuilding;
	GameObject enemybuilding;
	// Use this for initialization
	void Start () {
		mybuilding=Instantiate(allunitinformation.getGameObject("building_castle"));
		mybuilding.transform.position = new Vector3 (73, 0, 30);
		mybuilding.GetComponent<unitclass> ().trainunitobject= allunitinformation.getGameObject ("unit_warrior1");
		enemybuilding = Instantiate (allunitinformation.getGameObject ("building_castle"));
		enemybuilding.transform.position = new Vector3 (73, 0, 75);
		enemybuilding.GetComponent<unitclass> ().trainunitobject= allunitinformation.getGameObject ("building_castle");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
