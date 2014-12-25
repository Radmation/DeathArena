using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeathAreana
{
    class Weapon
    {
        public Texture2D texture;
        public Rectangle boundingBox;

        AnimationPlayer animationWeapon;
        Animation swordAnimation;
        SoundEffect swingSound;

        bool visible;
        float attackTimer;

        bool playSound;

        KeyboardState presentKey;
        KeyboardState pastKey;

        public Vector2 position = new Vector2(250, 100); //start position

        public void Load(ContentManager Content)
        {
            swordAnimation = new Animation(Content.Load<Texture2D>("weapon_images/swordSwing"), 48, 0.05f, true);  //texture2d, int framewidth,float frametim, bool looping
            swingSound = Content.Load<SoundEffect>("sounds/wooshSound");
        }

        public void Update(GameTime gameTime, Vector2 playerPosition, bool isLeft)
        {
            presentKey = Keyboard.GetState();
            if (isLeft)
            {
                position.X = playerPosition.X + 40;
                position.Y = playerPosition.Y;
                if(visible)
                    boundingBox = new Rectangle((int)position.X - 20, (int)position.Y - 25, 48, 50);
            }
            else
            {
                position.X = playerPosition.X - 45;
                position.Y = playerPosition.Y;
                if (visible)
                    boundingBox = new Rectangle((int)position.X - 24, (int)position.Y - 25, 48, 50);
            }
            if(!visible)
                boundingBox = new Rectangle(0, 0, 0, 0);
            

            attackTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Input(gameTime);


            if (attackTimer > 150)
            {
                visible = false;
            }

            if (playSound && pastKey.IsKeyUp(Keys.R))
            {
                swingSound.Play();
                playSound = false; 
            }

            pastKey = presentKey;

        }

        public void Input(GameTime gameTime)
        {
            if (presentKey.IsKeyDown(Keys.R) && pastKey.IsKeyUp(Keys.R))
            {
                visible = true;
                attackTimer = 0;
                animationWeapon.PlayAnimation(swordAnimation);
                playSound = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool isLeft)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (isLeft)
            {
                flip = SpriteEffects.None;
            }
            else
                flip = SpriteEffects.FlipHorizontally;
            if (visible)
                animationWeapon.Draw(gameTime, spriteBatch, position, flip);
        }

    }
}
