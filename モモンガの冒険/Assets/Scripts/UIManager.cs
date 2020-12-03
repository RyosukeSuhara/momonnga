using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //シングルトン
    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    CountDownUIController countDownUIController;

    [SerializeField]
    TransitionUIController transitionUIController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //カウントダウンUIの更新
    public void CountDown(float timer)
    {
        this.countDownUIController.UpdateUI(timer);
    }

    public void FadeOut()
    {
        //this.transitionUIController.Fade(0.5f, 1f);

        this.transitionUIController.FadeOut();

        GameManager.Instance.ChangeState(GameManager.State.pause);
    }

    public void FadeIn()
    {
        //this.transitionUIController.Fade(0.0f, 1f);

        this.transitionUIController.FadeIn();

        GameManager.Instance.ChangeState(GameManager.State.Game);
    }
}
