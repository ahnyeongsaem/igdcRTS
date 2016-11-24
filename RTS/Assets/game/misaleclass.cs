using UnityEngine;
using System.Collections;

public class misaleclass : MonoBehaviour {

	float misalespeed;
	int dmg;
	GameObject targetgo;
	Vector3 targetposition;
	float splashrange;
	unitclass.Attacktarget attacktarget;
	public GameObject particle;
	public bool shooterisplayer;
	unitclass.Attribute shooterattribute;

	/// 0 move 1 dest and applydmg 2 animation and die
	int destycheck=0;

	void Start () {
		
	}
	/// <summary>
	/// Misaleinit the specified damege, target, misalespeed, splashrange and attarget.
	/// </summary>
	/// <param name="damege">Damege.</param>
	/// <param name="tggo">Target. object</param>
	/// <param name="mspeed">if mislae speed=0 immidiate.</param>
	/// <param name="sprange">if splash=0 && misalespeed=0 immidate one target</param>
	/// <param name="attarget">Attack target</param>
	/// <param name="shootisplayer">shooter is player?</param>
	public void misaleinit(int damege, GameObject tggo, float mspeed, float sprange, unitclass.Attacktarget attarget,bool shootisplayer,unitclass.Attribute shootattribute)
	{
		dmg = damege;
		targetgo = tggo;
		misalespeed = mspeed;
		splashrange = sprange;
		attacktarget = attarget;
		shooterisplayer = shootisplayer;
		shooterattribute = shootattribute;

		targetposition = targetgo.transform.position;

		if (mspeed == 0) {
			destycheck = 1;
			transform.position = tggo.transform.position;
		}
	}
	// Update is called once per frame
	void Update () {
		if (destycheck == 0) {
			if (splashrange == 0) {
				//단일타겟일때
				if (targetgo == null) {
					Destroy (gameObject);
					return;
				}

				if (Vector3.Distance (transform.position, targetgo.transform.position)
					-targetgo.GetComponent<unitclass>().unitsize.x< 0.1f) {
					destycheck = 1;
				}
				transform.LookAt (targetgo.transform);
				transform.Translate (Vector3.forward*Time.deltaTime*misalespeed);

			} else {
				//멀티타겟일때
				if (Vector3.Distance (transform.position, targetposition)
					-targetgo.GetComponent<unitclass>().unitsize.x< 0.1f) {
					destycheck = 1;
				}
				transform.LookAt (targetposition);
				transform.Translate (Vector3.forward*Time.deltaTime*misalespeed);
			}


		} else if (destycheck == 1) {
			if (splashrange == 0) {
				if(targetgo!=null)
				targetgo.GetComponent<unitclass> ().ApplyDamege (dmg);
			} 
			else {
				GameObject[] enemys=null;
				if (shooterisplayer == true) {
					enemys = GameObject.FindGameObjectsWithTag ("ENEMY");
				} else {
					enemys = GameObject.FindGameObjectsWithTag ("PLAYER");
				}
				if (enemys == null) {
					for (int i = 0; i < enemys.Length; i++) {
						if (Vector3.Distance (transform.position, enemys [i].transform.position)
							-enemys[i].GetComponent<unitclass>().unitsize.x < splashrange) {
							enemys [i].GetComponent<unitclass> ().ApplyDamege (dmg);
						}
					}
				}
			}
			destycheck = 2;
		} 
		else {
			if (particle != null) {
				Destroy(Instantiate (particle, transform.position, transform.rotation),1);
			}
			Destroy (gameObject);
		}
	}


}
