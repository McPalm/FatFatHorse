using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTextCycle : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public string[] lines;
    public float speed = 1f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                text.text = lines[i];
                yield return new WaitForSeconds(speed);
            }
        }
    }

}
