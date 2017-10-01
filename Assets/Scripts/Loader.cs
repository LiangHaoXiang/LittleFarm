using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

    private void Awake()
    {
        //若gameManager实例为空，就创建一个
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
