using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 4;//移動速度

    [SerializeField]
    float wallSpeed = 3;//壁登りの速さ

    [SerializeField]
    float jumpForce = 300;//ジャンプ力

    [SerializeField]
    float wallJumpForce = 300;//壁掴まりジャンプ力

    Rigidbody2D rigidbody;//ジャンプ用

    bool isGround = true;//地面についているか

    float timer = 0;//滑空用タイマー

    public Image GlidingTimer;//滑空時間のUI

    bool Gliding = false;//滑空状態かどうか

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //左右方向のキー入力受付
        float move = Input.GetAxis("Horizontal");

        //移動
        transform.Translate(this.speed * move * Time.deltaTime, 0, 0);

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2) && this.isGround)
        {
            this.rigidbody.AddForce(transform.up * this.jumpForce);


            this.isGround = false;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            if (this.timer == 0)
            {
                this.timer = 5;

                this.Gliding = true;
            } else if (this.timer > 0 && this.Gliding)
            {
                this.Gliding = false;

            } else if (this.timer > 0 && !this.Gliding)
            {
                this.Gliding = true;
            }

        }

        if(this.timer > 0 && this.Gliding)
        {
            this.timer -= Time.deltaTime;

            this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, -0.1f);

            GlidingTimer.fillAmount -= 1.0f / 5.0f * Time.deltaTime;
        }

        if(this.timer < 0)
        {
            this.timer = 0;

            GlidingTimer.fillAmount = 1.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            this.isGround = true;

            this.timer = -1;
        }

        if(collision.gameObject.tag == "Wall")
        {
            this.isGround = true;

            this.timer = -1;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            this.rigidbody.velocity = new Vector2(0, 0);

            if (Input.GetKey(KeyCode.W))
            {
                this.rigidbody.velocity = new Vector2(0, this.wallSpeed);
            }

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2)){
                Vector2 hitpos = collision.ClosestPoint(transform.position);

                if(hitpos.x > transform.position.x)
                {
                    this.rigidbody.AddForce(new Vector2(-1, 1) * this.wallJumpForce);
                }else if(hitpos.x < transform.position.x)
                {
                    this.rigidbody.AddForce(new Vector2(1, 1) * this.wallJumpForce);
                }
            }
        }
        
    }


}
