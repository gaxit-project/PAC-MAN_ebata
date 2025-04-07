using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyController : MonoBehaviour
{
    private float speed;

    [SerializeField] private Material[] mat = new Material[3];
    [SerializeField] private GameObject[] scanners = new GameObject[4];
    private float[] distances = new float[3];
    private Vector3 targetPosition;
    private bool canMove = false;
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = true;
    private bool isMovingRight = false;
    private bool canMoveUp = false;
    private bool canMoveDown = false;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private float moveAxis_x = 10.6f; //横移動(y座標)
    private float moveAxis_y = 0f; //縦移動(x座標)

    // Start is called before the first frame update
    void Start()
    {
        speed = GameController.ghostSpeed;
        //int rnd = Random.Range(1, 11);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.4f);
        canMove = GameController.canGhostMove;

        if(canMove)
        {
            if(isMovingUp && canMoveUp)
            {
                transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
                transform.position += transform.up * speed * Time.deltaTime;
            }
            if(isMovingDown && canMoveDown)
            {
                transform.position = new Vector3(moveAxis_y, transform.position.y, 0.4f);
                transform.position += -transform.up * speed * Time.deltaTime;
            }
            if(isMovingLeft && canMoveLeft)
            {
                transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
                transform.position += -transform.right * speed * Time.deltaTime;
            }
            if(isMovingRight && canMoveRight)
            {
                transform.position = new Vector3(transform.position.x, moveAxis_x, 0.4f);
                transform.position += transform.right * speed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Point1") || other.CompareTag("Point2") || other.CompareTag("Point3") || other.CompareTag("Point4") || other.CompareTag("Point5") || other.CompareTag("Point6") || other.CompareTag("Point7") || other.CompareTag("Point8") || other.CompareTag("Point9") || other.CompareTag("Point12"))
        {
            targetPosition = PlayerController.currentPlayerPosition;
        }

        if(other.CompareTag("Point1"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
            if(isMovingUp)
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
            else if(isMovingDown)
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
            else if(isMovingLeft)
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
            else if(isMovingRight)
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
        }
        else if(other.CompareTag("Point2"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = false;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
            if(isMovingLeft)
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
            else if(isMovingRight)
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
            else if(isMovingDown)
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
        }
        else if(other.CompareTag("Point4"))
        {
            moveAxis_x = other.transform.position.y;
            moveAxis_y = other.transform.position.x;
            canMoveUp = true;
            canMoveDown = false;
            canMoveLeft = true;
            canMoveRight = true;
            if(isMovingLeft)
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
            else if(isMovingRight)
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
            else if(isMovingDown)
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
    }
}
