using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab02William
{
    // This extension is inspired by https://community.monogame.net/t/line-drawing/6962/5
    public static class SpriteBatchExtensions
    {
        private static Texture2D _texture;

        private static Texture2D GetTexture1x1(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }

            return _texture;
        }

        public static void DrawVerticalLine(this SpriteBatch spriteBatch, Vector2 position, float length
                                           , Color color, float thickness = 1f)
        {
            Vector2 origin = new Vector2(0.5f, 0.0f);
            Vector2 scale = new Vector2(thickness, length);
            spriteBatch.Draw(GetTexture1x1(spriteBatch), position, null, color, 0.0f, origin, scale, SpriteEffects.None, 0);
        }
    }
}
