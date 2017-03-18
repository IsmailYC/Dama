using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    public struct Move
    {
        public Move(int s)
        {
            score = s;
            x1 = 0;
            x2 = 0;
            y1 = 0;
            y2 = 0;
        }
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public int score;
    }
    public static AI instance;
    public bool isWhite;

    int d;
    private void Start()
    {
        if (instance == null)
            instance = this;
        d = PlayerPrefsManager.GetDepth();
    }
    public Vector4 getMove(Piece[,] b)
    {
        Board board = new Board(b);
        Move move = getBestMove(board, 0, false,-1,-1);
        return new Vector4(move.x1, move.y1, move.x2, move.y2);
    }

    public Move getBestMove(Board board, int depth, bool whitePlayer, int x, int y)
    {
        if(depth==d)
        {
            int score = board.GetScore();
            Move move;
            move.score = score;
            return new Move(score);
        }

        List<Move> moves = new List<Move>();
        List<Vector4> vectors = new List<Vector4>();

        if (x==-1 && y==-1)
        {
            vectors = board.GetPossibleMoves(whitePlayer);
        }
        else
        {
            vectors = board.GetPossibleMoves(whitePlayer, x, y);
        }

        if(vectors.Count==0)
        {
            int score = board.EndState();
            Move move;
            move.score = score;
            return new Move(score);
        }

        foreach(Vector4 vector in vectors)
        {
            Move move;
            int x1 = (int)vector.x;
            int y1 = (int)vector.y;
            int x2 = (int)vector.z;
            int y2 = (int)vector.w;
            move.x1 = x1;
            move.y1 = y1;
            move.x2 = x2;
            move.y2 = y2;
            Board temp = board.PerformMove(x1,y1,x2,y2);
            int previousScore = board.GetScore();
            int currentScore = temp.GetScore();
            if(currentScore!=previousScore && temp.IsForcedPiece(x2,y2))
            {
                if (whitePlayer)
                    move.score = getBestMove(temp, depth + 1, true,x2,y2).score;
                else
                    move.score = getBestMove(temp, depth + 1, false,x2,y2).score;
            }
            else
            {
                if (whitePlayer)
                    move.score = getBestMove(temp, depth + 1, false,-1,-1).score;
                else
                    move.score = getBestMove(temp, depth + 1, true,-1,-1).score;
            }
            moves.Add(move);
        }

        int bestMove = 0;
        if (whitePlayer)
        {
            int bestScore = 1000000;
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].score < bestScore)
                {
                    bestMove = i;
                    bestScore = moves[i].score;
                }
            }
        }
        else
        {
            int bestScore = -1000000;
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].score > bestScore)
                {
                    bestMove = i;
                    bestScore = moves[i].score;
                }
            }
        }

        return moves[bestMove];
    }
}

