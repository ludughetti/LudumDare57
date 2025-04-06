using UnityEngine;
using UnityEngine.UI;

public class GUIStatsBars : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image oxigenBarFill;

    [SerializeField] private Health health;
    [SerializeField] private OxigenLogic oxigenLogic;

    private float _maxHealth;

    private void OnEnable()
    {
        health.MaxLifeEvent += SetMaxHealth;
        health.OnHealthChange += UpdateHealthBar;
        oxigenLogic.CurrentAndFullOxigenEvent += UpdateOxigenBar;
    }

    private void OnDisable()
    {
        health.MaxLifeEvent -= SetMaxHealth;
        health.OnHealthChange -= UpdateHealthBar;
        oxigenLogic.CurrentAndFullOxigenEvent -= UpdateOxigenBar;
    }

    private void SetMaxHealth(int maxHealth)
    {
        healthBarFill.fillAmount = 1f;
        _maxHealth = maxHealth;
    }

    private void UpdateHealthBar(int currentHealth)
    {
        healthBarFill.fillAmount = currentHealth / _maxHealth;
    }

    private void UpdateOxigenBar(float currentOxigen, float maxOxigen)
    {
        oxigenBarFill.fillAmount = currentOxigen / maxOxigen;
    }
}
