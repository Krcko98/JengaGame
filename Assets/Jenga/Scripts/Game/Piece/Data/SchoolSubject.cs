using System.Collections.Generic;
using UnityEngine;

namespace JengaGame.Game.Piece.Data
{
    public partial class PieceData
    {
        [System.Serializable]
        public class SchoolSubject
        {
            [SerializeField]
            private Subject subject;

            public Subject GetSubject { get => subject; protected set => subject = value; }

            public static Dictionary<string, Subject> subjectValues = new Dictionary<string, Subject>()
            {
                { "Math", Subject.Math }
            };

            public enum Subject
            {
                Math = 0
            }

            public SchoolSubject(string subject)
            {
                GetSubject = TryParseSubject(subject);
            }

            public static Subject TryParseSubject(string subject)
            {
                if (!subjectValues.ContainsKey(subject)) return Subject.Math;

                return subjectValues[subject];
            }
        }
    }
}