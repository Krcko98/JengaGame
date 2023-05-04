using JengaGame.Game.Piece.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JengaGame.Game.Piece.Object
{
    public class PieceObject : MonoBehaviour
    {
        [SerializeField]
        private Transform rootParent;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private Rigidbody rig;

        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        private Color deselectedColor;

        public PieceData Piece { get; protected set; }

        public void SetupPiece(Vector3 position, Vector3 size, bool flip, Transform parent, PieceData data)
        {
            transform.SetParent(parent);
            transform.localPosition = new Vector3(
                flip ? position.z : position.x,
                position.y,
                flip ? position.x : position.z
            );

            rootParent.transform.localScale = size;
            rootParent.transform.localEulerAngles = flip ? new Vector3(0 , 90f, 0) : Vector3.zero;

            Piece = data;
        }

        public void SelectPiece()
        {
            meshRenderer.material.SetColor("_SpecColor", selectedColor);
        }

        public void DeselectPiece()
        {
            meshRenderer.material.SetColor("_SpecColor", deselectedColor);
        }

        public void EnablePhysics()
        {
            rig.isKinematic = false;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}