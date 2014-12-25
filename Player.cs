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
    class Player
    {

        
        
        AnimationPlayer animationPlayer;

        Animation walkAnimation;
        Animation idleAnimation;
        Animation jumpAnimation;
        Animation punchAnimation;
        Animation dashAnimation;
        Animation wallClingAnimation; 

        bool walkPlaying = false;
        bool indlePlay = false;
        bool jumpPlaying = false;
        bool punchPlaying = false;

        SoundEffect jumpEffect; 

        //this is the start position - will change to spawn tiles!
        public Vector2 position = new Vector2(250, 100);
        public Vector2 velocity;
        public Rectangle boundingBox, punchingBox; 

        bool isRight;
        bool isLeft;

        bool hasJumped;
        bool hasDashed;
        bool isJumping; 
        bool isOnPlatform;
        bool isWalkingLeft, isWalkingRight, isWalking;
        bool isIdle;
        bool isPunching;
        bool isDashingLeft, isDashingRight, isDashing;
        bool isClingingLeft, isClingingRight, isClinging;
        bool isOnWallRight, isOnWallLeft, isOnWall;

        bool playLand = true; 

        float interval = 90;

        float timer, dashTimer, punchTimer;
        

        SoundEffect jumpSound, punchSound, landSound, dashSound, wooshSound;

        KeyboardState presentKey;
        KeyboardState pastKey;

        //input handles movement, direciton velocity
        //select animation handles which animation to display at any given time
        //update handles 
        //collision handles movement restrictions...overrides movement aka input

        public Player()
        {
        }

        public void Load(ContentManager Content)
        {
            walkAnimation = new Animation(Content.Load<Texture2D>("player_images/saske_run"), 48, 0.07f, true);  //texture2d, int framewidth,float frametim, bool looping
            idleAnimation = new Animation(Content.Load<Texture2D>("player_images/saske_stance3"), 39, 0.1f, true);  //texture2d, int framewidth,float frametim, bool looping  -comes from the animation class
            jumpAnimation = new Animation(Content.Load<Texture2D>("player_images/saske_jump"), 34, 0.1f, false);  //texture2d, int framewidth,float frametim, bool looping
            punchAnimation = new Animation(Content.Load<Texture2D>("player_images/saskePunch"), 62, 0.03f, true);  //texture2d, int framewidth,float frametim, bool looping
            dashAnimation = new Animation(Content.Load<Texture2D>("player_images/saskeDash"), 91, 0.05f, true);  //texture2d, int framewidth,float frametim, bool looping
            wallClingAnimation = new Animation(Content.Load<Texture2D>("player_images/wallClingRight"), 40, 0.05f, false); 

            jumpSound = Content.Load<SoundEffect>("sounds/jump");
            punchSound = Content.Load<SoundEffect>("sounds/oooSound");
            landSound = Content.Load<SoundEffect>("sounds/pong");
            dashSound = Content.Load<SoundEffect>("sounds/laserFire");
            wooshSound = Content.Load<SoundEffect>("sounds/wooshSound");
            isRight = true; 
        }

        //IMPORTANT: Having Issues with the collision - I have a bounding box for each animation - Can have a generic one
        //Its easier to have a generic one - may revise
        public void Update(GameTime gameTime)
        {


            position += velocity;

            boundingBox = new Rectangle((int)(position.X) - 20, (int)(position.Y) - 30, idleAnimation.FrameWidth, idleAnimation.FrameHeight);

            punchTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            

            if (velocity.Y != 0 && isDashing == false)
            {
                if (velocity.Y < 0)
                {
                    animationPlayer.PlayAnimation(jumpAnimation);
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    animationPlayer.Frameindex = 0;
                    if (timer > interval && animationPlayer.Frameindex == 0)
                    {
                        animationPlayer.Frameindex = 1;
                        timer = 0;
                    }
                    if (timer > interval && animationPlayer.Frameindex == 1)
                    {
                        animationPlayer.Frameindex = 0;
                        timer = 0;
                    }
                }
                else if (velocity.Y > 0)
                {
                    animationPlayer.PlayAnimation(jumpAnimation);
                    animationPlayer.Frameindex = 2;
                    if (timer > interval && animationPlayer.Frameindex == 2)
                    {
                        animationPlayer.Frameindex = 3;
                        timer = 0;
                    }
                    if (timer > interval && animationPlayer.Frameindex == 3)
                    {
                        animationPlayer.Frameindex = 2;
                        timer = 0;
                    }
                }
            }

            Input(gameTime, jumpEffect);

            //IMPORTANT: GRAVITY HERE and TERMINAL VELOCITY
            if (isOnPlatform == false)
                velocity.Y += 0.3f;

        }

        private void Input(GameTime gameTime, SoundEffect jumpEffect)
        {
            presentKey = Keyboard.GetState();

            if (playLand == true)
            {
                landSound.Play();
                playLand = false;
            }
            

            if (IsKeyPressed(Button.Space) && hasJumped == false)
            {
                isJumping = true; //omve to select animation
                if (isOnWallRight)
                {
                    hasJumped = true;
                    position.Y -= 10f; //speed height
                    velocity.Y = -10f;//speed jump
                    position.X -= 25f;
                    velocity.X = -20f;
                    boundingBox = new Rectangle((int)(position.X) - 20, (int)(position.Y) - 25, idleAnimation.FrameWidth, idleAnimation.FrameHeight);
                }
                else if (isOnWallLeft)
                {
                    hasJumped = true;
                    position.Y -= 10f; //speed height
                    velocity.Y = -10f;//speed jump
                    position.X += 25f;
                    velocity.X = 20f;
                    boundingBox = new Rectangle((int)(position.X) - 20, (int)(position.Y) - 25, idleAnimation.FrameWidth, idleAnimation.FrameHeight);
                }
                else
                {
                    hasJumped = true;
                    position.Y -= 10f; //speed height
                    velocity.Y = -10f;//speed jump
                    boundingBox = new Rectangle((int)(position.X) - 20, (int)(position.Y) - 25, idleAnimation.FrameWidth, idleAnimation.FrameHeight);
                }
                isOnPlatform = false;
                jumpSound.Play();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && isDashing == false)  //walks right CANNOT BE MY KEY PRESS METHOD
            {
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
                //animation handles
                isRight = false;
                isLeft = true;
               

                isWalking = true;
                isIdle = false;
                isClinging = false; 
                isPunching = false;
                isDashing = false;
                isOnWallRight = false;
                isClinging = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && isDashing == false) //walks left
            {
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
                //animation handles
                isRight = true;
                isLeft = false;

                isWalking = true;
                isIdle = false;
                isClinging = false; 
                isPunching = false;
                isDashing = false;
                isOnWallRight = false;
                isClinging = false;
            }
            else if (IsKeyPressed(Button.F)) //Dash
            {
                if (isLeft)
                {
                    //put velocity here...
                }
                if (isRight)
                {
                    //put velocity here...
                }
                dashTimer = 0;
                isWalking = false;
                isIdle = false;
                isPunching = false;
                isClinging = false;
                isOnWallRight = false;
                isClinging = false;

                isDashing = true;
                hasDashed = true;
            }
            else
            {
                velocity.X = 0f;
                isIdle = true;
                isWalking = false;
                isClinging = false; 
                isPunching = false;
                isOnWallRight = false;
                isClinging = false;
            }

            //IMPORTANT: ANIMATION DETERMINED HERE

            pastKey = presentKey;
        }
        //improve collision to use texture heights and widths instead of rectangles

        //IMPORTANT: COLLISION HERE COLLISION COLLISION

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if (velocity.Y > 5)
                playLand = true;

            if (boundingBox.TouchTopOf(newRectangle) == true)
            {
                //boundingBox.Y = newRectangle.Y - boundingBox.Height; //improve top landings!! 
                velocity.Y = 0;
                isOnPlatform = true;
                hasJumped = false; // in input method
                if (boundingBox.Bottom > newRectangle.Top)
                    position.Y -= 1;
            }
            else
                isOnPlatform = false;

            //TOUCH THE LEFT SIDE OF THE NEW RECTANGLE 
            if (boundingBox.TouchLeftOf(newRectangle) == true)
            {
                velocity.X = 0;
                if(presentKey.IsKeyDown(Keys.A))
                    velocity.X = -5;
                if (presentKey.IsKeyDown(Keys.D) && isOnPlatform == false)
                {
                    velocity.Y = 0;
                    hasJumped = false;
                    isOnWallRight = true;
                    isClinging = true;
                }
                else
                {
                    isOnWallRight = false;
                    isClinging = false;
                }
            }
            //TOUCH THE RIGHT SIDE OF NEW RECTANGLE
            if (boundingBox.TouchRightOf(newRectangle) == true)
            {
                //position.X = newRectangle.X + newRectangle.Width + 29;
                velocity.X = 0;
                if (presentKey.IsKeyDown(Keys.D))
                    velocity.X = 5;
                 if (presentKey.IsKeyDown(Keys.A) && isOnPlatform == false)
                {
                    velocity.Y = 0;
                    hasJumped = false;
                    isOnWallLeft = true;
                    isClinging = true;
                }
                else
                {
                    isOnWallLeft = false;
                    isClinging = false;
                }
            }

            if (boundingBox.TouchBottomOf(newRectangle) == true)
                    velocity.Y = 1f;



            //edge of screen
            if (position.X < 0)
                position.X = 0;
            if (position.X > xOffset - boundingBox.Width) position.X = xOffset - boundingBox.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - boundingBox.Height) position.Y = yOffset - boundingBox.Height; 
        }

        enum Button
        {
            A,
            S,
            D,
            W,
            F,
            R,
            Space
        };

        bool IsKeyPressed(Button button)
        {
            // Switch on the Button enum.
            switch (button)
            {
                case Button.A:
                    return (presentKey.IsKeyDown(Keys.A) && pastKey.IsKeyUp(Keys.A));
                case Button.S:
                    return (presentKey.IsKeyDown(Keys.S) && pastKey.IsKeyUp(Keys.S));
                case Button.D:
                    return (presentKey.IsKeyDown(Keys.D) && pastKey.IsKeyUp(Keys.D));
                case Button.W:
                    return (presentKey.IsKeyDown(Keys.W) && pastKey.IsKeyUp(Keys.W));
                case Button.F:
                    return (presentKey.IsKeyDown(Keys.F) && pastKey.IsKeyUp(Keys.F));
                case Button.R:
                    return (presentKey.IsKeyDown(Keys.R) && pastKey.IsKeyUp(Keys.R));
                case Button.Space:
                    return (presentKey.IsKeyDown(Keys.Space) && pastKey.IsKeyUp(Keys.R));
            }
            return false; 
        }


        public void SelectAnimation(GameTime gameTime)
        {
            dashTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (isClinging)
            {
                animationPlayer.PlayAnimation(wallClingAnimation);
            }
            if (isIdle)
            {
                animationPlayer.PlayAnimation(idleAnimation);
            }
            if (isWalking)
            {
                animationPlayer.PlayAnimation(walkAnimation);
            }
            if (isPunching)
            {
                animationPlayer.PlayAnimation(punchAnimation);
            }
            if (isDashing)
            {
                dashSound.Play();
                animationPlayer.PlayAnimation(dashAnimation);
            }


            if (dashTimer > 200)
            {
                isDashing = false;
                hasDashed = false;
                dashTimer = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects flip = SpriteEffects.None;

            if (isLeft)
                flip = SpriteEffects.None;
            else if (isRight)
                flip = SpriteEffects.FlipHorizontally;

            animationPlayer.Draw(gameTime, spriteBatch, position, flip);
        }
    }
}
