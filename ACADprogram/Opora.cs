using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ACADprogram
{

    public enum OporaType
    {
        U500n_1, U500n_1_5,U500n_1_12, U500n_1_17, PS500n_3,PS500n_3_5,PS500n_3_12
    }
    public class Opora
    {
        public string Name { get; set; }
        public int Baza_A { get; set; }
        public int Baza_B { get; set; }

        public class OporaName
        {
            public OporaType Type { get; set; }
            public string Name { get; set; }
        }
        public static List<OporaName> oporanames { get; set; } = new List<OporaName>()
        {
            new OporaName() { Type = OporaType.U500n_1, Name = "Опора У500н-1" },
            new OporaName() { Type = OporaType.U500n_1_5, Name = "Опора У500н-1+5" },
            new OporaName() { Type = OporaType.U500n_1_12, Name = "Опора У500н-1+12" },
            new OporaName() { Type = OporaType.U500n_1_17, Name = "Опора У500н-1+17" },
            new OporaName() { Type = OporaType.PS500n_3, Name = "Опора ПС500н-3" },
            new OporaName() { Type = OporaType.PS500n_3_5, Name = "Опора ПС500н-3+5" },
            new OporaName() { Type = OporaType.PS500n_3_12, Name = "Опора ПС500н-3+12" }
        };
        public static readonly Dictionary<OporaType, Opora> AllOpora = new Dictionary<OporaType, Opora>()
        {
            {
                OporaType.U500n_1, new Opora()
                {
                    Baza_A = 5530,
                    Baza_B = 5530
                }
            },
            {
                OporaType.U500n_1_5, new Opora()
                {
                    Baza_A = 6700,
                    Baza_B = 6700
                }
            },
            {
                OporaType.U500n_1_12, new Opora()
                {
                    Baza_A = 8335,
                    Baza_B = 8335
                }
            },
            {
                OporaType.U500n_1_17, new Opora()
                {
                    Baza_A = 9505,
                    Baza_B = 9505
                }
            },
            {
                OporaType.PS500n_3, new Opora()
                {
                    Baza_A = 8300,
                    Baza_B = 5700
                }
            },
            {
                OporaType.PS500n_3_5, new Opora()
                {
                    Baza_A = 9450,
                    Baza_B = 6490
                }
            },
            {
                OporaType.PS500n_3_12, new Opora()
                {
                    Baza_A = 11050,
                    Baza_B = 7590
                }
            },
        };
    }
    


}
