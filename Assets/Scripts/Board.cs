using System.Collections.Generic;
using UnityEngine;

public class Board{
    int[,] board;
    public Board()
    {
        board = new int[8, 8];
        for(int i=0; i<8; i++)
        {
            for (int j = 0; j < 8; j++)
                board[i, j] = 0;
        }
    }

    public Board(Board b)
    {
        board = new int[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
                board[i, j] = b.board[i,j];
        }
    }

    public Board(Piece[,] b)
    {
        board = new int[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(b[i,j]==null)
                {
                    board[i, j] = 0;
                }
                else if(b[i,j].isWhite)
                {
                    if (b[i, j].isQueen)
                        board[i, j] = -5;
                    else
                        board[i, j] = -1;
                }
                else
                {
                    if (b[i, j].isQueen)
                        board[i, j] = 5;
                    else
                        board[i, j] = 1;
                }
            }
        }
    }

    public int GetScore()
    {
        int result = 0;
        for(int i=0; i<8; i++)
        {
            for (int j = 0; j < 8; j++)
                result += board[i, j];
        }
        return result;
    }

    public List<Vector4> GetPossibleMoves(bool isWhite, int x1, int y1)
    {
        List<Vector4> possibleMoves = new List<Vector4>();
        if (Mathf.Abs(board[x1, y1]) == 5)
        {
            bool killed = false;
            for (int i = 1; i < 8; i++)
            {
                if (x1 + i <= 7 && y1 + i <= 7)
                {
                    if (board[x1 + i, y1 + i] != 0)
                    {
                        if (board[x1 + i, y1 + i] * board[x1, y1] < 0 && !killed)
                        {
                            if (x1 + i + 1 <= 7 && y1 + i + 1 <= 7)
                            {
                                if (board[x1 + i + 1, y1 + i + 1] == 0)
                                {
                                    killed = true;
                                }
                                else
                                    break;
                            }
                        }
                        else
                            break;
                    }
                    else if (killed)
                    {
                        possibleMoves.Add(new Vector4(x1, y1, x1 + i, y1 + i));
                    }
                }
                else
                    break;
            }
            killed = false;
            for (int i = 1; i < 8; i++)
            {
                if (x1 - i >= 0 && y1 + i <= 7)
                {
                    if (board[x1 - i, y1 + i] != 0)
                    {
                        if (board[x1 - i, y1 + i] * board[x1, y1] < 0 && !killed)
                        {
                            if (x1 - i - 1 >= 0 && y1 + i + 1 <= 7)
                            {
                                if (board[x1 - i - 1, y1 + i + 1] == 0)
                                {
                                    killed = true;
                                }
                                else
                                    break;
                            }
                        }
                        else
                            break;
                    }
                    else if (killed)
                    {
                        possibleMoves.Add(new Vector4(x1, y1, x1 - i, y1 + i));
                    }
                }
                else
                    break;
            }
            killed = false;
            for (int i = 1; i < 8; i++)
            {
                if (x1 + i <= 7 && y1 - i >= 0)
                {
                    if (board[x1 + i, y1 - i] != 0)
                    {
                        if (board[x1 + i, y1 - i] * board[x1, y1] < 0 && !killed)
                        {
                            if (x1 + i + 1 <= 7 && y1 - i - 1 >= 0)
                            {
                                if (board[x1 + i + 1, y1 - i - 1] == 0)
                                {
                                    killed = true;
                                }
                                else
                                    break;
                            }
                        }
                        else
                            break;
                    }
                    else if (killed)
                    {
                        possibleMoves.Add(new Vector4(x1, y1, x1 + i, y1 - i));
                    }
                }
                else
                    break;
            }
            killed = false;
            for (int i = 1; i < 8; i++)
            {
                if (x1 - i >= 0 && y1 - i >= 0)
                {
                    if (board[x1 - i, y1 - i] != 0)
                    {
                        if (board[x1 - i, y1 - i] * board[x1, y1] < 0 && !killed)
                        {
                            if (x1 - i - 1 >= 0 && y1 - i - 1 >= 0)
                            {
                                if (board[x1 - i - 1, y1 - i - 1] == 0)
                                {
                                    killed = true;
                                }
                                else
                                    break;
                            }
                        }
                        else
                            break;
                    }
                    else if (killed)
                    {
                        possibleMoves.Add(new Vector4(x1, y1, x1 - i, y1 - i));
                    }
                }
                else
                    break;
            }
        }
        else if (board[x1, y1] == -1)
        {
            //Top left piece
            if (x1 >= 2 && y1 <= 5)
            {
                if (board[x1 - 1, y1 + 1] != 0)
                {
                    if (board[x1 - 1, y1 + 1] * board[x1, y1] < 0 && board[x1 - 2, y1 + 2] == 0)
                        possibleMoves.Add(new Vector4(x1, y1, x1 - 2, y1 + 2));
                }
            }
            //Top right piece
            if (x1 <= 5 && y1 <= 5)
            {
                if (board[x1 + 1, y1 + 1] != 0)
                {
                    if (board[x1 + 1, y1 + 1] * board[x1, y1] < 0 && board[x1 + 2, y1 + 2] == 0)
                        possibleMoves.Add(new Vector4(x1, y1, x1 + 2, y1 + 2));
                }
            }
        }
        else if (board[x1, y1] == 1)
        {
            //Bottom left piece
            if (x1 >= 2 && y1 >= 2)
            {
                if (board[x1 - 1, y1 - 1] != 0)
                {
                    if (board[x1 - 1, y1 - 1] * board[x1, y1] < 0 && board[x1 - 2, y1 - 2] == 0)
                        possibleMoves.Add(new Vector4(x1, y1, x1 - 2, y1 - 2));
                }
            }
            //Bottom right piece
            if (x1 <= 5 && y1 >= 2)
            {
                if (board[x1 + 1, y1 - 1] != 0)
                {
                    if (board[x1 + 1, y1 - 1] * board[x1, y1] < 0 && board[x1 + 2, y1 - 2] == 0)
                        possibleMoves.Add(new Vector4(x1, y1, x1 + 2, y1 - 2));
                }
            }
        }
        return possibleMoves;
    }

    public List<Vector4> GetPossibleMoves(bool isWhite)
    {
        int player;
        if (isWhite)
            player = -1;
        else
            player = 1;
        List<Vector2> forcedPieces = GetForcedMoves(isWhite);
        List<Vector4> possibleMoves = new List<Vector4>();
        if(forcedPieces.Count>0)
        {
            foreach(Vector2 piece in forcedPieces)
            {
                int x1 = (int)piece.x;
                int y1 = (int)piece.y;
                if (Mathf.Abs(board[x1, y1]) == 5)
                {
                    bool killed = false;
                    for (int i = 1; i < 8; i++)
                    {
                        if (x1 + i <= 7 && y1 + i <= 7)
                        {
                            if (board[x1 + i, y1 + i] != 0)
                            {
                                if (board[x1 + i, y1 + i] * board[x1, y1] < 0 && !killed)
                                {
                                    if (x1 + i + 1 <= 7 && y1 + i + 1 <= 7)
                                    {
                                        if (board[x1 + i + 1, y1 + i + 1] == 0)
                                        {
                                            killed = true;
                                        }
                                        else
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else if (killed)
                            {
                                possibleMoves.Add(new Vector4(x1, y1, x1 + i, y1 + i));
                            }
                        }
                        else
                            break;
                    }
                    killed = false;
                    for (int i = 1; i < 8; i++)
                    {
                        if (x1 - i >= 0 && y1 + i <= 7)
                        {
                            if (board[x1 - i, y1 + i] != 0)
                            {
                                if (board[x1 - i, y1 + i] * board[x1, y1] < 0 && !killed)
                                {
                                    if (x1 - i - 1 >= 0 && y1 + i + 1 <= 7)
                                    {
                                        if (board[x1 - i - 1, y1 + i + 1] == 0)
                                        {
                                            killed = true;
                                        }
                                        else
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else if (killed)
                            {
                                possibleMoves.Add(new Vector4(x1, y1, x1 - i, y1 + i));
                            }
                        }
                        else
                            break;
                    }
                    killed = false;
                    for (int i = 1; i < 8; i++)
                    {
                        if (x1 + i <= 7 && y1 - i >= 0)
                        {
                            if (board[x1 + i, y1 - i] != 0)
                            {
                                if (board[x1 + i, y1 - i] * board[x1, y1] < 0 && !killed)
                                {
                                    if (x1 + i + 1 <= 7 && y1 - i - 1 >= 0)
                                    {
                                        if (board[x1 + i + 1, y1 - i - 1] == 0)
                                        {
                                            killed = true;
                                        }
                                        else
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else if (killed)
                            {
                                possibleMoves.Add(new Vector4(x1, y1, x1 + i, y1 - i));
                            }
                        }
                        else
                            break;
                    }
                    killed = false;
                    for (int i = 1; i < 8; i++)
                    {
                        if (x1 - i >= 0 && y1 - i >= 0)
                        {
                            if (board[x1 - i, y1 - i] != 0)
                            {
                                if (board[x1 - i, y1 - i] * board[x1, y1] < 0 && !killed)
                                {
                                    if (x1 - i - 1 >= 0 && y1 - i - 1 >= 0)
                                    {
                                        if (board[x1 - i - 1, y1 - i - 1] == 0)
                                        {
                                            killed = true;
                                        }
                                        else
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else if (killed)
                            {
                                possibleMoves.Add(new Vector4(x1, y1, x1 - i, y1 - i));
                            }
                        }
                        else
                            break;
                    }
                }
                else if (board[x1, y1] == -1)
                {
                    //Top left piece
                    if (x1 >= 2 && y1 <= 5)
                    {
                        if (board[x1 - 1, y1 + 1] != 0)
                        {
                            if (board[x1 - 1, y1 + 1] * board[x1, y1] < 0 && board[x1 - 2, y1 + 2] == 0)
                                possibleMoves.Add(new Vector4(x1, y1, x1 - 2, y1 + 2));
                        }
                    }
                    //Top right piece
                    if (x1 <= 5 && y1 <= 5)
                    {
                        if (board[x1 + 1, y1 + 1] != 0)
                        {
                            if (board[x1 + 1, y1 + 1] * board[x1, y1] < 0 && board[x1 + 2, y1 + 2] == 0)
                                possibleMoves.Add(new Vector4(x1, y1, x1 + 2, y1 + 2));
                        }
                    }
                }
                else if (board[x1, y1] == 1)
                {
                    //Bottom left piece
                    if (x1 >= 2 && y1 >= 2)
                    {
                        if (board[x1 - 1, y1 - 1] != 0)
                        {
                            if (board[x1 - 1, y1 - 1] * board[x1, y1] < 0 && board[x1 - 2, y1 - 2] == 0)
                                possibleMoves.Add(new Vector4(x1, y1, x1 - 2, y1 - 2));
                        }
                    }
                    //Bottom right piece
                    if (x1 <= 5 && y1 >= 2)
                    {
                        if (board[x1 + 1, y1 - 1] != 0)
                        {
                            if (board[x1 + 1, y1 - 1] * board[x1, y1] < 0 && board[x1 + 2, y1 - 2] == 0)
                                possibleMoves.Add(new Vector4(x1, y1, x1 + 2, y1 - 2));
                        }
                    }
                }
            }
        }
        else
        {
            for(int x=0; x<8; x++)
            {
                for(int y=0; y<8; y++)
                {
                    if(board[x,y]*player>0)
                    {
                        if (Mathf.Abs(board[x, y]) == 5)
                        {
                            for (int i = 1; i < 8; i++)
                            {
                                if (x - i >= 0 && y + i <= 7)
                                {
                                    if (board[x - i, y + i] == 0)
                                    {
                                        possibleMoves.Add(new Vector4(x, y, x - i, y + i));
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                            }

                            for (int i = 1; i < 8; i++)
                            {
                                if (x + i <= 7 && y + i <= 7)
                                {
                                    if (board[x + i, y + i] == 0)
                                    {
                                        possibleMoves.Add(new Vector4(x, y, x + i, y + i));
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                            }

                            for (int i = 1; i < 8; i++)
                            {
                                if (x - i >= 0 && y - i >= 0)
                                {
                                    if (board[x - i, y - i] == 0)
                                    {
                                        possibleMoves.Add(new Vector4(x, y, x - i, y - i));
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                            }

                            for (int i = 1; i < 8; i++)
                            {
                                if (x + i <= 7 && y - i >= 0)
                                {
                                    if (board[x + i, y - i] == 0)
                                    {
                                        possibleMoves.Add(new Vector4(x, y, x + i, y - i));
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                            }
                        }
                        else if (board[x, y] == -1)
                        {
                            //Top left piece
                            if (x >= 1 && y <= 6)
                            {
                                if (board[x - 1, y + 1] == 0)
                                {
                                    possibleMoves.Add(new Vector4(x, y, x - 1, y + 1));
                                }
                            }
                            //Top right piece
                            if (x <= 5 && y <= 5)
                            {
                                if (board[x + 1, y + 1] == 0)
                                {
                                    possibleMoves.Add(new Vector4(x, y, x + 1, y + 1));
                                }
                            }
                        }
                        else if (board[x, y] == 1)
                        {
                            //Bottom left piece
                            if (x >= 1 && y >= 1)
                            {
                                if (board[x - 1, y - 1] == 0)
                                {
                                    possibleMoves.Add(new Vector4(x, y, x - 1, y - 1));
                                }
                            }
                            //Bottom right piece
                            if (x <= 6 && y >= 1)
                            {
                                if (board[x + 1, y - 1] == 0)
                                {
                                    possibleMoves.Add(new Vector4(x, y, x + 1, y - 1));
                                }
                            }
                        }
                    }
                }
            }
        }

        return possibleMoves;
    }

    public List<Vector2> GetForcedMoves(bool isWhite)
    {
        int player;
        if (isWhite)
            player = -1;
        else
            player = 1;
        List<Vector2> forcedPieces = new List<Vector2>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] * player > 0)
                {
                    if (IsForcedPiece(x, y))
                        forcedPieces.Add(new Vector2(x, y));
                }
            }
        }
        return forcedPieces;
    }

    public bool IsForcedPiece(int x, int y)
    {
        if (Mathf.Abs(board[x, y]) == 5)
        {
            for (int i = 1; i < 7; i++)
            {
                if (x + i <= 6 && y + i <= 6)
                {
                    if (board[x + i, y + i] != 0)
                    {
                        if (board[x + i, y + i] * board[x, y] < 0)
                        {
                            if (board[x + i + 1, y + i + 1] == 0)
                            {
                                return true;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }

            for (int i = 1; i < 7; i++)
            {
                if (x - i >= 1 && y + i <= 6)
                {
                    if (board[x - i, y + i] != 0)
                    {
                        if (board[x - i, y + i] * board[x, y] < 0)
                        {
                            if (board[x - i - 1, y + i + 1] == 0)
                            {
                                return true;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
            for (int i = 1; i < 7; i++)
            {
                if (x + i <= 6 && y - i >= 1)
                {
                    if (board[x + i, y - i] != 0)
                    {
                        if (board[x + i, y - i] * board[x, y] < 0)
                        {
                            if (board[x + i + 1, y - i - 1] == 0)
                            {
                                return true;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }

            for (int i = 1; i < 7; i++)
            {
                if (x - i >= 1 && y - i >= 1)
                {
                    if (board[x - i, y - i] != 0)
                    {
                        if (board[x - i, y - i] * board[x, y] < 0)
                        {
                            if (board[x - i - 1, y - i - 1] == 0)
                            {
                                return true;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                else
                    break;
            }
        }
        else if (board[x, y] == -1)
        {
            //Top left piece
            if (x >= 2 && y <= 5)
            {
                if (board[x - 1, y + 1] != 0)
                {
                    if (board[x - 1, y + 1] * board[x, y] < 0 && board[x - 2, y + 2] == 0)
                        return true;
                }
            }
            //Top right piece
            if (x <= 5 && y <= 5)
            {
                if (board[x + 1, y + 1] != 0)
                {
                    if (board[x + 1, y + 1] * board[x, y] < 0 && board[x + 2, y + 2] == 0)
                        return true;
                }
            }
        }
        else if (board[x, y] == 1)
        {
            //Bottom left piece
            if (x >= 2 && y >= 2)
            {
                if (board[x - 1, y - 1] != 0)
                {
                    if (board[x - 1, y - 1] * board[x, y] < 0 && board[x - 2, y - 2] == 0)
                        return true;
                }
            }
            //Bottom right piece
            if (x <= 5 && y >= 2)
            {
                if (board[x + 1, y - 1] != 0)
                {
                    if (board[x + 1, y - 1] * board[x, y] < 0 && board[x + 2, y - 2] == 0)
                        return true;
                }
            }
        }
        return false;
    }

    public Board PerformMove(int x1, int y1, int x2, int y2)
    {
        Board temp = new Board(this);
        int deltaX = Mathf.Abs(x2 - x1);
        if (deltaX > 1)
        {
            int xdir = (x2 - x1) / deltaX;
            int ydir = (y2 - y1) / deltaX;
            int x = x1 + xdir;
            int y = y1 + ydir;
            while (x != x2)
            {
                if (temp.board[x, y] != 0)
                {
                    temp.board[x, y] = 0;
                }
                x = x + xdir;
                y = y + ydir;
            }
        }
        if (temp.board[x1, y1] == -1 && y2 == 7)
            temp.board[x1, y1] = -5;
        else if (temp.board[x1, y1] == 1 && y2 == 0)
            temp.board[x1, y1] = 5;
        temp.board[x2, y2] = temp.board[x1, y1];
        temp.board[x1, y1] = 0;
        return temp;
    }

    public int EndState()
    {
        int result = 0;
        for(int x=0; x<8; x++)
        {
            for(int y=0; y<8; y++)
            {
                if (result == 0)
                {
                    result = result + board[x, y];
                }
                else
                {
                    if (board[x, y] * result < 0)
                        return 0;
                    else
                        result = result + board[x, y];
                }
            }
        }
        if (result > 0)
            return 1000;
        else
            return -1000;
    }
}
