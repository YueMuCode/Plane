using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthBar : StatsBar_HUD
{
    protected override void SetPercentText()
    {
        percentText.text = (targetFillAmount * 100f).ToString("f2") + "%";
    }
    
    
}
