using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITitle : UIBase
{
    [SerializeField] private Button btnGameStart;
    [SerializeField] private Button btnOption;
    [SerializeField] private Button btnShop;


    // Title UI�� ȣ������ �� �ʱ�ȭ ������ �����ϴ� �޼���
    protected override void OnOpen()
    {
        base.OnOpen();
        btnGameStart?.onClick.AddListener(OnClickBtnGameStart);
        btnOption?.onClick.AddListener(OnClickBtnOption);
        btnShop?.onClick.AddListener(OnClickBtnShop);
        // cur, best ���ھ� ���� �޾ƿ���
        // �޾ƿ� ������ UI�� �����ִ� �޼���
    }

    protected override void OnClose()
    {
        base.OnClose();
        btnGameStart?.onClick.RemoveListener(OnClickBtnGameStart);
        btnOption?.onClick.RemoveListener(OnClickBtnOption);
        btnShop?.onClick.RemoveListener(OnClickBtnShop);
    }
    
    public void OnClickBtnGameStart()
    {
        // �� �ε�
    }

    public void OnClickBtnOption()
    {
        // UIOption �� ����
    }

    public void OnClickBtnShop()
    {
        // UIShop �� ����
        // ���� ���� ��Ⱑ ��� ������� �մϴ�.
    }
}
