using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1.05f; //プレイヤーの動く速さ

    [SerializeField] private new Rigidbody rigidbody; //プレイヤーのregidbodyを入れる
    [SerializeField] private GameObject WarpPosition_1;
    [SerializeField] private GameObject WarpPosition_2;
    private GameObject WarpPosition;
    private bool gameClear;
    private bool canMoveUp = false;
    private bool canMoveDown = false;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = true;
    private bool isMovingRight = false;
    private float moveAxis_x = 8.8f; //横移動(y座標)
    private float moveAxis_y = 0f; //縦移動(x座標)
    public static Vector3 currentPlayerPosition;

    private bool canMove;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.4f);
        gameClear = GameController.isCleared;
        canMove = GameController.canPlayerMove;

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

        if (isMovingUp && canMoveUp && canMove && !gameClear)
        {
            transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
            transform.position += transform.up * playerSpeed * Time.deltaTime;
        }
        if (isMovingDown && canMoveDown && canMove && !gameClear)
        {
            transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
            transform.position += -transform.up * playerSpeed * Time.deltaTime;
        }
        if (isMovingLeft && canMoveLeft && canMove && !gameClear)
        {
            transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
            transform.position += -transform.right * playerSpeed * Time.deltaTime;
        }
        if (isMovingRight && canMoveRight && canMove && !gameClear)
        {
            transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
            transform.position += transform.right * playerSpeed * Time.deltaTime;
        }

        if(gameClear)
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    public void InitializePlayerMove()
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

    void OnTriggerEnter(Collider other)
    {
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
        else if(other.CompareTag("Point1"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
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
        else if(other.CompareTag("Ghost"))
        {
            FindObjectOfType<GameController>().Miss();
        }
    }
}