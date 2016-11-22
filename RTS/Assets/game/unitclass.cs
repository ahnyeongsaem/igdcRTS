using UnityEngine;
using System.Collections;


public class unitclass : MonoBehaviour {
	public string unitname;
	public enum Unittype{
		unit,hero,building
	}
	public enum Movetype{
		ground,sky
	}
	public enum Attacktarget{
		all, ground, building
		//TODO : if not attack object attacktarget none
	}

	public enum Attribute{
		none, fire, water, wind
	}
	/// building,unit,hero
	public Unittype unittype;
	/// ground,sky
	public Movetype movetype;

	/// all,sky,ground,building
	public Attacktarget attacktarget;
	/// none, fire, water, wind
	public Attribute attribute;

	public Vector2 unitsize=new Vector2(1,1);

	public int maxhp;
	int hp;
	public int maxdmg;
	int dmg;
	public float maxspeed;
	float speed;
	public float attackrange;
	public float attackspeed;
	float attackspeednow=0;
	public float misalespeed;
	public float attacksplashrange=0;
	public float maxtrainingspeed;
	public float sightrange=5;
	float trainingspeed;
	/// <summary>trainingspeed-=nowtime </summary>
	float nowtrainingtime=0;

	public int card=0;
	public int level=0;

	public GameObject misaleobject;
	public GameObject trainunitobject;
	NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		gameinit ();
	}

	public void gameinit()
	{
		hp = (int)(maxhp * (1 + 0.05f * level));
		dmg = (int)(maxdmg * (1 + 0.05f * level));
		speed = maxspeed;

		trainingspeed = maxtrainingspeed * (1 - 0.05f * level);
		if (speed != 0) {
			agent = GetComponent<NavMeshAgent> ();

			agent.speed = speed;
			agent.angularSpeed = 180;
			agent.acceleration = 20;
		}
		attackspeednow = attackspeed;
	}

	// Update is called once per frame
	void Update () {
	

		//train(if building)

		if (trainingspeed != 0) {
			nowtrainingtime += Time.deltaTime;
			if (nowtrainingtime > trainingspeed) {
				nowtrainingtime = 0;

                //자기 객체의 생산속도는 자기가 건물일때 생산속도지 유닛의 생산속도가 아닙니다. 유닛일경우 trainingspeed를 0으로 해주세요.
				if (tag == "PLAYER")
					trainunitobject.tag = "PLAYER";
				else
					trainunitobject.tag = "ENEMY";
				Instantiate<GameObject> (trainunitobject).transform.position=transform.position;
			}
		}

		//hero(if hero)




		//search
		int searchenemy=0; //if nearbyenemy sightrange 
		GameObject[] enemys;
		if (tag == "PLAYER") {
			enemys = GameObject.FindGameObjectsWithTag ("ENEMY");
			GetComponent<LOSEntity> ().IsRevealer = true;
		} else {
			enemys = GameObject.FindGameObjectsWithTag ("PLAYER");
			GetComponent<LOSEntity> ().IsRevealer = false;
		}
		GameObject targetenemy=null;
		float mindis = sightrange;
		for (int i = 0; i < enemys.Length; i++) {
			if (attacktarget == Attacktarget.building) {
				if (enemys [i].GetComponent<unitclass> ().unittype != Unittype.building) {
					continue;
				} 
			} 
			else if (attacktarget == Attacktarget.ground) {
				if (enemys [i].GetComponent<unitclass> ().movetype != Movetype.ground) {
					continue;
				}
			}
			//if all not continue

			if (mindis > Vector3.Distance (enemys [i].transform.position, transform.position)) {
				mindis=Vector3.Distance (enemys [i].transform.position, transform.position);
				targetenemy = enemys [i];
				searchenemy++;
			}

		}
		//attack(if enemy closed attackrange)
		if (targetenemy != null && mindis<attackrange) {
			if(speed!=0) agent.Stop();
			//attackanimation need
			attackspeednow+=Time.deltaTime;
			if(attackspeednow>0.2f && GetComponent<Animator> () != null) 
				GetComponent<Animator>().SetInteger("animation",0);
			if (attackspeednow > attackspeed) {
				attackspeednow = 0;

				if (misaleobject == null) {
					Debug.Log ("no misale object");
				} else {

					if (GetComponent<Animator> () != null) {
						//animatorneed
						GetComponent<Animator>().SetInteger("animation",2);

					}

					GameObject tmpmisale = Instantiate<GameObject> (misaleobject);
					tmpmisale.transform.position = transform.position;
					if (tag == "player")
						tmpmisale.GetComponent<misaleclass> ().misaleinit (dmg, targetenemy, misalespeed, attacksplashrange, attacktarget, true, attribute);
					else
						tmpmisale.GetComponent<misaleclass> ().misaleinit (dmg, targetenemy, misalespeed, attacksplashrange, attacktarget, false, attribute);
				}
			}


		}
		else if (searchenemy != 0) {
			
			if (targetenemy != null) {
				if (speed != 0) {
					agent.SetDestination (targetenemy.transform.position);

					agent.Resume ();
				}
			}
	
		}
			
		//move (if not attack)
		else if(speed!=0)
		{
			GameObject destflag;
			if (tag == "PLAYER")
				destflag = GameObject.Find ("destflag");
			else {
				destflag = GameObject.Find ("enemydestflag");
				///////////////////////////
				/// enemy flag need
				/// ///////////////////////////
			}
			if (destflag != null) {
				agent.SetDestination (destflag.transform.position);
				agent.Resume ();
				if (GetComponent<Animator> () != null) {
					//animatorneed
					GetComponent<Animator>().SetInteger("animation",1);
				}
			}
			
		}


	}

	public void ApplyDamege(int damege)
	{
		hp -= damege;
		Debug.Log (name + "dameged " + damege + " now hp:" + hp);
		if (hp < 0) {
			//animation after destroy
			if (GetComponent<Animator> () != null) {
				//animatorneed
				GetComponent<Animator>().SetInteger("animation",4);
			}
			Destroy (gameObject, 1f);
			//destroy 1secondafter
		}

	}
	public void ApplyHealing(int heal)
	{
		hp += heal;
		if (hp > (int)(maxhp * (1 + 0.05f * level))) {
			hp = (int)(maxhp * (1 + 0.05f * level));
		}
	}
}
