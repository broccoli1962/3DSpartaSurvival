using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWaveInfo : UIBase
{
    [SerializeField] private TextMeshProUGUI WaveInfoTxt;
    [SerializeField] private TextMeshProUGUI WaveCountTxt;

    Coroutine coroutine;
    protected override void OnOpen()
    {
        base.OnOpen();
        WaveInfoTxt.text = $"Wave {GameManager.Instance.currentWaveIndex+1}";
        if (coroutine == null)
        {
            coroutine = StartCoroutine(WaveCountDown());
        }
    }

    IEnumerator WaveCountDown()
    {
        if (WaveCountTxt != null)
        {
            WaveCountTxt.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                WaveCountTxt.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
        }

        coroutine = null;
    }


}
