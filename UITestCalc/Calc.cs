using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Automation;

namespace UITestCalc
{
    class Calc
    {
        /// <summary>
        /// 需要测试的 EXE 程序.
        /// </summary>
        private const String APP_NAME = @"C:\Windows\system32\calc.exe";
        /// <summary>
        /// 每个操作 默认的间隔时间.
        /// </summary>
        private const int DEFAULT_SLEEP_TIME = 2000;
        /// <summary>
        /// 用于启动 被测试程序 的进程
        /// </summary>
        Process process;
        /// <summary>
        /// 用于存储 被测试程序 的主窗体.
        /// </summary>
        AutomationElement testMainForm;
        /// <summary>
        /// 所有的按钮.
        /// </summary>
        AutomationElementCollection testAllButtons;
        /// <summary>
        /// 所有的文本框.
        /// </summary>
        AutomationElementCollection testAllText;

        /// <summary>
        /// 通过按钮名称，获取按钮对象.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AutomationElement GetButton(String name)
        {
            foreach (AutomationElement element in testAllButtons)
            {
                if (element.Current.Name == name)
                {
                    return element;
                }
            }
            // 遍历所有都没有检索到，返回NULL.
            return null;
        }
        /// <summary>
        /// 通过文本框名称，获取文本框对象.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AutomationElement GetText(String name)
        {
            foreach (AutomationElement element in testAllText)
            {
                if (element.Current.Name == name)
                {
                    return element;
                }
            }
            // 遍历所有都没有检索到，返回NULL.
            return null;
        }
        /// <summary>
        /// 按指定名称的 按钮.
        /// </summary>
        /// <param name="name"></param>
        void PressButton(String name)
        {
            Console.WriteLine("按 [{0}] 按钮。", name);
            // 通过名字，取得按钮对象.
            AutomationElement aeButton = GetButton(name);
            // 通过InvokePattern模拟点击按钮
            InvokePattern ipClickButton1 =
                (InvokePattern)aeButton.GetCurrentPattern(InvokePattern.Pattern);
            ipClickButton1.Invoke();
            // 休眠指定时间.
            Thread.Sleep(DEFAULT_SLEEP_TIME);
        }

        /// <summary>
        /// 模拟键盘输入.
        /// </summary>
        /// <param name="keys"></param>
        void SendKeys(String keys)
        {
            Console.WriteLine("在键盘上按 [{0}] 键。", keys);
            System.Windows.Forms.SendKeys.SendWait(keys);
            // 休眠指定时间.
            Thread.Sleep(DEFAULT_SLEEP_TIME);
        }
        /// <summary>
        /// 启动 被测试程序.
        /// </summary>
        public void StartApp()
        {
            Console.WriteLine("尝试启动程序：[{0}]", APP_NAME);
            // 启动被测试的程序
            process = Process.Start(APP_NAME);
            // 当前线程休眠2秒.
            Thread.Sleep(DEFAULT_SLEEP_TIME);
            // 获得对主窗体对象的引用
            testMainForm = AutomationElement.FromHandle(process.MainWindowHandle);
            // 计算器层次下，首先是一个 Pane.
            AutomationElementCollection panes = testMainForm.FindAll(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane));
            // 获取主窗体上的所有按钮.
            testAllButtons = panes[0].FindAll(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));
            // 获取主窗体上的所有文本框.
            testAllText = panes[0].FindAll(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));
        }

        /// <summary>
        /// 关闭 被测试程序.
        /// </summary>
        public void CloseApp()
        {
            Console.WriteLine("尝试关闭程序：[{0}]", APP_NAME);
            // 休眠指定时间.
            Thread.Sleep(DEFAULT_SLEEP_TIME);
            // 关闭被测试程序
            WindowPattern wpCloseForm = (WindowPattern)testMainForm.GetCurrentPattern(WindowPattern.Pattern);
            wpCloseForm.Close();
        }

        public void Test1()
        {
            // 取得 结果 框.
            AutomationElement resultElement = GetText("0");
            PressButton("1");
            PressButton("加");
            PressButton("1");
            PressButton("等于");
            Console.WriteLine("结果显示:{0}", resultElement.Current.Name);
            PressButton("清除");
        }

        public void Test2()
        {
            // 取得 结果 框.
            AutomationElement resultElement = GetText("0");
            SendKeys("1");
            SendKeys("{ADD}");
            SendKeys("1");
            SendKeys("{ENTER}");
            Console.WriteLine("结果显示:{0}", resultElement.Current.Name);
        }
    }
}
