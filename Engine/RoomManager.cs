using System;
using System.Collections.Generic;
using System.Linq;

// should be using your project here
using MonoGameTiles;
namespace Engine
{
    public static class RoomManager
    {
        static private readonly Dictionary<Type, Room> _rooms = new Dictionary<Type, Room>();
        static private Type _current;
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
            if (_rooms.ContainsKey(room.GetType()))
                _rooms.Remove(room.GetType());
            _rooms.Add (room.GetType(), room);
		}

        public static void Remove<T>() where T : Room
        {
            if (_rooms.ContainsKey(typeof(T)))
                _rooms.Remove(typeof(T));
        }

		public static Room Get<T>() where T : Room
        {
            if (_rooms.ContainsKey(typeof(T)))
                return _rooms [typeof(T)];
            return null;
		}

        public static bool ChangeRoom(Type room_type, params object[] args)
        {
            Type _previous = _current;
            if (_rooms.ContainsKey(room_type))
            {
                _current = room_type;
                if (_previous != null)
                {
                    _rooms[_previous].OnSwitchAway(_rooms[_current]);
                    EntityManager.ChangeRoom(_rooms[_previous], _rooms[_current]);
                    if (_current != _previous)
                        _rooms[_current].PreviousRoom = _previous;
                    _rooms[_current].OnSwitchTo(_rooms[_previous], args);
                }
                else
                    _rooms[_current].OnSwitchTo(null, args);
                return true;
            }
            return false;
            
        }

        public static bool ChangeRoom<T>(params object[] args) where T : Room
        {
            return ChangeRoom(typeof(T), args);
		}
	}
}

