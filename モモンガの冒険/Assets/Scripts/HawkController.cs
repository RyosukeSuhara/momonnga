using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkController : MonoBehaviour
{
    public enum State
    {
        wait,
        appear,
        ready,
        attack,
        back
    };
    State state = State.wait;

    //攻撃するターゲット
    [SerializeField]
    GameObject target;

    //鷹の最初の待機位置
    [SerializeField]
    GameObject hawkWaitPosition;

    //鷹のメインポジション
    [SerializeField]
    GameObject hawkPosition;

    //目標地点までにかかる時間
    [SerializeField]
    float totalTime = 3;

    //攻撃開始時のターゲットの位置
    Vector2 targetPosition;

    //攻撃中のタイマー
    float timer = 0;

    //攻撃開始してから何秒経過したか
    float attackTime = 0;

    //攻撃終了した位置
    Vector2 attackEndPosition;

    //元の位置に戻り始めて何秒経過したか
    float returnTime = 0;



    void Start()
    {
        ChangeState(State.wait);

        GameManager.Instance.AddListener(ReStart);
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case State.wait: Wait(); break;
            case State.appear: Appear(); break;
            case State.ready: Ready(); break;
            case State.attack: Attack(); break;
            case State.back: Back(); break;
            
        }
    }

    void Wait()
    {
        transform.position = hawkWaitPosition.transform.position;
        ChangeState(State.appear);
    }

    void Appear()
    {
        this.timer += Time.deltaTime;
        this.returnTime = this.timer / this.totalTime;
        if(this.returnTime > 1)
        {
            this.returnTime = 1;
        }
        transform.position = Vector2.Lerp(hawkWaitPosition.transform.position, hawkPosition.transform.position, this.returnTime);
        if(this.timer > this.totalTime)
        {
            ChangeState(State.ready);
        }
    }

    void Ready()
    {
        this.timer += Time.deltaTime;
        if(this.timer > 1)
        {
            ChangeState(State.attack);
        }
    }

    void Attack()
    {
        this.timer += Time.deltaTime;
        this.attackTime = this.timer / this.totalTime;
        if(this.attackTime > 1)
        {
            this.attackTime = 1;
        }
        transform.position = Vector2.Lerp(hawkPosition.transform.position, targetPosition, this.attackTime);

        if(this.timer > this.totalTime)
        {
            ChangeState(State.back);
        }
    }

    void Back()
    {
        this.timer += Time.deltaTime;
        this.returnTime = this.timer / this.totalTime;
        if(this.returnTime > 1)
        {
            this.returnTime = 1;
        }
        transform.position = Vector2.Lerp(this.attackEndPosition, hawkPosition.transform.position, this.returnTime);

        if(this.timer > this.totalTime)
        {
            ChangeState(State.ready);
        }
    }

    public void ChangeState(State next)
    {
        //後始末
        switch (this.state)
        {
            case State.wait:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
            case State.appear:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述

                    //タイマーを初期化
                    this.timer = 0;
                    this.returnTime = 0;
                }
                break;
            case State.ready:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述

                    //タイマーを初期化
                    this.timer = 0;
                }
                break;
            case State.attack:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述

                    //タイマーを０に戻す
                    this.timer = 0;
                    this.attackTime = 0;

                    //攻撃終了した場所を記憶する
                    this.attackEndPosition = transform.position;

                }
                break;
            case State.back:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述

                    //タイマーを初期化
                    this.timer = 0;
                    this.returnTime = 0;
                }
                break;
        }

        this.state = next;//stateの切り替え

        //初期化処理
        switch (this.state)
        {
            case State.wait:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
            case State.appear:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
            case State.ready:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
            case State.attack:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述

                    //目標地点を設定
                    this.targetPosition = this.target.transform.position;
                }
                break;
            case State.back:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
        }
    }

    //衝突処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeState(State.back);
    }

    void ReStart()
    {
        ChangeState(State.wait);
    }
}
