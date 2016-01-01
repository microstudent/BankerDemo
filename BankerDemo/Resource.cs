using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankerDemo
{
    class Resource
    {
        public delegate void DataChangedEventHandler(Object sender, DataChangedEventArgs e);
        public event DataChangedEventHandler dataChanged;

        int resNum;    
        List<Int32> data;        

        public class DataChangedEventArgs : EventArgs
        {
            public readonly int row, column, data;
            public readonly string name;
            public DataChangedEventArgs(int Column, int Data)
            {
                this.column = Column;
                this.data = Data;
            }
        }

        protected virtual void onDataChanged(DataChangedEventArgs e)
        {
            if (dataChanged != null)
            {
                dataChanged(this, e);
            }
        }

        public Resource(int columnCount)
        {
            resNum = columnCount;
            data = new List<Int32>();
            onDataChanged(null);
        }

        public int columnNum { get { return resNum; } }

        public int this[int c]
        {
            get
            {
                return data[c];
            }
            set
            {
                data[c] = value;
                DataChangedEventArgs e = new DataChangedEventArgs(c, value);
                onDataChanged(e);
            }
        }

        public void alter(List<int> a)
        {
            if(a.Count!=data.Count)
            {
                return;
            }
            data = new List<int>(a.ToArray());
            onDataChanged(null);
        }

        public void newColumn()
        {
            data.Add(0);
            resNum++;
            onDataChanged(null);
        }

        public DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < resNum; i++)         //添加列
            {
                dt.Columns.Add(((char)(i + 65)).ToString(), System.Type.GetType("System.Int32"));
            }
            DataRow dr = dt.NewRow();
            for (int j = 0; j < resNum; j++)
            {
                //添加数据
                dr[j] = Convert.ToString(data[j]);
            }
            dt.Rows.Add(dr);
            return dt;
        }
    }
}
