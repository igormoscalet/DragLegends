using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    public void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
    }
}
