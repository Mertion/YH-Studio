using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacodePrint
{
    //系统参数类，使用单例模式
    public sealed class SystemGlobalInfo
    {
        private static readonly SystemGlobalInfo instance = new SystemGlobalInfo();
        private static object obj = new object();

        //配置文件路径
        public string mstrConfigFilePath = Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + "Config.ini";

        List<UserControlTextBoxItems> listText = new List<UserControlTextBoxItems>();
        public const int nListMaxCount = 34;

        /// <summary>
        /// 显式的静态构造函数用来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static SystemGlobalInfo() { }

        private SystemGlobalInfo() { }

        public static SystemGlobalInfo Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
