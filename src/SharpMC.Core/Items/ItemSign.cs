﻿using System.Collections;
using SharpMC.Core.Blocks;
using SharpMC.Core.Entity;
using SharpMC.Core.Enums;
using SharpMC.Core.Networking.Packages;
using SharpMC.Core.Utils;
using SharpMC.Core.Worlds;

namespace SharpMC.Core.Items
{
	public class ItemSign : Item
	{
		internal ItemSign() : base(323, 0)
		{
			IsUsable = true;
		}

		public override void UseItem(Level world, Player player, Vector3 blockCoordinates, BlockFace face)
		{
			blockCoordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			if (face == BlockFace.PositiveY)
			{
				var bss = new BlockStandingSign
				{
					Coordinates = blockCoordinates,
					Metadata = 0x00
				};

				var rawbytes = new BitArray(new byte[] {bss.Metadata});
				
				var direction = player.GetDirection();
				switch (direction)
				{
					case 0:
						//South
						rawbytes[2] = true;
						break;
					case 1:
						//West
						rawbytes[3] = true;
						break;
					case 2:
						//North DONE
						rawbytes[2] = true;
						rawbytes[3] = true;
						break;
					case 3:
						//East
						
						break;
				}
				bss.Metadata = ConvertToByte(rawbytes);
				world.SetBlock(bss);
				new SignEditorOpen(player.Wrapper)
				{
					Coordinates = blockCoordinates
				}.Write();
			}
			else
			{
				//TODO: implement wall signs
			}
		}
	}
}
