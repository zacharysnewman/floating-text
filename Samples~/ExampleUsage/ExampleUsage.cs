using UnityEngine;
using System.Collections;

public class ExampleUsage : MonoBehaviour
{
    public FloatingTextData damageTextData;
    public FloatingTextData xpTextData;
    public FloatingTextData woodTextData;

    private FloatingTextManager floatingTextManager;
    public Transform target;

    IEnumerator Start()
    {
        floatingTextManager = GetComponent<FloatingTextManager>();

        floatingTextManager.CreateFloatingText(target: target, "100", xpTextData);
        yield return new WaitForSeconds(0.5f);
        floatingTextManager.CreateFloatingText(target: target, "9001", damageTextData);
        yield return new WaitForSeconds(0.5f);
        floatingTextManager.CreateFloatingText(target: target, "3", woodTextData);
    }
}
