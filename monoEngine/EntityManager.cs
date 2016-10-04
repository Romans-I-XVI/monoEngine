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
		static List<RenderTarget2D> RenderTargets = new List<RenderTarget2D> ();

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

		public static void DrawToRenderTargets (SpriteBatch spriteBatch){
			foreach (var renderCanvas in Entities.OfType<RenderCanvas>()) {
				GameRoot.graphicsDevice.SetRenderTarget (renderCanvas.othersRenderTarget);
				spriteBatch.Begin ();
				foreach (var entity in Entities) {
					if (entity.renderTarget == renderCanvas.othersRenderTarget) {
						entity.DrawBegin (spriteBatch);
						entity.Draw (spriteBatch);
						entity.DrawEnd (spriteBatch);
					}
				}
				spriteBatch.End ();
				GameRoot.graphicsDevice.SetRenderTarget (null);
			}
						
		}

		public static void Draw(SpriteBatch spriteBatch){
			spriteBatch.Begin ();
			foreach (var entity in Entities) {
				if (entity.renderTarget == null) {
					entity.DrawBegin (spriteBatch);
					entity.Draw (spriteBatch);
					entity.DrawEnd (spriteBatch);
				}
			}
			spriteBatch.End ();
		}

	}
}

