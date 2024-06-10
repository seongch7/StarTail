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
        Managers.UI.ClosePopupUI(this); // ��ӹ��� �͵��� ������ close
    }
}
