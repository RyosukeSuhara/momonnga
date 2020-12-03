using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    //遷移先のシーンを入れておくオブジェクト
    [SerializeField]
    string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    }
}
