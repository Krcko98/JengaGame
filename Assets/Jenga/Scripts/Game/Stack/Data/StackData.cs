using JengaGame.Game.Piece.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static JengaGame.API.Data.APIRequestData;

namespace JengaGame.Game.Stack.Data
{
    [System.Serializable]
    public class StackData
    {
        [SerializeField]
        private List<PieceData> pieces;

        public List<PieceData> GetPieceData { get => pieces; protected set => pieces = value; }

        public StackData(GetAssessmentData data)
        {
            if (data == null) return;
            if (data.jengaPieces == null) return;

            GetPieceData = new List<PieceData>();

            foreach(JengaPieceData piece in data.jengaPieces)
            {
                GetPieceData.Add(new PieceData(piece));
            }

            //Sort our list to fulfil set conditions
            GetPieceData = GetPieceData
                .OrderBy(x => (int)x.GetSchoolGrade.GetGrade)
                .ThenBy(x => x.Domain.DomainName)
                .ThenBy(x => x.Domain.Cluster)
                .ThenBy(x => x.Standard.StandardID).ToList();
        }
    }
}
