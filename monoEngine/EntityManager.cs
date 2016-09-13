using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
	static class EntityManager
	{
		static List<Entity> Entities = new List<Entity>();

		public static void Add(Entity entity){
			Entities.Add (entity);
		}

		public static void Clear() {
			Entities.Clear ();
		}

		public static int Count(Type entity_type){
			int entity_count = 0;
			foreach (var entity in Entities) {
				if (entity_type.IsEquivalentTo(entity.GetType())) {
					entity_count += 1;
				}
			}
			return entity_count;
		}

		public static void Update(GameTime gameTime){
			foreach (var entity in Entities.ToList()) {
				entity.Update (gameTime);
			}

			Entities = Entities.Where(x => !x.IsExpired).ToList();
		}

		public static void Draw(SpriteBatch spriteBatch){
			foreach (var entity in Entities) {
				entity.DrawBegin (spriteBatch);
				entity.Draw (spriteBatch);
				entity.DrawEnd (spriteBatch);
			}
		}

	}
}

