using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Slider SliderForFirldSize;

    [SerializeField]
    bool Bot1Enabled;

    [SerializeField]
    bool Bot2Enabled;

    [SerializeField]
    PlayField MyField;


    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        MyField.PrepareToGame((int)SliderForFirldSize.value, Bot1Enabled, Bot2Enabled);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
