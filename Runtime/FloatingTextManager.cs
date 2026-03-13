using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public Canvas targetCanvas; // The canvas to parent the floating text under

    // Method to create a floating text
    public void CreateFloatingText(Transform target, string text, FloatingTextData floatingTextData)//, Vector3 offset, float floatDistance, float duration)
    {
        // Instantiate the floating text prefab
        GameObject floatingTextInstance = Instantiate(floatingTextData.textPrefab, targetCanvas.transform);

        // Get the FloatingTextEffect component
        FloatingTextEffect floatingTextEffect = floatingTextInstance.GetComponent<FloatingTextEffect>();

        // Assign the target, offset, float distance, and duration
        floatingTextEffect.target = target;
        // floatingTextEffect.offset = offset;
        // floatingTextEffect.floatDistance = floatDistance;
        // floatingTextEffect.duration = duration;

        // Set the text of the TMP_Text component
        TMP_Text tmpText = floatingTextInstance.GetComponent<TMP_Text>();
        tmpText.text = $"{floatingTextData.prefix}{text}{floatingTextData.suffix}";
    }
}
