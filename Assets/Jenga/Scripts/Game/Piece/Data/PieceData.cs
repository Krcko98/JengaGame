using System.Collections;
using UnityEngine;
using static JengaGame.API.Data.APIRequestData;

namespace JengaGame.Game.Piece.Data
{
    [System.Serializable]
    public partial class PieceData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private SchoolSubject schoolSubject;
        [SerializeField]
        private SchoolGrade schoolGrade;
        [SerializeField]
        private int mastery;
        [SerializeField]
        private SchoolDomain schoolDomain;
        [SerializeField]
        private SchoolStandard schoolStandard;

        public int ID { get => id; protected set => id = value; }
        public SchoolSubject GetSchoolSubject { get => schoolSubject; protected set => schoolSubject = value; }
        public SchoolGrade GetSchoolGrade { get => schoolGrade; protected set => schoolGrade = value; }
        public int Mastery { get => mastery; protected set => mastery = value; }
        public SchoolDomain Domain { get => schoolDomain; protected set => schoolDomain = value; }
        public SchoolStandard Standard { get => schoolStandard; protected set => schoolStandard = value; }

        public PieceData(JengaPieceData data)
        {
            ID = data.id;
            GetSchoolSubject = new SchoolSubject(data.subject);
            GetSchoolGrade = new SchoolGrade(data.grade);
            Mastery = data.mastery;
            Domain = new SchoolDomain(
                domainID: data.domainid, 
                domainName: data.domain, 
                cluster: data.cluster
            );
            Standard = new SchoolStandard(
                standardID: data.standardid,
                standardDescription: data.standarddescription
            );
        }
    }
}