﻿using System.Collections.Generic;
using Rocket.API.DependencyInjection;
using Rocket.API.Permissions;
using Rocket.API.Scheduler;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using WreckingBall.Handlers;

namespace WreckingBall
{
	public class WreckingBallPlugin : Plugin<Config>
	{
		public readonly DestructionHandler DestructionHandler;

		public override Dictionary<string, string> DefaultTranslations => new Dictionary<string, string>
		{
			{ "wreckingball_prompt", "Type '<color=green>/wreck confirm</color>' or '<color=red>/wreck abort</color>'." },

			{ "wreckingball_confirmed", "Confirmed your last wreck request, calculating objects." },
			{ "wreckingball_aborted", "Your last wreck request has been aborted." },
			{ "wreckingball_no_request", "You currently don't have a pending wreck request!" },

			{ "wreckingball_scan", "Found {0} objects from your last request." },
			{ "wreckingball_added_destruction", "Added above objects to <color=red>destruction</color> queue. Estimated time: {0}" },

			{ "wreckingball_list_v", "[{0}] at position: {1} with {2} barricades." },
			{ "wreckingball_list_tp", "[#{0}] Steam ID: {0} Object count: {1}." }
		};

		public WreckingBallPlugin (
			IDependencyContainer container,
			ITaskScheduler taskScheduler,
			IPermissionProvider permissionProvider
			) : base ("WreckingBall II", container)
		{
			DestructionHandler = new DestructionHandler (this, taskScheduler, permissionProvider);
		}

		protected override void OnLoad (bool isFromReload)
		{
			DestructionHandler.Load ();

			if (ConfigurationInstance.DestructionInterval <= 0)
			{
				ConfigurationInstance.DestructionInterval = 1;
				Logger.LogWarning ("DestructionRate config value must be at or above 1.");
			}
			if (ConfigurationInstance.DestructionsPerInterval < 1)
			{
				ConfigurationInstance.DestructionsPerInterval = 1;
				Logger.LogWarning ("DestructionsPerInterval config value must be at or above 1.");
			}

			SaveConfiguration();
		}
	}
}
