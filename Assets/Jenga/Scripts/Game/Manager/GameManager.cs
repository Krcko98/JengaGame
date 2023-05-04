using JengaGame.API.Data;
using JengaGame.API.Manager;
using JengaGame.CameraNS;
using JengaGame.Game.Piece.Data;
using JengaGame.Game.Piece.Object;
using JengaGame.Game.Stack;
using JengaGame.Game.Stack.Data;
using JengaGame.Game.Stack.Object;
using JengaGame.UI.Info;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static JengaGame.API.Data.APIRequestData;
using static JengaGame.Game.Piece.Data.PieceData.SchoolGrade;

namespace JengaGame.Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField]
        private Transform stackParent;

        [SerializeField]
        private InfoWindow infoWindow;
        
        [Header("Objects")]
        [SerializeField]
        private PieceObject glassPiece;
        [SerializeField]
        private PieceObject woodPiece;
        [SerializeField]
        private PieceObject stonePiece;
        [SerializeField]
        private StackObject stackObject;
        [SerializeField]
        private CameraOrbit cameraOrbit;

        [SerializeField]
        private Button leftStackButton;
        [SerializeField]
        private Button rightStackButton;
        [SerializeField]
        private Button startSimulationButton;
        [SerializeField]
        private Button loadAssessmentButton;

        [Header("Interaction")]
        [SerializeField]
        private KeyCode selectPieceKey;

        [SerializeField]
        private LayerMask pieceLayerMask;

        [Header("Data")]
        [SerializeField]
        private List<StackObject> stacks = new List<StackObject>();

        [SerializeField]
        private StackData rootStackData;

        [SerializeField]
        private Vector3 stackSpacing;

        [SerializeField]
        private int removePieceMastery = 0;

        public static GameManager Instance = null;

        private int currentStackID = 0;

        private PieceObject lastPieceSelected = null;

        #region SETUP
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                setupAwake();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void setupAwake()
        {

        }

        private void addListeners()
        {
            rightStackButton.onClick.AddListener(OnFocusStackNext);
            leftStackButton.onClick.AddListener(OnFocusStackPrevious);
            startSimulationButton.onClick.AddListener(startSimulation);
            loadAssessmentButton.onClick.AddListener(loadAssesment);
        }

        private void removeListeners()
        {
            rightStackButton.onClick.RemoveListener(OnFocusStackNext);
            leftStackButton.onClick.RemoveListener(OnFocusStackPrevious);
            startSimulationButton.onClick.RemoveListener(startSimulation);
            loadAssessmentButton.onClick.RemoveListener(loadAssesment);
        }
        #endregion

        private void Update()
        {
            readInput();
        }

        #region CONTROL
        [ContextMenu("GetAssessment")]
        public void GetData()
        {
            APICommunicationManager.Instance.GetAssessmentStack(
                (data) => createStacks(data)
            );
        }

        private void createStacks(GetAssessmentData data)
        {
            rootStackData = new StackData(data);

            List<PieceData> pieceData = rootStackData.GetPieceData;

            if (pieceData.Count() <= 0) return;

            Grade lastGrade = pieceData.First().GetSchoolGrade.GetGrade;

            StackObject tempStackObj = createStack(stackPos: Vector3.zero, grade: lastGrade);

            //Create all stacks based on all available grades
            foreach(PieceData piece in pieceData)
            {
                if (piece.GetSchoolGrade.GetGrade != lastGrade)
                {
                    lastGrade = piece.GetSchoolGrade.GetGrade;
                    tempStackObj = createStack(stackPos: Vector3.zero, grade: lastGrade);
                }

                tempStackObj.InsertPiece(piece);
            }

            if (stacks.Count <= 0) return;

            int stackID = 0;
            float stackSize;
            float stackPos;
            float stackOffset;
            float spacing;

            //Position and start stack simulation
            foreach(StackObject obj in stacks)
            {
                obj.SetStackSize();

                stackSize = obj.StackSize.x;
                spacing = stackSpacing.x;

                stackPos = (stackSize + spacing) * stackID;
                stackOffset = ((stackSize + spacing) * (stacks.Count - 1)) / 2;

                obj.BuildStack(new Vector3(stackPos - stackOffset, 0, 0));

                stackID++;
            }

            focusCameraOnStack();
        }

        private StackObject createStack(Vector3 stackPos, Grade grade)
        {
            StackObject obj = Instantiate(stackObject, Vector3.zero, Quaternion.identity);
            obj.SetupStack(grade: grade, stackParent: stackParent, position: stackPos);

            stacks.Add(obj);

            return obj;
        }

        public PieceObject GetPieceObjectByMastery(int mastery)
        {
            switch(mastery)
            {
                case 0:
                return glassPiece;
                case 1:
                return woodPiece;
                case 2:
                return stonePiece;
            }

            return null;
        }

        private void OnDisable()
        {
            removeListeners();
        }

        private void OnEnable()
        {
            addListeners();
        }

        private void focusCameraOnStack()
        {
            cameraOrbit.SetFocusObject(stacks[currentStackID].transform);

            stacks[currentStackID].SelectStack();
        }

        private void pieceSelected(PieceObject piece)
        {
            PieceData data = piece.Piece;

            if (lastPieceSelected != null) lastPieceSelected.DeselectPiece();

            lastPieceSelected = piece;

            infoWindow.ShowInfoWindow($"{GetStringFromGrade(data.GetSchoolGrade.GetGrade)} : {data.Domain.DomainName}\n\n - {data.Domain.Cluster}\n\n{data.Standard.StandardID} :\n - {data.Standard.StandardDescription}");

            lastPieceSelected.SelectPiece();
        }

        private void emptySelected()
        {
            if(lastPieceSelected != null)
            {
                lastPieceSelected.DeselectPiece();
                lastPieceSelected = null;
            }

            infoWindow.CloseInfoWindow();
        }

        private void startSimulation()
        {
            foreach(StackObject obj in stacks)
            {
                obj.RemovePieces(removePieceMastery);
                obj.SimulatePhysics();
            }
        }

        private void loadAssesment()
        {
            GetData();

            loadAssessmentButton.gameObject.SetActive(false);

            startSimulationButton.gameObject.SetActive(true);
            rightStackButton.gameObject.SetActive(true);
            leftStackButton.gameObject.SetActive(true);
        }
        #endregion

        #region UI
        private void OnFocusStackNext()
        {
            if (stacks == null) return;
            if (stacks.Count <= 0) return;

            calculateNextStack(1);
        }

        private void OnFocusStackPrevious()
        {
            if (stacks == null) return;
            if (stacks.Count <= 0) return;

            calculateNextStack(-1);
        }

        private void calculateNextStack(int dir)
        {
            dir = dir / Mathf.Abs(dir);

            stacks[currentStackID].DeselectStack();

            currentStackID = (currentStackID + dir + stacks.Count) % stacks.Count;
            focusCameraOnStack();
        }
        #endregion

        #region INTERACTION
        private void readInput()
        {
            if (!Input.GetKeyUp(selectPieceKey)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            PieceObjectCollider pieceCol;

            emptySelected();

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, pieceLayerMask))
            {
                if (hit.transform == null) return;

                pieceCol = hit.transform.GetComponent<PieceObjectCollider>();

                if (pieceCol == null) return;
                if (pieceCol.Piece == null) return;

                pieceSelected(pieceCol.Piece);
            }
        }
        #endregion
    }
}