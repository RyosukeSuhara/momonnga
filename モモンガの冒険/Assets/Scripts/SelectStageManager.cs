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

    // Start is called before the first frame update
    void Start()
    {
        firstSelect = GameObject.Find("Canvas/StageOneButton").GetComponent<Button>();
        firstSelect.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }

    public void a(string b)
    {
        //シーン切り替え
        UnityEngine.SceneManagement.SceneManager.LoadScene(b);
    }
}
