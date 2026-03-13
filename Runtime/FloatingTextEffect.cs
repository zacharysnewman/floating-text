using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingTextEffect : MonoBehaviour
{
    public Transform target; // The target object to follow
    public Vector3 offset; // Offset from the target object
    public float floatDistance = 5f; // Distance to move upwards
    public float duration = 2f; // Duration of the animation
    public bool destroyOnComplete = true;

    private RectTransform rectTransform;
    private TMP_Text tmpText;
    private Camera mainCamera;

    IEnumerator Start()
    {
        rectTransform = GetComponent<RectTransform>();
        tmpText = GetComponent<TMP_Text>();
        mainCamera = Camera.main;

        Vector3 startingPosition = transform.position;
        Vector3 startingScreenPosition = mainCamera.WorldToScreenPoint(startingPosition);
        Color startColor = tmpText.color;

        while (true)
        {
            rectTransform.position = startingScreenPosition;
            tmpText.color = startColor;
            yield return FloatingTextAnimation();
        }
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

            Vector3 currentWorldPosition = Vector3.Lerp(startWorldPosition, endWorldPosition, t);
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(currentWorldPosition);
            rectTransform.position = screenPosition;
            tmpText.color = Color.Lerp(startColor, endColor, t);

            yield return new WaitForEndOfFrame();
        }

        if (destroyOnComplete)
        {
            Destroy(gameObject);
        }
    }
}
