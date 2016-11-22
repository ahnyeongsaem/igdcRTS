using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerinformation : MonoBehaviour {

    //유저의 정보 클래스
    static public int cashgold;
    static public int gold;
    static public string playerid;
    static public string nickname;


    /// <summary>
    /// player가 가지고 있는 건물 정보(buildinformation)들의 배열(buildinforarray) 클래스
    /// </summary>
    public ArrayList buildinforarray;
    public class buildinformation
    {
        public string name;
        public int level;
        public int count;
        public bool indeck;
        buildinformation(string _name, int _level, int _count, bool _indeck)
        {

        }
    }

    /// <summary>
    /// player가 가지고 있는 영웅 정보(heroinfromation)들의 배열(buildinforarray) 클래스
    /// </summary>
    public ArrayList heroinforarray;
    public class heroinformation
    {
        public string name;
        public int level;
        public int totalexp; //경험치
        public int grade; //영웅 합칠경우
        public int decknumber; //0이면 안들어가있는것
    }


    // Use this for initialization
    void Start () {
	
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
