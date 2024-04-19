using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Chess.Scripts.Core
{
    public class ChessPlayerPlacementHandler : MonoBehaviour
    {
        private bool _isHighlighting = false;
        private Piece _highlightedPiece;


        [SerializeField] private LayerMask _pieceLayer2D;
        [SerializeField] private GameObject _highlighter;
        [SerializeField] private GameObject _canBeCaptured; 

        private List<GameObject> _captureList; 

        private void Start()
        {
            _captureList = new List<GameObject>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                //Remove all Highlights before making move
                ChessBoardPlacementHandler.Instance.ClearHighlights();
                ClearPreviousChaptures(); 
                //identify piece picked using Raycasts
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, _pieceLayer2D);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<Piece>(out Piece piece))
                    {
                        if (piece.GetPieceColor == Piece.PieceColor.White)
                        {

                            HighlightSelectedPiece(piece);
                            SuggestMove(piece);
                        }
                    }
                }
                else if (_isHighlighting)
                {

                    ClearSelected();
                    ChessBoardPlacementHandler.Instance.ClearHighlights();
                }
            }
        }

        
        private void HighlightSelectedPiece(Piece piece)
        {
            // Hilights Piece you're moving 
            if (_isHighlighting)
            {
                ClearSelected();
            }
            _isHighlighting = true;
            _highlightedPiece = piece;
            _highlighter.SetActive(true);
            _highlighter.transform.position = piece.transform.position;
        }


        private void ClearSelected()
        {
            // clear selected piece 
            _isHighlighting = false;
            _highlightedPiece = null;
            _highlighter.SetActive(false);
        }


        private void SuggestMove(Piece piece)
        {

            // suggests moves based on piece type
            Vector2Int pos = piece.GetTilePos;

            switch (piece.GetPieceType)
            {
                case Piece.PieceType.Pawn:
                    //1 square -> up
                    GetPawnMovement(pos); 

                    break;
                case Piece.PieceType.Rook:
                    
                    GetStraightMovement(pos); 
                    break;

                case Piece.PieceType.Bishop:
                    GetDiagonalMovement(pos); 
                    //all squares -> diagonally
                    break;
                case Piece.PieceType.Queen:
                    //all squares -> up, down, left, right
                    //all squares -> diagonally

                    GetStraightMovement(pos); 
                    GetDiagonalMovement(pos); 

                    break;
                case Piece.PieceType.Knight:
                    //dhai -> all directions -> front left and right, back left and right
                    GetKnightMoves(pos); 
                    break;
                case Piece.PieceType.King:
                    //1 square -> up, down, left, right, diagonal
                    GetKingMoves(pos); 
                    break;

            }
        }


        private void GetPawnMovement(Vector2Int pos)
        {

            if (!IsOccupied(pos.x + 1, pos.y) && IsWithinBoard(pos.x+1, pos.y))
            {
                HighlightOnBoard(pos.x + 1, pos.y);
            }
            if (IsOccupied(pos.x + 1, pos.y+1) && IsWithinBoard(pos.x+1, pos.y+1))
            {
                // HighlightOnBoard(pos.x + 1, pos.y);
                // make it red
            }
        }
        private void GetStraightMovement(Vector2Int pos)
        {
            // Loop through all four directions
            Vector2Int[] directions = new Vector2Int [] {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
            foreach (var direction in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    
                    int x = pos.x + direction.x * i;
                    int y = pos.y + direction.y * i;

                    // Check if the square is within the board and not occupied
                    if (IsWithinBoard(x, y) && !IsOccupied(x, y))
                    {
                        HighlightOnBoard(x, y);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        
        private void GetDiagonalMovement(Vector2Int pos){
            // //right
            int j = pos.y+1; 
            int k = pos.y-1; 

            for (int i = pos.x + 1; i < 8; i++)
            {   
                if(j <=7)
                {
                    bool check = IsOccupied(i, j); 
                    if(check) break; 
                    HighlightOnBoard(i,j); 
                }      
                j+=1; 
            }
            for (int i = pos.x + 1; i < 8; i++)
            {   
                if(k >=0)
                {
                    bool check = IsOccupied(i, k); 
                    if(check) break; 
                    HighlightOnBoard(i,k); 
                }      
                k-=1; 
            }

            j = pos.y+1; 
            for (int i = pos.x - 1; i >= 0; i--)
            {   
                if(j <=7)
                {
                    bool check = IsOccupied(i, j); 
                    if(check) break; 
                    HighlightOnBoard(i,j); 
                }      
                j+=1; 
            }

            k = pos.y-1; 
            for (int i = pos.x - 1; i >= 0; i--)
            {   
                if(k >=0)
                {
                    bool check = IsOccupied(i, k); 
                    if(check) break; 
                    HighlightOnBoard(i,k); 
                }      
                k-=1; 
            }

        }


        private void GetKnightMoves(Vector2Int pos)
        {
            int[] xOffsets = { 2, 2, -2, -2, 1, -1, 1, -1 };
            int[] yOffsets = { 1, -1, 1, -1, 2, 2, -2, -2 };

            for (int i = 0; i < 8; i++)
            {
                int x = pos.x + xOffsets[i];
                int y = pos.y + yOffsets[i];

                if (IsWithinBoard(x, y) && !IsOccupied(x, y))
                {
                    HighlightOnBoard(x, y);
                }
            }
        }

       private void GetKingMoves(Vector2Int pos)
        {
            int[] xOffsets = { 1, 1, 1, 0, 0, -1, -1, -1 };
            int[] yOffsets = { 1, 0, -1, 1, -1, 1, 0, -1 };

            for (int i = 0; i < 8; i++)
            {
                int x = pos.x + xOffsets[i];
                int y = pos.y + yOffsets[i];

                if (IsWithinBoard(x, y) && !IsOccupied(x, y))
                {
                    HighlightOnBoard(x, y);
                }
            }
        }


        private bool IsWithinBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        private bool IsOccupied(int row, int column)
        {
            bool check = ChessBoardPlacementHandler.Instance.IsTileOccupied(row, column); 
            if(check)
            {
                // Piece capture = ChessBoardPlacementHandler.Instance.GetPieceAt(row,column); 
                // if(capture != null && capture.GetPieceColor == Piece.PieceColor.Black)
                // {
                    // Debug.Log(capture.gameObject); 
                    // GameObject g = Instantiate(_canBeCaptured, 
                    //     ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position,
                    //     Quaternion.identity
                    // ); 
                    // _captureList.Add(g); 
                // }


                // Currently Marks all pieces as capturable. 
                GameObject g = Instantiate(_canBeCaptured, 
                    ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position,
                    Quaternion.identity
                ); 
                _captureList.Add(g); 
            }

            return check; 
        }

        private void ClearPreviousChaptures()
        {
            if(_captureList.Count <= 0 ) return; 

            foreach(GameObject g in _captureList){
                Destroy(g); 
            }
        }

        private void HighlightOnBoard(int row, int column){
            ChessBoardPlacementHandler.Instance.Highlight(row, column); 
        }
    }
}