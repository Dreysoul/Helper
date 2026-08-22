namespace YiRongMachine
{
    /// <summary>
    /// 测试的结果
    /// </summary>
    ///
    public enum ResultCMD
    {
        OK = 1,
        NG,
        Null,
    }

    /// <summary>
    /// 设备状态，负责自动化状态的启用
    /// </summary>
    ///
    public enum LightType
    {
        Defalut,
        HaiShi_12T,
    }

    public enum PCState
    {
        Run,            //运行状态
        Pause,          //暂停状态
        Error,          //错误状态
    }

    public enum NGCode
    {
        OK = 1,
        BigNG = -1,
    }

    /// <summary>
    /// 盖子的种类
    /// </summary>
    public enum iBearingCapMaterial
    {
        /// <summary>
        /// 铁盖
        /// </summary>
        Metal,

        /// <summary>
        /// 胶盖
        /// </summary>
        Plastic,
    }

    /// <summary>
    /// 硬件连接状态
    /// </summary>
    public enum HardwareConnectType
    {
        Tcp,
        SerialPort,
    }

    public enum MachineType
    {
        FourCamera,
        ThreeCamera,
        VendorDebug,
    }
}