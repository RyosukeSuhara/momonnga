using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            UIManager.Instance.FadeOut();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            UIManager.Instance.FadeIn();
        }
        
        //左右方向のキー入力受付
        float move = Input.GetAxis("Horizontal");

        //一度移動入力が切れたらジャンプできるようにする
        if (move < 0.2)
        {
            leafStay = false;
        }
        
        //葉っぱに掴まった直後は移動できない
        if(this.leafStay == false)
        {
            //移動と葉っぱジャンプ
            if (leafJump == true)
            {
                if(move < -0.2)
                {
                    transform.Translate(-0.86f, 0, 0);
                    this.rigidbody.AddForce(new Vector2(-1, 1) * this.leafJumpPower);
                    leafJump = false;
                }else if(move > 0.2)
                {
                    transform.Translate(0.86f, 0, 0);
                    this.rigidbody.AddForce(new Vector2(1, 1) * this.leafJumpPower);
                    leafJump = false;
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
        }
        
        

        //ジャンプ
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2)) && this.isGround)
        {
            this.rigidbody.AddForce(transform.up * this.jumpForce);

            this.isGround = false;
        }

        //滑空モードの切り替え
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button4)) && this.isGliding)
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
            GameManager.Instance.ChangeState(GameManager.State.Dead);
        }
    }


    //タイマーを満タンに戻す
    void Fixtimer()
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

            this.leafStay = true;
            this.leafJump = true;

            this.isGliding = true;
            this.Gliding = false;

            //タイマーを初期化
            this.timer = -1;
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        /*バックアップ用にとっておいてあります
        if (collision.gameObject.tag == "Wall" && this.isLeave == false)
        {

            //壁ジャンプ時通常のジャンプが呼び出されることを避けるために一時的にfalseにする
            this.isGround = false;

            this.isGliding = true;

            this.Gliding = false;

            this.timer = -1;

            //壁に掴まるようにする
            this.rigidbody.velocity = new Vector2(0, 0);

            //壁との接触位置を取得する
            Vector2 hitpos = collision.ClosestPoint(transform.position);

            //壁に向かって移動できないようにする
            if (hitpos.x > transform.position.x)
            {
                this.wallState = IsWall.left;
            }
            else
            {
                this.wallState = IsWall.right;
            }

            //壁登り
            if (Input.GetKey(KeyCode.W))
            {
                this.rigidbody.velocity = new Vector2(0, this.wallSpeed);
            }

            //壁ジャンプ
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {

                this.isLeave = true;

                //左右どちらに飛ぶか
                if(hitpos.x > transform.position.x)
                {

                    this.rigidbody.AddForce(new Vector2(-1, 1) * this.wallJumpForce,ForceMode2D.Impulse);

                }
                else if (hitpos.x < transform.position.x)
                {
                    
                    this.rigidbody.AddForce(new Vector2(1, 1) * this.wallJumpForce,ForceMode2D.Impulse);
                }
            }
        }
        */

        //風
        if(collision.gameObject.tag == "Wind")
        {
            if(GameManager.Instance.GetState() == GameManager.State.Glide)
            this.rigidbody.AddForce(new Vector2(1, 0) * this.windPower);
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
        if (Input.GetKey(KeyCode.W))
        {
            this.rigidbody.velocity = new Vector2(0, this.wallSpeed);
        }

        //壁ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {

            this.isLeave = true;

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
}


