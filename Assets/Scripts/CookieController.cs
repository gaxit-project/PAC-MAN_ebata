using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EatCookie()
    {
        FindObjectOfType<AudioManager>().PlaySound(0);
        FindObjectOfType<ScoreController>().addScore(10);
        this.gameObject.SetActive(false);
    }
}
