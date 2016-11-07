using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public abstract class Room
	{
	    
        public Type PreviousRoom;
        protected readonly List<Entity> _saved_entities = new List<Entity>();
		protected Room(){
			RoomManager.Add(this);
		}

		public virtual void OnSwitchTo(Room previous_room, params object[] args){ 
            foreach (var entity in _saved_entities)
            {
                EntityManager.Add(entity);
            }
            _saved_entities.Clear();
        }

		public virtual void OnSwitchAway(Room next_room) { }
	}
}

