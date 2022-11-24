using RegionKit.Utils;
using RWCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static RegionKit.Utils.RKUtils;
using static System.Math;

namespace RegionKit.MiscPO
{
	internal class RoomBorderTeleport : UpdatableAndDeletable
	{
		public RoomBorderTeleport(PlacedObject owner, Room rm)
		{
			_ow = owner;
		}
		private readonly PlacedObject _ow;
		private BorderTpData ow_data => _ow.data as BorderTpData;
		private float buffPX => (float)ow_data.buff * 20f;

		public override void Update(bool eu)
		{
			base.Update(eu);
			foreach (UpdatableAndDeletable? uad in room.updateList)
			{
				if (uad is not PhysicalObject po) continue;

				//Vector2 shift = default;
				FloatRect rm = room.RoomRect;
				FloatRect outer = rm.Grow(buffPX);
				IntVector2 reqshift = default;
				foreach (BodyChunk? chunk in po.bodyChunks)
				{
					Vector2 cp = chunk.pos;
					if (cp.x > outer.right) reqshift.x--;
					if (cp.x < outer.left) reqshift.x++;
					if (cp.y > outer.top) reqshift.y--;
					if (cp.y < outer.bottom) reqshift.y++;
				}
				Vector2 shift = new()
				{
					x = (Abs(reqshift.x) == po.bodyChunks.Length && ow_data.hOn)
					? (room.PixelWidth + (buffPX * ow_data.tpFrac)) * Sign(reqshift.x)
					: 0f,
					y = (Abs(reqshift.y) == po.bodyChunks.Length && ow_data.vOn)
					? (room.PixelHeight + (buffPX * ow_data.tpFrac)) * Sign(reqshift.y)
					: 0f,
				};
				if (shift is { x: 0f, y: 0f }) continue;
				foreach (BodyChunk? chunk in po.bodyChunks) chunk.pos += shift;
				if (po.graphicsModule is not null) po.graphicsModule.Reset();
				PetrifiedWood.WriteLine("tp! " + po.firstChunk.pos);
			}
		}
	}
}
