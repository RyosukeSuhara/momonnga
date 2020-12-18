using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkStartController : MonoBehaviour
{
    //鷹
    [SerializeField]
    GameObject hawk;

    //鷹のコントローラー
    HawkController hawkController;

    //一度呼ばれたら、やられるまで呼ばれないようにする
    bool check = false;

    // Start is called before the first frame update
    void Start()
    {
        hawkController = hawk.GetComponent<HawkController>();
        GameManager.Instance.AddListener(ReStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーが特定の位置を最初に通り過ぎたときのみ呼ばれる
        if(collision.gameObject.tag == "Player" && !this.check)
        {
            this.check = true;

            hawkController.HawkStart();
        }
    }

    //やられたときのみコールバックで呼ばれる
    void ReStart()
    {
        this.check = false;
    }
}
