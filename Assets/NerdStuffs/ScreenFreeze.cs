using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour
{
    static ScreenFreeze _instance;
    static public float GameSpeed { get; set; } = 1f;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    static public void Freeze(int frames)
    {
        if (frames == 0) return;
        if(false == _instance)
        {
            GameSpeed = Time.timeScale;
            var o = new GameObject("Screen Freeze");
            _instance = o.AddComponent<ScreenFreeze>();
        }
        var converted = frames / 60f;
        _instance.duration = Mathf.Max(converted, _instance.duration);
    }

    float duration = 0f;

	// Update is called once per frame
	void Update ()
    {
        if (duration > 0f)
        {
            Time.timeScale = 0f;
            duration -= Time.unscaledDeltaTime;
        }
        else
            Time.timeScale = GameSpeed;
	}
}
