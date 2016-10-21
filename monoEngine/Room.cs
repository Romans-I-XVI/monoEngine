using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;


namespace Engine
{
	public abstract class Room
	{
	
		public Room(){
			RoomManager.Add(this);
		}

		public virtual void OnSwitchTo(){
		}

		public virtual void OnSwitchAway() {
		}
	}
}

