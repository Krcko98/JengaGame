using System.Collections.Generic;
using UnityEngine;

namespace JengaGame.Game.Piece.Data
{
    public partial class PieceData
    {
        //All enums should be separate files and objects, this is a faster solution
        [System.Serializable]
        public class SchoolGrade
        {
            [SerializeField]
            private Grade grade;

            public Grade GetGrade { get => grade; protected set => grade = value; }

            public static Dictionary<string, Grade> gradeValues = new Dictionary<string, Grade>()
            {
                { "6th Grade", Grade.grade_6 },
                { "7th Grade", Grade.grade_7 },
                { "8th Grade", Grade.grade_8 },
                { "Algebra I", Grade.algebra_1 }
            };

            public enum Grade
            {
                grade_6 = 0,
                grade_7 = 1,
                grade_8 = 2,
                algebra_1 = 3
            }

            public SchoolGrade(string grade)
            {
                GetGrade = TryParseGrade(grade);
            }

            public static Grade TryParseGrade(string grade)
            {
                if (!gradeValues.ContainsKey(grade)) return Grade.grade_6;

                return gradeValues[grade];
            }

            public static string GetStringFromGrade(Grade grade)
            {
                foreach(KeyValuePair<string, Grade> gradeValue in gradeValues)
                {
                    if(gradeValue.Value == grade) return gradeValue.Key;
                }

                return "";
            }
        }
    }
}