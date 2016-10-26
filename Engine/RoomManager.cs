using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
	public static class RoomManager
	{
		static private Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
		static private string current = "";

		public static void Add(Room room){
			Rooms.Add (room.GetType().Name, room);
		}

		public static Room Get(string name){
			return Rooms [name];
		}

		public static Room Current(){
			return Rooms [current];
		}

		public static void ChangeRoom(string room){
			if (Rooms.ContainsKey (room)) {
				if (Rooms.ContainsKey (current)) {
					Rooms [current].OnSwitchAway ();
				}
				EntityManager.Clear ();
				current = room;
				Rooms [current].OnSwitchTo ();
			}
		}
	}
}

