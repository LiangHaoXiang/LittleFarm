using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerManager : MonoBehaviour
{
    
    public Text answerA;
    public Text answerB;
    public Text answerC;
    public Text answerD;
    [HideInInspector]
    public int correctAnswer;

    private QuestionManager questionManager; //场景中的出题管理器
    private GameObject house;
    private Transform panelUI;
    private float houseOriginY;
    private Vector3 originA_pos;
    private Vector3 originB_pos;
    private Vector3 originC_pos;
    private Vector3 originD_pos;
    [HideInInspector]
    public bool isCorrect = false;          //是否回答正确
    [HideInInspector]
    public int correctCount = 0;            //回答正确的个数
    [HideInInspector]
    public bool houseMoveFirstTime = false; //第一次房屋移动（上移）
    [HideInInspector]
    public bool houseMoveSecondTime = false;//第二次房屋移动（下移）
    [HideInInspector]
    public bool houseFinishFirstMoving = false; //房屋完成第一次移动
    [HideInInspector]
    public bool houseFinishSecondMoving = false;//房屋完成第二次移动
    [HideInInspector]
    public bool canNext = false;            //可以继续下一题

    public AudioClip answerWrong;   //答对音频
    public AudioClip answerCorrect; //答错音频

    void Start()
    {
        house = GameObject.Find("house");
        houseOriginY = house.transform.position.y;

        panelUI = GameObject.Find("PanelUI").transform;

        questionManager = GameObject.Find("questionManager").GetComponent<QuestionManager>();
        originA_pos = answerA.transform.parent.GetComponent<RectTransform>().position;
        originB_pos = answerB.transform.parent.GetComponent<RectTransform>().position;
        originC_pos = answerC.transform.parent.GetComponent<RectTransform>().position;
        originD_pos = answerD.transform.parent.GetComponent<RectTransform>().position;
        //Debug.Log(house.GetComponent<RectTransform>().sizeDelta);
        //Debug.Log(house.GetComponent<RectTransform>().rect.size);
        //Debug.Log(house.GetComponent<RectTransform>().rect.size.x);
        //Debug.Log(house.GetComponent<RectTransform>().rect.size.y);
        //Debug.Log(house.GetComponent<RectTransform>().rect.width);
        //Debug.Log(house.GetComponent<RectTransform>().rect.height);
    }

    void Update()
    {
        if (houseMoveFirstTime)
        {
            house.transform.Translate(Vector3.up * 10 * UIAdaptive.k.y);
            if (house.transform.position.y >= houseOriginY + Screen.height)
            {
                houseMoveFirstTime = false;
                houseFinishFirstMoving = true;
            }
        }

        if (houseMoveSecondTime)
        {
            house.transform.Translate(Vector3.down * 10 * UIAdaptive.k.y);
            if (house.transform.position.y <= houseOriginY)
            {
                house.transform.position = new Vector3(house.transform.position.x, houseOriginY, 0);
                houseMoveSecondTime = false;
                houseFinishSecondMoving = true;
            }
        }
    }
    /// <summary>
    /// 生成答案
    /// </summary>
    /// <param name="level">关卡等级</param>
    /// <returns></returns>
    public IEnumerator createAnswer(int level)
    {
        //等待题目出完
        yield return questionManager.createQuestion(level);
        //窗户显示改变一下
        windowChange();
        //可以揭秘
        demystifyChange();
        //答案飞入
        AnswersIn();

        correctAnswer = questionManager.initCount + questionManager.addCount - questionManager.ranCount;
        int seed;
        if (correctAnswer == 0)
            seed = 1;   //保证生成答案不为负数
        else
            seed = correctAnswer;
        //使a,b,c,d为不同的答案
        int a, b, c, d;
        System.Random r = new System.Random();
        int x = r.Next(seed - 1, seed + 3);
        a = x;
        //b要与a不相等
        while (true)
        {
            x = r.Next(seed - 1, seed + 3);
            b = x;
            if (a != b)
                break;
        }
        //c要与前面不相等
        while (true)
        {
            x = r.Next(seed - 1, seed + 3);
            c = x;
            if ((a != c) && (b != c))
                break;
        }
        //同上
        while (true)
        {
            x = r.Next(seed - 1, seed + 3);
            d = x;
            if ((a != d) && (b != d) && (c != d))
                break;
        }
        answerA.text = a.ToString();
        answerB.text = b.ToString();
        answerC.text = c.ToString();
        answerD.text = d.ToString();
    }

    //回答正确后的其他逻辑，包括掀开房屋，让动物散场，继续玩下一题
    void correctAnswerManage()
    {
        StartCoroutine(answerSelectChange());   //选项的相关改变
        correctCount++;
        //回答正确
        isCorrect = true;
        //可以掀开房屋
        houseMoveFirstTime = true;
        //动物散场在动物脚本内已完成
        //----------------------//
        //星分数改变
        starPointChange();
        //头改变成笑脸
        StartCoroutine(headChange());
        //清空animals列表
        questionManager.animals.Clear();
        //可以继续下一题
        canNext = true;
    }
    //答对后得分的星星改变
    void starPointChange()
    {
        //for(int i = 1; i <= correctCount; i++)
        //{
        //    panelUI.FindChild("getPoint" + i.ToString()).FindChild("star").gameObject.SetActive(true);
        //}

        switch (correctCount)
        {
            case 1:
                panelUI.FindChild("getPoint1").FindChild("star").gameObject.SetActive(true);
                iTween.MoveTo(panelUI.FindChild("getPoint1").FindChild("star").gameObject,
                                iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "position", panelUI.FindChild("getPoint1").position,
                                            "easetype", iTween.EaseType.easeInOutSine));
                iTween.ScaleTo(panelUI.FindChild("getPoint1").FindChild("star").gameObject,
                               iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "scale", panelUI.FindChild("getPoint1").FindChild("bg").localScale,
                                            "easetype", iTween.EaseType.easeInOutSine));
                break;
            case 2:
                panelUI.FindChild("getPoint2").FindChild("star").gameObject.SetActive(true);
                iTween.MoveTo(panelUI.FindChild("getPoint2").FindChild("star").gameObject,
                                iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "position", panelUI.FindChild("getPoint2").position,
                                            "easetype", iTween.EaseType.easeInOutSine));
                iTween.ScaleTo(panelUI.FindChild("getPoint2").FindChild("star").gameObject,
                               iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "scale", panelUI.FindChild("getPoint2").FindChild("bg").localScale,
                                            "easetype", iTween.EaseType.easeInOutSine));
                break;
            case 3:
                panelUI.FindChild("getPoint3").FindChild("star").gameObject.SetActive(true);
                iTween.MoveTo(panelUI.FindChild("getPoint3").FindChild("star").gameObject,
                                iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "position", panelUI.FindChild("getPoint3").position,
                                            "easetype", iTween.EaseType.easeInOutSine));
                iTween.ScaleTo(panelUI.FindChild("getPoint3").FindChild("star").gameObject,
                               iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "scale", panelUI.FindChild("getPoint3").FindChild("bg").localScale,
                                            "easetype", iTween.EaseType.easeInOutSine));
                break;
            case 4:
                panelUI.FindChild("getPoint4").FindChild("star").gameObject.SetActive(true);
                iTween.MoveTo(panelUI.FindChild("getPoint4").FindChild("star").gameObject,
                                iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "position", panelUI.FindChild("getPoint4").position,
                                            "easetype", iTween.EaseType.easeInOutSine));
                iTween.ScaleTo(panelUI.FindChild("getPoint4").FindChild("star").gameObject,
                               iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "scale", panelUI.FindChild("getPoint4").FindChild("bg").localScale,
                                            "easetype", iTween.EaseType.easeInOutSine));
                break;
            case 5:
                panelUI.FindChild("getPoint5").FindChild("star").gameObject.SetActive(true);
                iTween.MoveTo(panelUI.FindChild("getPoint5").FindChild("star").gameObject,
                                iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "position", panelUI.FindChild("getPoint5").position,
                                            "easetype", iTween.EaseType.easeInOutSine));
                iTween.ScaleTo(panelUI.FindChild("getPoint5").FindChild("star").gameObject,
                               iTween.Hash("delay", 0.2f, "time", 0.8f,
                                            "scale", panelUI.FindChild("getPoint5").FindChild("bg").localScale,
                                            "easetype", iTween.EaseType.easeInOutSine));
                break;
        }
    }
    //答对后头改变成笑脸
    IEnumerator headChange()
    {
        for(int i = 0; i < questionManager.animals.Count; i++)
        {
            questionManager.animals[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = questionManager.animals[i].GetComponent<AnimalsBehaviourScript>().laughFace;
        }
        yield return new WaitForSeconds(1.0f);
    }
    //生成完问题后，可以揭秘
    void demystifyChange()
    {
        panelUI.FindChild("Demystify").gameObject.SetActive(true);
    }
    //生成完问题后，生成答案时 窗户改变为问号
    void windowChange()
    {
        house.transform.FindChild("window").gameObject.SetActive(false);
        house.transform.FindChild("WenHao").gameObject.SetActive(true);
    }
    //答对后的选项改变
    IEnumerator answerSelectChange()
    {
        yield return new WaitForSeconds(1);
        answerA.text = "";
        answerB.text = "";
        answerC.text = "";
        answerD.text = "";
        answerA.transform.parent.gameObject.SetActive(false);
        answerB.transform.parent.gameObject.SetActive(false);
        answerC.transform.parent.gameObject.SetActive(false);
        answerD.transform.parent.gameObject.SetActive(false);

        answerA.transform.parent.GetComponent<RectTransform>().position = originA_pos;
        answerB.transform.parent.GetComponent<RectTransform>().position = originB_pos;
        answerC.transform.parent.GetComponent<RectTransform>().position = originC_pos;
        answerD.transform.parent.GetComponent<RectTransform>().position = originD_pos;
    }
    //生成答案时，选项飞入进场
    void AnswersIn()
    {
        iTween.MoveTo(answerA.transform.parent.gameObject, iTween.Hash("x", Screen.width * 0.25, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));
        iTween.MoveTo(answerB.transform.parent.gameObject, iTween.Hash("x", Screen.width * 0.42, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));
        iTween.MoveTo(answerC.transform.parent.gameObject, iTween.Hash("x", Screen.width * 0.59, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));
        iTween.MoveTo(answerD.transform.parent.gameObject, iTween.Hash("x", Screen.width * 0.76, "time", 0.5f, "easetype", iTween.EaseType.easeInSine));
    }

    //开始按钮点击事件
    public void beginClick()
    {
        StartCoroutine(createAnswer(GameManager.level));
    }

    //A选项点击事件
    public void Aclick()
    {
        if (answerA.text != string.Empty)
        {
            if (int.Parse(answerA.text) == correctAnswer)
            {
                answerA.transform.parent.GetComponent<AudioSource>().clip = answerCorrect;
                answerA.transform.parent.FindChild("correctImage").gameObject.SetActive(true);
                correctAnswerManage();
            }
            else
            {
                answerA.transform.parent.GetComponent<AudioSource>().clip = answerWrong;
                iTween.ShakeRotation(answerA.transform.parent.gameObject, new Vector3(0, 0, 10), 1);
                iTween.ShakePosition(answerA.transform.parent.gameObject, new Vector3(3, 1, 0), 1);
            }
            answerA.transform.parent.GetComponent<AudioSource>().Play();
        }
    }
    public void Bclick()
    {
        if (answerB.text != string.Empty)
        {
            if (int.Parse(answerB.text) == correctAnswer)
            {
                answerB.transform.parent.GetComponent<AudioSource>().clip = answerCorrect;
                answerB.transform.parent.FindChild("correctImage").gameObject.SetActive(true);
                correctAnswerManage();
            }
            else
            {
                answerB.transform.parent.GetComponent<AudioSource>().clip = answerWrong;
                iTween.ShakeRotation(answerB.transform.parent.gameObject, new Vector3(0, 0, 10), 1);
                iTween.ShakePosition(answerB.transform.parent.gameObject, new Vector3(3, 1, 0), 1);
            }
            answerB.transform.parent.GetComponent<AudioSource>().Play();
        }

    }
    public void Cclick()
    {
        if (answerC.text != string.Empty)
        {
            if (int.Parse(answerC.text) == correctAnswer)
            {
                answerC.transform.parent.GetComponent<AudioSource>().clip = answerCorrect;
                answerC.transform.parent.FindChild("correctImage").gameObject.SetActive(true);
                correctAnswerManage();
            }
            else
            {
                answerC.transform.parent.GetComponent<AudioSource>().clip = answerWrong;
                iTween.ShakeRotation(answerC.transform.parent.gameObject, new Vector3(0, 0, 10), 1);
                iTween.ShakePosition(answerC.transform.parent.gameObject, new Vector3(3, 1, 0), 1);
            }
            answerC.transform.parent.GetComponent<AudioSource>().Play();
        }
    }
    public void Dclick()
    {
        if (answerD.text != string.Empty)
        {
            if (int.Parse(answerD.text) == correctAnswer)
            {
                answerD.transform.parent.GetComponent<AudioSource>().clip = answerCorrect;
                answerD.transform.parent.FindChild("correctImage").gameObject.SetActive(true);
                correctAnswerManage();
            }
            else
            {
                answerD.transform.parent.GetComponent<AudioSource>().clip = answerWrong;
                iTween.ShakeRotation(answerD.transform.parent.gameObject, new Vector3(0, 0, 10), 1);
                iTween.ShakePosition(answerD.transform.parent.gameObject, new Vector3(3, 1, 0), 1);
            }
            answerD.transform.parent.GetComponent<AudioSource>().Play();
        }
    }

}
