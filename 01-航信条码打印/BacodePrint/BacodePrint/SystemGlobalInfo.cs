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


        //以下是相关全局信息
        //配置文件路径
        public string mstrConfigFilePath = Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + "Config.ini";

        //行程单路径
        public string mstrItineraryFilePath = Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + "travel itinerary.jpg";

        //mm转像素的比例系数：打印机分辨率600/英寸，mm到英寸25.4/英寸,k=600/25.4
        //A4纸宽：793.70078740157476 210mm
        //A4纸高：1122.5196850393702 297mm
        public const double const_dScale = 3.779527559055118;
    }
}
