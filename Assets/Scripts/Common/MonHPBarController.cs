using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonHPBarController : MonoBehaviour
{
    public Slider _hpSlider;
    public TextMeshProUGUI _hpText;
    public TextMeshProUGUI _monName;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }
    public void UpdateHP(int currentHealth, int maxHealth)
    {
        _hpSlider.value = (float)currentHealth / maxHealth;

        if (_hpText != null)
        {
            _hpText.text = $"{currentHealth} / {maxHealth}";
        }
    }
    public void SetName(string name)
    {
        if (_monName != null)
        {
            string processedName = name;
            processedName = processedName.Replace("Red", "<color=red>Red</color>");
            processedName = processedName.Replace("Green", "<color=green>Green</color>");
            processedName = processedName.Replace("Purple", "<color=purple>Purple</color>");

            _monName.text = processedName;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.transform.forward);
    }
}