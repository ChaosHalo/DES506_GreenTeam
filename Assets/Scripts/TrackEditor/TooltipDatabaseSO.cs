using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

[CreateAssetMenu]
public class TooltipDatabaseSO : ScriptableObject
{
    [TextArea]
    public List<string> tooltipTexts;
}
