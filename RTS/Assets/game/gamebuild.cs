using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class gamebuild : MonoBehaviour {

    public const int PLAYER_UNIT_COUNT_MAX = 50;
    public const int ENEMY_UNIT_COUNT_MAX = 50;
    public static int playerunitcount = 0;
    public static int enemyunitcount = 0;

	public int[,] gametile=new int[26,42]; //0이면 빈칸 1이면 차있는칸 2면 자원
	GameObject buildobject=null;
	public GameObject greentile;

	public GameObject[,] buildtile=null; //x,y
	Vector2 buildsize=new Vector2();

	public GameObject destflag;
	public GameObject enemydestflag;
	public Vector3 destflagstartxy;
	static public Vector3 enemydestflagstartxy =new Vector3(15,0,15);
	// Use this for initialization
	void Start () {
		for (int i = 0; i < gametile.GetLength (0); i++) {
			for (int j = 0; j < gametile.GetLength (1); j++) {
				gametile [i, j] = 0;
			}
		}
		destflag=Instantiate<GameObject> (destflag);
		destflag.transform.position = destflagstartxy;
		destflag.name = "destflag";
		enemydestflag=Instantiate<GameObject> (enemydestflag);
		enemydestflag.transform.position = enemydestflagstartxy;
		enemydestflag.name = "enemydestflag";
	}
	
	// Update is called once per frame
	public void setting(GameObject bdobject, Vector2 bdsize )
	{
		if (buildobject != null) Destroy (buildobject);
		{
			buildobject = Instantiate<GameObject> (bdobject);
			buildobject.GetComponent<NavMeshObstacle> ().enabled = false;
			buildobject.GetComponent<unitclass> ().enabled = false;
		}
		if (buildtile != null) {
			for (int i = 0; i < buildtile.GetLength (0); i++) {
				for (int j = 0; j < buildtile.GetLength (1); j++) {
					if (buildtile [i,j] != null) {
						Destroy (buildtile [i,j]);
					}

				}
			}
			if (buildtile != null)
				buildtile = null;

		}
		buildtile=new GameObject[(int)bdsize.x,(int)bdsize.y];
		buildsize = bdsize;
		for (int i = 0; i < bdsize.x; i++) {
			for (int j = 0; j < bdsize.y; j++) {
				buildtile[i,j]= Instantiate<GameObject> (greentile);
			}
		}


	}

    public static int playerunitcountadd(int tier)
    {
        if(tier==1 && PLAYER_UNIT_COUNT_MAX>=playerunitcount+1)
        {
            playerunitcount += 1;
        }
        else if (tier == 2 && PLAYER_UNIT_COUNT_MAX >= playerunitcount + 2)
        {
            playerunitcount += 2;
        }
        else if (tier == 3 && PLAYER_UNIT_COUNT_MAX >= playerunitcount + 3)
        {
            playerunitcount += 3;
        }
        else if (tier == 3 && PLAYER_UNIT_COUNT_MAX >= playerunitcount + 3)
        {
            playerunitcount += 3;
        }
        else if(tier !=1 && tier !=2 && tier!=3 && tier!=4)
        {
            Debug.Log("max unit count");
            return 0;
        }

        //tier is wrong
        Debug.LogError("tier = " + tier);
        return -1;
    }

	void Update () {


		//Debug.Log ("aa");

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point);
			if (buildobject == null && buildtile==null) {
				if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0) && hit.collider.gameObject.tag == "map"
					&& hit.point.x > 0 && hit.point.z > 0 && hit.point.x < gametile.GetLength (0) && hit.point.z < gametile.GetLength (1)) 
				{
					destflag.transform.position = new Vector3 (hit.point.x,0,hit.point.z);
				}
			} 
			else {
				int ctflag = 0; //tile크기여야됨

				if (hit.collider.gameObject.tag == "map" && hit.point.x>0 && hit.point.z>0 && hit.point.x<gametile.GetLength(0) && hit.point.z<gametile.GetLength(1)) {
					buildobject.transform.position = new Vector3 (Mathf.Floor (hit.point.x), 0, Mathf.Floor (hit.point.z));
					//Debug.Log (buildobject.GetComponent<LOSEntity> ().RevealState+" "+LOSEntity.RevealStates.Unfogged);



					for (int i = 0; i < buildtile.GetLength (0); i++) {
						for (int j = 0; j < buildtile.GetLength (1); j++) { //집짓기전

							GameObject[] tmppl = GameObject.FindGameObjectsWithTag ("PLAYER");
							GameObject[] tmpem = GameObject.FindGameObjectsWithTag ("ENEMY");
							int tmpbool = 0;
							for (int k = 0; k < tmppl.Length; k++) {
								if ((int)Mathf.Floor (hit.point.x) + i == Mathf.RoundToInt (tmppl [k].transform.position.x) &&
								    (int)Mathf.Floor (hit.point.z) + j == Mathf.RoundToInt (tmppl [k].transform.position.z)) {
									tmpbool = 1;
								}
							}
							for (int k = 0; k < tmpem.Length; k++) {
								if ((int)Mathf.Floor (hit.point.x) + i == Mathf.RoundToInt (tmpem [k].transform.position.x) &&
								    (int)Mathf.Floor (hit.point.z) + j == Mathf.RoundToInt (tmpem [k].transform.position.z)) {
									tmpbool = 1;
								}
							}

							if (buildobject.GetComponent<LOSEntity> ().RevealState != LOSEntity.RevealStates.Unfogged || gametile [(int)(Mathf.Floor (hit.point.x) + i), (int)(Mathf.Floor (hit.point.z) + j)] == 1) {

								buildtile [i, j].GetComponentInChildren<MeshRenderer> ().material.color = Color.red;
								Debug.Log(""+(int)Mathf.Floor (hit.point.x)+", i is" + i+" "+(int)Mathf.Floor (hit.point.z) + ", j is" + j+" "+gametile [(int)(Mathf.Floor (hit.point.x) + i), (int)(Mathf.Floor (hit.point.z) + j)]
                                    + buildobject.GetComponent<LOSEntity>().RevealState + " ");
							} else if (tmpbool == 1) {
								buildtile [i, j].GetComponentInChildren<MeshRenderer> ().material.color = Color.red;
							}
							else 
							{
								buildtile [i, j].GetComponentInChildren<MeshRenderer> ().material.color = Color.green;
								ctflag +=1;
							}
							buildtile [i, j].transform.position =
								new Vector3 (Mathf.Floor (hit.point.x) + i, 0f, Mathf.Floor (hit.point.z) + j);
						}
					}
                    if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log(EventSystem.current.IsPointerOverGameObject() + " " + ctflag + " "+buildsize.x+ " "+buildsize.y);
                    }
					if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0) && ctflag==(int)buildsize.x*(int)buildsize.y ) {//건물조건
						for (int i = 0; i < buildtile.GetLength (0); i++) {
							for (int j = 0; j < buildtile.GetLength (1); j++) {
								gametile [(int)(Mathf.Floor (hit.point.x) + i), (int)(Mathf.Floor (hit.point.z) + j)] = 1;
								if (buildtile [i,j] != null) {
									Destroy (buildtile [i,j]);
								}
							}
						}
						if (buildtile != null)
							buildtile = null;
						buildobject.tag = "PLAYER";
						buildobject.GetComponent<NavMeshObstacle> ().enabled = true;
						buildobject.GetComponent<LOSEntity> ().IsRevealer =true;
						buildobject.GetComponent<unitclass> ().enabled = true;

						buildobject = null;
						//건물의 기능추가

					}

				}
			}



		}//raycast end





	}
	//update end
}
