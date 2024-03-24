using strange.extensions.command.impl;
using UnityEngine;

namespace Scripts.Commands
{
    public class StartAppCommand : Command
    {
        public override void Execute()
        {
            ConfigureApp();
        }

        private void ConfigureApp()
        {
            Application.targetFrameRate = 60;   
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}