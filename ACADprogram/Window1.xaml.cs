using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

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
        public int HShebenPodgotovki { get; set; } = 500;
        public int HBetonPodgotovki { get; set; } = 0;
              
        public FoundationType SelectedType1 { get; set; } = FoundationType.F2n_2;
        public FoundationType SelectedType2 { get; set; } = FoundationType.F3n_2;
        public FoundationType SelectedType3 { get; set; } = FoundationType.F1n_2;
        public FoundationType SelectedType4 { get; set; } = FoundationType.FS1n_2;

        public ObservableCollection<VolumeInfo> Volumes { get; set; } = new ObservableCollection<VolumeInfo>();
        public ObservableCollection<VolumeInfo> VolumesSR { get; set; } = new ObservableCollection<VolumeInfo>();
        public double VolumesN { get; set; }=new double();
        public double SQN { get; set; } = new double();

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
