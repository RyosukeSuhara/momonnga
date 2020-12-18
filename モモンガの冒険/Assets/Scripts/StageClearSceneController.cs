using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.HasKey(Common.PlayerPrefsKeyNameSelectStage))
        {
            //クリアしたステージを取得する
            string clearStage = PlayerPrefs.GetString(Common.PlayerPrefsKeyNameSelectStage);

            int clearStageInt = PlayerPrefs.GetInt(Common.PlayerPrefsKeyNameClearStage);

            //クリアしたステージごとに次のステージを開放する
            if (clearStage == Common.SceneNameStageOne && clearStageInt < 1)
            {
                PlayerPrefs.SetInt(Common.PlayerPrefsKeyNameClearStage, 1);
                PlayerPrefs.SetString(Common.PlayerPrefsKeyNameSelectStage, Common.SceneNameStageTwo);
                PlayerPrefs.Save();
            } else if (clearStage == Common.SceneNameStageTwo && clearStageInt < 2)
            {
                PlayerPrefs.SetInt(Common.PlayerPrefsKeyNameClearStage, 2);
                PlayerPrefs.SetString(Common.PlayerPrefsKeyNameSelectStage, Common.SceneNameStageThree);
                PlayerPrefs.Save();
            } else if (clearStage == Common.SceneNameStageThree && clearStageInt < 3)
            {
                PlayerPrefs.SetInt(Common.PlayerPrefsKeyNameClearStage, 3);
                PlayerPrefs.SetString(Common.PlayerPrefsKeyNameSelectStage, Common.SceneNameStageFour);
                PlayerPrefs.Save();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //〇ボタンでステージ選択画面に遷移
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectStageScene");
        }
    }
}
