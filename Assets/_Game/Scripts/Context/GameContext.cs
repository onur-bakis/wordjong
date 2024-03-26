using _Game.Scripts.Controller;
using Scripts.Managers;
using Scripts.Signals;
using Scripts.Commands;
using Scripts.Context.Signals;
using Scripts.Models;
using Scripts.Services;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Scripts.Context
{
	public class GameContext : MVCSContext
	{
		public GameContext (MonoBehaviour view) : base (view)
		{
		}
	
		public override IContext Start ()
		{
			base.Start ();
			var startSignal = injectionBinder.GetInstance<StartAppSignal> ();
			startSignal.Dispatch ();
			return this;
		}
	
		protected override void addCoreComponents ()
		{
			base.addCoreComponents ();
			injectionBinder.Unbind<ICommandBinder> ();
			injectionBinder.Bind<ICommandBinder> ().To<SignalCommandBinder> ().ToSingleton ();
		}

		protected override void mapBindings()
		{
			base.mapBindings();

			//signals to command
			commandBinder.Bind<StartAppSignal> ().To<StartAppCommand> ().Once ();
			injectionBinder.Bind<TapSignal> ().ToSingleton ();
		
			//services
			injectionBinder.Bind<TaskService> ().ToSingleton ();
		
			//models
			injectionBinder.Bind<SettingsModel> ().ToSingleton ();
			injectionBinder.Bind<GameModel> ().ToSingleton ();

			
			
			injectionBinder.Bind<UIManager>().ToSingleton();
			injectionBinder.Bind<CoreGameSignals>().ToSingleton();
			injectionBinder.Bind<CoreUISignals>().ToSingleton();
			injectionBinder.Bind<GameBoardSignals>().ToSingleton();
			injectionBinder.Bind<GameUISignals>().ToSingleton();
			injectionBinder.Bind<GameUIManager>().ToSingleton();
		}
		protected override void postBindings()
		{
			base.postBindings();
		
		}
	}
}