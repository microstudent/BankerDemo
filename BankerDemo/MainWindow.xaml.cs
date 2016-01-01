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
        DataTableManager maxManager,needManager,allocationManager;

        Matrix max, need, allocation;

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

            maxManager = new DataTableManager(dg_max);
            needManager = new DataTableManager(dg_need);
            allocationManager = new DataTableManager(dg_allocation);
            max = new Matrix(1, 0);
            need = new Matrix(1, 0);
            allocation = new Matrix(1, 0);
            max.dataChanged += maxManager.updateUI;
            need.dataChanged += maxManager.updateUI;
            allocation.dataChanged += maxManager.updateUI;
            DataTable dt = max.getDataTable();

            maxManager.updateUI(max, null);
            needManager.updateUI(need, null);
            allocationManager.updateUI(allocation, null);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            max.newRow();
            max.newColumn();
        }
    }
}
