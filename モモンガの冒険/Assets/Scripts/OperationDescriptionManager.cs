using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OperationDescriptionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ選択画面に遷移
        //Aボタンは仮、本来は〇ボタン
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectStageScene");
        }

        //タイトル画面に遷移
        //Bボタンは仮、本来はｘボタン
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}
