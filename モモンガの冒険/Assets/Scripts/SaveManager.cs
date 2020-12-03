using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //シングルトン
    static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SaveManager>();
            }
            return instance;
        }
    }

    //スコア
    int score;
    const string scoreKey = "Score";

    //ハイスコア
    int highScore;
    const string highScoreKey = "HighScore";

    //BGMボリューム
    int bgmVolume;
    const string bgmVolumeKey = "BGMVolume";

    //SEボリューム
    int seVolume;
    const string seVolumeKey = "SEVolume";

    //Voiceボリューム
    int voiceVolume;
    const string voiceVolumeKey = "VoiceVolume";


    //start関数より前に呼ばれる
    void Awake()
    {
        this.score = PlayerPrefs.GetInt(scoreKey, 0);
        this.highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        this.bgmVolume = PlayerPrefs.GetInt(bgmVolumeKey, 100);
        this.seVolume = PlayerPrefs.GetInt(seVolumeKey, 100);
        this.voiceVolume = PlayerPrefs.GetInt(voiceVolumeKey, 100);
    }

    //スコアを保存
    public void SaveScore(int score)
    {
        this.score = score;
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.Save();//ストレージに書き込み
    }

    //ハイスコアの更新チェック　戻り値として更新したならtrue、していないならfalse
    //一回しか呼んじゃダメ
    public bool CheckHighScore()
    {
        if(this.score > this.highScore)
        {
            this.highScore = this.score;
            PlayerPrefs.SetInt(highScoreKey, this.highScore);
            return true;
        }

        return false;
    }

    public int Score()
    {
        return this.score;
    }

    public int HighScore()
    {
        return this.highScore;
    }

    public void SaveVolume(int bgm, int se, int voice)
    {
        this.bgmVolume = bgm;
        this.seVolume = se;
        this.voiceVolume = voice;

        PlayerPrefs.SetInt(bgmVolumeKey, this.bgmVolume);
        PlayerPrefs.SetInt(seVolumeKey, this.seVolume);
        PlayerPrefs.SetInt(voiceVolumeKey, this.voiceVolume);
        PlayerPrefs.Save();
    }

    public int BGMVolume() { return this.bgmVolume; }
    public int SEVolume() { return this.seVolume; }
    public int VoiceVolume() { return this.voiceVolume; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
