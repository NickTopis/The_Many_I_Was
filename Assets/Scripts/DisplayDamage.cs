using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] Canvas damageIndicator;
    [SerializeField] Image damageSprite; // UI Image, not SpriteRenderer
    [SerializeField] float impactTime = 0.3f;
    [SerializeField] float fadeDuration = 1f;

    void Start()
    {
        damageIndicator.enabled = false;
    }

    public void ShowDamageImpact()
    {
        StartCoroutine(ShowDamage());
    }

    IEnumerator ShowDamage()
    {
        damageIndicator.enabled = true;

        // Set alpha to 1 (fully visible)
        Color color = damageSprite.color;
        color.a = 1f;
        damageSprite.color = color;

        yield return new WaitForSeconds(impactTime);

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            color.a = newAlpha;
            damageSprite.color = color;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure fully transparent
        color.a = 0f;
        damageSprite.color = color;

        damageIndicator.enabled = false;
    }
}
