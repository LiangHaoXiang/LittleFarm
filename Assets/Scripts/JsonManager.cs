using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class JsonManager : MonoBehaviour {

    public List<TestItem> tests;
    [Serializable]
    public class TestItem
    {
        public int ID;
        public int initCount;
        public int addCount;
        public int ranCount;
    }
    [Serializable]
    public class Test
    {
        public List<TestItem> test;
    }

    void Start () {
        JsonMy();
	}

    public void JsonMy()
    {
        string json = File.ReadAllText("H:\\云孩科技/Json/testLittleFarm.txt");
        if (json != string.Empty)
        {
            Test item = JsonUtility.FromJson<Test>(json);
            tests = item.test;
        }
    }

}

//{
//    "test": [
//        {"ID": 1,"initCount": 3,"addCount": 2,"ranCount": 3},
//        {"ID": 2,"initCount": 1,"addCount": 3,"ranCount": 4},
//        {"ID": 3,"initCount": 3,"addCount": 2,"ranCount": 1},
//        {"ID": 4,"initCount": 1,"addCount": 2,"ranCount": 3},
//        {"ID": 5,"initCount": 6,"addCount": 1,"ranCount": 4},
//        {"ID": 6,"initCount": 3,"addCount": 3,"ranCount": 5},
//        {"ID": 7,"initCount": 4,"addCount": 0,"ranCount": 0},
//        {"ID": 8,"initCount": 7,"addCount": 0,"ranCount": 4},
//        {"ID": 9,"initCount": 2,"addCount": 2,"ranCount": 0},
//        {"ID": 10,"initCount": 5,"addCount": 1,"ranCount": 1}
//    ]
//}

