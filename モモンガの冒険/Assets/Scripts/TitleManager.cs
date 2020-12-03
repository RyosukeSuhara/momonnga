using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ選択画面に飛ぶ
        //Aボタンは仮、本来は〇ボタン
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectStageScene");
        }

        //操作説明画面に飛ぶ
        //Bボタンは仮、本来はｘボタン
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("OperationDescriptionScene");
        }
    }
}
