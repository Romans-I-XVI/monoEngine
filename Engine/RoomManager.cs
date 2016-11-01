using System;
using System.Collections.Generic;
using System.Linq;

// should be using your project here
using MonoGameTiles;
namespace Engine
{
	public static class RoomManager
	{
        static private readonly Dictionary<Rooms, Room> _rooms = new Dictionary<Rooms, Room>();
        static private Rooms _current;
        public static Room CurrentRoom
        {
            get
            {
                if (_rooms.ContainsKey(_current))
                    return _rooms[_current];
                return null;
            }
        }

		public static void Add(Room room){
            if (_rooms.ContainsKey(room.Type))
                _rooms.Remove(room.Type);
            _rooms.Add (room.Type, room);
		}

        public static void Remove(Rooms name)
        {
            if (_rooms.ContainsKey(name))
                _rooms.Remove(name);
        }

		public static Room Get(Rooms name){
            if (_rooms.ContainsKey(name))
			    return _rooms [name];
            return null;
		}

        public static bool ChangeRoom(Rooms name, params object[] args){
            Rooms _previous = _current;
			if (_rooms.ContainsKey (name)) {
                _current = name;
                if (Get(_previous) != null)
                {
                    _rooms[_previous].OnSwitchAway(_rooms[_current]);
                    EntityManager.ChangeRoom(_rooms[_previous], _rooms[_current]);
                    _rooms[_current].OnSwitchTo(_rooms[_previous], args);
                }
                else
                    _rooms[_current].OnSwitchTo(null, args);
			}
            return false;
		}
	}
}

