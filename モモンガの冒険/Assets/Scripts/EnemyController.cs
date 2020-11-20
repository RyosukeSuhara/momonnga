using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Vector2 defaultPosition;//初期位置
    Quaternion defaultRotation;//初期向き

    // Start is called before the first frame update
    void Start()
    {
        //位置を記録
        this.defaultPosition = transform.position;
        this.defaultRotation = transform.rotation;
    }

    //再配置＆再表示
    void Restart()
    {
        transform.position = this.defaultPosition;
        transform.rotation = this.defaultRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //GameManagerのstateがgame以外でも何かしたい場合は
        //ここに処理を書く

        //GameManagerのStateがReadyの時に再配置を行う
        if(GameManager.Instance.GetState() == GameManager.State.Ready)
        {
            Restart();//再配置
        }

        //GameManagerのstateがgameの時だけ動けるようにする
        if(GameManager.Instance.GetState() != GameManager.State.Game)
        {
            return;//処理を抜ける
        }

        //移動スクリプト
        transform.Translate(1f * Time.deltaTime,0,0);

        //死ぬ処理(仮）
        if (Input.GetKeyDown(KeyCode.K))
        {
            //再配置を考え世界の果てに飛ばす
            transform.position = new Vector3(0, -500f, 0);
        }
    }
}
