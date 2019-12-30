using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInput : MonoBehaviour
{
    bool touch = false;

    public InputToken Token { get; private set; }

    private void Start()
    {
        Token = new InputToken();
        foreach (var item in GetComponents<IControllable>())
        {
            item.InputToken = Token;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Fire2"))
            MySceneManager.ReloadStage(); // this does not even like remotely belong here.
        else
            RegularControls();
    }

    private void OnDisable()
    {
        Token.SetDirection(Vector2.zero);
    }

    void RegularControls()
    {
        float deadzone = .3f;
        var h = Input.GetAxis("Horizontal");
        if (Mathf.Abs(h) < deadzone)
            h = 0f;
        var v = Input.GetAxis("Vertical");
        if (Mathf.Abs(v) < deadzone)
            v = 0f;
        Token.SetDirection(new Vector2(h, v));

        if (Input.GetButtonDown("Jump"))
            Token.PressJump();
        if (Input.GetButtonDown("Fire1"))
            Token.PressAttack();
        if (Input.GetButtonDown("Fire3"))
            Token.PressSpecial();
        Token.HoldBlock = Input.GetButton("Fire2");
        Token.HoldJump = Input.GetButton("Jump");

    }

    Vector2 touchPosition;
    Vector2Int heldDirection;

    void TouchControls()
    {
        if(Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            heldDirection = Vector2Int.zero;
        }
        else if(Input.GetMouseButton(0))
        {
            touchPosition = touchPosition * .75f + (Vector2)Input.mousePosition * .25f;
            var delta = (Vector2)Input.mousePosition - touchPosition;
            var x = Mathf.RoundToInt(delta.x / 25f);
            var y = Mathf.RoundToInt(delta.y / 25f);

            if (x != 0)
                x = x < 0 ? -1 : 1;
            else
                x = heldDirection.x;
            if (y != 0)
                y = y < 0 ? -1 : 1;
            else if (heldDirection.y == -1)
                y = -1;
            else
                y = 0;
            if (heldDirection.y != y && y == 1)
                Token.PressJump();
            Token.HoldJump = true;
            heldDirection = new Vector2Int(x, y);
            Token.SetDirection(heldDirection);
        }
        else
        {
            Token.SetDirection(Vector2.zero);
            heldDirection = Vector2Int.zero;
            Token.HoldJump = false;
        }
    }
}
