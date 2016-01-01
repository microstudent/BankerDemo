using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data;

namespace BankerDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Banker banker;

        private void tb_progressNum_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        public MainWindow()
        {
            InitializeComponent();
            init();    
        }

        private void init()
        {

            DataTableManager maxManager = new DataTableManager(dg_max);
            DataTableManager needManager = new DataTableManager(dg_need);
            DataTableManager allocationManager = new DataTableManager(dg_allocation);
            Matrix max = new Matrix(1, 0);
            Matrix need = new Matrix(1, 0);
            Matrix allocation = new Matrix(1, 0);
            max.dataChanged += maxManager.updateUI;
            need.dataChanged += needManager.updateUI;
            allocation.dataChanged += allocationManager.updateUI;

            maxManager.updateUI(max, null);
            needManager.updateUI(need, null);
            allocationManager.updateUI(allocation, null);

            Resource available = new Resource(0);
            Resource resCount = new Resource(0);
            ResourceManager avaManager = new ResourceManager(dg_available);
            ResourceManager resCountManager = new ResourceManager(dg_resCount);
            available.dataChanged += avaManager.updateUI;
            resCount.dataChanged += resCountManager.updateUI;
            avaManager.updateUI(available, null);
            resCountManager.updateUI(resCount, null);

           
            banker = new Banker(max, need, allocation, available, resCount);
        }

        private void newProgress_Click(object sender, RoutedEventArgs e)
        {
            banker.newProgress("a");
        }

        private void bt_addRes_Click(object sender, RoutedEventArgs e)
        {
            banker.newRes();
            tb_resCount.Text = ((Convert.ToInt32(tb_resCount.Text) + 1).ToString());
        }
    }
}
