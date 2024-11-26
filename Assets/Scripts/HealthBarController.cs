using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private float animTime;
    [SerializeField] private Image healthBar;
    private float elapsedTime;

    public void RefreshHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        StartCoroutine(HealthbarAnim(currentHealth, currentHealth / maxHealth));
    }


    private IEnumerator HealthbarAnim(float startPos, float endPos)
    {
        while(elapsedTime < animTime)
        {
            healthBar.fillAmount = Mathf.Lerp(startPos, endPos, (elapsedTime / animTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
