using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectStageManager : MonoBehaviour
{
    //最初にステージ1が選択されているようにする
    Button firstSelect;

    //各ステージの開放状態の有無を持つための変数
    [SerializeField]
    Button SecondStageButton;

    [SerializeField]
    Button TherdStageButton;

    [SerializeField]
    Button ForthStageButton;

    bool SecondStageCanSelect = false;

    bool TherdStageCanSelect = false;

    bool ForthStageCanSelect = false;

    //クリアしたステージの情報を持つための変数
    int ClearReturnNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        //ステージをクリアしていた場合、クリアしたステージに応じてステージを開放する
        if (PlayerPrefs.HasKey(Common.PlayerPrefsKeyNameClearStage)){
            this.ClearReturnNumber = PlayerPrefs.GetInt(Common.PlayerPrefsKeyNameClearStage);                   

            if (this.ClearReturnNumber == 1)
            {
                this.SecondStageCanSelect = true;
            }
            else if (this.ClearReturnNumber == 2)
            {
                this.SecondStageCanSelect = true;
                this.TherdStageCanSelect = true;
            }
            else if(this.ClearReturnNumber == 3)
            {
                this.SecondStageCanSelect = true;
                this.TherdStageCanSelect = true;
                this.ForthStageCanSelect = true;
            }
        }

        //各ステージの選択の可否を更新する
        this.SecondStageButton.interactable = this.SecondStageCanSelect;
        this.TherdStageButton.interactable = this.TherdStageCanSelect;
        this.ForthStageButton.interactable = this.ForthStageCanSelect;

        //string firstSelectStage = PlayerPrefs.GetString(Common.PlayerPrefsKeyNameSelectStage,Common.SceneNameStageOne);
        string firstSelectStage = PlayerPrefs.GetString(Common.PlayerPrefsKeyNameSelectStage, Common.SceneNameStageOne);

        /*
        if(firstSelectStage == Common.SceneNameStageOne)
        {
            firstSelect = GameObject.Find("Canvas/StageOneButton").GetComponent<Button>();
        }else if (firstSelectStage == Common.SceneNameStageTwo)
        {
            firstSelect = GameObject.Find("Canvas/StageTwoButton").GetComponent<Button>();
        }else if (firstSelectStage == Common.SceneNameStageThree)
        {
            firstSelect = GameObject.Find("Canvas/StageThreeButton").GetComponent<Button>();
        }else if (firstSelectStage == Common.SceneNameStageFour)
        {
            firstSelect = GameObject.Find("Canvas/StageFourButton").GetComponent<Button>();
        }
        */

        //ステージ選択画面に移動したとき、最初にカーソルがあっているステージを一番最新のステージにする
        if(firstSelectStage == Common.SceneNameStageOne)
        {
            firstSelect = GameObject.Find("Canvas/StageOneButton").GetComponent<Button>();
        }else if (firstSelectStage == Common.SceneNameStageTwo)
        {
            firstSelect = GameObject.Find("Canvas/StageTwoButton").GetComponent<Button>();
        }else if (firstSelectStage == Common.SceneNameStageThree)
        {
            firstSelect = GameObject.Find("Canvas/StageThreeButton").GetComponent<Button>();
        }else if (firstSelectStage == Common.SceneNameStageFour)
        {
            firstSelect = GameObject.Find("Canvas/StageFourButton").GetComponent<Button>();
        }
        firstSelect.Select();


    }

    // Update is called once per frame
    void Update()
    {
        //ｘボタンを押したらタイトル画面に遷移する
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }

    public void a(string b)
    {
        //移動したステージを保存しておいて、クリアしたら次のステージを開放するようにする
        PlayerPrefs.SetString(Common.PlayerPrefsKeyNameSelectStage, b);
        PlayerPrefs.Save();

        //シーン切り替え
        UnityEngine.SceneManagement.SceneManager.LoadScene(b);
    }
}
