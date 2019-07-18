using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace MonoEngine
{
	public abstract class Room
	{
		public abstract void onSwitchTo(Room previousRoom, Dictionary<string, object> args);

		public abstract void onSwitchAway(Room nextRoom);
	}
}

