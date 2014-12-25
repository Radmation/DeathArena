using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeathAreana
{
    class Map
    {

        public int[,] map1;
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        public List<CollisionTiles> CollisionTiles
        {
            get { return collisionTiles; }
        }


        private List<BackgroundTiles> backgroundTiles = new List<BackgroundTiles>();
        public List<BackgroundTiles> BackgroundTiles
        {
            get { return backgroundTiles; }
        }

        private int width, height;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }
        
        public Map()
        {

        }

        void mymethod(out int param1, out int param2) //test here 
        {
            param1 = 10;
            param2 = 20;
        }
        
        public string[] GetTileToRemove(int blockX, int blockY) //works if called after the generate method
        {
            string[] myStringArray = new string[9];
            int index = 0;
            for (int x = 0; x < map1.GetLength(1); x++)
                for (int y = 0; y < map1.GetLength(0); y++)
                {
                    if (map1[y, x] != 0)
                    {
                        if (x == blockX && y == blockY || x == blockX - 1 && y == blockY - 1 || x == blockX + 1 && y == blockY + 1 ||
                            x == blockX + 1 && y == blockY - 1 || x == blockX - 1 && y == blockY + 1 || x == blockX && y == blockY + 1 ||
                            x == blockX && y == blockY - 1 || x == blockX - 1 && y == blockY || x == blockX + 1 && y == blockY + 1)
                        {
                            //then return array of rectangles to draw || return coords of rectangles to draw
                            myStringArray[index] = y.ToString() + "," + x.ToString();
                            index++;
                        }
                    }
                        
                }
            return myStringArray;
        }

        public void Generate(int[,] map, int size)
        {
            for(int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0  && number < 8)  //if tile number is 3 add collision 
                    {
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size))); // need to be able to return this rectangle or specs
                    }
                    if (number == 9) //if tile number is 4 no collision
                    {
                        backgroundTiles.Add(new BackgroundTiles(number, new Rectangle(x * size, y * size, size, size)));
                    }

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
            map1 = map;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
            foreach (BackgroundTiles tile in backgroundTiles)
                tile.Draw(spriteBatch);
        }
    }
}
