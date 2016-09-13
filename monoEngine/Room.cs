using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;


namespace monogame
{
	abstract class Room : Entity
	{
		public Room ()
		{
			EntityManager.Clear ();
			EntityManager.Add (this);
		}

	}
}

