using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class WhoTurn
{
    public static CellShape Order = CellShape.Cross;

    public static void ChangeTurnOrder()
    {
        if (Order == CellShape.Cross)
        {
            Order = CellShape.Round;
        }
        else
        {
            Order = CellShape.Cross;
        }
    }

}

public class PlayField : MonoBehaviour
{
    public int Size = 3;

    public TicTacToeCell[,] Field;

    public int MoveCount;

    EnemyAi Bot1;
    EnemyAi Bot2;

    public GameObject whoWin;

    private void Start()
    {
        Field = new TicTacToeCell[Size, Size];
        Bot1 = gameObject.AddComponent<EnemyAi>();
        Bot1.GameField = this;

        Bot2 = gameObject.AddComponent<EnemyAi>();
        Bot2.MySymbol = CellShape.Cross;
        Bot2.GameField = this;
    }

    public void PrepareToGame(int mSize, bool Bot1Enabled, bool Bot2Enabled)
    {
        WhoTurn.Order = CellShape.Cross;

        Size = mSize;

        Bot1.Enable = Bot1Enabled;
        Bot2.Enable = Bot2Enabled;
        Bot1.Setup();
        Bot2.Setup();

        for (int i = 0; i < Size; i++)
        {
            GameObject VerticalPanel = new GameObject("Panel");
            VerticalPanel.AddComponent<RectTransform>();
            VerticalPanel.AddComponent<CanvasRenderer>();
            VerticalLayoutGroup vlg = VerticalPanel.AddComponent<VerticalLayoutGroup>();
            vlg.childControlHeight = true;
            vlg.childControlWidth = true;
            vlg.childForceExpandHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.spacing = 5;
            vlg.name = Convert.ToString(i);
            VerticalPanel.transform.SetParent(transform, true);

            for (int j = 0; j < Size; j++)
            {
                GameObject mButton = new GameObject("Button");
                mButton.AddComponent<Image>();
                mButton.AddComponent<Button>();

                TicTacToeCell tCell = mButton.AddComponent<TicTacToeCell>();
                tCell.OnClickEvent += OnClickAction;
                tCell.X = i;
                tCell.Y = j;

                Field[i, j] = tCell;
                mButton.transform.SetParent(VerticalPanel.transform, true);
                mButton.name = Convert.ToString(i) + Convert.ToString(j);

                GameObject mText = new GameObject("Text");
                mText.AddComponent<Text>();
                mText.transform.SetParent(mButton.transform, true);
            }
        }
        if (Bot2Enabled)
        {  
            StartCoroutine(MakeFirstTurn());
        }
    }

    IEnumerator MakeFirstTurn()
    {
        yield return new WaitForSeconds(0.1f);
        Field[Random.Range(0, Size), Random.Range(0, Size)].Click();
    }

    private void OnClickAction(TicTacToeCell tCell)
        {
        MoveCount++;

        if (CheckWin(tCell.X, tCell.Y, tCell.Value))
        {
            whoWin.SetActive(true);
            whoWin.GetComponentInChildren<Text>().text = tCell.Value + " победили!!!";
        }
        if (MoveCount == Size * Size)
        {
            whoWin.SetActive(true);
            whoWin.GetComponentInChildren<Text>().text = "Ќ»„№я!!!";
        }


        Bot1.MakeTurn();
        Bot2.MakeTurn();

        Bot1.Recalculate(tCell.X, tCell.Y);
        Bot2.Recalculate(tCell.X, tCell.Y);
    }

    public bool IsBetween(int testValue, int bound1, int bound2)
    {
        return (testValue >= bound1 && testValue < bound2);
    }

    public bool CheckWin(int x, int y, CellShape CellValue)
    {
        int[] Directions = new int[4];
        for (int i = 0; i < 9; i++)
        {
            if (i != 4)
            {
                for (int j = 1; j < Size; j++)
                {
                    if (IsBetween((x + (((i % 3) - 1) * j)), 0, Size))
                    {
                        if (IsBetween(y + (Math.Abs(i / 3) - 1) * j, 0, Size))
                        {
                            if (Field[x + (((i % 3) - 1) * j), y + ((Math.Abs(i / 3) - 1) * j)].Value == CellValue)
                            {
                                Directions[Math.Abs((4 - i) % 4)] += 1;

                                if (Directions[Math.Abs((4 - i) % 4)] == Size - 1)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }


        }

        return false;
    }



}
