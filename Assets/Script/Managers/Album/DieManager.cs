using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieManager : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt(AchiveManager.Achive.Unlock2.ToString(), 1);
    }
}
