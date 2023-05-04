using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JengaGame.Game.Piece.Object
{
    public class PieceObjectCollider : MonoBehaviour
    {
        [SerializeField]
        private PieceObject piece;

        public PieceObject Piece { get => piece; }
    }
}