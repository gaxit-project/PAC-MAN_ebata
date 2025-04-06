using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherKeysController : MonoBehaviour
{

    [SerializeField] private GameObject cookies;

    [SerializeField] private string loadScene;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            // 子オブジェクトをループして取得
            foreach (Transform child in cookies.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
