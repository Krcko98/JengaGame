using System;
using UnityEngine;

namespace JengaGame.Game.Piece.Data
{
    public partial class PieceData
    {
        [System.Serializable]
        public class SchoolDomain
        {
            [SerializeField]
            private DomainID domainID;
            [SerializeField]
            private string domainName;
            [SerializeField]
            private string cluster;

            public DomainID GetDomainID { get => domainID; protected set => domainID = value; }
            public string DomainName { get => domainName; protected set => domainName = value; }
            public string Cluster { get => cluster; protected set => cluster = value; }

            public enum DomainID
            {
                RP = 0,
                NS = 1,
                EE = 2,
                G = 3,
                SP = 4,
                F = 5,
                RN = 6
            }

            public SchoolDomain(string domainID, string domainName, string cluster)
            {
                DomainName = domainName;
                Cluster = cluster;
                GetDomainID = TryParseDomainID(domainID);
            }

            public static DomainID TryParseDomainID(string domainID)
            {
                DomainID id;

                Enum.TryParse(domainID, out id);

                return id;
            }
        }
    }
}