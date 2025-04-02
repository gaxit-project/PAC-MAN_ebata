using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2f; //プレイヤーの動く速さ

    [SerializeField]private Rigidbody rigidbody; //プレイヤーのregidbodyを入れる


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.velocity = transform.up * playerSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.velocity = -transform.up * playerSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.velocity = transform.right * playerSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.velocity = -transform.right * playerSpeed * Time.deltaTime;
        }
    }
}
