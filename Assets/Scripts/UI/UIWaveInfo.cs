using System.Collections;
using TMPro;
using UnityEngine;

public class UI_WaveInfo : UIBase
{
    public TextMeshProUGUI waveTitleText;
    public TextMeshProUGUI waveObjectiveText;
    public float animDuration = 0.5f;
    private Vector3 titleOriginalPos;

    protected void Awake()
    {
        if (waveTitleText != null)
        {
            titleOriginalPos = waveTitleText.transform.position;
        }
    }

    public IEnumerator ShowAnimation(int waveIndex, int totalMonsters)
    {
        waveTitleText.text = $"Wave {waveIndex + 1}";
        waveObjectiveText.text = $"목표: 몬스터 {totalMonsters}마리 처치";

        OpenUI(); 

        RectTransform titleRect = waveTitleText.rectTransform;
        Vector3 startPos = titleOriginalPos - new Vector3(Screen.width, 0, 0);
        Vector3 endPos = titleOriginalPos + new Vector3(Screen.width, 0, 0);
        titleRect.position = startPos;

        yield return StartCoroutine(AnimateText(titleRect, startPos, titleOriginalPos));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(AnimateText(titleRect, titleOriginalPos, endPos));

        CloseUI();
    }

    private IEnumerator AnimateText(RectTransform rect, Vector3 start, Vector3 end)
    {
        float timer = 0f;
        while (timer < animDuration)
        {
            rect.position = Vector3.Lerp(start, end, timer / animDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        rect.position = end;
    }
}