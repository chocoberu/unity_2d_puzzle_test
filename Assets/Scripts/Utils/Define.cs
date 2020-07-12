using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Block
    {
        Red,
        Green,
        Blue,
        Size
    }
    public enum SuperBlock
    {
        Bomb,
        Eraser
    }
    public enum MouseEvent
    {
        Press,
        Down,
        Up,
        Click,
        Drag,
    }
    public enum TouchEvent
    {
        Press,
        Click,
        Drag,

    }
    public enum State
    {
        Idle,
        CatchBlock,
    }
}
