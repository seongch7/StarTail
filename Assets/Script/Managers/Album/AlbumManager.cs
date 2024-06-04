using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlbumManager : MonoBehaviour
{
    public GameObject[] LockImage;
    public GameObject[] UnlockImage;

    public enum Achive { Unlock1, Unlock2, Unlock3, Unlock4, Unlock5, Unlock6 }
    Achive[] achives;

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }
    public void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }
    void Start()
    {
        UnlockImages();
    }

    void UnlockImages()
    {
        for (int index = 0; index < LockImage.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            LockImage[index].SetActive(!isUnlock);
            UnlockImage[index].SetActive(isUnlock);
        }
    }
}
