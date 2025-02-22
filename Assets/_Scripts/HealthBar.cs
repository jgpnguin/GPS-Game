using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    EntityHealth entityHealth;
    public Slider slider;
    public Gradient Gradient;
    public Image fill;

    void Start()
    {
        entityHealth = Player.instance.entity.entityHealth;
        SetMaxHealth(entityHealth.GetMaxHealth());
        SetHealth(entityHealth.GetHealth());
        entityHealth.OnHealthChange += RetrieveHealth;
    }

    void OnDestroy()
    {
        entityHealth.OnHealthChange -= RetrieveHealth;
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = Gradient.Evaluate(1f);

    }
    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = Gradient.Evaluate(slider.normalizedValue);
    }
    public void RetrieveHealth(float delta)
    {
        slider.value = Mathf.Max(0, entityHealth.GetHealth());

        fill.color = Gradient.Evaluate(slider.normalizedValue);
    }
}
