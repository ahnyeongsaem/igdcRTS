using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class allunitinformation : MonoBehaviour {

	public string BundleURL="https://s3.ap-northeast-2.amazonaws.com/igurs-rts/assetBundle/unit.unity3d";
	public string unitTableURL="https://quuqawenjj.execute-api.us-west-2.amazonaws.com/unitclass";
	public int version = 1;
	static ArrayList Unit=new ArrayList();
	static ArrayList Building=new ArrayList();
	static ArrayList Hero=new ArrayList(); //Hero
	static ArrayList AllUnit=new ArrayList(); //모든 유닛
	static ArrayList allassetinfor=new ArrayList();
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
		((GameObject)gametmp).AddComponent <NavMeshAgent>();
		((GameObject)gametmp).AddComponent <LOSEntity>();

		((GameObject)gametmp).GetComponent<unitclass> ().unitname = (string)unitstatus [i] ["unitname"];
		((GameObject)gametmp).GetComponent<unitclass> ().unittype = 
			(unitclass.Unittype) Enum.Parse(typeof(unitclass.Unittype),(string)unitstatus [i] ["unittype"], true);
		((GameObject)gametmp).GetComponent<unitclass> ().movetype = 
			(unitclass.Movetype) Enum.Parse(typeof(unitclass.Movetype),(string)unitstatus [i] ["movetype"], true);
		((GameObject)gametmp).GetComponent<unitclass> ().attacktarget = 
			(unitclass.Attacktarget) Enum.Parse(typeof(unitclass.Attacktarget),(string)unitstatus [i] ["attacktarget"], true);
		((GameObject)gametmp).GetComponent<unitclass> ().attribute = 
			(unitclass.Attribute) Enum.Parse(typeof(unitclass.Attribute),(string)unitstatus [i] ["attribute"], true);
		((GameObject)gametmp).GetComponent<unitclass> ().unitsize=new Vector2((int)unitstatus [i] ["unitsize"],(int)unitstatus [i] ["unitsize"]);
		((GameObject)gametmp).GetComponent<unitclass> ().maxhp = (int)unitstatus [i] ["maxhp"];
		((GameObject)gametmp).GetComponent<unitclass> ().maxdmg = (int)unitstatus [i] ["maxdmg"];
		((GameObject)gametmp).GetComponent<unitclass> ().maxspeed = Convert.ToSingle(unitstatus [i] ["maxspeed"]);
		((GameObject)gametmp).GetComponent<unitclass> ().attackrange = Convert.ToSingle(unitstatus [i] ["attackrange"]);
		((GameObject)gametmp).GetComponent<unitclass> ().attackspeed = Convert.ToSingle(unitstatus [i] ["attackspeed"]);
		((GameObject)gametmp).GetComponent<unitclass> ().misalespeed = Convert.ToSingle(unitstatus [i] ["misalespeed"]);
		((GameObject)gametmp).GetComponent<unitclass> ().attacksplashrange = Convert.ToSingle(unitstatus [i] ["attacksplashrange"]);
		((GameObject)gametmp).GetComponent<unitclass> ().maxtrainingspeed = Convert.ToSingle(unitstatus [i] ["maxtrainingspeed"]);
		((GameObject)gametmp).GetComponent<unitclass> ().sightrange = Convert.ToSingle(unitstatus [i] ["sightrange"]);

        
		//TODO : misaleobject particle trainunitobject
	}

	///debug all unit asset
	void checkunitinstiate()
	{
		Debug.Log ("unit ins");
		for (int i = 0; i < AllUnit.Count; i++)
			Instantiate ((GameObject)AllUnit [i]);
	}

}
