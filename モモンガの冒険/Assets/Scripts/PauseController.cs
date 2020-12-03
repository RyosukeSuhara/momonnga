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
        if (Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            if(GameManager.Instance.GetState() == GameManager.State.pause)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("SelectStageScene");
            }
        }
    }
}
