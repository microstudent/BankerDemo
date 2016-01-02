using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankerDemo
{
    class BankerHelper
    {
        public static bool MatrixAdd(List<List<Int32>> a, List<List<Int32>> b, ref List<List<Int32>> c)
        {
            if (a.Count == 0 || b.Count == 0 || c.Count == 0)
                return false;
            if (a.Count != b.Count || a[0].Count != b[0].Count || a.Count != c.Count || a.Count != c.Count)
                return false;
            for(int i = 0; i < a.Count; i++)
            {
                for(int j = 0; j < a[i].Count; j++)
                {
                    c[i][j] = a[i][j] + b[i][j];
                }
            }
            return true;
        }

        public static bool MatrixRed(List<List<Int32>> a, List<List<Int32>> b, ref List<List<Int32>> c)
        {
            if (a.Count == 0 || b.Count == 0 || c.Count == 0)
                return false;
            if (a.Count != b.Count || a[0].Count != b[0].Count || a.Count != c.Count || a.Count != c.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < a[i].Count; j++)
                {
                    c[i][j] = a[i][j] - b[i][j];
                }
            }
            return true;
        }
        public static bool VectorAdd(List<Int32> a,List<Int32> b,ref List<Int32> c)
        {
            if (a.Count != b.Count || a.Count != c.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
            {
                c[i] = a[i] + b[i];
            }
            return true;
        }

        public static bool VectorRed(List<Int32> a, List<Int32> b, ref List<Int32> c)
        {
            if (a.Count != b.Count || a.Count != c.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
            {
                c[i] = a[i] - b[i];
            }
            return true;
        }
        //判断a向量中是否任意元素都小于b向量
        public static bool VectorLess(List<int> a,List<int> b)
        {
            if(a.Count!=b.Count)
            {
                return false;
            }
            for (int i = 0;i < a.Count; i++)
            {
                if (a[i] > b[i]) return false;
            }
            return true;
        }
        public static bool isAllFinish(List<bool> a)
        {
            foreach(bool b in a)
            {
                if (b == false)
                    return false;
            }
            return true;
        }
    }
}
