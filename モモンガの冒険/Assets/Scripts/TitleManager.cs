using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();

        PlayerPrefs.SetString(Common.PlayerPrefsKeyNameSelectStage, Common.SceneNameStageOne);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ選択画面に飛ぶ
        //Aボタンは仮、本来は〇ボタン
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            PlayerPrefs.SetString(Common.PlayerPrefsKeyNameSelectStage, Common.SceneNameStageOne);

            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectStageScene");
        }

        //操作説明画面に飛ぶ
        //Bボタンは仮、本来はｘボタン
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("OperationDescriptionScene");
        }

        //動作確認用
        //selectボタンを押すと全セーブデータを削除する
        if (Input.GetKeyDown(KeyCode.Joystick1Button8))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
