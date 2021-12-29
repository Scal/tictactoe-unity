using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{
    Text iAmText;
    void Start()
    {
        iAmText = GetComponent<Text>();
    }

    public void SetTxt(Slider txt) => iAmText.text = "Размер поля: " + txt.value;
}
