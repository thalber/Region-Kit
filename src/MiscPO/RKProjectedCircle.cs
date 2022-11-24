using CoralBrain;
using UnityEngine;

namespace RegionKit.MiscPO
{
	public class RKProjectedCircle
	{
		public static class EnumExt_ProjectedCircle
		{
			public static PlacedObject.Type ProjectedCircle;
		}

		public static void ApplyHooks()
		{
			On.Room.Loaded += (orig, self) =>
			{
				orig(self);
				for (int i = 0; i < self.roomSettings.placedObjects.Count; i++)
				{
					PlacedObject pObj = self.roomSettings.placedObjects[i];
					if (pObj.active && pObj.type == EnumExt_ProjectedCircle.ProjectedCircle)
						self.AddObject(new ProjectedCircleObject(self, pObj));
				}
			};
		}
	}

	public class ProjectedCircleObject : UpdatableAndDeletable, IOwnProjectedCircles
	{
		public PlacedObject pObj;

		public ProjectedCircleObject(Room room, PlacedObject pObj)
		{
			this.room = room;
			this.pObj = pObj;
			room.AddObject(new ProjectedCircle(room, this, 0, 180f));
		}

		public Vector2 CircleCenter(int index, float timeStacker)
		{
			return pObj.pos;
		}
	}
}