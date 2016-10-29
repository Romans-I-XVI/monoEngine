using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public abstract class Room
	{
	    
        protected Rooms _type;
        public Rooms Type { get { return _type;} }
		protected Room(Rooms type){
            _type = type;
			RoomManager.Add(this);
		}

		public virtual void OnSwitchTo(Room previous_room, params object[] args){ }

		public virtual void OnSwitchAway(Room next_room) { }
	}
}

