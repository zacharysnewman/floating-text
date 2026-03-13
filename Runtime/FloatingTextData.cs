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
}
