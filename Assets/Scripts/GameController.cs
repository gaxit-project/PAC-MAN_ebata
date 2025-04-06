using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{

    public static bool isCleared = false;
    public static int currentRound = 1;

    private GameObject[] restOfCookie;
    private GameObject[] restOfPowerCookie;
    private int cookieCount;
    [SerializeField] private GameObject cookies;
    [SerializeField] private GameObject player;

    // Update is called once per frame
    void Update()
    {
        restOfCookie = GameObject.FindGameObjectsWithTag("Cookie");
        restOfPowerCookie = GameObject.FindGameObjectsWithTag("PowerCookie");
        cookieCount = restOfCookie.Length + restOfPowerCookie.Length;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("残りクッキー：" + cookieCount + "個");
        }

        if(cookieCount == 0 && !isCleared)
        {
            isCleared = true;
            Invoke("GameClear", 0f);
        }

    }

    private async void GameClear()
    {
        Debug.Log("ゲームクリア！！");
        await Task.Delay(3000);
        Invoke("Clear", 0f);
    }

    public void Clear()
    {

        player.transform.position = new Vector3(0f, 8.8f, 0.4f);

        foreach (Transform child in cookies.transform)
        {
            child.gameObject.SetActive(true);
        }

        isCleared = false;
        currentRound++;
        
    }
}
