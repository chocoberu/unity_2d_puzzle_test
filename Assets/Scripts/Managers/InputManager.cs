using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    public Action<Define.TouchEvent> TouchAction = null;

    //bool _pressed = false;
    //float _pressedTime = 0.0f;

    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();
        if(MouseAction != null)
        {
            // TODO
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
        TouchAction = null;
    }
}
