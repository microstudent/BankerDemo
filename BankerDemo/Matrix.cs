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

        protected int row, column;    //行和列
        protected List<List<Int32>> data;        //存放矩阵的二维数组
        protected List<String> progressName;        //进程的名字

        public class DataChangedEventArgs : EventArgs
        {
            public readonly int row, column, data;
            public DataChangedEventArgs(int Row, int Column, int Data)
            {
                this.row = Row;
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

        public Matrix()
        {
            data = new List<List<Int32>>();
            progressName = new List<string>();
            onDataChanged(null);
        }

        public Matrix(int rowCount, int columnCount)
        {
            row = rowCount;
            column = columnCount;
            data = new List<List<Int32>>();
            progressName = new List<string>();
            onDataChanged(null);
        }

        public List<List<Int32>> getData()
        {
            if (data != null)
            {
                List<List<int>> t = new List<List<int>>(data.ToArray());
                return t;
            }
            else return null;
        }
        //获取某进程的资源向量
        public List<int> getVector(string name)
        {
            int index = progressName.IndexOf(name);
            List<Int32> vector = new List<int>();
            for (int i = 0; i < column; i++)
            {
                vector.Add(Convert.ToInt32(data[index][i]));
            }
            return vector;
        }

        //根据位置获取某进程的资源向量
        public List<int> getVector(int index)
        {
            List<Int32> vector = new List<int>();
            for (int i = 0; i < column; i++)
            {
                vector.Add(Convert.ToInt32(data[index][i]));
            }
            return vector;
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
                DataChangedEventArgs e = new DataChangedEventArgs(r, c, value);
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

        public virtual bool newRow(string header)
        {
            if(progressName.Contains(header))
            {
                LogoutEventArgs e = new LogoutEventArgs("error: 已存在同名进程，创建失败.");
                Logout.NewMsg(e);
                return false;
            }
            else
            {
                progressName.Add(header);

                List<Int32> t = new List<int>();
                for (int i = 0; i < columnNum; i++)
                {
                    t.Add(0);
                }
                data.Add(t);
                row++;
                onDataChanged(null);
                return true;
            }
        }

        public List<string> getProgressNameList()
        {
            return progressName;
        }

        //修改单条进程所需最大资源数
        public bool modify(string name,List<int> a)
        {
            if (a.Count != columnNum)
            {
                return false;
            }
            int index = progressName.IndexOf(name);
            for(int i = 0; i < columnNum; i++)
            {
                data[index][i] = a[i];
            }
            onDataChanged(null);
            return true;
        }
        //修改整个max表
        public bool modify(List<List<Int32>> a)
        {
            if (a==null|| a.Count == 0)
                return false;
            if (a.Count != data.Count || a[0].Count != data[0].Count)
                return false;
            List<List<Int32>> t = new List<List<Int32>>(a.ToArray());
            data = t;
            onDataChanged(null);
            return true;
        }

        public virtual DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("进程",System.Type.GetType("System.String"));
            for (int i = 0; i < column; i++)         //添加列
            {
                dt.Columns.Add(((char)(i + 65)).ToString(),System.Type.GetType("System.String"));
            }
            for(int i = 0;i<row;i++)
            {
                DataRow dr = dt.NewRow();
                //先加进程名
                dr[0] = progressName[i];
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
