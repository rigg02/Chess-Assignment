using UnityEngine;

namespace Chess.Scripts.Core
{
    [System.Serializable]
    public class Piece : MonoBehaviour
    {
       
        public enum PieceColor
        {
            Black,
            White
        }

        
        public enum PieceType
        {
            Pawn,
            Bishop,
            Rook,
            Knight,
            Queen,
            King
        }


        [SerializeField] private PieceType _pieceType;
        [SerializeField] private int _cost;
        [SerializeField] private PieceColor _color;
        [SerializeField] private Vector2Int _tilePos; 

        public PieceType GetPieceType => _pieceType;
        public PieceColor GetPieceColor => _color;
        public int GetCost => _cost;

        public Vector2Int GetTilePos => _tilePos; 
        public void SetTilePos (Vector2Int tile){ 
            _tilePos = tile;
            ChessBoardPlacementHandler.Instance.SetOccupied(tile.x, tile.y); 
        }
    }
}
