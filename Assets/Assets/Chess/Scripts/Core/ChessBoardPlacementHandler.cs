using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using Chess.Scripts.Core; 

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public sealed class ChessBoardPlacementHandler : MonoBehaviour {
    [SerializeField] private GameObject[] _rowsArray;
    [SerializeField] private GameObject _highlightPrefab;
    private GameObject[,] _chessBoard;

    private bool[,] _occupiedTiles = new bool[8, 8];

    internal static ChessBoardPlacementHandler Instance;

    private void Awake() {
        Instance = this;
        GenerateArray();
    }

    private void GenerateArray() {
        _chessBoard = new GameObject[8, 8];
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++) {
                _chessBoard[i, j] = _rowsArray[i].transform.GetChild(j).gameObject;
            }
        }
    }

    internal GameObject GetTile(int i, int j) {
        try {
            return _chessBoard[i, j];
        } catch (Exception) {
            Debug.LogError("Invalid row or column.");
            return null;
        }
    }

   internal Piece GetPieceAt(int row, int column)

    {
        try
        {
            // Convert row and column to world position
            Vector3 worldPosition = new Vector3(row, 0, column);

            // Perform a 3D overlap sphere check with a small radius
            Collider[] hitColliders = Physics.OverlapSphere(worldPosition, 0.1f, 9);

            // Check if any pieces were found within the sphere
            foreach (Collider collider in hitColliders)
            {
                // Get the Piece component from the collider
                Piece piece = collider.GetComponent<Piece>();

                if (piece != null)
                {
                    return piece;
                }
            }

            // Return null if no piece was found
            return null;
        }
        catch (Exception)
        {
            Debug.LogError("Invalid row or column.");
            return null;
        }
    }



    internal int[] GetRowCol(GameObject tile)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (tile == _chessBoard[i,j])
                {
                    return new int[] { i, j }; // Return the row and column as an array
                }
            }
        }
        return null; // Return null if the tile is not found
    }


    internal void Highlight(int row, int col) {
        var tile = GetTile(row, col).transform;
        if (tile == null) {
            Debug.LogError("Invalid row or column.");
            return;
        }

        Instantiate(_highlightPrefab, tile.transform.position, Quaternion.identity, tile.transform);
    }

    internal void ClearHighlights() {
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++) {
                var tile = GetTile(i, j);
                if (tile.transform.childCount <= 0) continue;
                foreach (Transform childTransform in tile.transform) {
                    Destroy(childTransform.gameObject);
                }
            }
        }
    }


    internal bool IsTileOccupied(int row, int column){
        return _occupiedTiles[row,column]; 
    }

    internal void SetOccupied(int i, int j)
    {
        _occupiedTiles[i,j] = true; 
    }
    #region Highlight Testing

    // private void Start() {
    //     StartCoroutine(Testing());
    // }

    // private IEnumerator Testing() {
    //     Highlight(2, 7);
    //     yield return new WaitForSeconds(1f);
    
    //     ClearHighlights();
    //     Highlight(2, 7);
    //     Highlight(2, 6);
    //     Highlight(2, 5);
    //     Highlight(2, 4);
    //     yield return new WaitForSeconds(1f);
    
    //     ClearHighlights();
    //     Highlight(7, 7);
    //     Highlight(2, 7);
    //     yield return new WaitForSeconds(1f);
    // }

    #endregion
}