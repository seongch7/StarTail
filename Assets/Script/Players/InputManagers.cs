using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<KeyCode> KeyAction = null;
    public Action<KeyCode> KeyDownAction = null;
    public void OnUpdate()
    {
        if (KeyAction != null)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                KeyAction.Invoke(KeyCode.RightArrow);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                KeyAction.Invoke(KeyCode.LeftArrow);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                KeyAction.Invoke(KeyCode.DownArrow);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                KeyAction.Invoke(KeyCode.DownArrow);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                KeyAction.Invoke(KeyCode.LeftShift);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                KeyAction.Invoke(KeyCode.LeftShift);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                KeyAction.Invoke(KeyCode.Space);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                KeyAction.Invoke(KeyCode.D);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                KeyAction.Invoke(KeyCode.A);
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
    }
}
