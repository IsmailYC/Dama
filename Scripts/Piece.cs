using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    public enum PieceType { Coin, Cap, Pebble};
    public PieceType pieceType;
    public bool isWhite;
    public bool isQueen;

    Vector3 pieceOffset = new Vector3(0.5f, 0.05f, 0.5f);
    int x, y;

    public Vector2 GetCoordinate() { return new Vector2(x, y); }
    public void SetCoordinate(int c1, int c2)
    {
        x = c1;
        y = c2;
    }

    public void Move(Piece[,] board, Vector3 boardOffset, int c1, int c2)
    {
        transform.position = (Vector3.right * c1) + (Vector3.forward * c2) + boardOffset + pieceOffset;
        SetCoordinate(c1, c2);
        board[x, y] = this;
    }

    public void Promote()
    {
        switch(pieceType)
        {
            case PieceType.Coin:
                isQueen = true;
                transform.Rotate(Vector3.right * 180);
                break;
            case PieceType.Cap:
                isQueen = true;
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case PieceType.Pebble:
                isQueen = true;
                transform.GetChild(0).gameObject.SetActive(true);
                break;
        }
    }

    public bool isForcedToMove(Piece[,] board)
    {
        if(isQueen)
        {
            for (int i = 1; i < 6; i++)
            {
                if (x + i <= 6 && y + i <= 6)
                {
                    if (board[x + i, y + i] != null)
                    {
                        if (board[x + i, y + i].isWhite != isWhite)
                        {
                            if (board[x + i + 1, y + i + 1] == null)
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
            for (int i = 1; i < 6; i++)
            {
                if (x - i >= 1 && y + i <= 6)
                {
                    if (board[x - i, y + i] != null)
                    {
                        if (board[x - i, y + i].isWhite != isWhite)
                        {
                            if (board[x - i - 1, y + i + 1] == null)
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
            for (int i = 1; i < 6; i++)
            {
                if (x + i <= 6 && y - i >= 1)
                {
                    if (board[x + i, y - i] != null)
                    {
                        if (board[x + i, y - i].isWhite != isWhite)
                        {
                            if (board[x + i + 1, y - i - 1] == null)
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
            for (int i = 1; i < 6; i++)
            {
                if (x - i >= 1 && y - i >= 1)
                {
                    if (board[x - i, y - i] != null)
                    {
                        if (board[x - i, y - i].isWhite != isWhite)
                        {
                            if (board[x - i - 1, y - i - 1] == null)
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
        else if(isWhite)
        {
            //Top left piece
            if(x>=2 && y<=5)
            {
                if(board[x-1,y+1]!=null)
                {
                    if (board[x - 1, y + 1].isWhite != isWhite && board[x - 2, y + 2] == null)
                        return true;
                }
            }
            //Top right piece
            if(x<=5 && y<=5)
            {
                if (board[x + 1, y + 1] != null)
                {
                    if (board[x + 1, y + 1].isWhite != isWhite && board[x + 2, y + 2] == null)
                        return true;
                }
            }
        }
        else if(!isWhite)
        {
            //Bottom left piece
            if (x >= 2 && y >=2)
            {
                if (board[x - 1, y - 1] != null)
                {
                    if (board[x - 1, y - 1].isWhite != isWhite && board[x - 2, y - 2] == null)
                        return true;
                }
            }
            //Bottom right piece
            if (x <= 5 && y >= 2)
            {
                if (board[x + 1, y - 1] != null)
                {
                    if (board[x + 1, y - 1].isWhite != isWhite && board[x + 2, y - 2] == null)
                        return true;
                }
            }
        }
        return false;
    }

    public bool CanMove(Piece[,] board, int x2, int y2)
    {
        if (x2 == -1)
            return false;
        if (board[x2, y2] != null)
            return false;

        int deltaX = x2 - x;
        int deltaY = y2 - y;
        if (isQueen)
        {
            deltaX = Mathf.Abs(deltaX);
            deltaY = Mathf.Abs(deltaY);
            if(deltaY==deltaX)
            {
                int xdir = (x2 - x) / deltaX;
                int ydir = (y2 - y) / deltaY;
                int x1 = x+xdir;
                int y1 = y+ydir;
                int enemyPieceCount=0;
                while(x1!=x2)
                {
                    if(board[x1,y1]!=null)
                    {
                        if (board[x1, y1].isWhite == isWhite)
                            return false;
                        else
                            enemyPieceCount++;
                    }
                    x1 = x1 + xdir;
                    y1 = y1 + ydir;
                }
                if (enemyPieceCount > 1)
                    return false;
                else if (enemyPieceCount == 1)
                    return true;
                else
                    return true;
            }
        }
        else if (isWhite)
        {
            deltaX = Mathf.Abs(deltaX);
            if (deltaX == 2)
            {
                if (deltaY == 2)
                {
                    int x1 = (x + x2) / 2;
                    int y1 = (y + y2) / 2;
                    if (board[x1, y1] != null)
                    {
                        if (board[x1, y1].isWhite != isWhite)
                            return true;
                    }
                }
            }
            else if (deltaX == 1)
            {
                if (deltaY == 1)
                    return true;
            }
        }
        else if (!isWhite)
        {
            deltaX = Mathf.Abs(deltaX);
            if (deltaX == 2)
            {
                if (deltaY == -2)
                {
                    int x1 = (x + x2) / 2;
                    int y1 = (y + y2) / 2;
                    if (board[x1, y1] != null)
                    {
                        if (board[x1, y1].isWhite != isWhite)
                            return true;
                    }
                }
            }
            else if (deltaX == 1)
            {
                if (deltaY == -1)
                    return true;
            }
        }
        return false;
    }

    public bool CanMove(Piece[,] board)
    {
        if(isQueen || isWhite)
        {
            if(x>=1 && y<=6)
            {
                if (board[x - 1, y + 1] == null)
                    return true;
                else if (board[x - 1, y + 1].isWhite != isWhite)
                {
                    if(x>=2 && y<=5)
                    {
                        if (board[x - 2, y + 2] == null)
                            return true;
                    }
                }   
            }
            if (x <= 6 && y <= 6)
            {
                if (board[x + 1, y + 1] == null)
                    return true;
                else if (board[x + 1, y + 1].isWhite != isWhite)
                {
                    if (x <= 5 && y <= 5)
                        if (board[x + 2, y + 2] == null)
                        return true;
                }
            }
        }
        if(isQueen || !isWhite)
        {
            if (x >= 1 && y >= 1)
            {
                if (board[x - 1, y - 1] == null)
                    return true;
                else if (board[x - 1, y - 1].isWhite != isWhite)
                {
                    if (x >= 2 && y >= 2)
                    {
                        if (board[x - 2, y - 2] == null)
                            return true;
                    }
                }
            }
            if (x <= 6 && y >= 1)
            {
                if (board[x + 1, y - 1] == null)
                    return true;
                else if (board[x + 1, y - 1].isWhite != isWhite)
                {
                    if (x <= 5 && y >= 2)
                        if (board[x + 2, y - 2] == null)
                            return true;
                }
            }
        }
        return false;
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
}
