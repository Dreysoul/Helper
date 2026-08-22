using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YiRongMachine
{
    public class CreateThread
    {
        public static bool StartThread(out string strError)
        {
            strError = "";
            try
            {
                if (GlobalVariable.iWorkStation == 2)
                {
                    Thread th_DuanMianA_Ctrl = new Thread(GlobalVariable.duanMianAAutoFlow.CreateControlThread);
                    th_DuanMianA_Ctrl.IsBackground = true;
                    th_DuanMianA_Ctrl.Start();
                    Thread th_DuanMianA_Calc = new Thread(GlobalVariable.duanMianAAutoFlow.CreateCalculateThread);
                    th_DuanMianA_Calc.IsBackground = true;
                    th_DuanMianA_Calc.Start();
                    Thread th_DuanMianA_Show = new Thread(GlobalVariable.duanMianAAutoFlow.CreateShowThread);
                    th_DuanMianA_Show.IsBackground = true;
                    th_DuanMianA_Show.Start();

                    Thread th_DuanMianB_Ctrl = new Thread(GlobalVariable.duanMianBAutoFlow.CreateControlThread);
                    th_DuanMianB_Ctrl.IsBackground = true;
                    th_DuanMianB_Ctrl.Start();
                    Thread th_DuanMianB_Calc = new Thread(GlobalVariable.duanMianBAutoFlow.CreateCalculateThread);
                    th_DuanMianB_Calc.IsBackground = true;
                    th_DuanMianB_Calc.Start();
                    Thread th_DuanMianB_Show = new Thread(GlobalVariable.duanMianBAutoFlow.CreateShowThread);
                    th_DuanMianB_Show.IsBackground = true;
                    th_DuanMianB_Show.Start();
                }
                else
                {
                    Thread th_DuanMianA_Ctrl = new Thread(GlobalVariable.duanMianAAutoFlow.CreateControlThread);
                    th_DuanMianA_Ctrl.IsBackground = true;
                    th_DuanMianA_Ctrl.Start();
                    Thread th_DuanMianA_Calc = new Thread(GlobalVariable.duanMianAAutoFlow.CreateCalculateThread);
                    th_DuanMianA_Calc.IsBackground = true;
                    th_DuanMianA_Calc.Start();
                    Thread th_DuanMianA_Show = new Thread(GlobalVariable.duanMianAAutoFlow.CreateShowThread);
                    th_DuanMianA_Show.IsBackground = true;
                    th_DuanMianA_Show.Start();

                    Thread th_WaiYuanA_Ctrl = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateControlThread);
                    th_WaiYuanA_Ctrl.IsBackground = true;
                    th_WaiYuanA_Ctrl.Start();
                    Thread th_WaiYuanA_Calc = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateCalculateThread);
                    th_WaiYuanA_Calc.IsBackground = true;
                    th_WaiYuanA_Calc.Start();
                    Thread th_WaiYuanA_Show = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateShowThread);
                    th_WaiYuanA_Show.IsBackground = true;
                    th_WaiYuanA_Show.Start();

                    Thread th_DaoJiaoA_Ctrl = new Thread(GlobalVariable.daoJiaoAAutoFlow.CreateControlThread);
                    th_DaoJiaoA_Ctrl.IsBackground = true;
                    th_DaoJiaoA_Ctrl.Start();
                    Thread th_DaoJiaoA_Calc = new Thread(GlobalVariable.daoJiaoAAutoFlow.CreateCalculateThread);
                    th_DaoJiaoA_Calc.IsBackground = true;
                    th_DaoJiaoA_Calc.Start();
                    Thread th_DaoJiaoA_Show = new Thread(GlobalVariable.daoJiaoAAutoFlow.CreateShowThread);
                    th_DaoJiaoA_Show.IsBackground = true;
                    th_DaoJiaoA_Show.Start();

                    Thread th_NeiKong_Ctrl = new Thread(GlobalVariable.neiKongAutoFlow.CreateControlThread);
                    th_NeiKong_Ctrl.IsBackground = true;
                    th_NeiKong_Ctrl.Start();
                    Thread th_NeiKong_Calc = new Thread(GlobalVariable.neiKongAutoFlow.CreateCalculateThread);
                    th_NeiKong_Calc.IsBackground = true;
                    th_NeiKong_Calc.Start();
                    Thread th_NeiKong_Show = new Thread(GlobalVariable.neiKongAutoFlow.CreateShowThread);
                    th_NeiKong_Show.IsBackground = true;
                    th_NeiKong_Show.Start();

                    Thread th_DuanMianB_Ctrl = new Thread(GlobalVariable.duanMianBAutoFlow.CreateControlThread);
                    th_DuanMianB_Ctrl.IsBackground = true;
                    th_DuanMianB_Ctrl.Start();
                    Thread th_DuanMianB_Calc = new Thread(GlobalVariable.duanMianBAutoFlow.CreateCalculateThread);
                    th_DuanMianB_Calc.IsBackground = true;
                    th_DuanMianB_Calc.Start();
                    Thread th_DuanMianB_Show = new Thread(GlobalVariable.duanMianBAutoFlow.CreateShowThread);
                    th_DuanMianB_Show.IsBackground = true;
                    th_DuanMianB_Show.Start();

                    Thread th_WaiYuanB_Ctrl = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateControlThread);
                    th_WaiYuanB_Ctrl.IsBackground = true;
                    th_WaiYuanB_Ctrl.Start();
                    Thread th_WaiYuanB_Calc = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateCalculateThread);
                    th_WaiYuanB_Calc.IsBackground = true;
                    th_WaiYuanB_Calc.Start();
                    Thread th_WaiYuanB_Show = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateShowThread);
                    th_WaiYuanB_Show.IsBackground = true;
                    th_WaiYuanB_Show.Start();

                    Thread th_DaoJiaoB_Ctrl = new Thread(GlobalVariable.daoJiaoBAutoFlow.CreateControlThread);
                    th_DaoJiaoB_Ctrl.IsBackground = true;
                    th_DaoJiaoB_Ctrl.Start();
                    Thread th_DaoJiaoB_Calc = new Thread(GlobalVariable.daoJiaoBAutoFlow.CreateCalculateThread);
                    th_DaoJiaoB_Calc.IsBackground = true;
                    th_DaoJiaoB_Calc.Start();
                    Thread th_DaoJiaoB_Show = new Thread(GlobalVariable.daoJiaoBAutoFlow.CreateShowThread);
                    th_DaoJiaoB_Show.IsBackground = true;
                    th_DaoJiaoB_Show.Start();
                }
                return true;
            }
            catch (Exception e)
            {
                strError = "开始线程出现异常，异常信息为" + e.Message;
                //LogHelper.AddThrowLog(strError);
                return false;
            }
        }
    }
}