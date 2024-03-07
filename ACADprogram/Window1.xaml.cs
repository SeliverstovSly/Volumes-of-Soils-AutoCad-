using ACADprogram;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using static ACADprogram.Crossbar;

namespace ACADprogram
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MainWindowContext Context => (MainWindowContext)this.DataContext;
        public Window1(System.Windows.Threading.Dispatcher autocadDispatcher)
        {
            InitializeComponent();
            Context.AutocadDispatcher = autocadDispatcher;
        }
    }

    public partial class MainWindowContext : Notified
    {
        public string NumberOpory { get; set; } = "157";
        public int LpoperekVL { get; set; } = 10000;
        public int LvdolVL { get; set; } = 10000;
        public int GlubinaKotlovana { get; set; } = 3000;
        public double OtkosKotlovana { get; set; } = 1;
        public double OtkosSrezki { get; set; } = 1;
        public double OtkosNasypi { get; set; } = 1.5;
        public int LdopNasypi { get; set; } = 0;
        public int HReliefPoperekVLeft { get; set; } = 500;
        public int HReliefPoperekVRight { get; set; } = -500;
        public int HReliefVdolVLDown { get; set; } = 0;
        public int HReliefVdolVLUp { get; set; } = 0;
        public int IGE1 { get; set; } = 200;
        public int IGE2 { get; set; } = 800;
        public int IGE3 { get; set; } = 1200;
        public int IGE4 { get; set; } = 1500;
        public int IGE5 { get; set; } = 1900;
        public int IGE6 { get; set; } = 0;
        public int OtmetkaPonizh { get; set; } = 0;
        public int HShebenPodgotovki { get; set; }
        public int HBetonPodgotovki { get; set; }
        public int SvesaBetonPod { get; set; }
        public int SvesaShebenPod { get; set; }
        public int DopRazmerNizhaKotlovana { get; set; }
        public int crossbarlocation { get; set; } = 500;
        public int Beamlocation { get; set; } = 500;

        public Angles<int> CrossbarQuantitiesA { get; } = new Angles<int>();
        public Angles<int> CrossbarQuantitiesB { get; } = new Angles<int>();


        public int crossbarquantityType1 { get; set; }
        public int crossbarquantityType2 { get; set; }
        public int crossbarquantityType3 { get; set; }
        public int crossbarquantityType4 { get; set; }
        public int crossbarquantityType1A { get; set; }
        public int crossbarquantityType2A { get; set; }
        public int crossbarquantityType3A { get; set; }
        public int crossbarquantityType4A { get; set; }

        //public bool dddd { get; set; } = true;
        public Angles<FoundationType> SelectedTypes { get; } = new Angles<FoundationType>();
        public FoundationType SelectedType1 { get; set; } = FoundationType.F3n_A;
        public FoundationType SelectedType2 { get; set; } = FoundationType.F3n_A;
        public FoundationType SelectedType3 { get; set; } = FoundationType.F3n_A;
        public FoundationType SelectedType4 { get; set; } = FoundationType.F3n_A;
        public int FoundationQuantity1 { get; set; } = 1;
        public int FoundationQuantity2 { get; set; } = 1;
        public int FoundationQuantity3 { get; set; } = 1;
        public int FoundationQuantity4 { get; set; } = 1;
        public OporaType SelectedType { get; set; } = OporaType.U500n_1_12;
        public CrossbarType SelectedTypeCr1 { get; set; } = CrossbarType.R1n;
        public CrossbarType SelectedTypeCr2 { get; set; } = CrossbarType.R1n;
        public CrossbarType SelectedTypeCr3 { get; set; } = CrossbarType.R1n;
        public CrossbarType SelectedTypeCr4 { get; set; } = CrossbarType.R1n;

        public ObservableCollection<B5n> MainBeams { get; } = new ObservableCollection<B5n>() { new B4n_250(), new B4n_350(), new B5n() };
        public ObservableCollection<B5n> SecondaryBeams { get; } = new ObservableCollection<B5n>() { new B1n(), new B2n(), new B3n() };
        public B5n SelectedBeam1 { get; set; }
        public B5n SelectedBeam2 { get; set; }
        public B5n SelectedBeam3 { get; set; }
        public B5n SelectedBeam4 { get; set; }
        public B5n SelectedMainBeam1 {  get; set; }
        public B5n SelectedMainBeam2 { get; set; }
        public B5n SelectedMainBeam3 { get; set; }
        public B5n SelectedMainBeam4 { get; set; }

        public ObservableCollection<FoundationBlock> FBlock { get; } = new ObservableCollection<FoundationBlock>() { new OneFoundation(), new TwoFoundation(), new FourFoundation() };
        public FoundationBlock SelectedBlock1 { get; set; }
        public FoundationBlock SelectedBlock2 { get; set; }
        public FoundationBlock SelectedBlock3 { get; set; }
        public FoundationBlock SelectedBlock4 { get; set; }

        public ObservableCollection<VolumeInfo> Volumes { get; set; } = new ObservableCollection<VolumeInfo>();
        public ObservableCollection<VolumeInfo> VolumesSR { get; set; } = new ObservableCollection<VolumeInfo>();
        public double VolumesN { get; set; }=new double();
        public double SQN { get; set; } = new double();
        public double VolumeShebenPodg { get; set; } = new double();
        public double VolumeBetonPodg { get; set; } = new double();


        public string ExceptionMessage { get; set; }
        public Dispatcher AutocadDispatcher;
        public MainWindowContext()
        {
            startCommand = new RelayCommand(obj =>
            {
                AutocadDispatcher.Invoke(() =>
                {
                    ExceptionMessage = "StartCommand";
                    try
                    {
                        //if(dddd)
                        //    SelectedType4 = SelectedType3 = SelectedType2 = SelectedType1;
                        ACAD.Program(this);
                    }
                    catch (Exception e)
                    {
                        ExceptionMessage = e.MessageExpress();
                    }
                });
            });
        }
        private RelayCommand startCommand;
        public RelayCommand StartCommand => startCommand;
    }

    public class VolumeInfo
    {
        public VolumeInfo()
        {
            Description = "TestDescription";
            Value = 100.99;
        }
        public VolumeInfo(string description, double value)
        {
            Description = description;
            Value = value;
        }
        public string Description { get; set; }
        public double Value { get; set; }
    }
}

public class Angles<T>
{
    public T Angle1
    {
        get => collection[0];
        set => collection[0] = value;
    }
    public T Angle2
    {
        get => collection[1];
        set => collection[1] = value;
    }
    public T Angle3
    {
        get => collection[2];
        set => collection[2] = value;
    }
    public T Angle4
    {
        get => collection[3];
        set => collection[3] = value;
    }

    public readonly T[] collection = new T[4];
}
