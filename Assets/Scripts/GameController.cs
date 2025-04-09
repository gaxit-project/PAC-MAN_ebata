using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;

public class GameController : MonoBehaviour
{

    public static bool isCleared = false;
    public static int currentRound = 1;
    public static bool canPlayerMove = false;
    public static bool canGhostMove = false;
    public static bool isGameOver = false; 
    [SerializeField]public static float ghostSpeed = 1.1f;

    private GameObject[] restOfCookie;
    private GameObject[] restOfPowerCookie;
    private int cookieCount;
    private int life = 2;
    [SerializeField] private GameObject cookies;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ghosts;
    [SerializeField] private GameObject Blinky;
    //[SerializeField] private GameObject Pinky;
    //[SerializeField] private GameObject Inky;
    //[SerializeField] private GameObject Clyde;
    [SerializeField] private TextMeshProUGUI roundUI;
    [SerializeField] private TextMeshProUGUI lifeUI;
    [SerializeField] private GameObject ready;
    [SerializeField] private GameObject GameOverUI;

    void Start()
    {
        Invoke("GameStart", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        roundUI.text = currentRound.ToString();

        restOfCookie = GameObject.FindGameObjectsWithTag("Cookie");
        restOfPowerCookie = GameObject.FindGameObjectsWithTag("PowerCookie");
        cookieCount = restOfCookie.Length + restOfPowerCookie.Length;

        if(Input.GetKeyDown(KeyCode.Q) && !isGameOver)
        {
            Debug.Log("残りクッキー：" + cookieCount + "個");
        }

        if(cookieCount == 0 && !isCleared && !isGameOver)
        {
            isCleared = true;
            Invoke("GameClear", 0f);
        }

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
        player.SetActive(true);
        foreach (Transform child in cookies.transform)
        {
            child.gameObject.SetActive(true);
        }
        FindObjectOfType<ScoreController>().InitializeScore();
        player.transform.position = new Vector3(0f, 8.8f, 0.4f);
        Blinky.transform.position = new Vector3(0f, 10.6f, 0.4f);
        life = 2;
        lifeUI.text = life.ToString();
        Invoke("StartMoving", 4.5f);
        GameOverUI.SetActive(false);
        ready.SetActive(true);
        ghosts.SetActive(true);
    }

    private void GameRestart()
    {
        player.SetActive(true);
        player.transform.position = new Vector3(0f, 8.8f, 0.4f);
        Blinky.transform.position = new Vector3(0f, 10.6f, 0.4f);
        Invoke("StartMoving", 2f);
        lifeUI.text = life.ToString();
        ready.SetActive(true);
        ghosts.SetActive(true);
    }

    private async void StartMoving()
    {
        FindObjectOfType<PlayerController>().InitializePlayerMove();
        FindObjectOfType<BlinkyController>().InitializeBlinkyMove();
        // FindObjectOfType<PinkyController>().InitializePinkyMove();
        // FindObjectOfType<InkyController>().InitializeInkyMove();
        // FindObjectOfType<ClydeController>().InitializeClydeMove();
        ready.SetActive(false);
        canPlayerMove = true;
        canGhostMove = true;
        await Task.Delay(100);
    }

    private async void GameClear()
    {
        ghosts.SetActive(false);
        Debug.Log("ゲームクリア！！");
        await Task.Delay(3000);
        Invoke("Clear", 0f);
    }

    public void Clear()
    {
        foreach (Transform child in cookies.transform)
        {
            child.gameObject.SetActive(true);
        }

        canPlayerMove = false;
        canGhostMove = false;
        isCleared = false;
        currentRound++;
        Invoke("GameRestart", 0f);
        
    }

    public async void Miss()
    {
        canPlayerMove = false;
        canGhostMove = false;
        if(life > 0)
        {
            life--;
            Invoke("GameRestart", 4f);
        }
        else
        {
            Invoke("GameOver", 4f);
        }
        await Task.Delay(1000);
        ghosts.SetActive(false);
        await Task.Delay(1500);
        player.SetActive(false);
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
        GameOverUI.SetActive(true);
        isGameOver = true;
    }

    public void PowerCookie()
    {
        
    }

    public void addLife()
    {
        life++;
    }

}
