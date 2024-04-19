using System.Collections;
using UnityEngine;

namespace Chess.Scripts.Core
{
    public class SetupBoard : MonoBehaviour
    {
        // [Header("White Pieces")]

        [SerializeField] private GameObject whitePawn, whiteBishop, whiteKnight, whiteRook, whiteQueen, whiteKing;

        // [Header("Black Pieces")]
        [SerializeField] private GameObject blackPawn, blackBishop, blackKnight, blackRook, blackQueen, blackKing;

        private void Start()
        {
            GenerateRandomStartPos();
        }

        private void GenerateRandomStartPos()
        {
            // Place white pawns
            for (int i = 0; i < 4; i++)
            {
                InstantiatePiece(whitePawn, Random.Range(2, 4), i);
            }

            // Place black pawns
            for (int i = 0; i < 4; i++)
            {
                InstantiatePiece(blackPawn, Random.Range(5, 7), i);
            }


            // Place remaining white pieces
            PlaceRandomPiece(whiteRook);
            PlaceRandomPiece(whiteRook);
            PlaceRandomPiece(whiteBishop);
            PlaceRandomPiece(whiteBishop);
            PlaceRandomPiece(whiteKnight);
            // PlaceRandomPiece(whiteKnight);
            PlaceRandomPiece(whiteQueen); 
            // Place remaining black pieces
            PlaceRandomPiece(blackRook);
            // PlaceRandomPiece(blackRook);
            PlaceRandomPiece(blackBishop);
            // PlaceRandomPiece(blackBishop);
            PlaceRandomPiece(blackKnight);
            // PlaceRandomPiece(blackKnight);
            PlaceRandomPiece(blackQueen);

            // Place kings
            PlaceRandomPiece(whiteKing); 
            PlaceRandomPiece(blackKing); 
        }

        private void PlaceRandomPiece(GameObject piece)
        {
            int row, column;
            do
            {
                row = Random.Range(1, 7);
                column = Random.Range(1, 7);
            } while (ChessBoardPlacementHandler.Instance.IsTileOccupied(row, column));
            InstantiatePiece(piece, row, column);
        }


        private void InstantiatePiece(GameObject piece, int row, int column)
        {
            GameObject newPiece = Instantiate(piece, ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position, Quaternion.identity);
            Vector2Int tile = new Vector2Int(row, column);
            newPiece.GetComponent<Piece>().SetTilePos(tile);
        }
    }
}
