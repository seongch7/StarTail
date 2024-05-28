using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;
    }

    public void GameStart()
    {
        Managers.Scene.LoadScene(Define.Scene.Rookie_Boss);
    }
        

    public override void Clear()
    {

    }
}
