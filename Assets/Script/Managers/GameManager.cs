using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject ChatBubble;
    public TypeEffect talk;
    public Text talkText;
    public GameObject scanObect;

    public bool isAction = false;
    public int talkIndex;
    public int totalPoint;
    public int stagePoint;
    public int stageIndex = 0;
    public int Hp;

    public PlayerController player;
    public GameObject[] stages;

    public GameObject hill;
    private PolygonCollider2D hillCollider;
    [SerializeField]
    private GameObject Wood;
    private SpriteRenderer woodRend;
    [SerializeField]
    private GameObject Exit;

    [SerializeField]
    private GameObject OptionPanel;
    [SerializeField]
    private GameObject Circle;

    public Animator FadeCanvas;
    private void Start()
    {
        Invoke("DeActive", 2.5f);
        if (hill != null)
        {
            hillCollider = hill.GetComponent<PolygonCollider2D>();
        }
        woodRend = Wood.GetComponent<SpriteRenderer>();
        
    }

    public void Action(GameObject scanObj)
    {
        scanObect = scanObj;
        ObjectData objData = scanObect.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNPC);

        //대화창 활성화
        ChatBubble.SetActive(isAction);
    }

    private void Talk(int id, bool isNPC)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }
            

        if (isNPC)
        {
            //talkText.text = talkData;
            talk.SetMsg(talkData);
        }
        else
        {
            //talkText.text = talkData;
            talk.SetMsg(talkData);
        }

        isAction = true;
        talkIndex++;
    }
    public void NextStage()
    {
        if(stageIndex < stages.Length - 1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        {
            Time.timeScale = 0;
        }
        

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if(Hp > 0)
            Hp--;
        else
        {
            player.OnDie();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(Hp > 1)
                PlayerReposition();

            HealthDown();
        }

        if (collision.gameObject.name == "Entry")
            return;
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-7, 11.5f, -1);
        player.VelocityZero();
    }

    public void OpenWay()
    {
        if (hillCollider != null)
        {
            hillCollider.enabled = true;
        }
    }

    public void CloseWay()
    {
        if (hillCollider != null)
        {
            hillCollider.enabled = false;
        }
        DeActiveExit();
    }

    public void ActiveExit()
    {
        Exit.SetActive(true);
    }

    public void DeActiveExit()
    {
        Exit.SetActive(false);
    }
    public void ChangeLayer(string layerName) //추후 레이어를 바꿔야 할 오브젝트가 추가로 생기면 인자를 받는 함수로 개선
    {
        woodRend.sortingLayerName = layerName;
    }
    private void FadeOut()
    {
        FadeCanvas.SetBool("IsFadeOut", true);
    }
    private void DeActive()
    {
        Circle.SetActive(false);
    }
}
