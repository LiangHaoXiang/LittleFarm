using UnityEngine;
using System.Collections;

public class headShake : MonoBehaviour {
	void Start () {
        iTween.MoveBy(gameObject, iTween.Hash("time", 0.5f, "y", -4.0f * UIAdaptive.k.y,
                                 "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));
    }
	
}
