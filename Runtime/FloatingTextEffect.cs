using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingTextEffect : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float floatDistance = 5f;
    public float duration = 2f;
    public AnimationType animationType = AnimationType.Linear;
    public Camera overrideCamera;

    private RectTransform rectTransform;
    private TMP_Text tmpText;
    private Camera mainCamera;

    IEnumerator Start()
    {
        rectTransform = GetComponent<RectTransform>();
        tmpText = GetComponent<TMP_Text>();
        mainCamera = overrideCamera != null ? overrideCamera : Camera.main;
        yield return FloatingTextAnimation();
    }

    IEnumerator FloatingTextAnimation()
    {
        Vector3 startWorldPosition = target.position + offset;
        Vector3 endWorldPosition = startWorldPosition + new Vector3(0, floatDistance, 0);
        Color startColor = tmpText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = ApplyEasing(t);

            Vector3 currentWorldPosition = Vector3.Lerp(startWorldPosition, endWorldPosition, easedT);
            rectTransform.position = mainCamera.WorldToScreenPoint(currentWorldPosition);
            tmpText.color = Color.Lerp(startColor, endColor, easedT);

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    float ApplyEasing(float t)
    {
        return animationType switch
        {
            AnimationType.EaseIn => t * t,
            AnimationType.EaseOut => t * (2 - t),
            AnimationType.EaseInOut => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t,
            _ => t,
        };
    }
}
