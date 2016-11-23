using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerinformation : MonoBehaviour
{

    //유저의 정보 클래스
    static public int cashgold;
    static public int gold;
    static public string playerid;
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
            foreach(GameObject i in allunitinformation.Hero )
            {
                if(i.name.Equals(name))
                {
                    return i;
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
    // Use this for initialization
    void Start()
    {
        dummyinit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
