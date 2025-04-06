using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2f; //プレイヤーの動く速さ

    [SerializeField] private new Rigidbody rigidbody; //プレイヤーのregidbodyを入れる
    [SerializeField] private GameObject WarpPosition_1;
    [SerializeField] private GameObject WarpPosition_2;
    private GameObject WarpPosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.velocity = transform.up * playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.velocity = -transform.up * playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.velocity = transform.right * playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.velocity = -transform.right * playerSpeed * Time.deltaTime;
        }
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
            Debug.Log("ワープポイントが回復");
        }
        else
        {
            Debug.LogError("タグが設定されていません！");
        }

    }
}