using UnityEngine;
using System.Collections;
using System;

public class AnimalsBehaviourScript : MonoBehaviour
{
    private AnswerManager answerManager;
    private QuestionManager questionManager;
    private Vector3 leftCreatePosition;
    [HideInInspector]
    public GameObject child;    //自身物体的子孩子
    [HideInInspector]
    public float exitX;         //散场时的水平移动值，随机
    //动物在房屋内的位置，排列过后方便数数，都有初始值，让不在动物列表的动物有值
    [HideInInspector]
    public Vector3 destination = Vector3.zero;
    [HideInInspector]
    public Vector3 size;    //自身大小
    [HideInInspector]
    public Vector3 childSize;    //子物体大小
    [HideInInspector]
    public bool entered = false;            //是否进了场
    [HideInInspector]
    public bool stopMoving = false;         //是否停下移动
    [HideInInspector]
    public bool canRan = false;             //是否可以逃跑
    [HideInInspector]
    public bool canExit = false;            //是否可以散场
    [HideInInspector]
    public bool exitRun = false;            //是否运行散场

    [HideInInspector]
    public bool changeVoice = false;        //是否可以改变声音
    public AudioClip cryVoice;       //出场叫声
    public AudioClip leaveVoice;     //离开声音
    public AudioClip runAwayVoice;   //散场声音
    protected AudioSource audioSource;

    public Sprite laughFace;
    

    protected virtual void Start()
    {
        answerManager = GameObject.Find("answerManager").GetComponent<AnswerManager>();
        questionManager = GameObject.Find("questionManager").GetComponent<QuestionManager>();
        child = transform.GetChild(0).gameObject;
        leftCreatePosition = questionManager.leftCreatePoint.GetComponent<RectTransform>().position;
        size = transform.GetComponent<RectTransform>().sizeDelta;
        childSize = child.GetComponent<RectTransform>().sizeDelta;
        audioSource = transform.GetComponent<AudioSource>();
        audioSource.clip = cryVoice;
        audioSource.Play();
    }

    protected virtual void Update()
    {
        animalsEnter(); //首先，动物进场
        animalsRan();   //有动物逃跑
        while ((destination != Vector3.zero) && (transform.localPosition != destination) 
                && (!canExit))
        {
            Debug.Log("位置是" + destination);
            transform.localPosition = destination;  //排好序
        }
        animalsExit();  //动物散场
    }
    //动物进场
    void animalsEnter()
    {
        if (!entered)
        {
            #region 注释运动
            ////从出生到完成第一跳需要0.1+1.1=1.2秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 0.1f, "time", 1.1f, "x", Screen.width / 8.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child, iTween.Hash("delay", 0.1f, "time", 0.55f, "y", 25, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child, iTween.Hash("delay", 0.65f, "time", 0.55f, "y", -25, "easetype", iTween.EaseType.easeInSine));
            ////完成第二跳需要1.2+0.2+1.1=2.5秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 1.4f, "time", 1.1f, "x", Screen.width / 8.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child, iTween.Hash("delay", 1.4f, "time", 0.55f, "y", 25, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child, iTween.Hash("delay", 1.95f, "time", 0.55f, "y", -25, "easetype", iTween.EaseType.easeInSine));
            ////完成第三跳需要2.5+0.2+1.1=3.8秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 2.7f, "time", 1.1f, "x", Screen.width / 8.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child, iTween.Hash("delay", 2.7f, "time", 0.55f, "y", 25, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child, iTween.Hash("delay", 3.25f, "time", 0.55f, "y", -25, "easetype", iTween.EaseType.easeInSine));
            ////完成第四跳需要3.8+0.2+1.1=5.1秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 4.0f, "time", 1.1f, "x", Screen.width / 8.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child, iTween.Hash("delay", 4.0f, "time", 0.55f, "y", 25, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child, iTween.Hash("delay", 4.55f, "time", 0.55f, "y", -25, "easetype", iTween.EaseType.easeInSine));


            ////从出生到完成第一跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 0.0f, "time", 0.8f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child,      iTween.Hash("delay", 0.0f, "time", 0.4f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 0.4f, "time", 0.4f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第二跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 1.0f, "time", 0.8f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child,      iTween.Hash("delay", 1.0f, "time", 0.4f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 1.4f, "time", 0.4f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第三跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 2.0f, "time", 0.8f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child,      iTween.Hash("delay", 2.0f, "time", 0.4f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 2.4f, "time", 0.4f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第四跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 3.0f, "time", 0.8f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child,      iTween.Hash("delay", 3.0f, "time", 0.4f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 3.4f, "time", 0.4f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第五跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 4.0f, "time", 0.8f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child,      iTween.Hash("delay", 4.0f, "time", 0.4f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 4.4f, "time", 0.4f, "y", -30, "easetype", iTween.EaseType.easeInSine));

            ////--五段跳--//
            ////从出生到完成第一跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 0.0f, "time", 0.6f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 0.0f, "time", 0.3f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 0.3f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第二跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 0.8f, "time", 0.6f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 0.8f, "time", 0.3f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 1.1f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第三跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 1.6f, "time", 0.6f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 1.6f, "time", 0.3f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 1.9f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第四跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 2.4f, "time", 0.6f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 2.4f, "time", 0.3f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 2.7f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第五跳需要秒
            //iTween.MoveBy(gameObject, iTween.Hash("delay", 3.2f, "time", 0.6f, "x", Screen.width / 10.0f, "easetype", iTween.EaseType.linear));
            //iTween.MoveBy(child,      iTween.Hash("delay", 3.2f, "time", 0.3f, "y",  30, "easetype", iTween.EaseType.easeOutSine));
            //iTween.MoveBy(child,      iTween.Hash("delay", 3.5f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
#endregion
            //--七段跳--//
            //从出生到完成第一跳需要秒
            iTween.MoveBy(gameObject, iTween.Hash("delay", 0.0f, "time", 0.6f, "x", Screen.width / 14.0f, "easetype", iTween.EaseType.linear));
            iTween.MoveBy(child,      iTween.Hash("delay", 0.0f, "time", 0.3f, "y", 30, "easetype", iTween.EaseType.easeOutSine));
            iTween.MoveBy(child,      iTween.Hash("delay", 0.3f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            //完成第二跳需要秒
            iTween.MoveBy(gameObject, iTween.Hash("delay", 0.8f, "time", 0.6f, "x", Screen.width / 14.0f, "easetype", iTween.EaseType.linear));
            iTween.MoveBy(child,      iTween.Hash("delay", 0.8f, "time", 0.3f, "y", 30, "easetype", iTween.EaseType.easeOutSine));
            iTween.MoveBy(child,      iTween.Hash("delay", 1.1f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            //完成第三跳需要秒
            iTween.MoveBy(gameObject, iTween.Hash("delay", 1.6f, "time", 0.6f, "x", Screen.width / 14.0f, "easetype", iTween.EaseType.linear));
            iTween.MoveBy(child,      iTween.Hash("delay", 1.6f, "time", 0.3f, "y", 30, "easetype", iTween.EaseType.easeOutSine));
            iTween.MoveBy(child,      iTween.Hash("delay", 1.9f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            //完成第四跳需要秒
            iTween.MoveBy(gameObject, iTween.Hash("delay", 2.4f, "time", 0.6f, "x", Screen.width / 14.0f, "easetype", iTween.EaseType.linear));
            iTween.MoveBy(child,      iTween.Hash("delay", 2.4f, "time", 0.3f, "y", 30, "easetype", iTween.EaseType.easeOutSine));
            iTween.MoveBy(child,      iTween.Hash("delay", 2.7f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            //完成第五跳需要秒
            iTween.MoveBy(gameObject, iTween.Hash("delay", 3.2f, "time", 0.6f, "x", Screen.width / 14.0f, "easetype", iTween.EaseType.linear));
            iTween.MoveBy(child,      iTween.Hash("delay", 3.2f, "time", 0.3f, "y", 30, "easetype", iTween.EaseType.easeOutSine));
            iTween.MoveBy(child,      iTween.Hash("delay", 3.5f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            ////完成第六跳需要秒
            iTween.MoveBy(gameObject, iTween.Hash("delay", 4.0f, "time", 0.6f, "x", Screen.width / 14.0f, "easetype", iTween.EaseType.linear));
            iTween.MoveBy(child,      iTween.Hash("delay", 4.0f, "time", 0.3f, "y", 30, "easetype", iTween.EaseType.easeOutSine));
            iTween.MoveBy(child,      iTween.Hash("delay", 4.3f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));
            //完成第七跳需要秒
            iTween.MoveBy(gameObject, iTween.Hash("delay", 4.8f, "time", 0.6f, "x", Screen.width / 14.0f, "easetype", iTween.EaseType.linear));
            iTween.MoveBy(child,      iTween.Hash("delay", 4.8f, "time", 0.3f, "y", 30, "easetype", iTween.EaseType.easeOutSine));
            iTween.MoveBy(child,      iTween.Hash("delay", 5.0f, "time", 0.3f, "y", -30, "easetype", iTween.EaseType.easeInSine));


            entered = true;
        }
    }
    //动物散场
    void animalsExit()
    {
        if (answerManager.isCorrect)
        {
            //当回答正确，等待房屋掀起完毕后，动物开始散场
            if (answerManager.houseFinishFirstMoving == true)
            {
                canExit = true;
                changeVoice = true; //要改变声音
            }
            if (canExit)
            {
                if (changeVoice)
                {
                    if (audioSource.clip != runAwayVoice)
                    {
                        audioSource.clip = runAwayVoice;
                        audioSource.Play();
                        changeVoice = false;
                    }
                }
                if (!exitRun)
                {
                    //随机散场，速度随机，位置随机
                    randomExit_X();
                    iTween.MoveBy(child, iTween.Hash("time", 0.7f, "y", -leftCreatePosition.y, "easetype", iTween.EaseType.linear));
                    iTween.MoveBy(gameObject, iTween.Hash("time", 0.7f, "x", exitX, "easetype", iTween.EaseType.linear, "oncomplete", "destroy"));
                    exitRun = true;
                }
            }
        }

    }
    //动物逃跑
    void animalsRan()
    {
        if (canRan)
        {
            if (changeVoice)    //改变声音
            {
                audioSource.clip = leaveVoice;
                audioSource.Play();
                changeVoice = false;
            }
            iTween.MoveBy(gameObject,iTween.Hash( "delay",0.1f,"time",4.0f,
                                                  "x",Screen.width/ 2 + transform.GetComponent<RectTransform>().rect.width,
                                                  "easetype",iTween.EaseType.linear,"oncomplete","destroy"));

            canRan = false;
        }
    }
    
    void destroy()
    {
        Destroy(gameObject);
    }
    //随机产生散场水平移动值
    void randomExit_X()
    {
        System.Random r = new System.Random();
        switch (r.Next(0, 9))
        {
            case 0:
                exitX = 0;
                break;
            case 1:
                exitX = Screen.width * 2 / 20;
                break;
            case 2:
                exitX = Screen.width * 3 / 20;
                break;
            case 3:
                exitX = Screen.width * 4 / 20;
                break;
            case 4:
                exitX = Screen.width * 5 / 20;
                break;
            case 5:
                exitX = Screen.width * 6 / 20;
                break;
            case 6:
                exitX = Screen.width * 7 / 20;
                break;
            case 7:
                exitX = Screen.width * 8 / 20;
                break;
            case 8:
                exitX = Screen.width * 9 / 20;
                break;
        }
    }
}
