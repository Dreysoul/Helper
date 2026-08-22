namespace YiRongMachine
{
    public class UserManager
    {
        public static System.DateTime _dtLastOperate = System.DateTime.Now;              //程序最后操作时间
        private static int _noOperationTimeSpan = 60 * 3;                                //没有操作就锁定的间隔时间,单位秒

        public static int NoOperationTimeSpan
        {
            get
            {
                return _noOperationTimeSpan;
            }
            set
            {
                if (value > 60 * 10 || value < 5)  //必须大于5秒钟，小于10分钟
                {
                    _noOperationTimeSpan = 60 * 5;
                }
                else
                {
                    _noOperationTimeSpan = value;
                }
            }
        }

        /// <summary>
        /// 是否长时间没有用户操作
        /// </summary>
        /// <returns></returns>
        private static bool IsLongTimeNoOperation()
        {
            if (TimeHelper.TimeSpanSecods(_dtLastOperate, System.DateTime.Now) > NoOperationTimeSpan)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  是否长时间没有用户操作监测
        /// </summary>
        public static bool NotifyLongTimeNoOperation()
        {
            bool bChangeAuthority = false;
            if (LoginForm.UserAuthority != UserAutority.Operater)
            {
                if (IsLongTimeNoOperation())
                {
                    LoginForm.UserAuthority = UserAutority.Operater;
                    bChangeAuthority = true;
                }
            }
            return bChangeAuthority;
        }
    }
}