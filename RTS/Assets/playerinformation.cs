using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerinformation : MonoBehaviour
{
    public string playerinformationtableurl= "https://yhb1l3dykd.execute-api.us-west-2.amazonaws.com/playerinformation";
    public string playerheroinformationtableurl="https://qwy8gf6zvj.execute-api.us-west-2.amazonaws.com/playerheroinformation";
    public string playerbuildinginfromationtableurl = "https://vqeazgbatk.execute-api.us-west-2.amazonaws.com/playerbuildinginformation";
    //유저의 정보 클래스
    static public int cash;
    static public int gold;
    static public string playerid="saemy90";
    static public string nickname;


    static public int status=STATUS_NOT_START;

    public const int STATUS_NOT_START = 0;
    public const int STATUS_PLAYERINFOR_END = 5;

    public const int STATUS_HEROINFOR_START = 10;
    public const int STATUS_HEROINFOR_END = 11;

    public const int STATUS_BUILDINGINFOR_START = 12;
    public const int STATUS_BUILDINGINFOR_END = 13;

    /// <summary>
    /// player가 가지고 있는 건물 정보(buildinformation)들의 배열(buildinforarray) 클래스
    /// </summary>
    static public List<buildinformation> buildinforarray=new List<buildinformation>();
    public class buildinformation
    {
        public string name;
        public int level;
        public int count;
        public bool indeck;
        public buildinformation(string _name, int _level, int _count, bool _indeck)
        {
            name = _name; level = _level; count = _count; indeck = _indeck;
        }
        public GameObject getgameobjectmodel()
        {
            GameObject tmp;
            foreach (GameObject i in allunitinformation.Building)
            {
                if (i.name.Equals(name))
                {
                    tmp = i;

                    return tmp;
                }
            }
            Debug.LogError("allinfromation.building not have " + name);
            return null;
        }
        public GameObject getgameobjecttrainingunitmodel()
        {
            GameObject tmp;
            foreach (GameObject i in allunitinformation.Building)
            {
                if (i.name.Equals(name))
                {
                    tmp = i.GetComponent<unitclass>().trainunitobject;

                    return tmp;
                }
            }
            Debug.LogError("allinfromation.building not have " + name);
            return null;
        }
        public Sprite getspritebuildmodel()
        {
            Sprite tmp;
            foreach (GameObject i in allunitinformation.Building)
            {
                if (i.name.Equals(name))
                {
                    tmp = i.GetComponent<unitclass>().icon;

                    return tmp;
                }
            }
            Debug.LogError("allinfromation.building not have " + name);
            return null;
        }
    }

    /// <summary>
    /// player가 가지고 있는 영웅 정보(heroinfromation)들의 배열(buildinforarray) 클래스
    /// </summary>
    static public List<heroinformation> heroinforarray = new List<heroinformation>();
    public class heroinformation
    {
        public string name;
        public int level;
        public int totalexp; //경험치
        public int grade; //영웅 합칠경우
        public int decknumber; //0이면 안들어가있는것
        public heroinformation(string _name, int _level, int _totalexp, int _grade, int _decknumber)
        {
            name = _name; level = _level; totalexp = _totalexp; grade = _grade; decknumber = _decknumber;
        }
        /// <summary>
        /// getgameobjectmodel only model(not change information)
        /// </summary>
        /// <returns></returns>
        public GameObject getgameobjectmodel()
        {
            GameObject tmp;
            foreach(GameObject i in allunitinformation.Hero )
            {
                if(i.name.Equals(name))
                {
                    tmp = i;

                    return tmp;
                }
            }
            Debug.LogError("allinfromation.hero not have "+name);
            return null;
        }

        public Sprite getspriteheromodel()
        {
            Sprite tmp;
            foreach (GameObject i in allunitinformation.Hero)
            {
                if (i.name.Equals(name))
                {
                    tmp = i.GetComponent<unitclass>().icon;

                    return tmp;
                }
            }
            Debug.LogError("allinfromation.hero not have " + name);
            return null;
        }

    }
    /// <summary>
    /// only dummy TODO : 나중에 인터넷다운로드로 대체 필요
    /// </summary>
    void dummyinit()
    {
        buildinforarray.Add(new buildinformation("building_castle",1,1,true));
        heroinforarray.Add(new heroinformation("hero_bladeGirl", 1, 0, 0, 0));
        heroinforarray.Add(new heroinformation("hero_bladeMan", 1, 0, 0, 0));
    }


    private IEnumerator playerinfordownload(string url)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("playerid", playerid);
        string data = jsonf.Write(dic);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "text/json");
        header.Add("Content-Length", ""+data.Length);
        var encoding = new System.Text.UTF8Encoding();
        WWW www = new WWW(url,encoding.GetBytes(data),header);
        
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
            string wwwstring = www.text;//.Replace("//","");
            Debug.Log("Parsing : " + wwwstring);
            Dictionary<string, object> dicjson;
            dicjson = jsonf.Read(wwwstring);
            Debug.Log("" + dicjson);

            if(int.Parse(dicjson["Count"].ToString())==0)
            {
                Debug.LogError("todo : error in count 0");
            }
            
            Dictionary<string,object>[] playerinformationtable = (Dictionary<string, object>[])dicjson["Items"];
            cash = (int)playerinformationtable[0]["cash"];
            gold = (int)playerinformationtable[0]["gold"];
            nickname = (string)playerinformationtable[0]["nickname"];


            status = STATUS_PLAYERINFOR_END;
        }
        else
        {
            Debug.LogError("WWW Error: " + www.error);
        }
    }
    private IEnumerator playerheroinfordownload(string url)
    {
        status = STATUS_HEROINFOR_START;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("playerid", playerid);
        string data = jsonf.Write(dic);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "text/json");
        header.Add("Content-Length", "" + data.Length);
        var encoding = new System.Text.UTF8Encoding();
        WWW www = new WWW(url, encoding.GetBytes(data), header);

        yield return www;

        // check for errors
        if (www.error == null)
        {
            //Debug.Log("WWW Ok!: " + www.text);
            string wwwstring = www.text;//.Replace("//","");
            Debug.Log("Parsing : " + wwwstring);
            Dictionary<string, object> dicjson;
            dicjson = jsonf.Read(wwwstring);
            Debug.Log("" + dicjson);

            if (int.Parse(dicjson["Count"].ToString()) == 0)
            {
                Debug.LogError("todo : error in count 0");
            }

            Dictionary<string, object>[] playerheroinformationtable = (Dictionary<string, object>[])dicjson["Items"];
            
            for(int i=0;i<playerheroinformationtable.Length;i++)
            {
                heroinforarray.Add(new heroinformation(
                    (string)playerheroinformationtable[i]["name"],
                    (int)playerheroinformationtable[i]["level"],
                    (int)playerheroinformationtable[i]["totalexp"],
                    (int)playerheroinformationtable[i]["grade"],
                    (int)playerheroinformationtable[i]["decknumber"]));
            }
            
            //cash = (int)playerinformationtable[0]["cash"];
            //gold = (int)playerinformationtable[0]["gold"];
            //nickname = (string)playerinformationtable[0]["nickname"];


            status = STATUS_HEROINFOR_END;
            StartCoroutine(playerbuildinginfordownload(playerbuildinginfromationtableurl));
        }
        else
        {
            Debug.LogError("WWW Error: " + www.error);
        }
    }
    private IEnumerator playerbuildinginfordownload(string url)
    {
        status = STATUS_BUILDINGINFOR_START;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("playerid", playerid);
        string data = jsonf.Write(dic);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "text/json");
        header.Add("Content-Length", "" + data.Length);
        var encoding = new System.Text.UTF8Encoding();
        WWW www = new WWW(url, encoding.GetBytes(data), header);

        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
            string wwwstring = www.text;//.Replace("//","");
            Debug.Log("Parsing : " + wwwstring);
            Dictionary<string, object> dicjson;
            dicjson = jsonf.Read(wwwstring);
            Debug.Log("" + dicjson);

            if (int.Parse(dicjson["Count"].ToString()) == 0)
            {
                Debug.LogError("todo : error in count 0");
            }

            Dictionary<string, object>[] playerbuildinginformationtable = (Dictionary<string, object>[])dicjson["Items"];

            for (int i = 0; i < playerbuildinginformationtable.Length; i++)
            {
                buildinforarray.Add(new buildinformation(
                    (string)playerbuildinginformationtable[i]["name"],
                    (int)playerbuildinginformationtable[i]["level"],
                    (int)playerbuildinginformationtable[i]["count"],
                    (bool)playerbuildinginformationtable[i]["indeck"]));
            }

            //cash = (int)playerinformationtable[0]["cash"];
            //gold = (int)playerinformationtable[0]["gold"];
            //nickname = (string)playerinformationtable[0]["nickname"];


            status = STATUS_BUILDINGINFOR_END;
            Debug.Log("building infor end");
        }
        else
        {
            Debug.LogError("WWW Error: " + www.error);
        }
    }


    // Use this for initialization
    void Start()
    {
        StartCoroutine(playerinfordownload(playerinformationtableurl));
       // dummyinit(); //TODO :delete need
    }

    // Update is called once per frame
    void Update()
    {
        if(allunitinformation.status==allunitinformation.STATUS_FINISH_WORK && status==STATUS_PLAYERINFOR_END)
        {
            status = STATUS_HEROINFOR_START;
            StartCoroutine(playerheroinfordownload(playerheroinformationtableurl));
        }
    }
}
