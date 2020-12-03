using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//全体の進行を管理するクラス
public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public GameObject startPosition;

    public GameObject Player;

    UnityEvent unityEvent = new UnityEvent();

    public static GameManager Instance//関数ではなくプロパティという書き方
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    //状態を列挙
    public enum State
    {
        Ready,//みんなが動き出せる前の状態（スタート時の演出に利用）
        Game,//ゲーム中
        Glide,//滑空中
        Dead,//死んだ演出用
        Clear,//クリア演出用
        pause,//ポーズ状態用
    }
    State state = State.Ready;//状態を管理する変数

    float timer = 0;//汎用タイマー

    [SerializeField]
    float timeScale;



    //現在の状態を取得する関数
    public State GetState()
    {
        return this.state;
    }



    // Start is called before the first frame update
    void Start()
    {
        ChangeState(State.Ready);
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case State.Ready: Ready(); break;
            case State.Game:  Game(); break;
            case State.Glide: Game(); break;
            case State.Dead:  Dead(); break;
            case State.Clear: Clear(); break;
            case State.pause: pause();break;
        }
    }

    public void TimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }

    void Ready()
    {
        this.timer -= Time.deltaTime;
        if(this.timer <= 0)
        {
            ChangeState(State.Game);
        }

        
    }

    void Game()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeState(State.Ready);
        }
    }

    void Glide()
    {

    }

    void Dead()
    {

    }

    void Clear()
    {

    }

    void pause()
    {

    }

    //stateを切り替える
    public void ChangeState(State next)
    {
        //後始末
        switch (this.state)
        {
            case State.Ready:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                    this.timer = 0;
                }
                break;
            case State.Game:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
            case State.Glide:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
            case State.Dead:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
            case State.Clear:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
            case State.pause:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
        }

        this.state = next;//stateの切り替え

        //初期化処理
        switch (this.state)
        {
            case State.Ready:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                    this.timer = 3f;
                }
                break;
            case State.Game:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
            case State.Glide:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
            case State.Dead:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                    this.Player.transform.position = this.startPosition.transform.position;
                    this.unityEvent.Invoke();
                }
                break;
            case State.Clear:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
            case State.pause:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述

                }
                break;
        }
    }

    public void AddListener(UnityAction method)
    {
        this.unityEvent.AddListener(method);
    }
}
