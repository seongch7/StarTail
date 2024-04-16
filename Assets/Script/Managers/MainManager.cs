using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private GameObject OptionPanel;
    [SerializeField]
    private GameObject Circle;
    
    public Animator FadeCanvas;
    private void Start()
    {
        Invoke("DeActive", 2.5f);
    }

    public void StartBtn()
    {
        Circle.SetActive(true);
        FadeOut();
        Invoke("LoadScene", 3.0f);
    }
    
    public void AlbumBtn()
    {
        Circle.SetActive(true);
        FadeOut();
        Invoke("LoadAlbum", 3.0f);
    }
    public void option_clicked()
    {
        OptionPanel.SetActive(true);
    }

    public void Exit()
    {
        OptionPanel.SetActive(false);
    }

    private void DeActive()
    {
        Circle.SetActive(false);
    }

    private void FadeOut()
    {
        FadeCanvas.SetBool("IsFadeOut", true);
    }
    
    private void LoadScene()
    {
        SceneManager.LoadScene("Test");
    }

    private void LoadAlbum()
    {
        SceneManager.LoadScene("Album");
    }

    private void LoadMain()
    {
        SceneManager.LoadScene("≈∏¿Ã∆≤");
    }

    public void ExitAlbum()
    {
        Circle.SetActive(true);
        FadeOut();
        Invoke("LoadMain", 3.0f);
    }
}
