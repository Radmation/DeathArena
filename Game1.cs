#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
#endregion

namespace DeathAreana
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //************************
        string[] newArray;
        string[] words;

        SpriteFont fontVelY;
        SpriteFont fontVelX; 
        SpriteFont fontPosX;
        SpriteFont fontPosY;

        string strVelY;
        string strVelX;
        string strPosX;
        string strPosY; 

        
        Vector2 screenPositionVelY = new Vector2(0,0);
        Vector2 screenPositionVelX = new Vector2(0,11);
        Vector2 screenPositionPosX = new Vector2(0,22);
        Vector2 screenPositionPosY = new Vector2(0,33); 
        //************************
        
        Player myPlayer;
        Camera camera; 
        Map map;
        Weapon weapon;
        Background background1;

        List<Rectangle> collisionRects = new List<Rectangle>();

        Song song; 

        MouseState mouseState = Mouse.GetState();
        Texture2D texture;

        KeyboardState presentKey;
        KeyboardState pastKey;///TESTING PURPOSES
      
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            myPlayer = new Player();
            weapon = new Weapon();
            map = new Map();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera(GraphicsDevice.Viewport);
            myPlayer.Load(Content);
            weapon.Load(Content);
            Tiles.Content = Content;
            song = Content.Load<Song>("sounds/gameMusic");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true; 
            texture = Content.Load<Texture2D>("player_images/paddle");
            map.Generate(new int[,]{
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,2,3,4,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,4,2,3,1,5,4,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,6,7,6,4,5,1,3,6,4,1,0,0,0,0,0,0,0,3,7,7,5,4,6,1,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,4,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,2,3,4,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,4,2,3,1,5,4,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,6,7,6,4,5,1,3,6,4,1,0,0,0,0,0,0,0,3,7,7,5,4,6,1,0,0,0,0,0,3},
                {3,7,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {1,3,1,2,4,7,5,3,2,5,6,0,0,0,0,0,0,0,0,7,6,4,1,6,0,0,0,0,0,0,0,0,7,6,5,4,7,6,4,1,2,3,5,4,1,2,4,3},
                {0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,7,0,0,0,4,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                {0,0,0,0,0,0,0,0,0,0,6,7,1,3,5,4,2,1,5,6,0,0,0,6,7,1,3,5,4,2,1,5,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3}, // 29 blocks wide
            }, 43);


            background1 = new Background(Content.Load<Texture2D>("map_images/backgroundGame"), new Rectangle(0, 0, map.Width, map.Height));
            fontVelY = Content.Load<SpriteFont>("text/SpriteFontVelocity"); //remove this later and fontstring
            fontVelX = Content.Load<SpriteFont>("text/SpriteFontVelocity");
            fontPosX = Content.Load<SpriteFont>("text/SpriteFontVelocity");
            fontPosY = Content.Load<SpriteFont>("text/SpriteFontVelocity");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            
            //test fonts 
            strVelY = myPlayer.velocity.Y.ToString();
            strVelX = myPlayer.velocity.X.ToString();
            strPosX = myPlayer.position.X.ToString();
            strPosY = myPlayer.position.Y.ToString();
            //check if tile is in range to be checked for collision

//           if ((newRectOriginX + 50 >= boundingBoxOriginX) && (newRectOriginY - 50 <= boundingBoxOriginY) || //top right quadrat 
//              (newRectOriginX - 50 <= boundingBoxOriginX) && (newRectOriginY - 50 <= boundingBoxOriginY) || //top left 
//              (newRectOriginX + 50 >= boundingBoxOriginX) && (newRectOriginY + 50 >= boundingBoxOriginY) || //bottom right
//              (newRectOriginX - 50 <= boundingBoxOriginX) && (newRectOriginY + 50 >= boundingBoxOriginY))  //bottom left
//           


            //foreach (CollisionTiles tile in map.CollisionTiles)
            //{
            //    //if (myPlayer.position.X + 100 >= tile.Rectangle.X && myPlayer.position.Y - 100 <= tile.Rectangle.Y)
            //    //{
            //        myPlayer.Collision(tile.Rectangle, map.Width, map.Height);
            //        //camera.Update(myPlayer.position, map.Width, map.Height);
            //    //}
            //}
            myPlayer.SelectAnimation(gameTime);
            myPlayer.Update(gameTime);
            //weapon.Update(gameTime, myPlayer.position, myPlayer.isLeft);
   

            camera.Update(myPlayer.position, map.Width, map.Height);



            //GET HELP HERE NOW

            //COLLISION RECTS FOR MAP ADDED HERE ----------- COLLISION RECTS FOR MAP ADDED HERE -----------COLLISION RECTS FOR MAP ADDED HERE -----------

            //NOTE: Need to remove rects out of the list if I am not near them -- 
            int blockX = (int)myPlayer.position.X / 43;
            int blockY = (int)myPlayer.position.Y / 43;

            //only if player position changed add new Rectangles
            //COLLISION HERE -- get help maybe, works though


            newArray = map.GetTileToRemove(blockX, blockY);

            for (int i = 0; i < newArray.Length; i++)
            {
                if (newArray[i] != null)
                {
                    words = newArray[i].Split(',');
                    Rectangle newRect = new Rectangle(Int32.Parse(words[1]) * 43, Int32.Parse(words[0]) * 43, 43, 43); //this is getting redrawn each time -- need to make perish
                    collisionRects.Add(newRect); //JUST KEEPS ADDING NEED TO OPTIMZE !!!!!!! IMPORTANT COUNT GOT TO 2,000 REAL QUICK
                }
            }

            foreach(Rectangle value in collisionRects)
            {
                //if (myPlayer.position.X + 100 >= tile.Rectangle.X && myPlayer.position.Y - 100 <= tile.Rectangle.Y)
                //{
                myPlayer.Collision(value, map.Width, map.Height);
                //camera.Update(myPlayer.position, map.Width, map.Height);
                //}
            }
             //maybe remove here ????
            collisionRects.Clear();

                //COLLISION RECTS FOR MAP ADDED HERE ----------- COLLISION RECTS FOR MAP ADDED HERE -----------COLLISION RECTS FOR MAP ADDE


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            background1.Draw(spriteBatch);
            
            map.Draw(spriteBatch);
            
            //spriteBatch.Draw(texture, myPlayer.boundingBox, Color.Pink); //debug 
            //spriteBatch.Draw(texture, myPlayer.punchingBox, Color.Red); //debug
            //spriteBatch.Draw(texture, weapon.boundingBox, Color.Red); //debug
            
            myPlayer.Draw(gameTime, spriteBatch);
            //weapon.Draw(gameTime, spriteBatch, myPlayer.isLeft);
            
            //spriteBatch.Draw(texture, myPlayer.boundingBox, Color.Red);
            spriteBatch.End();

            //test fonts
            spriteBatch.Begin();
            spriteBatch.DrawString(fontVelX, "X Velocity: " + strVelX, screenPositionVelX, Color.Red);
            spriteBatch.DrawString(fontVelY,"Y Velocity: "+ strVelY, screenPositionVelY, Color.Black);
            spriteBatch.DrawString(fontPosY, "Y Position: " + strPosY, screenPositionPosY, Color.Blue);
            spriteBatch.DrawString(fontPosX, "X Position: " + strPosX, screenPositionPosX, Color.Green);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
