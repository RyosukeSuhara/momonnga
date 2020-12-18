using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 4;//移動速度

    [SerializeField]
    float wallSpeed = 3;//壁登りの速さ

    [SerializeField]
    float jumpForce = 300;//ジャンプ力

    [SerializeField]
    float wallJumpForce = 0.5f;//壁掴まりジャンプ力

    [SerializeField]
    float windPower = 1;//風の強さ

    [SerializeField]
    float leafJumpPower = 1f;//葉っぱジャンプ力

    [SerializeField]
    float slowingDoun = 0.9f;//プレイヤーの減速率

    Rigidbody2D rigidbody;//ジャンプ用

    bool isGround = true;//地面についているか

    bool isGliding = true;//滑空可能かどうか

    float timer = 0;//滑空用タイマー

    public Image GlidingTimer;//滑空時間のUI

    bool Gliding = false;//滑空状態かどうか

    bool isLeave = false;//壁から離れたかどうか

    bool wallStay = false;//壁に掴まっているかどうか

    Vector2 wallPosition;//掴まっている壁の位置

    BoxCollider2D boxCollider2D;//壁に触れている判定を行うコライダー

    float wallHeight;//掴まっている壁の高さ

    bool leafStay = false;//葉っぱにつかまっているか

    bool leafJump = false;//葉っぱジャンプできるか

    bool pause = false;//ポーズ状態かどうか

    float deadline = -6;//画面外に落ちたときの死亡判定のライン

    float editLine = 9;//右または左の画面外に移動できなくなるライン

    float fallSpeed = 4;//やられたときに落ちる速度

    UnityEvent unityEvent = new UnityEvent();


    //壁に掴まっているかどうか
    enum IsWall
    {
        not,
        right,
        left
    };
    //現在の状態
    IsWall wallState;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ画面にする
        //startボタンかbボタン
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Joystick1Button9) && this.pause == false) 
        {
            //Invokeを使用せずに呼ぶと、2回目以降即ポーズ画面から抜けてしまうバグが起きたため使用している
            Invoke("latePause", 0.01f);
        }


        //ポーズ画面を解除する
        //startボタンかｖボタン
        if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Joystick1Button9) && this.pause == true)
        {
            this.pause = false;

            UIManager.Instance.FadeIn();
        }
       
        //画面外に落ちたら死亡判定にする
        if(transform.position.y < this.deadline)
        {
            GameManager.Instance.ChangeState(GameManager.State.DeadReaction);
        }
        

        //画面外に行かないようにする
        if(transform.position.x > this.editLine)
        {
            transform.position = new Vector2(this.editLine, transform.position.y);
        }else if(transform.position.x < -this.editLine)
        {
            transform.position = new Vector2(-this.editLine, transform.position.y);
        }  

        //stateがgameかglideじゃなければ操作できないようにする
        if (GameManager.Instance.GetState() != GameManager.State.Game && GameManager.Instance.GetState() != GameManager.State.Glide)
        {
            return;
        }

        //左右方向のキー入力受付
        float move = Input.GetAxis("Horizontal");

        if(this.rigidbody.velocity.x > 0 && move < 0)
        {
            this.rigidbody.velocity *= new Vector2(this.slowingDoun, 1);
        }else if(this.rigidbody.velocity.x < 0 && move > 0)
        {
            this.rigidbody.velocity *= new Vector2(this.slowingDoun, 1);
        }

        //移動と葉っぱジャンプ
        if (leafJump == true)
        {
            //移動ボタンを倒した状態でジャンプボタンを押すと飛ぶ
            if (move < 0)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    transform.Translate(-2f, 0, 0);
                    this.rigidbody.AddForce(new Vector2(-1, 1) * this.leafJumpPower);
                    leafJump = false;
                }
            }
            else if (move > 0)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    transform.Translate(2f, 0, 0);
                    this.rigidbody.AddForce(new Vector2(1, 1) * this.leafJumpPower);
                    leafJump = false;
                }
            }
        }
        else if (wallState == IsWall.not)
        {
            transform.Translate(this.speed * move * Time.deltaTime, 0, 0);
        }
        else if (wallState == IsWall.right)
        {
            if (move > 0)
            {
                transform.Translate(this.speed * move * Time.deltaTime, 0, 0);
            }
        }
        else if (wallState == IsWall.left)
        {
            if (move < 0)
            {
                transform.Translate(this.speed * move * Time.deltaTime, 0, 0);
            }
        }



        //ジャンプ
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1)) && this.isGround)
        {
            this.rigidbody.AddForce(transform.up * this.jumpForce);

            this.isGround = false;
        }

        //滑空モードの切り替え
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button5)) && this.isGliding)
        {
            //タイマー起動
            if (this.timer == 0)
            {
                this.timer = 5;

                //グライディング中にする
                this.Gliding = true;
                GameManager.Instance.ChangeState(GameManager.State.Glide);

            } else if (this.timer > 0 && this.Gliding)
            {
                this.Gliding = false;
                GameManager.Instance.ChangeState(GameManager.State.Game);

            } else if (this.timer > 0 && !this.Gliding)
            {
                this.Gliding = true;
                GameManager.Instance.ChangeState(GameManager.State.Glide);
            }

        }

        //滑空処理
        if(this.timer > 0 && this.Gliding)
        {
            this.timer -= Time.deltaTime;

            this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, -0.1f);

            GlidingTimer.fillAmount -= 1.0f / 5.0f * Time.deltaTime;
        }

        //滑空ゲージを元に戻す
        if (this.timer < 0 && !this.Gliding)
        {
            Fixtimer();
        }else if(this.timer < 0 && this.Gliding)
        {
            GameManager.Instance.ChangeState(GameManager.State.Game);
        }

        if (wallStay)
        {
            WallStay();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //地面についたら
        if(collision.gameObject.tag == "Ground")
        {
            this.isGround = true;
            this.isGliding = true;

            this.Gliding = false;

            this.timer = -1;

            GameManager.Instance.ChangeState(GameManager.State.Game);
        }

        //壁についたら
        if(collision.gameObject.tag == "Wall")
        {
            this.isGround = true;
            this.isGliding = true;
            this.Gliding = false;

            this.wallPosition = collision.transform.position;

            //タイマーを初期化
            this.timer = -1;

            GameManager.Instance.ChangeState(GameManager.State.Game);
        }

        //ギミックに触れたら
        if (collision.gameObject.tag == "Gimmick")
        {
            GameManager.Instance.ChangeState(GameManager.State.DeadReaction);
        }
    }


    //タイマーを満タンに戻す
    public void Fixtimer()
    {
        this.timer = 0;

        GlidingTimer.fillAmount = 1.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            this.isGround = true;
            this.isGliding = true;
            this.Gliding = false;

            this.wallPosition = collision.transform.position;

            //タイマーを初期化
            this.timer = -1;

            //壁に掴まっている状態にする
            this.wallStay = true;

            GameManager.Instance.ChangeState(GameManager.State.Game);
        }


        //葉っぱに当たったら
        if (collision.gameObject.tag == "Leaf")
        {
            GameManager.Instance.ChangeState(GameManager.State.Game);

            transform.position = collision.transform.position;

            //落ちないようにする
            this.rigidbody.velocity = new Vector2(0, 0);

            this.leafJump = true;

            this.isGliding = true;
            this.Gliding = false;

            //タイマーを初期化
            this.timer = -1;
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        //風
        if(collision.gameObject.tag == "Wind")
        {
            if(GameManager.Instance.GetState() == GameManager.State.Glide)
            {
                //風の向きによって力を掛ける方向を変える
                if((315 < collision.gameObject.transform.rotation.eulerAngles.z && collision.gameObject.transform.rotation.eulerAngles.z <= 360) || 
                   (0 <= collision.gameObject.transform.rotation.eulerAngles.z && collision.gameObject.transform.rotation.eulerAngles.z < 45))
                {
                    this.rigidbody.AddForce(new Vector2(1, 0) * this.windPower * 2);
                }else if(45 <= collision.gameObject.transform.rotation.eulerAngles.z && collision.gameObject.transform.rotation.eulerAngles.z < 135)
                {
                    this.rigidbody.AddForce(new Vector2(0, 1) * this.windPower * 50);
                }else if(135 <= collision.gameObject.transform.rotation.eulerAngles.z && collision.gameObject.transform.rotation.eulerAngles.z < 225 )
                {
                    this.rigidbody.AddForce(new Vector2(-1, 0) * this.windPower * 2);
                }else if(225 <= collision.gameObject.transform.rotation.eulerAngles.z && collision.gameObject.transform.rotation.eulerAngles.z < 315)
                {
                    this.rigidbody.AddForce(new Vector2(0, -1) * this.windPower);
                }
                
            }
            
        }

        //葉っぱ
        if(collision.gameObject.tag == "Leaf")
        {
            this.rigidbody.velocity = new Vector2(0, 0);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //壁から離れたら
        if(collision.gameObject.tag == "Wall")
        {
            this.isLeave = false;
            this.wallState = IsWall.not;
            this.wallStay = false;

            Vector3 pos = transform.position;
            pos.y -= 0.95f;
            int layerMask = LayerMask.GetMask(new string[] { "Ground"});
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector3(0, -1, 0), 1f,layerMask);

            if (hit.collider!= null && hit.collider.gameObject.tag == "Ground")
            {
                this.isGround = true;
                this.wallStay = false;
            }
        }
    }

    //壁に掴まっている時
    void WallStay()
    {
        //壁登りように上下方向のキー入力受付
        float updown = Input.GetAxis("Vertical");

        //壁ジャンプ時通常のジャンプが呼び出されることを避けるために一時的にfalseにする
        this.isGround = false;

        this.isGliding = true;

        this.Gliding = false;

        this.timer = -1;

        //壁に掴まるようにする
        this.rigidbody.velocity = new Vector2(0, 0);

        //壁に向かって移動できないようにする
        if (this.wallPosition.x > transform.position.x)
        {
            this.wallState = IsWall.left;
        }
        else
        {
            this.wallState = IsWall.right;
        }

        //壁登り
        if (updown > 0 || updown < 0)
        {
            this.rigidbody.velocity = new Vector2(0, this.wallSpeed * updown);
        }

        //壁ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {

            this.isLeave = true;
            this.wallStay = false;

            //左右どちらに飛ぶか
            if (wallPosition.x > transform.position.x)
            {

                this.rigidbody.AddForce(new Vector2(-1, 1) * this.wallJumpForce, ForceMode2D.Impulse);

            }
            else if (wallPosition.x < transform.position.x)
            {

                this.rigidbody.AddForce(new Vector2(1, 1) * this.wallJumpForce, ForceMode2D.Impulse);
            }
        }
    }

    //ポーズ
    void latePause()
    {
        this.pause = true;

        UIManager.Instance.FadeOut();
    }

    //死んだときの挙動
    public void deadReaction()
    {
        Debug.Log("a");

        this.rigidbody.isKinematic = true;

        StartCoroutine("Fall");
    }
    
    //コルーチン
    IEnumerator Fall()
    {
        //やられたとき、一度飛び上がってから落ちていく処理
        while(transform.position.y > this.deadline -1)
        {
            this.rigidbody.velocity = Vector2.zero;

            transform.position += new Vector3(0, fallSpeed * Time.deltaTime, 0);

            this.fallSpeed -= 3f * Time.deltaTime;

            yield return null;
        }
        this.rigidbody.isKinematic = false;
        this.rigidbody.velocity = Vector2.zero;

        this.fallSpeed = 4;

        //コールバック
        this.unityEvent.Invoke();

        StopCoroutine("Fall");

    }

    //コールバック関数
    public void AddListener(UnityAction method)
    {
        this.unityEvent.AddListener(method);
    }
}


