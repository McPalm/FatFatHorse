using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float plane;
    float ease = 1f;
    float slowEase = 1f;
    float Ease => Mathf.Min(ease, slowEase);

    public Mobile Follow;

    // Update is called once per frame
    void FixedUpdate()
    {
        ease = Follow.Suspend || Follow.Flinching  ? .1f : ease + .1f;
        ease = Mathf.Clamp01(ease);
        slowEase += .03f;
        slowEase = Mathf.Clamp01(slowEase);


        if (Follow.Grounded || Follow.Suspend)
            plane = Follow.Suspend ? Follow.transform.position.y : Follow.transform.position.y - Follow.radius;
        else
            plane = Mathf.Clamp(plane, Follow.transform.position.y - 3, Follow.transform.position.y);
        var y = Mathf.Clamp(plane, transform.position.y - Time.fixedDeltaTime * 7f, transform.position.y + Time.fixedDeltaTime * 5f);
        y = Mathf.Clamp(y, Follow.transform.position.y - 3, Follow.transform.position.y + 3f);

        
        var x = Follow.transform.position.x;

        transform.position = Vector3.Lerp(transform.position,  new Vector3(x, y), Ease);
    }


    public void Lag()
    {
        slowEase = 0f;
    }
}
