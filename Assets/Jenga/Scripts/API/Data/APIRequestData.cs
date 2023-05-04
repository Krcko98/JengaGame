using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JengaGame.API.Data
{
    public class APIRequestData
    {
        [Serializable]
        public class GetAssessmentData
        {
            public List<JengaPieceData> jengaPieces = new List<JengaPieceData>();
        }

        [Serializable]
        public struct JengaPieceData
        {
            public int id;
            public string subject;
            public string grade;
            public int mastery;
            public string domainid;
            public string domain;
            public string cluster;
            public string standardid;
            public string standarddescription;
        }
    }
}