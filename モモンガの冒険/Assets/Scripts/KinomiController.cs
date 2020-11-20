using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinomiController : MonoBehaviour
{
    //きのみ落下用
    Rigidbody2D rigidbody;

    //プレイヤーの位置把握用
    public GameObject Player;

    //初期位置保存用
    Vector2 StartPosition;

    //落下開始距離
    int distance = 3;

    float mini = -0.01f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.StartPosition = transform.position;

        GameManager.Instance.AddListener(ReturnPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが範囲内に入ったら落ちる
        if ((-this.distance < transform.position.x - this.Player.transform.position.x && transform.position.x - this.Player.transform.position.x < this.distance) ||
            (-this.distance < this.Player.transform.position.x - transform.position.x && this.Player.transform.position.x - transform.position.x < this.distance))

        {
            this.rigidbody.constraints = RigidbodyConstraints2D.None;
            this.rigidbody.WakeUp();
        }
    }

    void ReturnPosition()
    {
        transform.position = StartPosition;

        this.rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
