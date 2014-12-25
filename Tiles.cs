using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeathAreana
{
    class Tiles
    {
        protected Texture2D texture;

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }

        }

        //this will enable texture to be loaded in this class
        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

    }
    
    class CollisionTiles : Tiles  //numbers reserved 11-1000 -- but for purposes 3
    {
        public CollisionTiles(int i, Rectangle newRectangle)
        { 
            //this will be for the tile names ex: "Tile1" "Tile2" "Tile3" "Tile0"
            texture = Content.Load<Texture2D>("map_images/Tile" + i);
            this.Rectangle = newRectangle;
        }
    }

    //can extend this class to make different types of tiles in the future ex door tiles, or teleport tiles --
    class BackgroundTiles : Tiles  //numbers reserved 1-10 -- but for purposes 4
    {
        public BackgroundTiles(int i, Rectangle newRectangle)
        {
            //this will be for the tile names ex: "Tile1" "Tile2" "Tile3" "Tile0"
            texture = Content.Load<Texture2D>("map_images/Tile" + i);
            this.Rectangle = newRectangle;
        }
    }

}
