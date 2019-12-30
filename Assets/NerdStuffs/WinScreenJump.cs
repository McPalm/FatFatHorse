using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenJump : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DJ>().PlayCreditSong();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            MySceneManager.NextStage();
    }
}
