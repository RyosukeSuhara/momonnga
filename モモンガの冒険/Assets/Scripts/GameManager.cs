using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//全体の進行を管理するクラス
public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public GameObject startPosition;

    //プレイヤーの状態管理用の変数
    public GameObject Player;
    PlayerController playerController;
    Rigidbody2D rigidbody;

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
        DeadReaction,//死亡演出用
        Dead,//死んだ時の処理用
        Clear,//クリア演出用
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

        this.playerController = this.Player.GetComponent<PlayerController>();

        this.rigidbody = this.Player.GetComponent<Rigidbody2D>();

        this.playerController.AddListener(OnDeadReactionEnd);
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case State.Ready: Ready(); break;
            case State.Game:  Game(); break;
            case State.Glide: Game(); break;
            case State.DeadReaction: DeadReaction();  break;
            case State.Dead:  Dead(); break;
            case State.Clear: Clear(); break;
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

    void DeadReaction()
    {

    }

    void Dead()
    {

    }

    void Clear()
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
            case State.DeadReaction:
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
            case State.DeadReaction:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述

                    this.playerController.deadReaction();
                }
                break;
            case State.Dead:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述

                    //プレイヤーをエリアの初期位置に戻す
                    this.Player.transform.position = this.startPosition.transform.position;

                    //コールバック
                    this.unityEvent.Invoke();

                    //滑空ゲージを元に戻す
                    this.playerController.Fixtimer();

                    //プレイヤーにかかっている力を０にする

                    //状態をゲームに戻す
                    ChangeState(State.Game);
                }
                break;
            case State.Clear:
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

    public void OnDeadReactionEnd()
    {

        ChangeState(State.Dead);
    }
}
