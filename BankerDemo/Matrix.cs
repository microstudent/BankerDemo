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
        int[,] data;        //存放矩阵的二维数组
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
            data = new int[rowCount, columnCount];
            onDataChanged(null);
        }

        public Matrix(int[,] members)
        {
            row = members.GetUpperBound(0) + 1;
            column = members.GetUpperBound(1) + 1;
            data = new int[row, column];
            Array.Copy(members, data, row * column);
        }

        public int rowNum { get { return row; } }
        public int columnNum {
            set
            {
                int [,] t = new int[row, value];
                Array.Copy(data, t, row * (value > column ? column : value));
                data = t;
                column = value;
                onDataChanged(null);
            }
            get { return column; } }

        public int this[int r, int c]
        {
            get
            {
                return data[r, c];
            }
            set
            {
                data[r, c] = value;
                DataChangedEventArgs e = new DataChangedEventArgs(r, c, value, name);
                onDataChanged(e);
            }
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
                    dr[j + 1] = Convert.ToString(data[i,j]);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
