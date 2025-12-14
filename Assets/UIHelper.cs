using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> textList;
    
    private void LateUpdate()
    {
        textList[0].text = GameManagerScript.instance.keyCount.ToString("000");
        textList[1].text = GameManagerScript.instance.coinCount.ToString("000");
        textList[2].text = GameManagerScript.instance.artifactCount.ToString("000");
    }
}
