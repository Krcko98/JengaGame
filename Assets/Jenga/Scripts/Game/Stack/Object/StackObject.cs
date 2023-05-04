using JengaGame.Game.Manager;
using JengaGame.Game.Piece.Data;
using JengaGame.Game.Piece.Object;
using JengaGame.Game.Stack.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace JengaGame.Game.Stack.Object
{
    public class StackObject : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private int maxRows = 0;

        [SerializeField]
        private Vector3 pieceSize;

        [SerializeField]
        private Vector3 pieceSpacing;
        
        [SerializeField]
        private Vector3 stackOffset;

        [SerializeField]
        private Color selectStackCol;

        [SerializeField]
        private Color deselectedStackCol;

        [Header("Objects")]
        [SerializeField]
        private List<PieceObject> pieces = new List<PieceObject>();

        [SerializeField]
        private TextMeshPro gradeLabel;

        [SerializeField]
        private PieceData.SchoolGrade.Grade stackGrade;

        [SerializeField]
        private Transform rootParent;

        [SerializeField]
        private List<PieceData> pieceDataList = new List<PieceData>();

        private Vector3 stackSize = Vector3.zero;

        public Vector3 StackSize { get => stackSize; }

        #region SETUP
        public void SetupStack(PieceData.SchoolGrade.Grade grade, Transform stackParent, Vector3 position)
        {
            stackGrade = grade;
            transform.SetParent(stackParent, false);
            transform.localPosition = position;

            gradeLabel.text = PieceData.SchoolGrade.GetStringFromGrade(stackGrade);

            DeselectStack();
        }

        private void setPieceSize()
        {
            pieceSize.x = (pieceSize.z + pieceSpacing.z) * maxRows - pieceSpacing.z;
        }

        private void setStackPos(Vector3 pos)
        {
            transform.localPosition = new Vector3(
                pos.x,
                transform.localPosition.y,
                pos.z
            );
        }

        public void SetStackSize()
        {
            float sizeX = (pieceSize.z + pieceSpacing.z) * maxRows - pieceSpacing.z;

            stackSize = new Vector3(
                sizeX,
                0,
                sizeX
            );
        }

        private void setStackOffset()
        {
            stackOffset = new Vector3(0, 0, ((pieceSize.z + pieceSpacing.z) * (maxRows - 1)) / 2);
        }
        #endregion

        #region CONTROL
        public void InsertPiece(PieceData pieceData)
        {
            if (pieceDataList == null) return;

            pieceDataList.Add(pieceData);
        }

        public void BuildStack(Vector3 pos)
        {
            setStackPos(pos);
            createStack();
        }

        private void createStack()
        {
            PieceObject obj;

            int posY = 0;
            int posZ = 0;
            bool flipPiece;

            foreach (PieceData piece in pieceDataList)
            {
                flipPiece = posY % 2 == 0;

                obj = Instantiate(GameManager.Instance.GetPieceObjectByMastery(piece.Mastery), Vector3.zero, Quaternion.identity);
                obj.SetupPiece(
                    position: getPiecePosition(z: posZ, y: posY) - stackOffset,
                    size: pieceSize,
                    flip: flipPiece,
                    parent: rootParent,
                    data: piece
                );

                posZ++;

                if (posZ >= maxRows)
                {
                    posY++;
                    posZ = 0;
                }

                pieces.Add(obj);
            }
        }

        private Vector3 getPiecePosition(int z, int y)
        {
            float posZ = z * (pieceSize.z + pieceSpacing.z);
            float posY = y * pieceSize.y;

            return new Vector3(0, posY, posZ);
        }
        #endregion

        public void OnValidate()
        {
            setPieceSize();
            setStackOffset();
        }

        public void SelectStack()
        {
            gradeLabel.color = selectStackCol;
        }

        public void DeselectStack()
        {
            gradeLabel.color = deselectedStackCol;
        }

        public void RemovePieces(int mastery)
        {
            List<PieceObject> objs = pieces.FindAll(x => x.Piece.Mastery == mastery);

            pieces.RemoveAll(x => x.Piece.Mastery == mastery);

            objs.ForEach(x => x.Destroy());

        }

        public void SimulatePhysics()
        {
            if (pieces == null) return;

            pieces.ForEach(x => x.EnablePhysics());
        }
    }
}
