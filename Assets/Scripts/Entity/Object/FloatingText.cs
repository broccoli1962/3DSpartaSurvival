using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 100f;    
    public float fadeOutTime = 1f;  

    private TextMeshProUGUI _textMesh;
    private Color startColor;
    private float timer;

    void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        startColor = _textMesh.color;
        timer = 0f;
    }

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        timer += Time.deltaTime;
        if (timer < fadeOutTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            _textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetText(string text)
    {
        if (_textMesh == null)
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }
        _textMesh.text = text;
    }
}