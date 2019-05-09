using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class CheckerBoard : MonoBehaviour {
    public static CheckerBoard instance;
    public bool singlePlayer;
    public bool twoPlayer;
    public bool onlinePlayer;
    public Piece[,] pieces = new Piece[8, 8];
    public GameObject[] whitePiecePrefab;
    public GameObject[] blackPiecePrefab;
    public bool isWhite;
    public GameObject winPanel;
    public Text matchResultText;

    List<Piece> forcedPieces;
    Vector3 boardOffset = new Vector3(-4.0f, 0f, -4.0f);
    Vector2 mouseOver;
    Vector2 startDrag;
    Vector2 endDrag;
    bool isWhiteTurn;
    bool hasKilled;
    bool endGame;
    bool gettingMove;
    Piece selectedPiece;
    int whiteCount, blackCount,finalCountDown;
    int pieceType;
    Client client;
	// Use this for initialization
	void Start () {
        instance = this;
        if(onlinePlayer)
        {
            client = FindObjectOfType<Client>();
            isWhite = client.isHost;
        }
        pieceType = PlayerPrefsManager.GetPieceType();
        GenerateBoard();
        hasKilled = false;
        endGame = false;
        gettingMove = false;
        finalCountDown = 0;
	}

    private void Update()
    {
        if(!endGame)
        {
            if (singlePlayer)
            {
                if (isWhiteTurn == isWhite)
                {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
                    UpdateMouseOver();
                    if (selectedPiece != null)
                        UpdatePieceDrag();
                    int x = (int)mouseOver.x;
                    int y = (int)mouseOver.y;
                    //if player turn
                    if (Input.GetMouseButtonDown(0))
                        SelectPiece(x, y);
                    else if (Input.GetMouseButtonUp(0))
                    {
                        int x1 = (int)startDrag.x;
                        int y1 = (int)startDrag.y;
                        if (selectedPiece != null)
                            TryMovePiece(x1, y1, x, y);
                    }
#else
            if (Input.touchCount > 0)
            {
                Touch firstTouch = Input.touches[0];
                UpdateMouseOver();
                if (selectedPiece != null)
                    UpdatePieceDrag();
                int x = (int)mouseOver.x;
                int y = (int)mouseOver.y;
                if (firstTouch.phase == TouchPhase.Began)
                    SelectPiece(x, y);
                else if (firstTouch.phase == TouchPhase.Ended)
                {
                    int x1 = (int)startDrag.x;
                    int y1 = (int)startDrag.y;
                    TryMovePiece(x1, y1, x, y);
                }
            }
#endif
                }
                else if(!gettingMove)
                {
                    gettingMove = true;
                    StartCoroutine(StartAIThread());
                }
            }
            if (twoPlayer)
            {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
                UpdateMouseOver();
                if (selectedPiece != null)
                    UpdatePieceDrag();
                int x = (int)mouseOver.x;
                int y = (int)mouseOver.y;
                //if player turn
                if (Input.GetMouseButtonDown(0))
                    SelectPiece(x, y);
                else if (Input.GetMouseButtonUp(0))
                {
                    int x1 = (int)startDrag.x;
                    int y1 = (int)startDrag.y;
                    if (selectedPiece != null)
                        TryMovePiece(x1, y1, x, y);
                }
#else
            if (Input.touchCount > 0)
            {
                Touch firstTouch = Input.touches[0];
                UpdateMouseOver();
                if (selectedPiece != null)
                    UpdatePieceDrag();
                int x = (int)mouseOver.x;
                int y = (int)mouseOver.y;
                if (firstTouch.phase == TouchPhase.Began)
                    SelectPiece(x, y);
                else if (firstTouch.phase == TouchPhase.Ended)
                {
                    int x1 = (int)startDrag.x;
                    int y1 = (int)startDrag.y;
                    TryMovePiece(x1, y1, x, y);
                }
            }
#endif
            }
            if(onlinePlayer)
            {
                if (isWhiteTurn == isWhite)
                {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
                    UpdateMouseOver();
                    if (selectedPiece != null)
                        UpdatePieceDrag();
                    int x = (int)mouseOver.x;
                    int y = (int)mouseOver.y;
                    //if player turn
                    if (Input.GetMouseButtonDown(0))
                        SelectPiece(x, y);
                    else if (Input.GetMouseButtonUp(0))
                    {
                        int x1 = (int)startDrag.x;
                        int y1 = (int)startDrag.y;
                        if (selectedPiece != null)
                            TryMovePiece(x1, y1, x, y);
                    }
#else
            if (Input.touchCount > 0)
            {
                Touch firstTouch = Input.touches[0];
                UpdateMouseOver();
                if (selectedPiece != null)
                    UpdatePieceDrag();
                int x = (int)mouseOver.x;
                int y = (int)mouseOver.y;
                if (firstTouch.phase == TouchPhase.Began)
                    SelectPiece(x, y);
                else if (firstTouch.phase == TouchPhase.Ended)
                {
                    int x1 = (int)startDrag.x;
                    int y1 = (int)startDrag.y;
                    TryMovePiece(x1, y1, x, y);
                }
            }
#endif
                }
            }
        }
    }

    IEnumerator StartAIThread()
    {
        Vector4 move = AI.instance.getMove(pieces);
        int x1 = (int)move.x;
        int y1 = (int)move.y;
        int x2 = (int)move.z;
        int y2 = (int)move.w;
        TryMovePiece(x1, y1, x2, y2);
        yield return null;
    }

    void UpdateMouseOver()
    {

        if(!Camera.main)
        {
            Debug.Log("No camera found");
            return;
        }

        RaycastHit hit;
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition) , out hit,25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
#else
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position),out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
#endif

    }
    void UpdatePieceDrag()
    {
        if (!Camera.main)
        {
            Debug.Log("No camera found");
            return;
        }

        RaycastHit hit;
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            selectedPiece.transform.position = hit.point + Vector3.up;
        }
#else
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            selectedPiece.transform.position = hit.point + Vector3.up;
        }
#endif

    }

    void SelectPiece(int x, int y)
    {
        if (x == -1)
        {
            return;
        }
        else if(pieces[x,y]==null)
        {
            return;
        }
        else if(pieces[x,y].isWhite==isWhiteTurn)
        {
            if (forcedPieces.Count == 0)
            {
                startDrag = new Vector2(x, y);
                selectedPiece = pieces[x, y];
            }
            else if (forcedPieces.Find(fp => fp == pieces[x, y]) != null)
            {
                startDrag = new Vector2(x, y);
                selectedPiece = pieces[x, y];
            }
        }
    }

    public void TryMovePiece(int x1, int y1, int x2, int y2)
    {
        selectedPiece = pieces[x1, y1];
        if(selectedPiece!=null)
        {
            if (selectedPiece.CanMove(pieces, x2, y2))
            {
                int deltaX = Mathf.Abs(x2 - x1);
                if (deltaX > 1)
                {
                    int xdir = (x2 - x1) / deltaX;
                    int ydir = (y2 - y1) / deltaX;
                    int x = x1 + xdir;
                    int y = y1 + ydir;
                    while (x != x2)
                    {
                        if (pieces[x, y] != null)
                        {
                            pieces[x, y].Kill();
                            pieces[x, y] = null;
                            hasKilled = true;
                            if (selectedPiece.isWhite)
                                blackCount--;
                            else
                                whiteCount--;
                        }
                        x = x + xdir;
                        y = y + ydir;
                    }
                }
                if (forcedPieces.Count > 0 && !hasKilled)
                {
                    selectedPiece.Move(pieces, boardOffset, x1, y1);
                    selectedPiece = null;
                    startDrag = Vector2.zero;
                    return;
                }
                selectedPiece.Move(pieces, boardOffset, x2, y2);
                pieces[x1, y1] = null;
                EndTurn(x2, y2);
            }
            else
            {
                selectedPiece.Move(pieces, boardOffset, x1, y1);
                selectedPiece = null;
                startDrag = Vector2.zero;
            }
        }
    }

    void ScanForForcedPieces(Piece piece, int x, int y)
    {
        forcedPieces = new List<Piece>();
        if (piece.isForcedToMove(pieces))
            forcedPieces.Add(piece);
    }

    void ScanForForcedPieces()
    {
        forcedPieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] != null)
                {
                    if (pieces[i, j].isForcedToMove(pieces) && pieces[i, j].isWhite == isWhiteTurn)
                    {
                        forcedPieces.Add(pieces[i, j]);
                    }

                }
            }
        }
    }

    void GenerateBoard()
    {
        isWhiteTurn = true;
        for (int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 0) ? true : false;
            for (int x = 0; x < 8; x += 2)
            {
                GeneratePiece((oddRow) ? x : x + 1, y);
            }
        }

        for (int y = 5; y < 8; y++)
        {
            bool oddRow = (y % 2 == 0) ? true : false;
            for (int x = 0; x < 8; x += 2)
            {
                GeneratePiece((oddRow) ? x : x + 1, y);
            }
        }
        blackCount = 12;
        whiteCount = 12;
        ScanForForcedPieces();
    }

    void GeneratePiece(int x, int y)
    {
        bool isWhitePiece = (y < 3) ? true : false;
        GameObject temp = Instantiate((isWhitePiece) ? whitePiecePrefab[pieceType] : blackPiecePrefab[pieceType]) as GameObject;
        temp.transform.SetParent(transform);
        Piece p = temp.GetComponent<Piece>();
        p.Move(pieces, boardOffset,x,y);
    }

    void EndTurn(int x, int y)
    {
        if (selectedPiece.isWhite && !selectedPiece.isQueen && y == 7)
            selectedPiece.Promote();
        else if (!selectedPiece.isWhite && !selectedPiece.isQueen && y == 0)
            selectedPiece.Promote();
        else if(hasKilled)
        {
            ScanForForcedPieces(selectedPiece, x, y);
            if (forcedPieces.Count != 0)
            {
                if (onlinePlayer)
                {
                    string msg = "CMOV|";
                    msg += startDrag.x.ToString() + "|";
                    msg += startDrag.y.ToString() + "|";
                    msg += x.ToString() + "|";
                    msg += y.ToString();

                    client.Send(msg);
                }
                hasKilled = false;
                gettingMove = false;
                selectedPiece = null;
                startDrag = Vector2.zero;
                return;
            }
        }
        if (onlinePlayer)
        {
            string msg = "CMOV|";
            msg += startDrag.x.ToString() + "|";
            msg += startDrag.y.ToString() + "|";
            msg += x.ToString() + "|";
            msg += y.ToString();

            client.Send(msg);
        }
        hasKilled = false;
        gettingMove = false;
        isWhiteTurn = !isWhiteTurn;
        //Board board = new Board(pieces);
        //List<Vector2> forced = board.GetForcedMoves(isWhiteTurn);
        //if (forced.Count > 0)
        //    Debug.Log("Checking Forced Pieces");
        //else
        //    Debug.Log("Checking Normal Pieces");
        //List<Vector4> possibleMoves = board.GetPossibleMoves(isWhiteTurn);
        //foreach (Vector4 vector in possibleMoves)
        //{
        //    Debug.Log(vector);
        //}
        selectedPiece = null;
        startDrag = Vector2.zero;
        CheckVictory();
        ScanForForcedPieces();
    }

    void CheckVictory()
    {
        if (blackCount == 0)
        {
            endGame = true;
            matchResultText.text = "White Team Wins";
            winPanel.SetActive(true);
        }
        else if (whiteCount == 0)
        {
            endGame = true;
            matchResultText.text = "Black Team Wins";
            winPanel.SetActive(true);
        }
        else if (!CanPlayerMove(isWhiteTurn))
        {
            if (isWhiteTurn)
            {
                endGame = true;
                matchResultText.text = "White team is stuck";
                winPanel.SetActive(true);
            }
            else
            {
                endGame = true;
                matchResultText.text = "Black Team is stuck";
                winPanel.SetActive(true);
            }
        }
        else if (blackCount==1 || whiteCount==1)
        {
            if (finalCountDown == 12)
            {
                endGame = true;
                matchResultText.text = "Tie after 12 moves";
                winPanel.SetActive(true);
            }
            else
                finalCountDown++;
        }
    }

    bool CanPlayerMove(bool player)
    {
        for(int i=0; i<8; i++)
        {
            for(int j=0; j<8; j++)
            {
                if(pieces[i,j]!=null)
                {
                    if(pieces[i,j].isWhite==player && pieces[i,j].CanMove(pieces))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
