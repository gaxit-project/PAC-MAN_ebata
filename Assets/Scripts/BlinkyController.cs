using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BlinkyController : MonoBehaviour
{
    private float speed; //ゴーストの移動速度 場合によって変動する

    [SerializeField] private Animator BlinkyAnim; //Blinkyのアニメーターを入れる
    //WarpPointの座標取得とOn/Off切り替え用
    [SerializeField] private GameObject WarpPosition_5; 
    [SerializeField] private GameObject WarpPosition_6;
    private GameObject WarpPosition;
    //プレイヤーを追跡するアルゴリズム用
    [SerializeField] private GameObject[] scanners = new GameObject[4]; //Scannerの座標取得用
    private float[] distances = new float[3]; //現在地からどの方向に移動するか計算する用
    private Vector3 targetPosition; //プレイヤーが最後に通過したPointの座標
    //動く方向を管理する用
    private bool canMove = false; //動けるかどうか
    //その方向に動けるかどうか
    private bool canMoveUp = false;
    private bool canMoveDown = false;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    //今度の方向に動いているか
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = true;
    private bool isMovingRight = false;
    private bool canEated = false; //プレイヤーに食べられるかどうか
    private bool isEated = false; //プレイヤーに食べられたかどうか
    private int effectTime; //パワークッキーの効果が持続する時間
    private bool isWhite = false; //点滅を管理する用
    private float moveAxis_x = 10.6f; //横移動(y座標)
    private float moveAxis_y = 0f; //縦移動(x座標)
    private int rnd; //乱数 方向をランダムに決める用

    // Start is called before the first frame update
    void Start()
    {
        speed = GameController.ghostSpeed; //速度を取得
    }

    // Update is called once per frame
    void Update()
    {
        //z軸がズレないようにする
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.4f);
        //GameControllerから取得
        canMove = GameController.canGhostMove;
        effectTime = GameController.effectTime;

        if(canMove)
        {
            BlinkyAnim.speed = 1;
            //ゴーストの動きを制御する
            if(isMovingUp && canMoveUp)
            {
                if(!canEated && !isEated)
                {
                    BlinkyAnim.Play("Blinky_Up");
                }
                else if(isEated)
                {
                    BlinkyAnim.Play("Eye_Up");
                }
                transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
                transform.position += transform.up * speed * Time.deltaTime;
            }
            if(isMovingDown && canMoveDown)
            {
                if(!canEated && !isEated)
                {
                    BlinkyAnim.Play("Blinky_Down");
                }
                else if(isEated)
                {
                    BlinkyAnim.Play("Eye_Down");
                }
                transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
                transform.position += -transform.up * speed * Time.deltaTime;
            }
            if(isMovingLeft && canMoveLeft)
            {
                if(!canEated && !isEated)
                {
                    BlinkyAnim.Play("Blinky_Left");
                }
                else if(isEated)
                {
                    BlinkyAnim.Play("Eye_Left");
                }
                transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
                transform.position += -transform.right * speed * Time.deltaTime;
            }
            if(isMovingRight && canMoveRight)
            {
                if(!canEated && !isEated)
                {
                    BlinkyAnim.Play("Blinky_Right");
                }
                else if(isEated)
                {
                    BlinkyAnim.Play("Eye_Right");
                }
                transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
                transform.position += transform.right * speed * Time.deltaTime;
            }
        }
    }

    public void InitializeBlinkyMove() //動きを制御するbool変数の初期化
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
    public void StopBlinkyAnimation()
    {
        BlinkyAnim.speed = 0;
    }

    public async void EatPowerCookie()
    {
        canEated = true;
        speed = GameController.ghostSpeed / 2;
        BlinkyAnim.Play("Ghost_Blue");
        //進行方向を反転させる
        if(isMovingUp)
        {
            isMovingUp = false;
            isMovingDown = true;
        }
        else if(isMovingDown)
        {
            isMovingDown = false;
            isMovingUp = true;
        }
        else if(isMovingLeft)
        {
            isMovingLeft = false;
            isMovingRight = true;
        }
        else if(isMovingLeft)
        {
            isMovingRight = false;
            isMovingLeft = true;
        }
        await Task.Delay(effectTime * 2 / 3);
        if(canEated)
        {
            Invoke("Blink", 0f);
            await Task.Delay(effectTime / 3);
            if(canEated)
            {
                speed = GameController.ghostSpeed;
                canEated = false;
            }
        }
    }

    private async void Blink() //点滅させる
    {
        if(canEated)
        {
            if(!isWhite)
            {
                isWhite = true;
                BlinkyAnim.Play("Ghost_Blink");
            }
            else
            {
                isWhite = false;
                BlinkyAnim.Play("Ghost_Blue");
            }
            await Task.Delay(250);
            Invoke("Blink", 0f);
        }
        else
        {
            isWhite = false;
        }
    }

    public void Eated()
    {
        FindObjectOfType<GameController>().EatGhost();
        FindObjectOfType<AudioManager>().PlaySound(3);
        FindObjectOfType<AudioManager>().PlayBGM(2);
        isEated = true;
        canEated = false;
        speed = GameController.ghostSpeed * 2;
    }

    void OnTriggerEnter(Collider other) //何かしらに触れた際に処理を行う
    {
        //ワープ関係
        if(other.CompareTag("WarpPoint_5"))
        {
            WarpPosition = WarpPosition_6;
            WarpPosition_6.SetActive(false);
            WarpPosition_5.SetActive(true);
            transform.position = WarpPosition.transform.position;
            Debug.Log("Blinky 右にワープ");
        }
        else if(other.CompareTag("WarpPoint_6"))
        {
            WarpPosition = WarpPosition_5;
            WarpPosition_5.SetActive(false);
            WarpPosition_6.SetActive(true);
            transform.position = WarpPosition.transform.position;
            Debug.Log("Blinky 左にワープ");
        }
        else if(other.CompareTag("Recover"))
        {
            WarpPosition_5.SetActive(true);
            WarpPosition_6.SetActive(true);
            Debug.Log("Blinky用のワープポイントが回復");
        }
        //目標位置の更新
        if(other.CompareTag("Point1") || other.CompareTag("Point2") || other.CompareTag("Point3") || other.CompareTag("Point4") || other.CompareTag("Point5") || other.CompareTag("Point6") || other.CompareTag("Point7") || other.CompareTag("Point8") || other.CompareTag("Point9"))
        {
            if(!isEated)
            {
                targetPosition = PlayerController.currentPlayerPosition;
            }
            else
            {
                targetPosition = new Vector3(0f, 10.6f, 0.4f);
            }
        }
        //ゴーストの移動関係+追跡アルゴリズム(目標位置に最短で到達できるように移動する)
        if(other.CompareTag("Point1"))
        {
            if(!canEated && !isEated)
            {
                speed = GameController.ghostSpeed;
            }
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
            if(isMovingUp)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    distances[2] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1] && distances[0] <= distances[2])
                    {
                        return;
                    }
                    else if(distances[1] <= distances[2])
                    {
                        isMovingUp = false;
                        isMovingLeft = true;
                    }
                    else
                    {
                        isMovingUp = false;
                        isMovingRight = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 4);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingUp = false;
                            isMovingLeft = true;
                            break;
                        case 3:
                             isMovingUp = false;
                            isMovingRight = true;
                            break;
                    }
                }
            }
            else if(isMovingDown)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    distances[2] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1] && distances[0] <= distances[2])
                    {
                        return;
                    }
                    else if(distances[1] <= distances[2])
                    {
                        isMovingDown = false;
                        isMovingLeft = true;
                    }
                    else
                    {
                        isMovingDown = false;
                        isMovingRight = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 4);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingDown = false;
                            isMovingLeft = true;
                            break;
                        case 3:
                             isMovingDown = false;
                            isMovingRight = true;
                            break;
                    }
                }
            }
            else if(isMovingLeft)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    distances[2] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1] && distances[0] <= distances[2])
                    {
                        isMovingLeft = false;
                        isMovingUp = true;
                    }
                    else if(distances[1] <= distances[2])
                    {
                        isMovingLeft = false;
                        isMovingDown = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 4);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingLeft = false;
                            isMovingUp = true;
                            break;
                        case 3:
                             isMovingLeft = false;
                            isMovingDown = true;
                            break;
                    }
                }
            }
            else if(isMovingRight)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    distances[2] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1] && distances[0] <= distances[2])
                    {
                        isMovingRight = false;
                        isMovingUp = true;
                    }
                    else if(distances[1] <= distances[2])
                    {
                        isMovingRight = false;
                        isMovingDown = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 4);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingRight = false;
                            isMovingUp = true;
                            break;
                        case 3:
                             isMovingRight = false;
                            isMovingDown = true;
                            break;
                    }
                }
            }
        }
        else if(other.CompareTag("Point2"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = false;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
            if(isMovingUp)
            {
                isMovingUp = false;
                if(canEated)
                {
                    distances[0] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingLeft = true;
                    }
                    else
                    {
                        isMovingRight = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            isMovingLeft = true;
                            break;
                        case 2:
                            isMovingRight = true;
                            break;
                    }
                }
            }
            if(isMovingLeft)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingLeft = false;
                        isMovingDown = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingLeft = false;
                            isMovingDown = true;
                            break;
                    }
                }
            }
            else if(isMovingRight)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingRight = false;
                        isMovingDown = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingRight = false;
                            isMovingDown = true;
                            break;
                    }
                }
            }
        }
        else if(other.CompareTag("Point3"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = false;
            if(isMovingUp)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        return;
                    }
                    else
                    {
                        isMovingUp = false;
                        isMovingLeft = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingUp = false;
                            isMovingLeft = true;
                            break;
                    }
                }
            }
            else if(isMovingDown)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        return;
                    }
                    else
                    {
                        isMovingDown = false;
                        isMovingLeft = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingDown = false;
                            isMovingLeft = true;
                            break;
                    }
                }
            }
            if(isMovingRight)
            {
                isMovingRight = false;
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingUp = true;
                    }
                    else
                    {
                        isMovingDown = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            isMovingUp = true;
                            break;
                        case 2:
                            isMovingDown = true;
                            break;
                    }
                }
            }
        }
        else if(other.CompareTag("Point4"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = false;
            canMoveLeft = true;
            canMoveRight = true;
            if(isMovingDown)
            {
                isMovingDown = false;
                if(!canEated)
                {
                    distances[0] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingLeft = true;
                    }
                    else
                    {
                        isMovingRight = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            isMovingLeft = true;
                            break;
                        case 2:
                            isMovingRight = true;
                            break;
                    }
                }
                
            }
            if(isMovingLeft)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[2].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingLeft = false;
                        isMovingUp = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingLeft = false;
                            isMovingUp = true;
                            break;
                    }
                }
            }
            else if(isMovingRight)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingRight = false;
                        isMovingUp = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingRight = false;
                            isMovingUp = true;
                            break;
                    }
                }
            }
        }
        else if(other.CompareTag("Point5"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = false;
            canMoveRight = true;
            if(isMovingUp)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        return;
                    }
                    else
                    {
                        isMovingUp = false;
                        isMovingRight = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingUp = false;
                            isMovingRight = true;
                            break;
                    }
                }
            }
            else if(isMovingDown)
            {
                if(!canEated)
                {
                    distances[0] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[3].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        return;
                    }
                    else
                    {
                        isMovingDown = false;
                        isMovingRight = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            break;
                        case 2:
                            isMovingDown = false;
                            isMovingRight = true;
                            break;
                    }
                }
            }
            if(isMovingLeft)
            {
                isMovingLeft = false;
                if(!canEated)
                {
                    distances[0] = (scanners[0].transform.position - targetPosition).sqrMagnitude;
                    distances[1] = (scanners[1].transform.position - targetPosition).sqrMagnitude;
                    if(distances[0] <= distances[1])
                    {
                        isMovingUp = true;
                    }
                    else
                    {
                        isMovingDown = true;
                    }
                }
                else
                {
                    rnd = UnityEngine.Random.Range(1, 3);
                    switch(rnd)
                    {
                        case 1:
                            isMovingUp = true;
                            break;
                        case 2:
                            isMovingDown = true;
                            break;
                    }
                }
            }
        }
        else if(other.CompareTag("Point6"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = false;
            canMoveLeft = false;
            canMoveRight = true;
            if(isMovingDown)
            {
                isMovingDown = false;
                isMovingRight = true;
            }
            else
            {
                isMovingLeft = false;
                isMovingUp = true;
            }
        }
        else if(other.CompareTag("Point7"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = false;
            canMoveDown = true;
            canMoveLeft = false;
            canMoveRight = true;
            if(isMovingUp)
            {
                isMovingUp = false;
                isMovingRight = true;
            }
            else
            {
                isMovingLeft = false;
                isMovingDown = true;
            }
        }
        else if(other.CompareTag("Point8"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = false;
            canMoveLeft = true;
            canMoveRight = false;
            if(isMovingDown)
            {
                isMovingDown = false;
                isMovingLeft = true;
            }
            else
            {
                isMovingRight = false;
                isMovingUp = true;
            }
        }
        else if(other.CompareTag("Point9"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = false;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = false;
            if(isMovingUp)
            {
                isMovingUp = false;
                isMovingLeft = true;
            }
            else
            {
                isMovingRight = false;
                isMovingDown = true;
            }
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
        //ワープ前後の通路で減速する
        else if(other.CompareTag("Decelerator"))
        {
            speed = GameController.ghostSpeed / 2f;
        }
        else if(other.CompareTag("Restart"))
        {
            if(isEated)
            {
                isEated = false;
                FindObjectOfType<AudioManager>().PlayBGM(0); //BGMを戻す
                speed = GameController.ghostSpeed;
                Invoke("InitializeBlinkyMove", 0f);
            }
        }
    }
}
