using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfShower : CalculationShower
{
    protected override IEnumerator Calculation(TextMesh tm)
    {
        ScoreManager.CalculatePerformance();
        for(int i = 0; i < 3; i++)
        {
            tm.text += ".";
            yield return new WaitForSeconds(.5f);
        }

        tm.text = ScoreManager.perf;
        
        Cursor.visible = true;
    }
}
