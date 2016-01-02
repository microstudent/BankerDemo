using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BankerDemo
{
    class ResourceManager
    {
        public DataGrid dg;
        public DataTable datatable;
        public ResourceManager(DataGrid datagrid)
        {
            this.dg = datagrid;
            init();
        }

        private void init()
        {
            datatable = new DataTable();
            dg.ItemsSource = datatable.DefaultView;
        }

        public void updateUI(Object sender, Resource.DataChangedEventArgs e)
        {
            Resource res = sender as Resource;
            if (dg != null)
            {
                if (e == null)
                {
                    datatable = res.getDataTable();
                    dg.ItemsSource = null;
                    dg.ItemsSource = datatable.DefaultView;
                }
                else
                {
                    DataRow drOperate = datatable.Rows[e.row];
                    drOperate[e.column] = e.data;
                }
            }
        }

        public List<int> getVector()
        {
            DataTable t = ((DataView)dg.ItemsSource).Table;
            List<Int32> vector = new List<int>();
            for (int i = 0; i < t.Columns.Count; i++)
            {
                vector.Add(Convert.ToInt32(t.Rows[0][i]));
            }
            return vector;
        }
    }
}
