using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ArenaFighter
{
    class Healthbar
    {
        int maxHealth;
        int currentHealth;
        Vector2 healthbarLoc;

        Texture2D healthRect;
        Texture2D damageRect;

        Game1 game;
        GraphicsDevice graphics;

        public Healthbar(Game1 g, GraphicsDevice theGraphics, int health, Vector2 location)
        {
            maxHealth = health;
            currentHealth = maxHealth;
            healthbarLoc = location;

            // Sets up the health rectangle (only computed once)
            healthRect = new Texture2D(theGraphics, maxHealth, GameConstants.HEALTHBAR_WIDTH);
            Color[] data1 = new Color[maxHealth * GameConstants.HEALTHBAR_WIDTH];
            for (int i = 0; i < data1.Length; ++i) data1[i] = Color.MediumSeaGreen;
            healthRect.SetData(data1);

            damageRect = null;

            game = g;
            graphics = theGraphics;
        }

        public void changeHealth(int health)
        {
            // Changes the current health and recomputes the damage rectangle accordingly
            currentHealth = health;
            if (currentHealth != maxHealth)
            {
                damageRect = new Texture2D(graphics, maxHealth - currentHealth, GameConstants.HEALTHBAR_WIDTH);
                Color[] data2 = new Color[(maxHealth - currentHealth) * GameConstants.HEALTHBAR_WIDTH];
                for (int i = 0; i < data2.Length; ++i) data2[i] = Color.Red;
                damageRect.SetData(data2);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draws rectangles to depict the healthbar
            spriteBatch.Begin();
            spriteBatch.Draw(healthRect, healthbarLoc, Color.MediumSeaGreen);
            if (currentHealth != maxHealth)
                spriteBatch.Draw(damageRect, new Vector2(healthbarLoc.X + currentHealth, healthbarLoc.Y), Color.Red);
            spriteBatch.End();

            // Don't touch this stuff. These reset some problems that come from writing text on the screen
            graphics.BlendState = BlendState.Opaque;
            graphics.DepthStencilState = DepthStencilState.Default;
            graphics.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }
}
