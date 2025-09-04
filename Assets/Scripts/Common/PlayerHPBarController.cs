using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPBarController : MonoBehaviour
{
    public Slider _hpSlider;
    public TextMeshProUGUI _hpText;
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

    void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.transform.forward);
    }
}