using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ選択画面に遷移
        if (Input.GetKeyDown(KeyCode.Joystick1Button8))
        {
            if(Time.timeScale == 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("SelectStageScene");

                Time.timeScale = 1;
            }
        }
    }
}
