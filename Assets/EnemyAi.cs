using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public bool Enable = false;
    public CellShape MySymbol = CellShape.Round;

    public PlayField GameField;

    public List<int[,]> WinCond = new List<int[,]>();

    public List<int> CanWinMe = new List<int>();
    public List<int> CanWinEnemy = new List<int>();
    
    public int X;
    public int Y;

    CellShape InvertCellShape(CellShape a)
    {
        if (a == CellShape.Cross)
            return CellShape.Round;
        else if (a == CellShape.Round)
            return CellShape.Cross;
        else
            return CellShape.Empty;
    }

    public int[,] weight;

    public void Setup()
    {
        weight = new int[GameField.Size, GameField.Size];
        for (int i = 0; i < GameField.Size; i++)
        {
            int[,] RoundWeightAi = new int[GameField.Size, GameField.Size];
            for (int j = 0; j < GameField.Size; j++)
            {
                RoundWeightAi[i, j] = 1;
            }
            WinCond.Add(RoundWeightAi);
        }
        for (int i = 0; i < GameField.Size; i++)
        {
            int[,] RoundWeightAi = new int[GameField.Size, GameField.Size];
            for (int j = 0; j < GameField.Size; j++)
            {
                RoundWeightAi[j, i] = 1;
            }
            WinCond.Add(RoundWeightAi);

        }

        int[,] Diagonal1 = new int[GameField.Size, GameField.Size];
        for (int i = 0; i < GameField.Size; i++)
        {
            Diagonal1[i, i] = 1;
        }
        WinCond.Add(Diagonal1);


        int[,] Diagonal2 = new int[GameField.Size, GameField.Size];
        for (int i = 0; i < GameField.Size; i++)
        {
            Diagonal2[i, GameField.Size - i - 1] = 1;
        }

        WinCond.Add(Diagonal2);


        for (int i = 0; i < WinCond.Count; i++)
        {
            CanWinMe.Add(i);
            CanWinEnemy.Add(i);
        }
    }

    int HasWinCondInCell(int x, int y)
    {
        int count = 0;

        List<int> CanWinMeIfIPutXY = CanWinMe;

        foreach (int a in CanWinEnemy)
        {
            if (WinCond[a][x, y] == 1)
            {
                count++;
            }
        }

        int ab = 0;
        foreach (int b in CanWinMe)
        {
            if (WinCond[b][x, y] == 1)
            {
                ab++;
            }
        }

        count -= CanWinMe.Count - ab;

        return count;
    }

    public void Recalculate(int x, int y)
    {
        List<int> TempListA = new List<int>();
        if (GameField.Field[x, y].Value == MySymbol)
        {
            TempListA = new List<int>();
            foreach (int Str in CanWinEnemy)
            {
                if (WinCond[Str][x, y] == 1)
                {
                    TempListA.Add(Str);
                }
            }
            CanWinEnemy = CanWinEnemy.Except(TempListA).ToList();
        }
        else
        {
            TempListA = new List<int>();
            foreach (int Str in CanWinMe)
            {
                if (WinCond[Str][x, y] == 1)
                {
                    TempListA.Add(Str);
                }
            }
            CanWinMe = CanWinMe.Except(TempListA).ToList();
        }
    }

    void CalculateMyWeight()
    {
        for (int i = 0; i < GameField.Size; i++)
        {
            for (int j = 0; j < GameField.Size; j++)
            {
                if (GameField.Field[i, j].Value == CellShape.Empty)
                {
                    weight[i, j] += HasWinCondInCell(i, j);
                    if (GameField.CheckWin(i, j, InvertCellShape(MySymbol)))
                        weight[i, j] = 1000;
                    if (GameField.CheckWin(i, j, MySymbol) )
                        weight[i, j] = 1000;
                }
                else
                {
                    weight[i, j] = -1000;
                }
            }
        }   

        int minWght = -125;

        for (int i = 0; i < GameField.Size; i++)
        {
            for (int j = 0; j < GameField.Size; j++)
            {
                if (weight[i, j] > minWght)
                {
                    minWght = weight[i, j];
                    X = i;
                    Y = j;
                }
            }
        }

    }

    public void MakeTurn()
    {
        if (Enable)
        {
            if (WhoTurn.Order == MySymbol)
            {
                CalculateMyWeight();

                if (GameField.Field[X, Y].Value == CellShape.Empty)
                {
                    GameField.Field[X, Y].Click();
                }
            }

        }

    }



}

