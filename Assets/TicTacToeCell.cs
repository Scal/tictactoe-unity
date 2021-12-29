using System;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeCell : MonoBehaviour
{
    public int X;
    public int Y;

    public CellShape Value = CellShape.Empty;

    Image CellImage;
    Button mButton;
    Text mText;

    public event Action<TicTacToeCell> OnClickEvent;

    private void Start()
    {
        CellImage = GetComponent<Image>();
        mButton = GetComponent<Button>();
        Value = CellShape.Empty;
        mButton.onClick.AddListener(Click);
        mText = GetComponentInChildren<Text>();
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        mText.font = ArialFont;
        mText.color = Color.black;
        mText.fontSize = 64;
        mText.alignment = TextAnchor.MiddleCenter;
    }

    public void Click()
    {
        if (Value == CellShape.Empty)
        {
            ChangeShape(WhoTurn.Order);
            OnClickEvent(this);

        }
    }

    private void ChangeShape(CellShape value)
    {

        switch (value)
        {
            case CellShape.Empty:
                {
                    mText.text = "";
                    break;
                }
            case CellShape.Cross:
                {
                    CellImage.color = new Color(1f, 0.4f, 0.4f, 1);
                    mText.text = "X";
                    break;
                }
            case CellShape.Round:
                {
                    CellImage.color = new Color(0.4f, 0.4f, 1f, 1);
                    mText.text = "O";
                    break;
                }
        }

        Value = value;
        WhoTurn.ChangeTurnOrder();
    }
}
