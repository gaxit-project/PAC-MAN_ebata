using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1.05f; //プレイヤーの動く速さ

    [SerializeField] private new Rigidbody rigidbody; //プレイヤーのregidbodyを入れる
    [SerializeField] private Animator PlayerAnim; //プレイヤーのアニメーターを入れる
    //WarpPointの座標取得とOn/Off切り替え用
    [SerializeField] private GameObject WarpPosition_1;
    [SerializeField] private GameObject WarpPosition_2;
    private GameObject WarpPosition; //ワープ先の座標
    private bool gameClear; //すべてのクッキーを取ったかどうか
    //動く方向を管理する用
    //その方向に動くことが出来るかどうか
    private bool canMoveUp = false;
    private bool canMoveDown = false;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    //今どの方向に移動しているか
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = true;
    private bool isMovingRight = false;
    private bool canEatGhost; //ゴーストを食べられるかどうか
    private float moveAxis_x = 8.8f; //横移動(y座標)
    private float moveAxis_y = 0f; //縦移動(x座標)
    public static Vector3 currentPlayerPosition; //最後に触れたPointの座標

    private bool canMove; //プレイヤーが動けるかどうか

    // Update is called once per frame
    void Update()
    {
        //z軸がズレないようにする
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.4f);
        //GameControllerから取得
        gameClear = GameController.isCleared;
        canMove = GameController.canPlayerMove;
        canEatGhost = GameController.canEatGhost;

        //プレイヤーの動きを制御する(入力した方向に動けるかを確認してからその方向のみに動くようにする)
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !gameClear && canMoveUp)
        {
            isMovingUp = true;
            isMovingDown = false;
            isMovingLeft = false;
            isMovingRight = false;
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !gameClear && canMoveDown)
        {
            isMovingUp = false;
            isMovingDown = true;
            isMovingLeft = false;
            isMovingRight = false;
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !gameClear && canMoveRight)
        {
            isMovingUp = false;
            isMovingDown = false;
            isMovingLeft = false;
            isMovingRight = true;
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !gameClear && canMoveLeft)
        {
            isMovingUp = false;
            isMovingDown = false;
            isMovingLeft = true;
            isMovingRight = false;
        }

        //プレイヤーの座標を管理する(動ける場合のみ軸に合わせて動く)
        if (isMovingUp && canMoveUp && canMove && !gameClear)
        {
            PlayerAnim.Play("Player_Up");
            transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
            transform.position += transform.up * playerSpeed * Time.deltaTime;
        }
        if (isMovingDown && canMoveDown && canMove && !gameClear)
        {
            PlayerAnim.Play("Player_Down");
            transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
            transform.position += -transform.up * playerSpeed * Time.deltaTime;
        }
        if (isMovingLeft && canMoveLeft && canMove && !gameClear)
        {
            PlayerAnim.Play("Player_Left");
            transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
            transform.position += -transform.right * playerSpeed * Time.deltaTime;
        }
        if (isMovingRight && canMoveRight && canMove && !gameClear)
        {
            PlayerAnim.Play("Player_Right");
            transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
            transform.position += transform.right * playerSpeed * Time.deltaTime;
        }

        if ((GetComponent<Rigidbody>().IsSleeping()) && canMove)
        {
            PlayerAnim.speed = 0;
        }
        else if (canMove)
        {
            PlayerAnim.speed = 1;
        }
    }

    public void InitializePlayerMove() //動きを制御するbool変数の初期化
    {
        canMoveUp = false;
        canMoveDown = false;
        canMoveLeft = true;
        canMoveRight = true;
        isMovingUp = false;
        isMovingDown = false;
        isMovingLeft = true;
        isMovingRight = false;
    }

    public void StopPlayerAnimation() //アニメーションを停止させる
    {
        if (!canMove)
        {
            PlayerAnim.speed = 0;
        }
    }

    void OnTriggerEnter(Collider other) //何かしらに触れた際に処理を行う
    {
        //ワープ関係
        if(other.CompareTag("WarpPoint_1") || other.CompareTag("WarpPoint_3"))
        {
            WarpPosition = WarpPosition_2;
            WarpPosition_2.SetActive(false);
            WarpPosition_1.SetActive(true);
            transform.position = WarpPosition.transform.position;
            Debug.Log("プレイヤー 右にワープ");
        }
        else if(other.CompareTag("WarpPoint_2") || other.CompareTag("WarpPoint_4"))
        {
            WarpPosition = WarpPosition_1;
            WarpPosition_1.SetActive(false);
            WarpPosition_2.SetActive(true);
            transform.position = WarpPosition.transform.position;
            Debug.Log("プレイヤー 左にワープ");
        }
        else if(other.CompareTag("Recover"))
        {
            WarpPosition_1.SetActive(true);
            WarpPosition_2.SetActive(true);
            Debug.Log("プレイヤー用のワープポイントが回復");
        }
        //クッキー関係
        else if(other.CompareTag("Cookie"))
        {
            other.GetComponent<CookieController>().EatCookie();
            //Debug.Log("クッキーを食べた");
        }
        else if(other.CompareTag("PowerCookie"))
        {
            other.GetComponent<PowerCookieController>().EatPowerCookie();
            Debug.Log("パワークッキーを食べた");
        }
        //プレイヤーの動き関係
        else if(other.CompareTag("Point1"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point2"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = false;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point3"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = false;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point4"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = true;
            canMoveDown = false;
            canMoveLeft = true;
            canMoveRight = true;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point5"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = false;
            canMoveRight = true;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point6"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = true;
            canMoveDown = false;
            canMoveLeft = false;
            canMoveRight = true;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point7"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = false;
            canMoveDown = true;
            canMoveLeft = false;
            canMoveRight = true;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point8"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = true;
            canMoveDown = false;
            canMoveLeft = true;
            canMoveRight = false;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point9"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            transform.position = new Vector3(moveAxis_y, moveAxis_x, 0.4f);
            canMoveUp = false;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = false;
            currentPlayerPosition = other.transform.position;
        }
        else if(other.CompareTag("Point10"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = false;
            canMoveRight = false;
        }
        else if(other.CompareTag("Point11"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = false;
            canMoveDown = false;
            canMoveLeft = true;
            canMoveRight = true;
        }
        else if(other.CompareTag("Point12"))
        {
            currentPlayerPosition = other.transform.position;
        }
        //ゴースト
        else if(other.CompareTag("Ghost"))
        {
            if(!canEatGhost)
            {
                Invoke("Miss", 0f);
            }
            else
            {
                string ghostName = other.gameObject.name;
                if(ghostName == "Blinky")
                {
                    FindObjectOfType<BlinkyController>().Eated();
                }
                else if(ghostName == "Pinky")
                {
                    FindObjectOfType<PinkyController>().Eated();
                }
                else if(ghostName == "Inky")
                {
                    //FindObjectOfType<InkyController>().Eated();
                }
                else if(ghostName == "Clyde")
                {
                    //FindObjectOfType<ClydeController>().Eated();
                }
            }
        }
    }

    public async void Miss()
    {
        PlayerAnim.speed = 0;
        FindObjectOfType<GameController>().Miss();
        await Task.Delay(1000);
        PlayerAnim.speed = 1;
        PlayerAnim.Play("Player_Miss");
        FindObjectOfType<AudioManager>().PlaySound(4);
    }
}