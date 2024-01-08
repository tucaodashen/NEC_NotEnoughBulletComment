using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using NotEnoughBulletComment;
using YourModNamespace;
using System.Threading;

//请高性能机器人亚托莉保佑这段代码不会出bug吧！(/≧▽≦/)


namespace NotEnoughBulletComment
{

    internal sealed class ModEntry : Mod
    {
        private static Timer _timer;
        BulletComment commentHandel;
        public override void Entry(IModHelper helper)
        {

            commentHandel= new BulletComment();
            TimerCallback callback = new TimerCallback(DisplayComment);
            _timer = new Timer(callback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));



            helper.ConsoleCommands.Add("BC_Start", "Launch the BulletComment Display \n\nUsage: BC_Start(NoArguements)", this.BCStart);
            helper.ConsoleCommands.Add("BC_Stop", "Stop the BulletComment Display \n\nUsage: BC_Stop(NoArguements)", this.BCStop);
            helper.ConsoleCommands.Add("BC_Delay", "Set the delay count(ms) \n\nUsage: BC_Delay <int>", this.BCSetDelay);
            helper.ConsoleCommands.Add("BC_ID", "Set roomID \n\nUsage: BC_ID <int>", this.BCSetID);
            helper.ConsoleCommands.Add("BC_API", "SetAPI \n\nUsage: BC_API <string>", this.BCSetAPI);

        }

        
        private void DisplayComment(object state)
        {
            if (commentHandel.currentBulletComment != null) { 
                foreach(string cm in commentHandel.currentBulletComment)
                {
                    Monitor.Log(cm,LogLevel.Trace);
                    Game1.chatBox.addMessage(cm, Color.White);
                    commentHandel.currentBulletComment = null;
                }
            }
        }
        public void BCStart(string command, string[] args)
        {
            if(commentHandel.running==null||commentHandel.running==false)
            {
                if(commentHandel.roomID==null)
                {
                    Monitor.Log("You haven't set id yet.", LogLevel.Error);
                }
                else
                {
                    if (commentHandel.Delay <= 50)
                    {
                        Monitor.Log("Refresh too fast(Delay<=50)", LogLevel.Error);
                    }
                    else
                    {
                        commentHandel.StartLoop();
                    }
                }
                
            }
            else
            {
                Monitor.Log("You can't start when a instance is running.", LogLevel.Error);
            }
            
        }
        public void BCStop(string command, string[] args)
        {
            if (commentHandel.running)
            {
                commentHandel.StopLoop();
            }
            else
            {
                Monitor.Log("You haven't start yet.", LogLevel.Error);
            }
        }
        public void BCSetAPI(string command, string[] args)
        {
            commentHandel.urlAPI = args[0];
            string mes = "You have set your API to " + args[0];
            Monitor.Log(mes, LogLevel.Info);
        }
        public void BCSetID(string command, string[] args)
        {
            commentHandel.roomID = args[0];
            string mes = "You have set your roomID to " + args[0];
            Monitor.Log(mes, LogLevel.Info);
        }
        public void BCSetDelay(string command, string[] args)
        {
            commentHandel.Delay = int.Parse(args[0]);
            string mes = "You have set your Delay to " + args[0];
            Monitor.Log(mes, LogLevel.Info);
        }

    }
}
