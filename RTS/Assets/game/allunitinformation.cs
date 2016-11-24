using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class allunitinformation : MonoBehaviour {

	public string BundleURL="https://s3.ap-northeast-2.amazonaws.com/igurs-rts/assetBundle/unit.unity3d";
	public string unitTableURL="https://quuqawenjj.execute-api.us-west-2.amazonaws.com/unitclass";
	public int version = 1;
	static public ArrayList Unit=new ArrayList();
	static public ArrayList Building =new ArrayList();
	static public ArrayList Hero =new ArrayList(); //Hero
	static public ArrayList AllUnit =new ArrayList(); //모든 유닛
	public static GameObject gametmp;


	static Dictionary<string,object>[] unitstatus;

	void Start()
	{
		allunitsetting ();
	}

	public static GameObject getGameObject(string unitname)
	{
		for (int i = 0; i < Unit.Count; i++) {
			if (((GameObject)Unit[i]).GetComponent<unitclass> ().unitname == unitname) {
				return ((GameObject)Unit[i]);
			}
		}
		for (int i = 0; i < Building.Count; i++) {
			if (((GameObject)Building[i]).GetComponent<unitclass> ().unitname == unitname) {
				return ((GameObject)Unit[i]);
			}
		}
		for (int i = 0; i < Hero.Count; i++) {
			if (((GameObject)Hero[i]).GetComponent<unitclass> ().unitname == unitname) {
				return ((GameObject)Unit[i]);
			}
		}
		return null;
	}
	/// unitclass check and make
	public void allunitsetting()
	{
		WWW results = GET(unitTableURL);
        
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
			Debug.Log("WWW Ok!: " + www.text);
			string wwwstring = www.text;//.Replace("//","");
			Debug.Log("Parsing : " +wwwstring);
			Dictionary<string, object> dicjson;

			dicjson = jsonf.Read (wwwstring);


			Debug.Log (""+dicjson);
			Debug.Log ((string)dicjson ["body"]);
			Dictionary<string,object> aa;
			aa = jsonf.Read((string)dicjson["body"]);
			//Debug.Log ((string)aa["Items"]);
			unitstatus=(Dictionary<string,object>[])aa["Items"];

			

			Debug.Log("userstatus down complete -> assetbundle down start");
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
					//TODO : if unitname =null
				} else {
                    //gametmp = (GameObject)(bundle.LoadAsset (unitstatus [i] ["unitname"].ToString ()));
                    string tmpsssss="";
                    for(int j=0;j<bundle.GetAllAssetNames().Length;j++)
                    {
                        tmpsssss += bundle.GetAllAssetNames()[j];
                    }
                    Debug.Log(tmpsssss);
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
						Debug.LogError ("whatobject");
						//TODO : error
					}

				}
			
				// Unload the AssetBundles compressed contents to conserve memory

			}
			bundle.Unload (false);
            www.Dispose();
		} // memory is freed from the web stream (www.Dispose() gets called implicitly)
        
//		copyallunitclass ();
		checkunitinstiate ();
	}
	void copyallunitclass(int i)
	{

        if(gametmp==null)
        {
            Debug.LogError("nullpointexception object indexnumber = "+i+"unitname = "+ (string)unitstatus[i]["unitname"]);
            return;
        }
		gametmp.AddComponent<unitclass>();
		gametmp.AddComponent <NavMeshAgent>();
		gametmp.AddComponent <LOSEntity>();

		gametmp.GetComponent<unitclass> ().unitname = (string)unitstatus [i] ["unitname"];
		gametmp.GetComponent<unitclass> ().unittype = 
			(unitclass.Unittype) Enum.Parse(typeof(unitclass.Unittype),(string)unitstatus [i] ["unittype"], true);
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
        if (unitstatus[i].ContainsKey("icon"))
        {
            gametmp.GetComponent<unitclass>().icon = Resources.Load<Sprite>("icon/" + unitstatus[i]["icon"]);
        }
        else
        {
            Debug.Log("aws table not have icon" + unitstatus[i]["unitname"]);
        }
        if (unitstatus[i].ContainsKey("misaleobject"))
        {
            gametmp.GetComponent<unitclass>().misaleobject = Resources.Load("misale/" + unitstatus[i]["misaleobject"],typeof(GameObject)) as GameObject;
        }
        else
        {
            Debug.Log("aws table not have icon" + unitstatus[i]["unitname"]);
        }
        //TODO : trainunitobject
    }

    ///debug all unit asset
    void checkunitinstiate()
	{
		Debug.Log ("unit ins");
		for (int i = 0; i < AllUnit.Count; i++)
			Instantiate ((GameObject)AllUnit [i]);
	}

}
