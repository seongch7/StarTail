using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene //씬매니저에서 사용할 씬들
    {
        Unknown,
        Main,
        Rookie_Boss,
        Login
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }
}
