using UnityEngine;
using System.Collections;



public class ClickConstruct : MonoBehaviour {

    private Vector3 RollBack;                    // 처음 카메라 좌표

    private Vector3 target;                      // 클릭한 건물 좌표
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private bool check = false;                 // 카메라 건물로 근접할 것인가.
    private bool ESCcheck = false;              // ESC를 눌러서 카메라를 처음 위치로 당길 것인가
    private bool isClose = false;
    public changeScene change;

    // Use this for initialization
    void Start()
    {
        // 처음 카메라 위치 좌표
        RollBack = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    void Update()
    {
        // 마우스 클릭
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                // 건물을 클릭했다.
                if (hitInfo.transform.gameObject.tag == "Construction")
                {
                    // 카메라를 움직이고 싶다.
                    //Debug.Log("It's working!");
                    // 카메라 근접과 처음으로 당기는 것이 같이 돌아가는 경우가 있음을 방지
                    check = true;
                    ESCcheck = false;
                    // 건물 좌표에서 조금 떨어진곳을 타겟 좌표로 설정
                    target = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y+3, hitInfo.transform.position.z-10);

                    if (isClose == true)
                    {
                        if (hitInfo.transform.gameObject.name == "Gamestart")
                        {
                            //TODO : 게임 스타트 씬으로 로딩
                            change.SceneChange6();
                        }

                        if(hitInfo.transform.gameObject.name == "Sumon")
                        {
                            //TODO : 소환 씬으로 로딩
                        }
                        
                        if(hitInfo. transform.gameObject.name == "Deck")
                        {
                            //TODO : 덱 씬으로 로딩
                        }
                    }
                    
                }
                else
                {
                    // 클릭해서 인식한게 건물이 아니다.(tag Construction)
                    Debug.Log("nopz");
                    
                }
            }
            else
            {
                // 클릭해서 인식한게 null 이다( tag가 없다.)
                Debug.Log("No hit");
            }
        }

        // 카메라가 근접할 것이다.
        if(check == true)
        {
            // 처음 클릭할때 카메라가 건물로 근접하는것
            if (this.transform.position != target)
            {
                this.transform.position = Vector3.SmoothDamp(this.transform.position, target, ref velocity, smoothTime);
                // 카메라가 건물로 근접하는 것이 끝났다.
                if(Vector3.Distance(this.transform.position,target) < 0.5)
                {
                    // 카메라 근접과 처음으로 당기는 것이 같이 돌아가는 경우가 있음을 방지
                    check = false;
                    ESCcheck = false;
                    isClose = true;
                }
                else
                {
                    isClose = false;
                }
            }        
         }


        // Escape 키를 눌렀나?
        if (Input.GetKey(KeyCode.Escape)&&check == false)
        {
            // 카메라 근접과 처음으로 당기는 것이 같이 돌아가는 경우가 있음을 방지
            ESCcheck = true;
            check = false;
            isClose = false;
        }

        // 카메라를 처음으로 당기는 것
        if (ESCcheck == true)
        { 
            isClose = false;
            this.transform.position = Vector3.SmoothDamp(this.transform.position, RollBack, ref velocity, smoothTime);
            // 당기는 것을 끝냈다.
            if (Vector3.Distance(this.transform.position, RollBack) < 0.5)
            { 
                ESCcheck = false;
            }
        }

    }

}
