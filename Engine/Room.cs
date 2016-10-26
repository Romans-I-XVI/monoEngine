using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Engine
{
	public abstract class Room
	{
	
		protected Room(){
			RoomManager.Add(this);
		}

		public virtual void OnSwitchTo(Room previous_room){
		}

		public virtual void OnSwitchAway(Room next_room) {
		}
	}
}

