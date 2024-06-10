using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this); // 상속받은 것들은 빠르게 close
    }
}
