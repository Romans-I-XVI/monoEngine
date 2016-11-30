using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
	public abstract class Room
	{

        public bool Persistent = true;
        public List<Entity> Entities = new List<Entity>();
        public Type PreviousRoom;
		protected Room(){
			RoomManager.Add(this);
		}

        public void Initialize() 
        {
            if (Persistent)
            {
                Entities.Clear();
                List<Entity> temp_entities = EntityManager.Entities.Where(x => !(x.IsPersistent)).ToList();
                EntityManager.Clear();
                OnCreate();
                Entities = EntityManager.Entities.Where(x => !(x.IsPersistent)).ToList();
                EntityManager.Clear();
                foreach (var entity in temp_entities)
                {
                    bool old_expired_state = entity.IsExpired;
                    EntityManager.Add(entity);
                    entity.IsExpired = old_expired_state;
                }
            }
            else 
            {
                OnCreate();
            }
        }

        public virtual void OnCreate() { }

		public virtual void OnSwitchTo(Room previous_room, params object[] args){}

		public virtual void OnSwitchAway(Room next_room) { }
	}
}

