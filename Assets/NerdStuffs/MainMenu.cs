using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        DJ.Instance.PlayMenuMusic();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            StartGame();
    }

    public void StartGame()
    {
        DJ.Instance.PlayGameMusic();
        MySceneManager.LoadStage(1);
    }
}
