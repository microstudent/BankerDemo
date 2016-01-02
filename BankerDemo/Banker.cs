using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BankerDemo
{
    class Banker
    {
        Matrix max, need, allocation;
        MatrixWithFinish workAllocation;
        Resource available, resCount;
        int resNum = 0, progNum = 0;
        public Banker(Matrix max, Matrix need, Matrix allocation,Resource available,Resource resCount,MatrixWithFinish workAllocation)
        {
            this.max = max;
            this.need = need;
            this.allocation = allocation;
            this.available = available;
            this.resCount = resCount;
            this.workAllocation = workAllocation;
        }

        public int getResNum()
        {
            return resNum;
        }
        public bool newProgress(string name,List<int> vector)
        {
            //判断是否超过resCount向量,若超过，返回false报错
            List<int> resCountList = resCount.getDataList();
            if (!BankerHelper.VectorLess(vector, resCountList))
            {
                LogoutEventArgs e = new LogoutEventArgs("error: 新建进程的最大需求资源数超过最大资源数.");
                Logout.NewMsg(e);
                return false;
            }

            if (max.newRow(name))
            {
                //修改max
                max.modify(name,vector);
                need.newRow(name);
                allocation.newRow(name);
                //增加进程数
                progNum++;

                //计算need矩阵的值
                List<List<Int32>> t = new List<List<int>>();
                for (int i = 0; i < progNum; i++)
                {
                    t.Add(new List<int>());
                    for (int j = 0; j < resNum; j++)
                    {
                        t[i].Add(0);
                    }
                }
                BankerHelper.MatrixRed(max.getData(), allocation.getData(), ref t);
                need.modify(t);
                return true;
            }
            return false;
        }
        //修改资源数
        public bool modifyRes(List<Int32> newRes)
        {
            if (newRes.Count != resCount.columnNum)
            {
                return false;
            }
            //TODO: 判断是否超出了一些进程的Max
            //修改Available,先计算出当前的已分配的资源向量，再进行向量加减
            List<int> allocationList = getAllocationList();
            List<int> result = new List<int>();
            for(int i= 0; i < resNum; i++)
            {
                result.Add(0);
            }
            BankerHelper.VectorRed(newRes, allocationList, ref result);
            available.modify(result);
            resCount.modify(newRes);
            return true;
        }
        //获取当前已经分配的资源总数向量
        private List<Int32> getAllocationList()
        {
            List<Int32> allocationList = new List<int>();
            int t;
            for(int i = 0; i < resNum; i++)
            {
                t = 0;
                for(int j = 0; j < progNum; j++)
                {
                    t += allocation[j,i];
                }
                allocationList.Add(t);
            }
            return allocationList;
        }

        public void newRes()
        {
            resNum++;
            resCount.newColumn();
            available.newColumn();
            max.newColumn();
            allocation.newColumn();
            need.newColumn();
            workAllocation.newColumn();

            //计算need矩阵的值
            List<List<Int32>> t = new List<List<int>>();
            for(int i = 0; i < progNum; i++)
            {
                t.Add(new List<int>());
               for (int j = 0;j< resNum; j++)
                {
                    t[i].Add(0);
                }
            }
            BankerHelper.MatrixRed(max.getData(), allocation.getData(), ref t);
            need.modify(t);
        }
        public bool applyForRes(string name,List<int> request)
        {
            //清空workAllocation
            workAllocation.clearAll();
            List<string> safetyList = bankerAlg(name,request);
            if (safetyList != null)
            {
                StringBuilder sb = new StringBuilder("安全序列：{");
                foreach (string i in safetyList)
                {
                    sb.Append(i + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
                LogoutEventArgs e = new LogoutEventArgs(sb.ToString());
                Logout.NewMsg(e);
                return true;
            }
            return false;
        }

        //银行家算法
        private List<string> bankerAlg(string name, List<int> request)
        {
            //备份数据
            List<List<int>> allocationBackup, needBackup;
            List<int> availBackup;
            availBackup = available.getDataList();
            allocationBackup = allocation.getData();
            needBackup = need.getData();

            //如果Request大于need，报错
            if (BankerHelper.VectorLess(request, need.getVector(name)))
            {
                if (BankerHelper.VectorLess(request, available.getDataList()))
                {
                    //尝试赋值
                    List<int> newAvail = available.getDataList();
                    BankerHelper.VectorRed(available.getDataList(), request, ref newAvail);
                    available.modify(newAvail);

                    List<int> newAllocation = allocation.getVector(name);
                    BankerHelper.VectorAdd(allocation.getVector(name), request, ref newAllocation);
                    allocation.modify(name, newAllocation);

                    List<int> newNeed = need.getVector(name);
                    BankerHelper.VectorRed(need.getVector(name), request, ref newNeed);
                    need.modify(name, newNeed);

                    //进行安全性检测
                    List<string> safetyList = safetyAlg(request);
                    if (safetyList == null)
                    {
                        //恢复数据
                        LogoutEventArgs a = new LogoutEventArgs("error: 找不到合适的安全序列.");
                        Logout.NewMsg(a);
                        //恢复数据
                        available.modify(availBackup);
                        allocation.modify(allocationBackup);
                        need.modify(needBackup);
                    }
                    return safetyList;
                }
                else
                {
                    LogoutEventArgs a = new LogoutEventArgs("error: 尚无足够资源，需要等待.");
                    Logout.NewMsg(a);
                    //恢复数据
                    available.modify(availBackup);
                    allocation.modify(allocationBackup);
                    need.modify(needBackup);
                    return null;
                }
            }
            else
            {
                LogoutEventArgs a = new LogoutEventArgs("error: 申请的资源数已超过其宣布的最大值(need).");
                Logout.NewMsg(a);
                //恢复数据
                available.modify(availBackup);
                allocation.modify(allocationBackup);
                need.modify(needBackup);
            }
            return null;
        }
        //安全性算法，返回的是安全序列，当无安全序列时返回null
        private List<string> safetyAlg(List<int> request)
        {
            //步骤1
            List<int> work = new List<int>(available.getDataList().ToArray());
            List<bool> finish = new List<bool>();
            for(int i = 0; i < progNum; i++)
            {
                finish.Add(false);
            }
            List<string> safetyList = new List<string>();//安全序列
            int t = existProperProgress(finish, work);
            while (t != -1)
            {
                string name = max.getProgressNameList()[t];
                //步骤3
                for (int j = 0; j < resNum; j++)
                {
                    work[j] += allocation[t, j];
                }
                finish[t] = true;
                safetyList.Add(name);
                workAllocation.newRow(name);
                workAllocation.modify(name, work);
                t = existProperProgress(finish, work);
            }
            //步骤4
            if (BankerHelper.isAllFinish(finish))
            {
                return safetyList;
            }
            else
            {
                return null;
            }
        }

        private int existProperProgress(List<bool> finish, List<int> work)
        {
            for (int i = 0; i < progNum; i++)
            {
                if (finish[i] == false && BankerHelper.VectorLess(need.getVector(i), work))
                {
                    return i;
                }
            }
            return -1;
        }
        public List<string> getProgressNameList()
        {
            return max.getProgressNameList();
        }
    }
}
