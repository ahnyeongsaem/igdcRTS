using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class allunitinformation : MonoBehaviour {

	public string BundleURL="https://s3.ap-northeast-2.amazonaws.com/igurs-rts/assetBundle/unit.unity3d";
	public string unitTableURL="https://quuqawenjj.execute-api.us-west-2.amazonaws.com/unitclass";
	public int version = 1;
	static public List<GameObject> Unit=new List<GameObject>();
	static public List<GameObject> Building =new List<GameObject>();
	static public List<GameObject> Hero =new List<GameObject>(); //Hero
	static public List<GameObject> AllUnit =new List<GameObject>(); //모든 유닛
	public static GameObject gametmp;

    public const int STATUS_NOT_START = 0;
    public const int STATUS_ALLUNITINFOR_END = 5;

    public const int STATUS_FINISH_WORK = 100;

    static public int status = STATUS_NOT_START;
    

    static Dictionary<string,object>[] unitstatus;

	void Start()
	{
		allunitsetting ();
	}

	public static GameObject getGameObject(string unitname)
	{
		for (int i = 0; i < Unit.Count; i++) {
			if (Unit[i].GetComponent<unitclass> ().unitname == unitname) {
				return Unit[i];
			}
		}
		for (int i = 0; i < Building.Count; i++) {
			if (Building[i].GetComponent<unitclass> ().unitname == unitname) {
				return Unit[i];
			}
		}
		for (int i = 0; i < Hero.Count; i++) {
			if (Hero[i].GetComponent<unitclass> ().unitname == unitname) {
				return Unit[i];
			}
		}
		return null;
	}
	/// unitclass check and make
	public void allunitsetting()
	{
		GET(unitTableURL);
        
	}
	public WWW GET(string url)
	{

		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www));
		return www; 
	}
	private IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
        

		// check for errors
		if (www.error == null)
		{
			//Debug.Log("WWW Ok!: " + www.text);
			string wwwstring = www.text;//.Replace("//","");
			//Debug.Log("Parsing : " +wwwstring);
			Dictionary<string, object> dicjson;

			dicjson = jsonf.Read (wwwstring);


			//Debug.Log (""+dicjson);
			Debug.Log ((string)dicjson ["body"]);
			Dictionary<string,object> aa;
			aa = jsonf.Read((string)dicjson["body"]);
			//Debug.Log ((string)aa["Items"]);
			unitstatus=(Dictionary<string,object>[])aa["Items"];

			

			Debug.Log("unit infor down complete -> assetbundle down start");

            status = STATUS_ALLUNITINFOR_END;
			StartCoroutine(DownloadAndCache());


		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}

	IEnumerator DownloadAndCache()
	{
        // Wait for the Caching system to be ready
        Caching.CleanCache();
        while (!Caching.ready)
			yield return null;

        // Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache


 
        using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL, 1))
		{
			yield return www;
			if (www.error != null)
				throw new Exception("WWW download had an error:" + www.error);
			AssetBundle bundle = www.assetBundle;



			for (int i = 0; i < unitstatus.Length; i++) {
				if (unitstatus [i] ["unitname"] == null) {
                    Debug.LogError(i + "unitname is null");
					//TODO : if unitname =null
				} else {
                    //gametmp = (GameObject)(bundle.LoadAsset (unitstatus [i] ["unitname"].ToString ()));
                    string tmpsssss="";
                    for(int j=0;j<bundle.GetAllAssetNames().Length;j++)
                    {
                        tmpsssss += bundle.GetAllAssetNames()[j];
                    }

                    AssetBundleRequest abr= bundle.LoadAssetAsync(unitstatus[i]["unitname"].ToString(),typeof(GameObject));
                    yield return abr;
                    gametmp = abr.asset as GameObject;

                    copyallunitclass(i);
					AllUnit.Add (gametmp);



					if (unitstatus [i] ["unitname"].ToString ().IndexOf ("unit") != -1) {
						Unit.Add(AllUnit[i]);
					} else if (unitstatus [i] ["unitname"].ToString ().IndexOf ("building") != -1) {
						Building.Add(AllUnit[i]);
					} else if (unitstatus [i] ["unitname"].ToString ().IndexOf ("hero") != -1) {
						Hero.Add(AllUnit[i]);
					} 
					else {
						Debug.LogError ("whatobject not hero,unit,building ="+ unitstatus[i]["unitname"].ToString());

					}

				}

                //end for i
			}
            for(int i=0;i<AllUnit.Count;i++)
            {
                if(AllUnit[i].GetComponent<unitclass>().trainunitobjectname!=null &&
                    AllUnit[i].GetComponent<unitclass>().trainunitobjectname.Length >2)
                {
                    string tmp = AllUnit[i].GetComponent<unitclass>().trainunitobjectname;
                    int j;
                    for (j=0;j<Unit.Count;j++)
                    {
                        if(tmp.Equals(Unit[j].GetComponent<unitclass>().unitname))
                        {
                            AllUnit[i].GetComponent<unitclass>().trainunitobject = Unit[j];
                            break;
                        }
                        
                    }
                    if(j==Unit.Count && Unit.Count!=0)
                    {
                        Debug.LogError("trainunit Allunit" + i + "," + tmp + "not have in unit[]");
                    }
                }
            }
            
            
			bundle.Unload (false);
            www.Dispose();


            Debug.Log("assetdown finish");
            status = STATUS_FINISH_WORK;
        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
        
//		copyallunitclass ();
		checkunitinstiate (); //TODO : DELETE
	}
	void copyallunitclass(int i)
	{

        if(gametmp==null)
        {
            Debug.LogError("nullpointexception object indexnumber = "+i+"unitname = "+ (string)unitstatus[i]["unitname"]);
            return;
        }
        gametmp.tag = "Untagged";
		gametmp.AddComponent<unitclass>();
		
		gametmp.AddComponent <LOSEntity>();

		gametmp.name=gametmp.GetComponent<unitclass> ().unitname = (string)unitstatus [i] ["unitname"];
		gametmp.GetComponent<unitclass> ().unittype = 
			(unitclass.Unittype) Enum.Parse(typeof(unitclass.Unittype),(string)unitstatus [i] ["unittype"], true);

        if(gametmp.GetComponent<unitclass>().unittype==unitclass.Unittype.building)
        {
            gametmp.AddComponent<NavMeshObstacle>();
        }
        else
        {
            gametmp.AddComponent<NavMeshAgent>();
        }
        
		gametmp.GetComponent<unitclass> ().movetype = 
			(unitclass.Movetype) Enum.Parse(typeof(unitclass.Movetype),(string)unitstatus [i] ["movetype"], true);
		gametmp.GetComponent<unitclass> ().attacktarget = 
			(unitclass.Attacktarget) Enum.Parse(typeof(unitclass.Attacktarget),(string)unitstatus [i] ["attacktarget"], true);
		gametmp.GetComponent<unitclass> ().attribute = 
			(unitclass.Attribute) Enum.Parse(typeof(unitclass.Attribute),(string)unitstatus [i] ["attribute"], true);
		gametmp.GetComponent<unitclass> ().unitsize=new Vector2((int)unitstatus [i] ["unitsize"],(int)unitstatus [i] ["unitsize"]);
		gametmp.GetComponent<unitclass> ().maxhp = (int)unitstatus [i] ["maxhp"];
		gametmp.GetComponent<unitclass> ().maxdmg = (int)unitstatus [i] ["maxdmg"];
		gametmp.GetComponent<unitclass> ().maxspeed = Convert.ToSingle(unitstatus [i] ["maxspeed"]);
		gametmp.GetComponent<unitclass> ().attackrange = Convert.ToSingle(unitstatus [i] ["attackrange"]);
		gametmp.GetComponent<unitclass> ().attackspeed = Convert.ToSingle(unitstatus [i] ["attackspeed"]);
		gametmp.GetComponent<unitclass> ().misalespeed = Convert.ToSingle(unitstatus [i] ["misalespeed"]);
		gametmp.GetComponent<unitclass> ().attacksplashrange = Convert.ToSingle(unitstatus [i] ["attacksplashrange"]);
		gametmp.GetComponent<unitclass> ().maxtrainingspeed = Convert.ToSingle(unitstatus [i] ["maxtrainingspeed"]);
		gametmp.GetComponent<unitclass> ().sightrange = Convert.ToSingle(unitstatus [i] ["sightrange"]);
        gametmp.GetComponent<unitclass> ().tier = (int)unitstatus[i]["tier"];
        if (unitstatus[i].ContainsKey("icon") && unitstatus[i].ContainsKey("icon").ToString().Length>2)
        {
            gametmp.GetComponent<unitclass>().icon = Resources.Load<Sprite>("icon/" + unitstatus[i]["icon"]);
        }
        else{
            Debug.Log("aws table not have icon" + unitstatus[i]["unitname"]);
        }
        if (unitstatus[i].ContainsKey("misaleobject") && unitstatus[i].ContainsKey("misaleobject").ToString().Length>2)
        {
            gametmp.GetComponent<unitclass>().misaleobject = Resources.Load("misale/" + unitstatus[i]["misaleobject"],typeof(GameObject)) as GameObject;
        }
        else{
            Debug.Log("aws table not have misaleobject" + unitstatus[i]["unitname"]);
        }
        if (unitstatus[i].ContainsKey("trainunitobject") && unitstatus[i].ContainsKey("trainunitobject").ToString().Length>2)
        {
            gametmp.GetComponent<unitclass>().trainunitobjectname = (string)unitstatus[i]["trainunitobject"];
        }
        else{
            Debug.Log("aws table not have trainunitobject" + unitstatus[i]["unitname"]);
            gametmp.GetComponent<unitclass>().trainunitobjectname = null;
        }

        
    }

    ///debug all unit asset
    void checkunitinstiate()
	{
		Debug.Log ("unit ins");
		for (int i = 0; i < AllUnit.Count; i++)
			Instantiate (AllUnit [i]);
	}

}
