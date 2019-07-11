using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Engine
{
	public abstract class Room
	{
		public virtual void OnSwitchTo(Room previousRoom) {}

		public virtual void OnSwitchAway(Room nextRoom) {}
	}
}

