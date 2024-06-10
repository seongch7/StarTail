using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    int _order = 10; //�ֱ� ����� UI ����

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); //���� �������� ���� UI�� ���� �����Ǳ� ���� Stack����
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root"); // Manager�� �������� ������ Find�ؿ�
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort) //sorting ��û ���� ��
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }

    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene //���ڴ� �ɼ�
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; //T�� �̸� ����

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup //���ڴ� �ɼ�
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name; //T�� �̸� ����

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

            return popup;
    }

    public void ClosePopupUI(UI_Popup popup) //����� Close
    {
        if (_popupStack.Count == 0)
            return;

        if(_popupStack.Peek() != null)
        {
            Debug.Log("Close fail");
            return;
        }
        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
}
