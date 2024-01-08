using StardewModdingAPI;

namespace YourModNamespace
{
    public class MyClass
    {
        private readonly IMonitor Monitor;

        // 构造函数注入
        public MyClass(IMonitor monitor)
        {
            this.Monitor = monitor;
        }

        public void DoSomething()
        {
            // 使用 Monitor 记录
            Monitor.Log("这是 MyClass 中的一个日志消息。", LogLevel.Info);
        }
    }
}