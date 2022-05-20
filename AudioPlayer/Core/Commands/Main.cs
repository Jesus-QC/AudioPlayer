using System;
using CommandSystem;

namespace AudioPlayer.Core.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Main : ParentCommand
    {
        public Main() => LoadGeneratedCommands();
        
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(Play.Instance);
            RegisterCommand(Stop.Instance);
            RegisterCommand(Test.Instance);
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Please, specify a subcommand (play, stop)";
            return false;
        }

        public override string Command { get; } = "audioplayer";
        public override string[] Aliases { get; } = {"ap", "audio"};
        public override string Description { get; } = "AudioPlayer lets you play custom audio.";
    }
}