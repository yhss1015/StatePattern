using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{

    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI; // 이벤트 등록
        myStats.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }

    private void Update()
    {
        
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealth();
        slider.value = myStats.currentHealth;

    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);



    private void OnDisable()
    {
        entity.onFlipped -= FlipUI; // 이벤트 등록 해제
        myStats.onHealthChanged -= UpdateHealthUI;
    }

}
