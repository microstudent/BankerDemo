using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankerDemo
{
    class Banker
    {
        Matrix max, need, allocation;
        Resource available, resCount;
        public Banker(Matrix max, Matrix need, Matrix allocation,Resource available,Resource resCount)
        {
            this.max = max;
            this.need = need;
            this.allocation = allocation;
            this.available = available;
            this.resCount = resCount;
        }
        public void newProgress(string name)
        {

        }
        public bool alartRes(List<Int32> newRes)
        {
            if (newRes.Count != resCount.columnNum)
            {
                return false;
            }
            resCount.alter(newRes);
            return true;
        }

        public void newRes()
        {
            resCount.newColumn();
            available.newColumn();
            max.newColumn();
            need.newColumn();
            allocation.newColumn();
        }
        public bool applyForRes(string name,List<Int32> max)
        {
            return false;
        }
    }
}
