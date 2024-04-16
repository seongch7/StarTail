using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManage : MonoBehaviour
{
    Texture2D click;
    Texture2D original;

    void Start()
    {
        click = Resources.Load<Texture2D>("Click");
        original = Resources.Load<Texture2D>("Original");
    }

    public void OnMouseOver()
    {
        Cursor.SetCursor(click, new Vector2(click.width / 3, 0), CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(original, new Vector2(0, 0), CursorMode.Auto);
    }
}
