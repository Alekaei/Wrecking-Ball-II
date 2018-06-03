﻿using Rocket.API.Commands;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Linq;
using Rocket.UnityEngine.Extensions;
using System.Numerics;
using Rocket.API.Player;

namespace WreckingBall
{
	public class CommandTeleport : IChildCommand
	{
		private WreckingBallPlugin wreckPlugin;

		public string Name => "Teleport";
		public string [] Aliases => new string []
		{
			"t"
		};
		public string Summary => "Telleport to a random Barricade|Structure|Vehicle";
		public string Description => throw new NotImplementedException ();
		public string Permission => null;
		public string Syntax => "<barricade|structure|vehicle>";
		public IChildCommand [] ChildCommands => new IChildCommand [0];

		public CommandTeleport (WreckingBallPlugin plugin)
		{
			this.wreckPlugin = plugin;
		}

		public void Execute (ICommandContext context)
		{
			UnturnedPlayer player = ((UnturnedUser) wreckPlugin.Container.Resolve<IPlayerManager> ().GetOnlinePlayerById (context.User.Id)).Player;

			Random random = new Random (DateTime.Now.Millisecond);

			switch (context.Parameters.Get<string> (0).ToLower ())
			{
				case "b":
				case "barricade":
					BarricadeRegion randomBRegion = BarricadeManager.regions [random.Next (0, BarricadeManager.BARRICADE_REGIONS), random.Next (0, BarricadeManager.BARRICADE_REGIONS)];
					Vector3 randomPoint = randomBRegion.barricades [random.Next (0, randomBRegion.barricades.Count)].point.ToSystemVector ();
					randomPoint.Y += 10;
					player.Entity.Teleport (randomPoint);
					break;
				case "s":
				case "structure":
					StructureRegion randomSRegion = StructureManager.regions [random.Next (0, StructureManager.STRUCTURE_REGIONS), random.Next (0, StructureManager.STRUCTURE_REGIONS)];
					randomPoint = randomSRegion.structures [random.Next (0, randomSRegion.structures.Count)].point.ToSystemVector ();
					randomPoint.Y += 10;
					player.Entity.Teleport (randomPoint);
					break;
				case "v":
				case "vehicle":
					randomPoint = VehicleManager.vehicles [random.Next (0, VehicleManager.vehicles.Count)].transform.position.ToSystemVector ();
					randomPoint.Y += 10;
					player.Entity.Teleport (randomPoint);
					break;
			}
		}

		public bool SupportsUser (Type user) => typeof (UnturnedUser).IsAssignableFrom (user);
	}
}
