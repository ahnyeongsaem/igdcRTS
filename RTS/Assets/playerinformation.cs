using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerinformation : MonoBehaviour
{
    public string playerinformationtableurl= "https://yhb1l3dykd.execute-api.us-west-2.amazonaws.com/playerinformation";

    //유저의 정보 클래스
    static public int cash;
    static public int gold;
    static public string playerid="saemy90";
    static public string nickname;
    

    /// <summary>
    /// player가 가지고 있는 건물 정보(buildinformation)들의 배열(buildinforarray) 클래스
    /// </summary>
    static public ArrayList buildinforarray=new ArrayList();
    public class buildinformation
    {
        public string name;
        public int level;
        public int count;
        public bool indeck;
        buildinformation(string _name, int _level, int _count, bool _indeck)
        {
            name = _name; level = _level; count = _count; indeck = _indeck;
        }
    }

    /// <summary>
    /// player가 가지고 있는 영웅 정보(heroinfromation)들의 배열(buildinforarray) 클래스
    /// </summary>
    static public ArrayList heroinforarray = new ArrayList();
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
                    tmp = Instantiate(i);

                    return tmp;
                }
            }
            Debug.LogError("allinfromation.hero not have "+name);
            return null;
        }
        
    }
    /// <summary>
    /// only dummy TODO : 나중에 인터넷다운로드로 대체 필요
    /// </summary>
    void dummyinit()
    {
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
            cash = int.Parse((string)playerinformationtable[0]["cash"]);
            gold = int.Parse((string)playerinformationtable[0]["gold"]);
            nickname = (string)playerinformationtable[0]["nickname"];
        
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    private IEnumerator playerherodownload(string url)
    {
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
            Dictionary<string, object>[] playerinformationtable = (Dictionary<string, object>[])dicjson["Items"];
            cash = int.Parse((string)playerinformationtable[0]["cash"]);
            gold = int.Parse((string)playerinformationtable[0]["gold"]);
            nickname = (string)playerinformationtable[0]["nickname"];

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    // Use this for initialization
    void Start()
    {
        StartCoroutine(playerinfordownload(playerinformationtableurl));
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
