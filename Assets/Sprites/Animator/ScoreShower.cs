using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreShower : CalculationShower
{
    protected override IEnumerator Calculation(TextMesh tm)
    {
        for (int i = 0; i < ScoreManager.score; i++)
        {
            tm.text = i.ToString();
            yield return null;
        }
    }
}
