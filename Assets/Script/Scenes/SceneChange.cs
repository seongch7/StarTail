using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour //프로토타입용 임시, 추후 삭제
{
    [SerializeField]
    private Canvas canvas;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canvas.GetComponent<FadeController>().FadeOut();
            Invoke("LoadScene", 1.1f);
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene("루키 (헬스장보스)");
    }
}
