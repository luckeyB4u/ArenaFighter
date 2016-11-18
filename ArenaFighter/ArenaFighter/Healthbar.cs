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
        Game1 game;
        GraphicsDevice g;
        Texture2D healthRect;
        Texture2D damageRect;

        public Healthbar(Game1 theGame, GraphicsDevice graphics, int health, Vector2 location)
        {
            maxHealth = health;
            currentHealth = maxHealth;
            healthbarLoc = location;
            game = theGame;
            g = graphics;

            healthRect = new Texture2D(g, maxHealth, GameConstants.HEALTHBAR_WIDTH);
            Color[] data1 = new Color[maxHealth * GameConstants.HEALTHBAR_WIDTH];
            for (int i = 0; i < data1.Length; ++i) data1[i] = Color.ForestGreen;
            healthRect.SetData(data1);

            damageRect = null;
        }

        public void changeHealth(int health)
        {
            currentHealth = health;
            if (currentHealth != maxHealth)
            {
                damageRect = new Texture2D(g, maxHealth - currentHealth, GameConstants.HEALTHBAR_WIDTH);
                Color[] data2 = new Color[(maxHealth - currentHealth) * GameConstants.HEALTHBAR_WIDTH];
                for (int i = 0; i < data2.Length; ++i) data2[i] = Color.Red;
                damageRect.SetData(data2);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws rectangles to depict the healthbar
            spriteBatch.Begin();
            spriteBatch.Draw(healthRect, healthbarLoc, Color.ForestGreen);
            if(currentHealth != maxHealth)
                spriteBatch.Draw(damageRect, new Vector2(healthbarLoc.X + currentHealth, healthbarLoc.Y), Color.Red);
            spriteBatch.End();

            // Don't touch this stuff. These reset some problems that come out of writing text on the screen
            g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
        }

    }
}
