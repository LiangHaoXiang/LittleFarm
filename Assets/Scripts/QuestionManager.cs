using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour {

    public GameObject chickenPrefab1;
    public GameObject dogPrefab1;
    public GameObject lionPrefab1;
    public GameObject pigPrefab1;
    public GameObject sheepPrefab1;
    public GameObject wolfPrefab1;
    [HideInInspector]
    public GameObject prefab;   //最终选定的动物预设赋给它
    [HideInInspector]
    public Transform leftCreatePoint;   //左出生点
    [HideInInspector]
    public float houseSize_width;   //房屋的宽度
    [HideInInspector]
    public float houseCapcity;      //房屋的容量的宽度
    [HideInInspector]
    public List<GameObject> animals = new List<GameObject>();   //动物列表，方便管理
    [HideInInspector]
    public bool existRanCount = false;
    [HideInInspector]
    public int initCount;   //初始数量
    [HideInInspector]
    public int addCount;    //补进数量
    [HideInInspector]
    public int ranCount;    //逃跑数量

    float lastTime;         //最后一个动物进屋所需时间

    public static RandomManager rm = RandomManager.Instance();
    private JsonManager jm;
    private Transform canvas;   //画布的引用
    private Text TextID;        //显示的题号
    private int testID = 0;

    private List<Transform> createPointObjs = new List<Transform>();
    private RectTransform[] ct; //获取出生点下物体的RectTransform

    void Start () {
        jm = GameObject.Find("jsonManager").GetComponent<JsonManager>();
        canvas = GameObject.Find("Canvas").transform;
        TextID = canvas.FindChild("PanelUI").FindChild("TestID").GetComponent<Text>();
        //找到左出生点的引用
        leftCreatePoint = canvas.FindChild("leftCreatePoint");
    }
	
	void Update () {
	}

    public IEnumerator createQuestion(int level)
    {
        rm.createRandom(level);

        chooseQuestionStyle();
        if (testID > 0)
            TextID.text = "第" + testID + "题";
        yield return createAnimalsByCount(initCount, randomKindsPrefab());
        //等待最后一个动物进屋后再...
        lastTime = 5.6f;
        yield return addInOrder(addCount);
        yield return ranInOrder(ranCount);

        sortInHouse();
    }
    //根据输入数量生成相应数量的动物
    public IEnumerator createAnimalsByCount(int count, GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            animals.Add(Instantiate(prefab));
            animals[animals.Count - 1].transform.parent = leftCreatePoint;
            //--让动物自适应大小--//
            Vector3 size = animals[animals.Count - 1].GetComponent<RectTransform>().sizeDelta;
            animals[animals.Count - 1].GetComponent<RectTransform>().sizeDelta = new Vector3(size.x * UIAdaptive.k.x, size.y * UIAdaptive.k.y, 1);

            findAllChildren(animals[animals.Count - 1].transform);
            initUI();
            createPointObjs.Clear();    //每生出一个 自适应一个，并清空表，供下一个使用
            //--让动物自适应大小--//
            animals[animals.Count - 1].transform.localPosition = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(1.2f);
        }
    }
    //让动物有序的有间隔时间地逃跑
    public IEnumerator ranInOrder(int count)
    {
        if (count > 0)
        {
            //若有逃跑者，让它跑！跑到屏幕外就自动销毁
            yield return new WaitForSeconds(lastTime);
            //等完后，有逃跑的再逃跑
            existRanCount = true;
            for (int i = 0; i < count; i++)
            {
                //让相应动物逃跑
                animals[animals.Count - 1].GetComponent<AnimalsBehaviourScript>().canRan = true;
                animals[animals.Count - 1].GetComponent<AnimalsBehaviourScript>().changeVoice = true;
                yield return new WaitForSeconds(1.2f);
                //在列表中移除
                animals.RemoveAt(animals.Count - 1);
                //销毁相应动物在动物脚本内已经实现
            }
            existRanCount = false;//关闭逃跑
        }
        //再等逃跑的逃跑完后，在生成答案中调用时再出现答案
        yield return new WaitForSeconds(lastTime);
    }
    //让动物有序的有间隔时间地补进
    public IEnumerator addInOrder(int count)
    {
        if (count > 0)
        {
            yield return new WaitForSeconds(lastTime);
            //等完后，有补进的再补进
            yield return createAnimalsByCount(count, randomKindsPrefab());
        }
    }
    //根据随机数获取随机物种
    GameObject randomKindsPrefab()
    {
        
        switch (rm.kindsRandom)
        {
            case 0:
                prefab = chickenPrefab1;
                break;
            case 1:
                prefab = dogPrefab1;
                break;
            case 2:
                prefab = lionPrefab1;
                break;
            case 3:
                prefab = pigPrefab1;
                break;
            case 4:
                prefab = sheepPrefab1;
                break;
            case 5:
                prefab = wolfPrefab1;
                break;
        }
        return prefab;
    }

    //选择出题方式，题库or随机
    void chooseQuestionStyle()
    {
        //若有题库，优先使用题库出题
        if (jm.tests.Count > 0)
        {
            testID = jm.tests[0].ID;
            initCount = jm.tests[0].initCount;
            addCount = jm.tests[0].addCount;
            ranCount = jm.tests[0].ranCount;
            jm.tests.RemoveAt(0);
        }
        //否则,若题库用完了或没有题库，那么使用随机出题
        else
        {
            testID++;
            initCount = rm.initCountRandom;
            addCount = rm.addCountRandom;
            ranCount = rm.ranCountRandom;
        }
    }
    //最终留下的动物在房屋内排好序，方便数数
    void sortInHouse()
    {
        Vector3 destination;
        if (animals.Count <= 0) return;
        if (animals.Count == 1) //若只有一个，那就放在正中间咯
        {
            destination = new Vector3(Screen.width / 2, 0, 0);
            animals[0].transform.GetComponent<AnimalsBehaviourScript>().destination = destination;
            return;
        }

        houseSize_width = GameObject.Find("house").GetComponent<RectTransform>().rect.width * 0.90f;//计算得
        houseCapcity = houseSize_width - animals[0].transform.GetComponent<RectTransform>().sizeDelta.x;//减一个身位长度
        float interval = houseCapcity / (animals.Count - 1);    //动物间隔宽度
        float halfSize_X = animals[0].transform.GetComponent<RectTransform>().sizeDelta.x / 2;  //半个身位长度
        Debug.Log("houseSize_width = " + houseSize_width);
        Debug.Log("houseCapcity = " + houseCapcity);
        Debug.Log("interval = " + interval);
        Debug.Log("halfSize_X = " + halfSize_X);
        for (int i = 0; i < animals.Count; i++)
        {
            //计算分析得
            destination = new Vector3(Screen.width / 2.0f - houseCapcity / 2.0f + i * interval + halfSize_X, 0, 0);
            Debug.Log("第" + i + "个动物坐标 = " + destination);
            animals[i].transform.GetComponent<AnimalsBehaviourScript>().destination = destination;
        }
        Debug.Log(animals[animals.Count - 1].transform.GetComponent<AnimalsBehaviourScript>().destination
            - animals[0].transform.GetComponent<AnimalsBehaviourScript>().destination);
    }

    void findAllChildren(Transform uiObject)
    {
        if (uiObject.childCount <= 0) return;   //若没孩子就别往下了
        for (int i = 0; i < uiObject.childCount; i++)
        {
            createPointObjs.Add(uiObject.GetChild(i));
            findAllChildren(uiObject.GetChild(i));  //递归查找
        }
    }
    void initUI()
    {
        ct = new RectTransform[createPointObjs.Count];
        for (int i = 0; i < createPointObjs.Count; i++)
        {
            ct[i] = createPointObjs[i].GetComponent<RectTransform>(); //获得每个UI元素的RectTransform
        }

        for (int i = 0; i < createPointObjs.Count; i++)  //必须是计算以锚点位置的大小
        {
            ct[i].anchoredPosition = new Vector3(ct[i].anchoredPosition.x * UIAdaptive.k.x, ct[i].anchoredPosition.y * UIAdaptive.k.y, 0);
            ct[i].sizeDelta = new Vector3(ct[i].sizeDelta.x * UIAdaptive.k.x, ct[i].sizeDelta.y * UIAdaptive.k.y, 1);
        }
    }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(100, 300, 800, 800), "houseSize_width = " + houseSize_width.ToString());
    //    GUI.Label(new Rect(100, 350, 800, 800), "houseCapcity = " + houseCapcity.ToString());
    //}
}
