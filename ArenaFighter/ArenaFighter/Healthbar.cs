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
        GraphicsDeviceManager graphics;
        Texture2D healthRect;
        Texture2D damageRect;
        int constBarWidth = 20;

        public Healthbar(Game1 g, GraphicsDeviceManager graph, int health, Vector2 location)
        {

            maxHealth = health;
            currentHealth = maxHealth;
            healthbarLoc = location;
            game = g;
            graphics = graph;

            healthRect = new Texture2D(graphics.GraphicsDevice, maxHealth, constBarWidth);
            Color[] data1 = new Color[maxHealth * constBarWidth];
            for (int i = 0; i < data1.Length; ++i) data1[i] = Color.LawnGreen;
            healthRect.SetData(data1);

            damageRect = new Texture2D(graphics.GraphicsDevice, maxHealth - currentHealth, constBarWidth);
            Color[] data2 = new Color[(maxHealth - currentHealth) * constBarWidth];
            for (int i = 0; i < data2.Length; ++i) data2[i] = Color.Red;
            damageRect.SetData(data2);
        }

        public void changeHealth(int amount)
        {
            currentHealth += amount;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice g)
        {
            //Draws rectangles to depict the healthbar
            spriteBatch.Begin();
            spriteBatch.Draw(healthRect, healthbarLoc, Color.LawnGreen);
            spriteBatch.Draw(damageRect, new Vector2(healthbarLoc.X + maxHealth - currentHealth, healthbarLoc.Y), Color.Red);
            spriteBatch.End();

            // Dont touch this stuff. These reset some problems that come out of writing text on the screen
            g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
        }

    }
}
