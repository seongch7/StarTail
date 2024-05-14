using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionSetting : MonoBehaviour
{
    private GameObject player;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        player.transform.position = new Vector2(-2, -3.5f);
    }
}
