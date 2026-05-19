using UnityEngine;

public enum AnimationType
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut
}

[CreateAssetMenu(fileName = "FloatingTextData", menuName = "ScriptableObjects/FloatingTextData", order = 1)]
public class FloatingTextData : ScriptableObject
{
    public GameObject textPrefab;
    public string prefix;
    public string suffix;
    public Vector3 offset = Vector3.zero;
    public float floatDistance = 5f;
    public float duration = 2f;
    public AnimationType animationType = AnimationType.Linear;
}
