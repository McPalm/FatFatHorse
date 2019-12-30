using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public AudioClip WinJingle;

    static int current;

    bool dying = false;

    static MySceneManager _instance;
    public static MySceneManager Instance
    {
        get
        {
            if (_instance)
                return _instance;
            var go = new GameObject("Scene Manager");
            
            _instance = go.AddComponent<MySceneManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
            _instance = this;
        if (_instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    static public void Kill()
    {
        Instance.PlayKill();
    }

    void PlayKill() => StartCoroutine(KillRoutine());

    IEnumerator KillRoutine()
    {
        if (!dying)
        {
            dying = true;
            var playerAnim = FindObjectOfType<PlatformingCharacterAnimator>();
            playerAnim.PlayDeath();
            playerAnim.GetComponent<PlatformingCharacter>().enabled = false;
            FindObjectOfType<DJ>().PutASockInIt(1.5f);
            yield return new WaitForSeconds(1.5f);
            ReloadStage();
            dying = false;
        }
    }

    static public void ReloadStage()
    {
        LoadStage(current);
    }

    static public void NextStage()
    {
        current++;
        if (SceneManager.sceneCountInBuildSettings <= current + 1)
            SceneManager.LoadScene(0);
        else
            LoadStage(current);
    }

    public void WinStage() => StartCoroutine(WinRoutine());

    IEnumerator WinRoutine()
    {
        AudioPool.PlaySound(transform.position, WinJingle);
        FindObjectOfType<MyInput>().enabled = false;
        FindObjectOfType<DJ>().PutASockInIt(.9f);
        yield return new WaitForSeconds(1.2f);
        NextStage();
    }

    static public void LoadStage(int stage)
    {
        current = stage;
        // Load Gameplay
        SceneManager.LoadScene(1);
        // Load Stage
        SceneManager.LoadScene(stage + 1, LoadSceneMode.Additive);
    }
}
