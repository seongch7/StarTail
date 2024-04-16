using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenManager : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt(AchiveManager.Achive.Unlock1.ToString(), 1);
    }
}
