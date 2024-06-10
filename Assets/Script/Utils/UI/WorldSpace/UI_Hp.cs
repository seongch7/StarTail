using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hp : UI_Base
{
    private float PlayerLookDir;

    public enum Images
    {
        Hp1,
        Hp2,
        Hp3,
        Hp4
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
    }

    private void Update()
    {
        Transform parent = transform.parent;
        PlayerLookDir = parent.GetComponent<PlayerController>().isRight;
        transform.position = parent.position + Vector3.right * PlayerLookDir * (parent.GetComponent<CapsuleCollider2D>().bounds.size.x * -1) * 2.5f
            + Vector3.up * (parent.GetComponent<CapsuleCollider2D>().bounds.size.y * 1f);

        PrintHp();
    }

    public void SetHp(int hp)
    {
        switch (hp)
        {   
            case 0:
                GetImage((int)Images.Hp1).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                break;
            case 1:
                GetImage((int)Images.Hp2).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                break;
            case 2:
                GetImage((int)Images.Hp3).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                break;
            case 3:
                GetImage((int)Images.Hp4).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                break;
            default:
                break;
        }
    }

    public void PrintHp()
    {
        GetImage((int)Images.Hp1).GetComponent<Image>();
        GetImage((int)Images.Hp2).GetComponent<Image>();
        GetImage((int)Images.Hp3).GetComponent<Image>();
        GetImage((int)Images.Hp4).GetComponent<Image>();
    }
}
