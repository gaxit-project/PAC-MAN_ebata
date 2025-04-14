using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;
using System;

public class GameController : MonoBehaviour
{

    public static bool isCleared = false; //ラウンドをクリアしたかどうか(=クッキーを全て食べたかどうか)
    public static int currentRound = 1; //現在のラウンド
    public static bool canPlayerMove = false; //プレイヤーが動けるかどうか
    public static bool canGhostMove = false; //ゴーストが動けるかどうか
    public static bool canEatGhost = false; //ゴーストを食べられるかどうか
    public static int effectTime = 5000; //パワークッキーの効果時間
    public static bool isGameOver = false; //ゲームオーバーになったかどうか
    [SerializeField]public static float ghostSpeed = 1.1f; //ゴーストのデフォルトの速度

    private GameObject[] restOfCookie; //盤面に残っているクッキーの数
    private GameObject[] restOfPowerCookie; //盤面に残っているパワークッキーの数
    private int cookieCount; //上2つの合計
    private int life = 2; //残機数
    private int eatGhostCounter = 1; //効果期間内に食べたゴーストの数
    [SerializeField] private GameObject cookies; //クッキー+パワークッキー
    [SerializeField] private GameObject player; //プレイヤー
    [SerializeField] private GameObject ghosts; //ゴースト(全種類)
    //ゴースト(個体別)
    [SerializeField] private GameObject Blinky; 
    [SerializeField] private GameObject Pinky;
    [SerializeField] private GameObject Inky;
    [SerializeField] private GameObject Clyde;
    //UI管理用
    [SerializeField] private TextMeshProUGUI roundUI;
    [SerializeField] private TextMeshProUGUI lifeUI;
    [SerializeField] private GameObject ready;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject QuitUI;
    [SerializeField] private GameObject RestartUI;

    void Start()
    {
        Invoke("GameStart", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        roundUI.text = currentRound.ToString(); //ラウンド数を表示

        //残クッキーの数を取得
        restOfCookie = GameObject.FindGameObjectsWithTag("Cookie");
        restOfPowerCookie = GameObject.FindGameObjectsWithTag("PowerCookie");
        cookieCount = restOfCookie.Length + restOfPowerCookie.Length;

        if(Input.GetKeyDown(KeyCode.Q) && !isGameOver)
        {
            Debug.Log("残りクッキー：" + cookieCount + "個");
        }

        //残クッキー数が0ならクリア
        if(cookieCount == 0 && !isCleared && !isGameOver)
        {
            isCleared = true;
            Invoke("GameClear", 0f);
        }

        //ゲームオーバー Enterでリスタート
        if(isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                isGameOver = false;
                Invoke("GameStart", 0f);
            }
        }

    }
    private void GameStart()
    {
        FindObjectOfType<AudioManager>().PlaySound(5); //開始音を鳴らす
        //全てのクッキーを復活させる
        foreach (Transform child in cookies.transform)
        {
            child.gameObject.SetActive(true);
        }
        FindObjectOfType<ScoreController>().InitializeScore(); //スコアを0に戻す
        //プレイヤーとゴーストの初期化
        player.SetActive(true);
        ghosts.SetActive(true);
        player.transform.position = new Vector3(0f, 8.8f, 0.4f);
        Blinky.transform.position = new Vector3(0f, 10.6f, 0.4f);
        Pinky.transform.position = new Vector3(0f, 10.15f, 0.4f);
        //Inky.transform.position = new Vector3(0.3f, 10.15f, 0.4f);
        //Clyde.transform.position = new Vector3(-0.3f, 10.15f, 0.4f);
        Invoke("StopAnimation", 0f); //アニメーションを停止させる
        Invoke("StartMoving", 4.7f); //指定秒数後に動き始める
        //UIの初期化
        life = 2; //残機の初期化
        lifeUI.text = life.ToString();
        GameOverUI.SetActive(false);
        RestartUI.SetActive(false);
        ready.SetActive(true);
    }

    private void GameRestart()
    {
        //プレイヤーとゴーストを最初の位置に戻す
        player.SetActive(true);
        ghosts.SetActive(true);
        player.transform.position = new Vector3(0f, 8.8f, 0.4f);
        Blinky.transform.position = new Vector3(0f, 10.6f, 0.4f);
        Pinky.transform.position = new Vector3(0f, 10.15f, 0.4f);
        //Inky.transform.position = new Vector3(0.3f, 10.15f, 0.4f);
        //Clyde.transform.position = new Vector3(-0.3f, 10.15f, 0.4f);

        Invoke("StopAnimation", 0f); //アニメーションを停止させる
        Invoke("StartMoving", 2f); //指定秒数後に動き始める
        lifeUI.text = life.ToString(); //残機表示の更新
        ready.SetActive(true); //UIの表示
        QuitUI.SetActive(true);

    }

    private void StartMoving()
    {
        //プレイヤーとゴーストが動けるようにする
        canPlayerMove = true;
        canGhostMove = true;
        FindObjectOfType<PlayerController>().InitializePlayerMove();
        FindObjectOfType<BlinkyController>().InitializeBlinkyMove();
        FindObjectOfType<PinkyController>().InitializePinkyMove();
        // FindObjectOfType<InkyController>().InitializeInkyMove();
        // FindObjectOfType<ClydeController>().InitializeClydeMove();

        ready.SetActive(false); //UIを非表示にする
        QuitUI.SetActive(false);
        FindObjectOfType<AudioManager>().PlayBGM(0); //BGMを流す
    }

    private async void GameClear()
    {
        FindObjectOfType<AudioManager>().StopBGM(); //BGMを消す
        ghosts.SetActive(false); //ゴーストを消す
        Debug.Log("ゲームクリア！！");
        await Task.Delay(3000);
        Invoke("Clear", 0f);
    }

    public void Clear() //クリア後のステージの初期化
    {
        Invoke("GameRestart", 0f);
        //全てのクッキーを復活させる
        foreach (Transform child in cookies.transform)
        {
            child.gameObject.SetActive(true);
        }

        //プレイヤーとゴーストが動かないようにする
        canPlayerMove = false;
        canGhostMove = false;

        isCleared = false; //初期化
        currentRound++; //ラウンド数を増やす
        
    }

    public async void Miss()
    {
        FindObjectOfType<AudioManager>().StopBGM(); //BGMを消す
        //プレイヤーとゴーストが動かないようにする
        canPlayerMove = false;
        canGhostMove = false;
        FindObjectOfType<BlinkyController>().StopBlinkyAnimation();
        FindObjectOfType<PinkyController>().StopPinkyAnimation();
        //FindObjectOfType<InkyController>().StopInkyAnimation();
        //FindObjectOfType<ClydeController>().StopClydeAnimation();
        if(life > 0)
        {
            //残機を1減らして再開
            life--;
            Invoke("GameRestart", 4f);
        }
        else
        {
            //GameOver
            Invoke("GameOver", 4f);
        }
        //やられ演出用
        await Task.Delay(1000);
        ghosts.SetActive(false);
        await Task.Delay(1600);
        player.SetActive(false);
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
        GameOverUI.SetActive(true); //UIの表示
        QuitUI.SetActive(true);
        RestartUI.SetActive(true);
        isGameOver = true;
    }

    public async void PowerCookie()
    {
        FindObjectOfType<AudioManager>().PlayBGM(1); //BGMを変える
        eatGhostCounter = 1;
        canEatGhost = true;
        FindObjectOfType<BlinkyController>().EatPowerCookie();
        FindObjectOfType<PinkyController>().EatPowerCookie();
        //FindObjectOfType<InkyController>().EatPowerCookie();
        //FindObjectOfType<ClydeController>().EatPowerCookie();
        await Task.Delay(effectTime);
        canEatGhost = false;
        FindObjectOfType<AudioManager>().PlayBGM(0); //BGMを戻す
    }

    public void EatGhost()
    {
        FindObjectOfType<AudioManager>().PlayBGM(2); //BGMを変える
        FindObjectOfType<ScoreController>().addScore(100 * ((int)Math.Pow(2, eatGhostCounter)));
        Debug.Log(100 * ((int)Math.Pow(2, eatGhostCounter)) + "pt追加");
        eatGhostCounter++;
    }

    public void addLife() //1UP
    {
        life++;
    }

    private void StopAnimation() //各オブジェクトのアニメーションを停止させる
    {
        FindObjectOfType<PlayerController>().StopPlayerAnimation();
        FindObjectOfType<BlinkyController>().StopBlinkyAnimation();
        FindObjectOfType<PinkyController>().StopPinkyAnimation();
        //FindObjectOfType<InkyController>().StopInkyAnimation();
        //FindObjectOfType<ClydeController>().StopClydeAnimation();
    }

}
