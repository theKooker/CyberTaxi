using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CalculationShower : MonoBehaviour
{
    public void StartCalc()
    {
        StartCoroutine(Calculation(GetComponent<TextMesh>()));
    }

    protected abstract IEnumerator Calculation(TextMesh tm);
}
