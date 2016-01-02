using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BankerDemo
{
    class DataTableManager
    {
        public DataGrid dg;
        public DataTable datatable;
        public DataTableManager(DataGrid datagrid)
        {
            this.dg = datagrid;
            init();
        }

        private void init()
        {
            datatable = new DataTable();
            dg.ItemsSource = datatable.DefaultView;
        }

        public virtual void updateUI(Object sender,Matrix.DataChangedEventArgs e)
        {
            Matrix mx = sender as Matrix;
            if(dg!=null)
            {
                if(e==null)
                {
                    datatable = mx.getDataTable();
                    dg.ItemsSource = null;
                    dg.ItemsSource = datatable.DefaultView;
                }
                else
                {
                    DataRow drOperate = datatable.Rows[e.row];
                    drOperate[e.column+1] = e.data;
                }
            }
        }
    }
}
