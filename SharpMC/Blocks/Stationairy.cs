﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpMC.Utils;
using SharpMC.Worlds;

namespace SharpMC.Blocks
{
	public class Stationairy : Block
	{
		internal Stationairy(ushort id) : base(id)
		{
			IsSolid = false;
			IsBuildable = false;
			IsReplacible = true;
		}

		public override void DoPhysics(Level level)
		{
			CheckForHarden(level, (int) Coordinates.X, (int) Coordinates.Y, (int) Coordinates.Z);

			if (level.GetBlock(Coordinates).Id == Id)
			{
				SetToFlowing(level);
			}
		}

		private void SetToFlowing(Level world)
		{
			Block flowingBlock = BlockFactory.GetBlockById((byte)(Id - 1));
			flowingBlock.Metadata = Metadata;
			flowingBlock.Coordinates = Coordinates;
			world.SetBlock(flowingBlock, applyPhysics: false);
			world.ScheduleBlockTick(flowingBlock, 5);
		}

		private void CheckForHarden(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(new Vector3(x, y, z));
			{
				bool harden = false;
				if (block is BlockFlowingLava || block is BlockStationaryLava)
				{
					if (IsWater(world, x, y, z))
					{
						harden = true;
					}

					if (harden || IsWater(world, x, y, z + 1))
					{
						harden = true;
					}

					if (harden || IsWater(world, x - 1, y, z))
					{
						harden = true;
					}

					if (harden || IsWater(world, x + 1, y, z))
					{
						harden = true;
					}

					if (harden || IsWater(world, x, y + 1, z))
					{
						harden = true;
					}

					if (harden)
					{
						int meta = block.Metadata;

						if (meta == 0)
						{
							world.SetBlock(new BlockObsidian { Coordinates = new BlockCoordinates(x, y, z) });
						}
						else if (meta <= 4)
						{
							world.SetBlock(new BlockCobbleStone { Coordinates = new BlockCoordinates(x, y, z) });
						}
					}
				}
			}
		}

		private bool IsWater(Level world, int x, int y, int z)
		{
			Block block = world.GetBlock(new Vector3(x, y, z - 1));
			return block is BlockFlowingWater || block is BlockStationaryWater;
		}
	}
}