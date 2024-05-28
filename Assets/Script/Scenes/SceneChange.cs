using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour //������Ÿ�Կ� �ӽ�, ���� ����
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
        SceneManager.LoadScene("��Ű (�ｺ�庸��)");
    }
}
