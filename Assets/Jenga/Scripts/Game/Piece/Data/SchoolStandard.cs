using UnityEngine;

namespace JengaGame.Game.Piece.Data
{
    public partial class PieceData
    {
        [System.Serializable]
        public class SchoolStandard
        {
            [SerializeField]
            private string standardID;
            [SerializeField]
            private string standardDescription;

            public string StandardID { get => standardID; protected set => standardID = value; }
            public string StandardDescription { get => standardDescription; protected set => standardDescription = value; }

            public SchoolStandard(string standardID, string standardDescription)
            {
                StandardID = standardID;
                StandardDescription = standardDescription;
            }
        }
    }
}