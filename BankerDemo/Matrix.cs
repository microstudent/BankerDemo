using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;

namespace BankerDemo
{
    class Matrix
    {
        public delegate void DataChangedEventHandler(Object sender, DataChangedEventArgs e);
        public event DataChangedEventHandler dataChanged;

        int row, column;    //行和列
        List<List<Int32>> data;        //存放矩阵的二维数组
        string name;        //矩阵的名字

        public class DataChangedEventArgs : EventArgs
        {
            public readonly int row, column, data;
            public readonly string name;
            public DataChangedEventArgs(int Row, int Column, int Data, string Name)
            {
                this.row = Row;
                this.column = Column;
                this.data = Data;
                this.name = Name;
            }
        }

        protected virtual void onDataChanged(DataChangedEventArgs e)
        {
            if (dataChanged != null)
            {
                dataChanged(this, e);
            }
        }

        public Matrix(int rowCount, int columnCount)
        {
            row = rowCount;
            column = columnCount;
            data = new List<List<Int32>>();
            data.Add(new List<int>());
            onDataChanged(null);
        }


        public int rowNum
        {
            get { return row; }
        }
        public int columnNum {get { return column; } }

        public int this[int r, int c]
        {
            get
            {
                return data[r][c];
            }
            set
            {
                data[r][c] = value;
                DataChangedEventArgs e = new DataChangedEventArgs(r, c, value, name);
                onDataChanged(e);
            }
        }

        public void newColumn()
        {
            foreach (List<Int32> a in data)
            {
                a.Add(0);
            }
            column++;
            onDataChanged(null);
        }

        public void newRow()
        {
            List<Int32> t = new List<int>(data[0].ToArray());
            for(int i = 0;i<t.Count;i++)
            {
                t[i] = 0;
            }
            data.Add(t);
            row++;
            onDataChanged(null);
        }

        public DataTable getDataTable()
        {
            DataTable dt = new DataTable(name);
            dt.Columns.Add("进程",System.Type.GetType("System.String"));
            for (int i = 0; i < column; i++)         //添加列
            {
                dt.Columns.Add(((char)(i + 65)).ToString(),System.Type.GetType("System.String"));
            }
            for(int i = 0;i<row;i++)
            {
                DataRow dr = dt.NewRow();
                //先加进程名
                dr[0] = "P" + i;
                for (int j = 0;j<column;j++)
                {
                    //添加数据
                    dr[j + 1] = Convert.ToString(data[i][j]);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
