using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unlock : MonoBehaviour
{
    [SerializeField]
    private int page;

    private void Start()
    {
        switch (page)
        {
            case 1:
                PlayerPrefs.SetInt(AlbumManager.Achive.Unlock1.ToString(), 1);
                break;

            case 2:
                PlayerPrefs.SetInt(AlbumManager.Achive.Unlock2.ToString(), 1);
                break;

            case 3:
                PlayerPrefs.SetInt(AlbumManager.Achive.Unlock3.ToString(), 1);
                break;

            case 4:
                PlayerPrefs.SetInt(AlbumManager.Achive.Unlock4.ToString(), 1);
                break;

            case 5:
                PlayerPrefs.SetInt(AlbumManager.Achive.Unlock5.ToString(), 1);
                break;

            case 6:
                PlayerPrefs.SetInt(AlbumManager.Achive.Unlock6.ToString(), 1);
                break;

        }
    }
}
