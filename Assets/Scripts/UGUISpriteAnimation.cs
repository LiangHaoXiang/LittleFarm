using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class UGUISpriteAnimation : MonoBehaviour
{
    private Image ImageSource;
    private int mCurFrame = 0;  //当前帧
    private float mDelta = 0;

    public float FPS = 5;
    public List<Sprite> SpriteFrames;

    public int FrameCount
    {
        get{return SpriteFrames.Count;}
    }

    void Awake()
    {
        ImageSource = GetComponent<Image>();
    }

    private void SetSprite(int idx)
    {
        ImageSource.sprite = SpriteFrames[idx];
        //该部分为设置成原始图片大小，如果只需要显示Image设定好的图片大小，注释掉该行即可。
        //ImageSource.SetNativeSize();
    }

    void Update()
    {
        if (FrameCount == 0)
        {
            return;
        }

        mDelta += Time.deltaTime;
        if (mDelta > 1 / FPS)
        {
            mDelta = 0;
            mCurFrame++;

            if (mCurFrame >= FrameCount)    //若当前帧大于总帧数，归零
            {
                mCurFrame = 0;
            }
            SetSprite(mCurFrame);
        }
    }
}
