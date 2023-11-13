using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Windows.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ACADprogram
{
    public enum FoundationType
    {
        F3n_A, F4n_A, F5n_A, F5_5n_A,FP5n_A,F6n_A, FS1n_A,FS2n_A,FSP1n_A, FSP2n_A,F1n_2,F2n_2,F3n_2,F4n_2,F4n_4,F4_5n_2,F4_5n_4,F5n_2,F5n_4,F6n_2,F6n_4,FP6n_2,FP6n_4,FS1n_2,FS1n_4,FS2n_2,FS2n_4
    }

    public class Foundation
    {
        public string Name { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int a1 { get; set; }
        public int b1 { get; set; }
        public int ak { get; set; }
        public int bk { get; set; }
        public int ak1 { get; set; }
        public int bk1 { get; set; }
        public int h1 { get; set; }
        public int hk1 { get; set; }
        public int hf { get; set; }
        public int e { get; set; }
        public int bolt { get; set; }
        public int bolt_L { get; set; }
        public int bolt_z { get; set; }
        public int n_bolt { get; set; }
        public int Ugol_povorota { get; set; }
        public int Glubina_Zalozhenia { get; set; }

        /// <summary>
        /// Доп. размеры для специальных и составных фундаментов
        /// </summary>
        public int h2 { get; set; }
        public int a2 { get; set; }
        public int b2 { get; set; }
        public int h3 { get; set; }
        public int a3 { get; set; }
        public int b3 { get; set; }
        public int h4 { get; set; }
        public int a4 { get; set; }
        public int b4 { get; set; }





        public static List<FoundationName> foundationNames { get; set; } = new List<FoundationName>()
        {
            new FoundationName(){Type = FoundationType.F3n_A,Name = "Фундамент Ф3н-А"},
            new FoundationName(){Type = FoundationType.F4n_A,Name = "Фундамент Ф4н-А"},
            new FoundationName(){Type = FoundationType.F5n_A,Name = "Фундамент Ф5н-А"},
            new FoundationName(){Type = FoundationType.F6n_A,Name = "Фундамент Ф6н-А"},
            new FoundationName(){Type = FoundationType.F5_5n_A,Name = "Фундамент Ф5.5н-А"},
            new FoundationName(){Type = FoundationType.FP5n_A,Name = "Фундамент ФП5н-А"},
            new FoundationName(){Type = FoundationType.FS1n_A,Name = "Фундамент ФС1н-А"},
            new FoundationName(){Type = FoundationType.FS2n_A,Name = "Фундамент ФС2н-А"},
            new FoundationName(){Type = FoundationType.FSP1n_A,Name = "Фундамент ФСП1н-А"},
            new FoundationName(){Type = FoundationType.FSP2n_A,Name = "Фундамент ФСП2н-А"},
            new FoundationName(){Type = FoundationType.F1n_2,Name = "Фундамент Ф1н-2"},
            new FoundationName(){Type = FoundationType.F2n_2,Name = "Фундамент Ф2н-2"},
            new FoundationName(){Type = FoundationType.F3n_2,Name = "Фундамент Ф3н-2"},
            new FoundationName(){Type = FoundationType.F4n_2,Name = "Фундамент Ф4н-2"},
            new FoundationName(){Type = FoundationType.F4n_4,Name = "Фундамент Ф4н-4"},
            new FoundationName(){Type = FoundationType.F4_5n_2,Name = "Фундамент Ф4.5н-2"},
            new FoundationName(){Type = FoundationType.F4_5n_4,Name = "Фундамент Ф4.5н-4"},
            new FoundationName(){Type = FoundationType.F5n_2,Name = "Фундамент Ф5н-2"},
            new FoundationName(){Type = FoundationType.F5n_4,Name = "Фундамент Ф5н-4"},
            new FoundationName(){Type = FoundationType.F6n_2,Name = "Фундамент Ф6н-2"},
            new FoundationName(){Type = FoundationType.F6n_4,Name = "Фундамент Ф6н-4"},
            new FoundationName(){Type = FoundationType.FP6n_2,Name = "Фундамент ФП6н-2"},
            new FoundationName(){Type = FoundationType.FP6n_4,Name = "Фундамент ФП6н-4"},
            new FoundationName(){Type = FoundationType.FS1n_2,Name = "Фундамент ФС1н-2"},
            new FoundationName(){Type = FoundationType.FS1n_4,Name = "Фундамент ФС1н-4"},
            new FoundationName(){Type = FoundationType.FS2n_2,Name = "Фундамент ФС2н-2"},
            new FoundationName(){Type = FoundationType.FS2n_4,Name = "Фундамент ФС2н-4"},




        };

        public static readonly Dictionary<FoundationType, Foundation> AllFoundations = new Dictionary<FoundationType, Foundation>()
        {
            {
                FoundationType.F3n_A, new Foundation()
                {
                    a = 2100,
                    b=2100,
                    a1=2040,
                    b1=2040,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=3115,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=2850,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0,
                }
            },
            {
                FoundationType.F4n_A, new Foundation()
                {
                    a = 2400,
                    b=2400,
                    a1=2340,
                    b1=2340,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=3115,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=2850,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0,
                }
            },
            {
                FoundationType.F5n_A, new Foundation()
                {
                    a = 2700,
                    b=2700,
                    a1=2640,
                    b1=2640,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=350,
                    hf=3115,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=2850,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0,
                }
            },
            {
                FoundationType.F6n_A, new Foundation()
                {
                    a = 3000,
                    b=2020,
                    a1=2980,
                    b1=2000,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=0,
                    hf=3115,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=2850,
                    h2=120,
                    a2=2745,
                    b2=1760,
                    h3=100,
                    a3=2548,
                    b3=730,
                    h4=300,
                    a4=1960,
                    b4=700,
                }
            },
            {
                FoundationType.F5_5n_A,new Foundation()
                {
                    a = 2700,
                    b=2700,
                    a1=2640,
                    b1=2640,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=350,
                    hf=4115,
                    e=870,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=3850,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0,
                }
            },
            {
                FoundationType.FP5n_A,new Foundation()
                {
                    a = 2700,
                    b=2700,
                    a1=2640,
                    b1=2640,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=350,
                    hf=5115,
                    e=1084,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=4850,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0,
                }
            },
            {
                FoundationType.FS1n_A,new Foundation()
                {
                    a = 3000,
                    b=2020,
                    a1=2980,
                    b1=2000,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=0,
                    hf=3115,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=2850,
                    h2=120,
                    a2=2745,
                    b2=1760,
                    h3=100,
                    a3=2548,
                    b3=730,
                    h4=300,
                    a4=1960,
                    b4=700,
                }
            },
            {
                FoundationType.FS2n_A,new Foundation()
                {
                    a = 3000,
                    b=2020,
                    a1=2980,
                    b1=2000,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=0,
                    hf=3115,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=2850,
                    h2=120,
                    a2=2745,
                    b2=1760,
                    h3=100,
                    a3=2548,
                    b3=730,
                    h4=300,
                    a4=1960,
                    b4=700,
                }
            },
            {
                FoundationType.FSP1n_A,new Foundation()
                {
                    a = 3000,
                    b=2020,
                    a1=2980,
                    b1=2000,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=0,
                    hf=5265,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=5000,
                    h2=120,
                    a2=2745,
                    b2=1760,
                    h3=100,
                    a3=2548,
                    b3=730,
                    h4=300,
                    a4=1960,
                    b4=700,
                }
            },
            {
                FoundationType.FSP2n_A,new Foundation()
                {
                    a = 3000,
                    b=2020,
                    a1=2980,
                    b1=2000,
                    ak=600,
                    bk=600,
                    ak1=408,
                    bk1=400,
                    h1=100,
                    hk1=0,
                    hf=5265,
                    e=660,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=45,
                    Glubina_Zalozhenia=5000,
                    h2=120,
                    a2=2745,
                    b2=1760,
                    h3=100,
                    a3=2548,
                    b3=730,
                    h4=300,
                    a4=1960,
                    b4=700,
                }
            },
            {
                FoundationType.F1n_2,new Foundation()
                {
                    a = 1200,
                    b=1200,
                    a1=1170,
                    b1=1170,
                    ak=370,
                    bk=370,
                    ak1=320,
                    bk1=320,
                    h1=100,
                    hk1=200,
                    hf=2700,
                    e=0,
                    bolt=36,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=2500,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F2n_2,new Foundation()
                {
                    a = 1500,
                    b=1500,
                    a1=1470,
                    b1=1470,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=2700,
                    e=0,
                    bolt=36,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=2500,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F3n_2,new Foundation()
                {
                    a = 1800,
                    b=1800,
                    a1=1770,
                    b1=1770,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=2700,
                    e=0,
                    bolt=36,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=2500,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F4n_2,new Foundation()
                {
                    a = 2100,
                    b=2100,
                    a1=2070,
                    b1=2070,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=2700,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=2500,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F4n_4,new Foundation()
                {
                    a = 2100,
                    b=2100,
                    a1=2070,
                    b1=2070,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=2700,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=2500,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F4_5n_2,new Foundation()
                {
                    a = 2100,
                    b=2100,
                    a1=2070,
                    b1=2070,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=3000,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F4_5n_4,new Foundation()
                {
                    a = 2100,
                    b=2100,
                    a1=2070,
                    b1=2070,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=3000,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F5n_2,new Foundation()
                {
                    a = 2400,
                    b=2400,
                    a1=2370,
                    b1=2370,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=3000,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F5n_4,new Foundation()
                {
                    a = 2400,
                    b=2400,
                    a1=2370,
                    b1=2370,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=300,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=3000,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F6n_2,new Foundation()
                {
                    a = 2700,
                    b=2700,
                    a1=2670,
                    b1=2670,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=350,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=3000,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.F6n_4,new Foundation()
                {
                    a = 2700,
                    b=2700,
                    a1=2670,
                    b1=2670,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=350,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=3000,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.FP6n_2,new Foundation()
                {
                    a = 2700,
                    b=2700,
                    a1=2670,
                    b1=2670,
                    ak=500,
                    bk=500,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=350,
                    hf=5000,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=4800,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.FP6n_4,new Foundation()
                {
                    a = 2700,
                    b=2700,
                    a1=2670,
                    b1=2670,
                    ak=500,
                    bk=500,
                    ak1=400,
                    bk1=400,
                    h1=100,
                    hk1=350,
                    hf=5000,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=4800,
                    h2=0,
                    a2=0,
                    b2=0,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.FS1n_2,new Foundation()
                {
                    a = 3500,
                    b=2700,
                    a1=3480,
                    b1=2680,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=80,
                    hk1=292,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=4800,
                    h2=130,
                    a2=3440,
                    b2=530,
                    h3=98,
                    a3=3440,
                    b3=450,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.FS1n_4,new Foundation()
                {
                    a = 3500,
                    b=2700,
                    a1=3480,
                    b1=2680,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=80,
                    hk1=292,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=4800,
                    h2=130,
                    a2=3440,
                    b2=530,
                    h3=98,
                    a3=3440,
                    b3=450,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.FS2n_2,new Foundation()
                {
                    a = 4500,
                    b=2700,
                    a1=4480,
                    b1=2680,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=80,
                    hk1=292,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=200,
                    n_bolt=2,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=4800,
                    h2=130,
                    a2=4440,
                    b2=530,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
            {
                FoundationType.FS2n_4,new Foundation()
                {
                    a = 4500,
                    b=2700,
                    a1=4480,
                    b1=2680,
                    ak=450,
                    bk=450,
                    ak1=400,
                    bk1=400,
                    h1=80,
                    hk1=292,
                    hf=3200,
                    e=0,
                    bolt=42,
                    bolt_L=150,
                    bolt_z=250,
                    n_bolt=4,
                    Ugol_povorota=0,
                    Glubina_Zalozhenia=4800,
                    h2=130,
                    a2=4440,
                    b2=530,
                    h3=0,
                    a3=0,
                    b3=0,
                    h4=0,
                    a4=0,
                    b4=0
                }
            },
        };
        
    }

    public class FoundationName
    {
        public FoundationType Type { get; set; }
        public string Name { get; set; }
    }
    public class FoundationBuild
    {
        public static Solid3d Build(FoundationType FoundationTypei, Vector3d vector_ponizh)
        {
            ////ФУНДАМЕНТЫ. ТИП 4
            var DlPOD = Foundation.AllFoundations[FoundationTypei].a;
            var ShirPOD = Foundation.AllFoundations[FoundationTypei].b;
            var Dl_1Stup = Foundation.AllFoundations[FoundationTypei].a1;
            var Shir_1Stup = Foundation.AllFoundations[FoundationTypei].b1;
            var Dl_kol = Foundation.AllFoundations[FoundationTypei].ak;
            var Shir_kol = Foundation.AllFoundations[FoundationTypei].bk;
            var Dl_Ogolovor = Foundation.AllFoundations[FoundationTypei].ak1;
            var Shir_Ogolovok = Foundation.AllFoundations[FoundationTypei].bk1;
            var Vus_1Stup = Foundation.AllFoundations[FoundationTypei].h1;
            var Vus_doKo = Foundation.AllFoundations[FoundationTypei].hk1;
            var Vus_Fund = Foundation.AllFoundations[FoundationTypei].hf;
            var excentr = Foundation.AllFoundations[FoundationTypei].e;
            var diametr_bolta = Foundation.AllFoundations[FoundationTypei].bolt;
            var Dl_bolt = Foundation.AllFoundations[FoundationTypei].bolt_L;
            var between_bolt = Foundation.AllFoundations[FoundationTypei].bolt_z;
            var quantity_bolt = Foundation.AllFoundations[FoundationTypei].n_bolt;
            var Ugol_povorota = Foundation.AllFoundations[FoundationTypei].Ugol_povorota;
            var GlubZalozh = Foundation.AllFoundations[FoundationTypei].Glubina_Zalozhenia;
            //Доп.размеры для специальных фундаментов
            var Vus_2Stup = Foundation.AllFoundations[FoundationTypei].h2;
            var Dl_2Stup = Foundation.AllFoundations[FoundationTypei].a2;
            var Shir_2Stup = Foundation.AllFoundations[FoundationTypei].b2;
            var Vus_3Stup = Foundation.AllFoundations[FoundationTypei].h3;
            var Dl_3Stup = Foundation.AllFoundations[FoundationTypei].a3;
            var Shir_3Stup = Foundation.AllFoundations[FoundationTypei].b3;
            var Vus_4Stup = Foundation.AllFoundations[FoundationTypei].h4;
            var Dl_4Stup = Foundation.AllFoundations[FoundationTypei].a4;
            var Shir_4Stup = Foundation.AllFoundations[FoundationTypei].b4;

            //Построение фигуры фундамента
            //Отметки ступеней
            var hall_12 = Vus_1Stup + Vus_2Stup;
            var hall_123 = hall_12 + Vus_3Stup;
            var hall_1234st = hall_123 + Vus_4Stup;
            var hall_1324 = hall_1234st + Vus_doKo;

            //Полилиния подошвы
            Point3d[] pt_Pod;
            pt_Pod = new Point3d[4]
            {
                    new Point3d(-DlPOD*0.5,ShirPOD*0.5,0),
                    new Point3d(DlPOD*0.5,ShirPOD*0.5,0),
                    new Point3d(DlPOD*0.5,-ShirPOD*0.5,0),
                    new Point3d(-DlPOD*0.5,-ShirPOD*0.5,0)
            };
            var pl_Pod = new VirtualPolyline3d(pt_Pod[0], pt_Pod[1], pt_Pod[2], pt_Pod[3]);
            //Полилиния 1 ступени
            Point3d[] pt_1Stup;
            pt_1Stup = new Point3d[4]
            {
                    new Point3d(-Dl_1Stup*0.5,Shir_1Stup*0.5,Vus_1Stup),
                    new Point3d(Dl_1Stup*0.5,Shir_1Stup*0.5,Vus_1Stup),
                    new Point3d(Dl_1Stup*0.5,-Shir_1Stup*0.5,Vus_1Stup),
                    new Point3d(-Dl_1Stup*0.5,-Shir_1Stup*0.5,Vus_1Stup)
            };
            var pl_1Stup = new VirtualPolyline3d(pt_1Stup[0], pt_1Stup[1], pt_1Stup[2], pt_1Stup[3]);
            //Полилиния у колонной части
            //Полилинии 2 и 3 ступеней
            //Полилиния 2 ступени
            Point3d[] pt_2Stup;
            pt_2Stup = new Point3d[4]
            {
                    new Point3d(-Dl_2Stup*0.5,Shir_2Stup*0.5,hall_12),
                    new Point3d(Dl_2Stup*0.5,Shir_2Stup*0.5,hall_12),
                    new Point3d(Dl_2Stup*0.5,-Shir_2Stup*0.5,hall_12),
                    new Point3d(-Dl_2Stup*0.5,-Shir_2Stup*0.5,hall_12)
            };
            var pl_2Stup = new VirtualPolyline3d(pt_2Stup[0], pt_2Stup[1], pt_2Stup[2], pt_2Stup[3]);
            //Полилиния 3 ступени
            Point3d[] pt_3Stup;
            pt_3Stup = new Point3d[4]
            {
                    new Point3d(-Dl_3Stup*0.5,Shir_3Stup*0.5,hall_123),
                    new Point3d(Dl_3Stup*0.5,Shir_3Stup*0.5,hall_123),
                    new Point3d(Dl_3Stup*0.5,-Shir_3Stup*0.5,hall_123),
                    new Point3d(-Dl_3Stup*0.5,-Shir_3Stup*0.5,hall_123)
            };
            var pl_3Stup = new VirtualPolyline3d(pt_3Stup[0], pt_3Stup[1], pt_3Stup[2], pt_3Stup[3]);
            //Полилинии 2 и 3 ступеней
            //4-я ступень
            Point3d[] pt_4Stup;
            pt_4Stup = new Point3d[4]
            {
                    new Point3d(-Dl_4Stup*0.5,Shir_4Stup*0.5,hall_1234st),
                    new Point3d(Dl_4Stup*0.5,Shir_4Stup*0.5,hall_1234st),
                    new Point3d(Dl_4Stup*0.5,-Shir_4Stup*0.5,hall_1234st),
                    new Point3d(-Dl_4Stup*0.5,-Shir_4Stup*0.5,hall_1234st)
            };
            var pl_4Stup = new VirtualPolyline3d(pt_4Stup[0], pt_4Stup[1], pt_4Stup[2], pt_4Stup[3]);
            //4-я ступень

            Point3d[] pt_Kol;
            pt_Kol = new Point3d[4]
            {
                    new Point3d(-Dl_kol*0.5,Shir_kol*0.5,hall_1324),
                    new Point3d(Dl_kol*0.5,Shir_kol*0.5,hall_1324),
                    new Point3d(Dl_kol*0.5,-Shir_kol*0.5,hall_1324),
                    new Point3d(-Dl_kol*0.5,-Shir_kol*0.5,hall_1324)
            };
            var pl_Kol = new VirtualPolyline3d(pt_Kol[0], pt_Kol[1], pt_Kol[2], pt_Kol[3]);
            if (Vus_4Stup > 0)
            {
                pl_Kol.Move(new Vector3d(130, 0, 0));
            }
            //Полилиния верха колонны
            Point3d[] pt_VerhK;
            pt_VerhK = new Point3d[4]
            {
                    new Point3d(-Dl_Ogolovor*0.5,Shir_Ogolovok*0.5,Vus_Fund),
                    new Point3d(Dl_Ogolovor*0.5,Shir_Ogolovok*0.5,Vus_Fund),
                    new Point3d(Dl_Ogolovor*0.5,-Shir_Ogolovok*0.5,Vus_Fund),
                    new Point3d(-Dl_Ogolovor*0.5,-Shir_Ogolovok*0.5,Vus_Fund)
            };
            var pl_VerhKol = new VirtualPolyline3d(pt_VerhK[0], pt_VerhK[1], pt_VerhK[2], pt_VerhK[3]);
            pl_VerhKol.Move(new Vector3d(excentr, 0, 0));
            //Построение 3Dтела фундамента. Тип.
            LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder()
            {
                Ruled = true
            };
            Solid3d Fund_Type4 = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
            if (Vus_2Stup > 0 && Vus_3Stup > 0 && Vus_4Stup>0)
            {
                Fund_Type4.CreateLoftedSolid(new LoftProfile[]
                        {
                             new LoftProfile(pl_Pod.Gets()),
                             new LoftProfile(pl_1Stup.Gets()),
                             new LoftProfile(pl_2Stup.Gets()),
                             new LoftProfile(pl_3Stup.Gets()),
                             new LoftProfile(pl_4Stup.Gets()),
                             new LoftProfile(pl_Kol.Gets()),
                             new LoftProfile(pl_VerhKol.Gets()),
                        },
                      new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
            }
            else if (Vus_2Stup > 0 && Vus_3Stup > 0)
            {
                Fund_Type4.CreateLoftedSolid(new LoftProfile[]
                    {
                             new LoftProfile(pl_Pod.Gets()),
                             new LoftProfile(pl_1Stup.Gets()),
                             new LoftProfile(pl_2Stup.Gets()),
                             new LoftProfile(pl_3Stup.Gets()),
                             new LoftProfile(pl_Kol.Gets()),
                             new LoftProfile(pl_VerhKol.Gets()),
                    },
                  new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());

            }
            else if (Vus_2Stup > 0 && Vus_3Stup == 0)
            {
                Fund_Type4.CreateLoftedSolid(new LoftProfile[]
                        {
                             new LoftProfile(pl_Pod.Gets()),
                             new LoftProfile(pl_1Stup.Gets()),
                             new LoftProfile(pl_2Stup.Gets()),
                             new LoftProfile(pl_Kol.Gets()),
                             new LoftProfile(pl_VerhKol.Gets()),
                        },
                      new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
            }
            else
            {
                Fund_Type4.CreateLoftedSolid(new LoftProfile[]
                    {
                             new LoftProfile(pl_Pod.Gets()),
                             new LoftProfile(pl_1Stup.Gets()),
                             new LoftProfile(pl_Kol.Gets()),
                             new LoftProfile(pl_VerhKol.Gets()),
                    },
                  new LoftProfile[] { }, null, loftOptionsBuilder.ToLoftOptions());
            }
            Fund_Type4.TransformBy(Matrix3d.Displacement(new Vector3d(-excentr, 0, 0)));
            //Отрисовка болтов
            var bolt = new Circle(new Point3d(0, 0, Vus_Fund), new Vector3d(0, 0, Vus_Fund), diametr_bolta * 0.5);
            Solid3d bolt_solidT4 = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
            DBObjectCollection RegDBT4 = new DBObjectCollection();
            RegDBT4.Add(bolt);
            DBObjectCollection Region_bT4 = new DBObjectCollection();
            Region_bT4 = Region.CreateFromCurves(RegDBT4);
            Region acRegion_bT4 = Region_bT4[0] as Region;
            bolt_solidT4.Extrude(acRegion_bT4, Dl_bolt, 0);
            //Отрисовка болтов
            Vector3d[] Vecbolti;

            if (quantity_bolt == 4)
            {
                Vecbolti = new Vector3d[4]
                {
                    new Vector3d((-between_bolt * 0.5), between_bolt * 0.5, 0),
                    new Vector3d((between_bolt * 0.5), between_bolt * 0.5, 0),
                    new Vector3d((between_bolt * 0.5), -between_bolt * 0.5, 0),
                    new Vector3d((-between_bolt * 0.5), -between_bolt * 0.5, 0)
                };
                foreach (var vecBolt in Vecbolti)
                {
                    var Bolt3Di = new Solid3d();
                    Bolt3Di.CopyFrom(bolt_solidT4);
                    Bolt3Di.TransformBy(Matrix3d.Displacement(vecBolt));
                    Bolt3Di.TransformBy(Matrix3d.Rotation((Ugol_povorota * Math.PI / 180), new Vector3d(0, 0, 1), new Point3d(0, 0, 0)));
                    Fund_Type4.BooleanOperation(BooleanOperationType.BoolUnite, Bolt3Di);
                }
            }
            else if (quantity_bolt == 2)
            {
                Vecbolti = new Vector3d[2]
                {
                    new Vector3d((-between_bolt * 0.5), between_bolt * 0.5, 0),
                    new Vector3d((between_bolt * 0.5), -between_bolt * 0.5, 0)
                };

                foreach (var vecBolt in Vecbolti)
                {
                    var Bolt3Di = new Solid3d();
                    Bolt3Di.CopyFrom(bolt_solidT4);
                    Bolt3Di.TransformBy(Matrix3d.Displacement(vecBolt));
                    Fund_Type4.BooleanOperation(BooleanOperationType.BoolUnite, Bolt3Di);
                }
            }
            else
            {
                var Bolt3Dj = bolt_solidT4;
                Bolt3Dj.TransformBy(Matrix3d.Displacement(new Vector3d(0, 0, 0)));
                Fund_Type4.BooleanOperation(BooleanOperationType.BoolUnite, Bolt3Dj);
            }

            //System.Windows.MessageBox.Show(FoundationTypei.ToString());
            //Навесная плита НПн-1 и НПн-2
            //НПн-1
            if (FoundationTypei==FoundationType.FS1n_A || FoundationTypei == FoundationType.FSP1n_A )
            {
                Point3d[] pt_NP1;
                pt_NP1 = new Point3d[8] 
                {
                    new Point3d(-10000, 0, hall_1234st),
                    new Point3d(-10000, 700, hall_1234st),
                    new Point3d(-10000, 1700, hall_1234st),
                    new Point3d(-10000, 1685, (hall_1234st-233)),
                    new Point3d(-10000, 700, (hall_1234st-380)),
                    new Point3d(-10000, 550, (hall_1234st-280)),
                    new Point3d(-10000, 50, (hall_1234st-280)),
                    new Point3d(-10000, 0, (hall_1234st-140)),
                };
                VirtualPolyline3d8pt pl_NP1 = new VirtualPolyline3d8pt(pt_NP1[0], pt_NP1[1], pt_NP1[2], pt_NP1[3], pt_NP1[4], pt_NP1[5], pt_NP1[6], pt_NP1[7]);
                Autodesk.AutoCAD.DatabaseServices.Solid3d Solid_NP1x = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
                DBObjectCollection RegDB_NP1x = new DBObjectCollection();
                RegDB_NP1x.Add(pl_NP1.Gets());
                DBObjectCollection Region_NP1x = new DBObjectCollection();
                Region_NP1x = Region.CreateFromCurves(RegDB_NP1x);
                Region acRegion_NP1x = Region_NP1x[0] as Region;
                Solid_NP1x.Extrude(acRegion_NP1x, -20000, 0);

                Point3d[] pt_NP1y;
                pt_NP1y = new Point3d[12] 
                {
                    new Point3d(1500 - excentr, -10000, hall_1234st),
                    new Point3d(1485 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(990 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(965 - excentr, -10000, (hall_1234st-380)),
                    new Point3d(530 - excentr, -10000, (hall_1234st-380)),
                    new Point3d(510 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-510 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-530 - excentr, -10000, (hall_1234st-380)),
                    new Point3d(-965 - excentr, -10000, (hall_1234st-380)),
                    new Point3d(-990 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-1485 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-1500 - excentr, -10000, hall_1234st),
                };
                VirtualPolyline3d12pt pl_NP1y =new VirtualPolyline3d12pt(pt_NP1y[0], pt_NP1y[1], pt_NP1y[2], pt_NP1y[3], pt_NP1y[4], pt_NP1y[5], pt_NP1y[6], pt_NP1y[7], pt_NP1y[8], pt_NP1y[9], pt_NP1y[10], pt_NP1y[11]);
                
                Autodesk.AutoCAD.DatabaseServices.Solid3d Solid_NP1y = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
                DBObjectCollection RegDB_NP1y = new DBObjectCollection();
                RegDB_NP1y.Add(pl_NP1y.Gets());
                DBObjectCollection Region_NP1y = new DBObjectCollection();
                Region_NP1y = Region.CreateFromCurves(RegDB_NP1y);
                Region acRegion_NP1y = Region_NP1y[0] as Region;
                Solid_NP1y.Extrude(acRegion_NP1y, 20000, 0);
                Solid_NP1x.BooleanOperation(BooleanOperationType.BoolIntersect, Solid_NP1y);
                Solid_NP1x.TransformBy(Matrix3d.Rotation(-10.587*Math.PI/180, new Vector3d(1, 0, 0), new Point3d(0 - excentr,0, hall_1234st)));
                Solid_NP1x.TransformBy(Matrix3d.Displacement(new Vector3d(0, 428.94, 0)));
                var Solid_NP1x2 = new Solid3d();
                Solid_NP1x2.CopyFrom(Solid_NP1x);
                Solid_NP1x2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0 - excentr, 0, 0)));
                Solid_NP1x.BooleanOperation(BooleanOperationType.BoolUnite, Solid_NP1x2);
                Fund_Type4.BooleanOperation(BooleanOperationType.BoolUnite, Solid_NP1x);
            }
            //НПн-1
            //НПн-2
            if (FoundationTypei == FoundationType.FS2n_A || FoundationTypei == FoundationType.FSP2n_A)
            {
                Point3d[] pt_NP2;
                pt_NP2 = new Point3d[8]
                {
                    new Point3d(-10000, 0, hall_1234st),
                    new Point3d(-10000, 700, hall_1234st),
                    new Point3d(-10000, 2200, hall_1234st),
                    new Point3d(-10000, 2185, (hall_1234st-233)),
                    new Point3d(-10000, 700, (hall_1234st-410)),
                    new Point3d(-10000, 550, (hall_1234st-280)),
                    new Point3d(-10000, 50, (hall_1234st-280)),
                    new Point3d(-10000, 0, (hall_1234st-140)),
                };
                VirtualPolyline3d8pt pl_NP2 = new VirtualPolyline3d8pt(pt_NP2[0], pt_NP2[1], pt_NP2[2], pt_NP2[3], pt_NP2[4], pt_NP2[5], pt_NP2[6], pt_NP2[7]);
                Autodesk.AutoCAD.DatabaseServices.Solid3d Solid_NP2x = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
                DBObjectCollection RegDB_NP2x = new DBObjectCollection();
                RegDB_NP2x.Add(pl_NP2.Gets());
                DBObjectCollection Region_NP2x = new DBObjectCollection();
                Region_NP2x = Region.CreateFromCurves(RegDB_NP2x);
                Region acRegion_NP2x = Region_NP2x[0] as Region;
                Solid_NP2x.Extrude(acRegion_NP2x, -20000, 0);

                Point3d[] pt_NP2y;
                pt_NP2y = new Point3d[12]
                {
                    new Point3d(1500 - excentr, -10000, hall_1234st),
                    new Point3d(1485 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(990 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(965 - excentr, -10000, (hall_1234st-410)),
                    new Point3d(530 - excentr, -10000, (hall_1234st-410)),
                    new Point3d(510 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-510 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-530 - excentr, -10000, (hall_1234st-410)),
                    new Point3d(-965 - excentr, -10000, (hall_1234st-410)),
                    new Point3d(-990 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-1485 - excentr, -10000, (hall_1234st-120)),
                    new Point3d(-1500 - excentr, -10000, hall_1234st),
                };
                VirtualPolyline3d12pt pl_NP2y = new VirtualPolyline3d12pt(pt_NP2y[0], pt_NP2y[1], pt_NP2y[2], pt_NP2y[3], pt_NP2y[4], pt_NP2y[5], pt_NP2y[6], pt_NP2y[7], pt_NP2y[8], pt_NP2y[9], pt_NP2y[10], pt_NP2y[11]);

                Autodesk.AutoCAD.DatabaseServices.Solid3d Solid_NP2y = new Autodesk.AutoCAD.DatabaseServices.Solid3d();
                DBObjectCollection RegDB_NP2y = new DBObjectCollection();
                RegDB_NP2y.Add(pl_NP2y.Gets());
                DBObjectCollection Region_NP2y = new DBObjectCollection();
                Region_NP2y = Region.CreateFromCurves(RegDB_NP2y);
                Region acRegion_NP2y = Region_NP2y[0] as Region;
                Solid_NP2y.Extrude(acRegion_NP2y, 20000, 0);
                Solid_NP2x.BooleanOperation(BooleanOperationType.BoolIntersect, Solid_NP2y);
                Solid_NP2x.TransformBy(Matrix3d.Rotation(-6.79713 * Math.PI / 180, new Vector3d(1, 0, 0), new Point3d(0 - excentr, 0, hall_1234st)));
                Solid_NP2x.TransformBy(Matrix3d.Displacement(new Vector3d(0, (428.94-13.4772), 0)));
                var Solid_NP2x2 = new Solid3d();
                Solid_NP2x2.CopyFrom(Solid_NP2x);
                Solid_NP2x2.TransformBy(Matrix3d.Rotation(Math.PI, new Vector3d(0, 0, 1), new Point3d(0 - excentr, 0, 0)));
                Solid_NP2x.BooleanOperation(BooleanOperationType.BoolUnite, Solid_NP2x2);
                Fund_Type4.BooleanOperation(BooleanOperationType.BoolUnite, Solid_NP2x);
            }
            //НПн-2


            Fund_Type4.TransformBy(Matrix3d.Displacement(vector_ponizh));
            return Fund_Type4;
            
            
        }



    }

}
