using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt(AchiveManager.Achive.Unlock3.ToString(), 1);
    }
}
