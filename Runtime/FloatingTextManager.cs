using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public Canvas targetCanvas;

    public void CreateFloatingText(Transform target, string text, FloatingTextData data)
    {
        if (data == null) { Debug.LogError("[FloatingTextManager] data is null."); return; }
        if (data.textPrefab == null) { Debug.LogError("[FloatingTextManager] data.textPrefab is null."); return; }
        if (targetCanvas == null) { Debug.LogError("[FloatingTextManager] targetCanvas is not assigned."); return; }

        GameObject instance = Instantiate(data.textPrefab, targetCanvas.transform);

        FloatingTextEffect effect = instance.GetComponent<FloatingTextEffect>();
        if (effect == null)
        {
            Debug.LogError("[FloatingTextManager] textPrefab is missing a FloatingTextEffect component.");
            Destroy(instance);
            return;
        }

        effect.target = target;
        effect.offset = data.offset;
        effect.floatDistance = data.floatDistance;
        effect.duration = data.duration;
        effect.animationType = data.animationType;

        TMP_Text tmpText = instance.GetComponent<TMP_Text>();
        if (tmpText != null)
            tmpText.text = $"{data.prefix}{text}{data.suffix}";
    }
}
