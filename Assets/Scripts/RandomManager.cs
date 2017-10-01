using UnityEngine;
using System.Collections;
using System;

public class RandomManager
{
    private static RandomManager instance = null;

    private System.Random r = new System.Random();
    public int kindsRandom;         //生成的动物种类随机
    public int initCountRandom;     //生成的动物个数随机
    public int addCountRandom;      //补进的动物个数随机
    public int ranCountRandom;      //逃出屋子的动物个数随机

    private RandomManager() { }

    public static RandomManager Instance()
    {
        if (instance == null)
            instance = new RandomManager();
        return instance;
    }

    public void createRandom(int level)
    {
        kindsRandom = r.Next(0, 6);
        fromLevel(level);
    }

    public void fromLevel(int level)
    {
        switch (level)
        {
            case 1:
                initCountRandom = r.Next(1, 4);
                addCountRandom = 0;
                ranCountRandom = 0;
                break;
            case 2:
                initCountRandom = r.Next(3, 5);
                addCountRandom = 0;
                ranCountRandom = 0;
                break;
            case 3:
                initCountRandom = r.Next(2, 4);
                addCountRandom = r.Next(1, 3);
                ranCountRandom = 0;
                break;
            case 4:
                initCountRandom = r.Next(3, 5);
                addCountRandom = r.Next(1, 3);
                ranCountRandom = 0;
                break;
            case 5:
                initCountRandom = r.Next(1, 4);
                addCountRandom = 0;
                ranCountRandom = 1;
                break;
            case 6:
                initCountRandom = r.Next(3, 6);
                addCountRandom = 0;
                ranCountRandom = r.Next(1, initCountRandom);
                break;
            case 7:
                initCountRandom = r.Next(1, 4);
                addCountRandom = r.Next(1, 3);
                ranCountRandom = r.Next(1, initCountRandom + addCountRandom);
                break;
            case 8:
                initCountRandom = r.Next(2, 5);
                addCountRandom = r.Next(2, 5);
                ranCountRandom = r.Next(2, initCountRandom + addCountRandom);
                break;
            case 9:
                initCountRandom = r.Next(4, 6);
                addCountRandom = r.Next(2, 5);
                //逃跑的随机数不得大于初始数与补进数之和
                ranCountRandom = r.Next(2, initCountRandom + addCountRandom - 1);
                break;
            default:
                initCountRandom = 5;
                addCountRandom = 2;
                ranCountRandom = 3;
                break;
        }
    }
}
