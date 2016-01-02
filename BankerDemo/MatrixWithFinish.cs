using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankerDemo
{
    class MatrixWithFinish : Matrix
    {
        List<bool> disPlayFinish;
        public MatrixWithFinish() : base()
        {
            disPlayFinish = new List<bool>();
        }

        public override bool newRow(string header)
        {
            disPlayFinish.Add(true);//已插入的进程一定是已经finish的了
            return base.newRow(header);
        }

        public override DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("进程", System.Type.GetType("System.String"));
            for (int i = 0; i < column; i++)         //添加列
            {
                dt.Columns.Add(((char)(i + 65)).ToString(), System.Type.GetType("System.String"));
            }
            dt.Columns.Add(new DataColumn("Finish", System.Type.GetType("System.Boolean")));

            for (int i = 0; i < row; i++)
            {
                DataRow dr = dt.NewRow();
                //先加进程名
                dr[0] = progressName[i];
                for (int j = 0; j < column; j++)
                {
                    //添加数据
                    dr[j + 1] = Convert.ToString(data[i][j]);
                }
                dr[column + 1] = disPlayFinish[i];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public void clearAll()
        {
            data.Clear();
            progressName.Clear();
            disPlayFinish.Clear();
            row = 0;
            onDataChanged(null);
        }
    }
}
