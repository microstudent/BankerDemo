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
        Resource newProgMax,request;
        Banker banker;
        ResourceManager avaManager, resCountManager, newProgMaxManager,requestManager;

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
            DataTableManager workAllocationManager = new DataTableManager(dg_workPlusAllocation);
            Matrix max = new Matrix(0, 0);
            Matrix need = new Matrix(0, 0);
            Matrix allocation = new Matrix(0, 0);
            MatrixWithFinish workAllocation = new MatrixWithFinish();
            max.dataChanged += maxManager.updateUI;
            need.dataChanged += needManager.updateUI;
            allocation.dataChanged += allocationManager.updateUI;
            workAllocation.dataChanged += workAllocationManager.updateUI;
            maxManager.updateUI(max, null);
            needManager.updateUI(need, null);
            allocationManager.updateUI(allocation, null);

            Resource available = new Resource(0);
            Resource resCount = new Resource(0);
            newProgMax = new Resource(0);
            request = new Resource(0);
            avaManager = new ResourceManager(dg_available);
            resCountManager = new ResourceManager(dg_resCount);
            newProgMaxManager = new ResourceManager(dg_newProgMax);
            requestManager = new ResourceManager(dg_request);
            available.dataChanged += avaManager.updateUI;
            resCount.dataChanged += resCountManager.updateUI;
            newProgMax.dataChanged += newProgMaxManager.updateUI;
            request.dataChanged += requestManager.updateUI;

            Logout log = new Logout(tb_log);
            Logout.LogoutEvent += log.logoutToTextBox;

            banker = new Banker(max, need, allocation, available, resCount,workAllocation);
        }

        private void newProgress_Click(object sender, RoutedEventArgs e)
        {
            if(tb_newProgName.Text == "")
            {
                LogoutEventArgs a = new LogoutEventArgs("error: 进程名为空");
                Logout.NewMsg(a);
                return;
            }
            List<int> progMax = newProgMaxManager.getVector();
            if (banker.newProgress(tb_newProgName.Text, progMax)) 
            {
                cb_progName.ItemsSource = null;
                cb_progName.ItemsSource = banker.getProgressNameList();
                tb_progressNum.Text = ((Convert.ToInt32(tb_progressNum.Text) + 1).ToString());
            }
        }

        private void bt_rsRequest_Click(object sender, RoutedEventArgs e)
        {
            if(banker.applyForRes(cb_progName.Text, requestManager.getVector()))
            {
                LogoutEventArgs a = new LogoutEventArgs("success: 申请资源成功.");
                Logout.NewMsg(a);
            }
            else
            {
                LogoutEventArgs a = new LogoutEventArgs("申请资源失败.");
                Logout.NewMsg(a);
            }
        }

        private void bt_addRes_Click(object sender, RoutedEventArgs e)
        {
            if (banker.getResNum() >= 26)
            {
                LogoutEventArgs a = new LogoutEventArgs("error: 最多支持26个种类的资源");
                Logout.NewMsg(a);
                return;
            }
            newProgMax.newColumn();
            request.newColumn();
            banker.newRes();
            tb_resCount.Text = ((Convert.ToInt32(tb_resCount.Text) + 1).ToString());
        }

        private void bt_updateAvaliable_Click(object sender, RoutedEventArgs e)
        {
            List<int> newRes = resCountManager.getVector();
            if (banker.modifyRes(newRes))
            {
                LogoutEventArgs a = new LogoutEventArgs("success: 资源已更新.");
                Logout.NewMsg(a);
            }
            else
            {
                LogoutEventArgs a = new LogoutEventArgs("error: 资源更新失败.");
                Logout.NewMsg(a);
            }
        }

    }
}
