using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    //does collisions. What else?
    public class CollisionManager
    {
        public GraphicsDevice Device;
        public ActorManager Manager;

        //size fields here
        //NOTE: Width and height includes buffer zones
        //outside the draw range
        public const int GRID_WIDTH = 9,
                           GRID_HEIGHT = 9;

        private int ZoneWidth,
                    ZoneHeight;

        private int BorderWidth,
                    BorderHeight;

        private Texture2D ZoneSprite;
        private SpriteFont Font;

        //the grids. These help manage collisions.

        /* Just to clarify:
         * Friendly collidables, projectiles etc are just CATEGORIES.
         * They, most importantly, enumerate what an actor can collide with.
         * Collidables: All enemy types
         * Projectiles: only enemy collidables
         */
        private List<Collidable>[,] FriendlyCollidables,
                                FriendlyProjectiles,
                                EnemyCollidables,
                                EnemyProjectiles;

        //This variables track collision data
        //note that Collidables in each list can just be called with .Count, no need for extra variables
        private int CollisionChecks;

        public CollisionManager(ActorManager _actorManager)
        {
            Device = _actorManager.Device;
            Manager = _actorManager;
            Font = _actorManager.Font;
            GraphicsDeviceManager Graphics = _actorManager.Graphics;
            BorderWidth = Graphics.PreferredBackBufferWidth;
            BorderHeight = Graphics.PreferredBackBufferHeight;
            ZoneWidth = BorderWidth / (GRID_WIDTH-2);
            ZoneHeight = BorderHeight / (GRID_HEIGHT-2);

            FriendlyCollidables = new List<Collidable>[GRID_WIDTH, GRID_HEIGHT];
            FriendlyProjectiles = new List<Collidable>[GRID_WIDTH, GRID_HEIGHT];
            EnemyCollidables = new List<Collidable>[GRID_WIDTH, GRID_HEIGHT];
            EnemyProjectiles = new List<Collidable>[GRID_WIDTH, GRID_HEIGHT];

            CreateEmptyGrids();
            ZoneSprite = CreateZoneSprite();

        }

        public void SetFont(SpriteFont _font)
        {
            this.Font = _font;
        }

        //the method that does it all.
        public void CheckCollisions()
        {
            //Reset tracking data
            ResetData();
            //first, make sure the grids are empty
            CreateEmptyGrids();
            //then, we fill the zones

            var FriendlyCollidablesList = CullBorders(Manager.FriendlyCollidables);
            FillZone(Manager.FriendlyCollidables, FriendlyCollidables);

            var FriendlyProjectilesList = CullBorders(Manager.FriendlyProjectiles);
            FillZone(FriendlyProjectilesList, FriendlyProjectiles);

            var EnemyCollidablesList = CullBorders(Manager.EnemyCollidables);
            FillZone(EnemyCollidablesList, EnemyCollidables);

            var EnemyProjectilesList = CullBorders(Manager.EnemyProjectiles);
            FillZone(EnemyProjectilesList, EnemyProjectiles);
            //check collisions
            
            TestCollisionList(FriendlyProjectilesList, EnemyCollidables);
            TestCollisionList(EnemyProjectilesList, FriendlyCollidables);
            TestCollisionList(FriendlyCollidablesList, EnemyCollidables);
            
            //resolve
        }
        
        /* For a given list of collidables, check collisions between
         * that list and the specified grid
         */ 
        private void TestCollisionList(List<Collidable> list, List<Collidable>[,] grid)
        {
            foreach (var a in list)
            {
                TestCollisions(a, grid);
            }
        }

        public List<Collidable> CullBorders(List<Collidable> list)
        {
            List<Collidable> retList = new List<Collidable>();
            foreach(Collidable c in list)
            {
                if (IsInBorders(c))
                    retList.Add(c);
                else
                    c.Destroy();
            }
            return retList;
        }

        public bool IsInBorders(Collidable c)
        {
            int x = ((int)c.GetPosition().X / ZoneWidth) + 1;
            int y = ((int)c.GetPosition().Y / ZoneHeight) + 1;
            if (x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT)
                return true;
            else
                return false;
        }

        public void CreateEmptyGrids()
        {
            for (int i = 0; i < GRID_WIDTH; i++)
            {
                for (int j = 0; j < GRID_HEIGHT; j++)
                {
                    FriendlyCollidables[i, j] = new List<Collidable>();
                    FriendlyProjectiles[i, j] = new List<Collidable>();
                    EnemyCollidables[i, j] = new List<Collidable>();
                    EnemyProjectiles[i, j] = new List<Collidable>();
                }
            }
        }

        //given a collidable returns the primary zone it resides in
        public Coordinate GetPrimaryZone(Collidable c)
        {
            int row = ((int)c.GetPosition().X / ZoneWidth) + 1;
            int column = ((int)c.GetPosition().Y / ZoneHeight) + 1;
            return new Coordinate(row, column);
        }

        //returns a list of all of the zones a collidable object intersects
        public List<Coordinate> GetAllZones(Collidable c)
        { 
            List<Coordinate> retList = new List<Coordinate>();

            Coordinate pos = GetPrimaryZone(c);
            retList.Add(pos);
            Vector2 p = c.GetPosition();
            int radius = c.GetHitRadius();
            bool up, down, left, right;
            up = down = left = right = false;
            if (p.X % ZoneWidth <= radius)
            {
                retList.Add(new Coordinate(pos.X - 1, pos.Y));
                left = true;
            }
            else if ((p.X % ZoneWidth) + radius >= ZoneWidth)
            {
                retList.Add(new Coordinate(pos.X + 1, pos.Y));
                right = true;
            }
            //Check overlap on vertical zones
            if (p.Y % ZoneHeight <= radius)
            {
                retList.Add(new Coordinate(pos.X, pos.Y - 1));
                up = true;
            }
            else if((p.Y % ZoneHeight) + radius >= ZoneHeight)
            {
                retList.Add(new Coordinate(pos.X, pos.Y + 1));
                down = true;
            }
            //Checking corner overlaps
            if (left)
            {
                if (up)
                    retList.Add(new Coordinate(pos.X - 1, pos.Y - 1));
                else if (down)
                    retList.Add(new Coordinate(pos.X - 1, pos.Y + 1));

            }
            else if (right)
            {
                if (up)
                    retList.Add(new Coordinate(pos.X + 1, pos.Y - 1));
                else if (down)
                    retList.Add(new Coordinate(pos.X + 1, pos.Y + 1));
            }

            return retList;
        }

        //this takes the Collidable lists from the Collidable manager,
        //fills up the zones
        public void FillZone(List<Collidable> list, List<Collidable>[,] grid)
        {
            foreach(Collidable a in list)
            {
                InsertIntoZones(a, grid);
            }
        }

        //this puts an individual Collidable into a zone, then returns a tuple containing the Collidable
        // and their respective zone
        public void InsertIntoZones(Collidable a, List<Collidable>[,] grid)
        {
            List<Coordinate> zoneList = GetAllZones(a);
            foreach (var c in zoneList)
                grid[c.X, c.Y].Add(a);
        }
        

        //given an Collidable/coordinate tuple, this method processes those collisions
        public void TestCollisions(Collidable c, List<Collidable>[,] grid)
        {
            HashSet<Collidable> potentialCollisions = new HashSet<Collidable>();
            List<Coordinate> zones = GetAllZones(c);
            foreach (var zone in zones)
            {
                potentialCollisions.UnionWith(grid[zone.X, zone.Y]);
            }


            foreach(Collidable b in potentialCollisions)
            {
                CollisionChecks += 1;
                if (DoesCollide(c, b))
                    Collidable.ResolveCollision(c, b);
            }

        }

        //checks if two Collidables collide
        public bool DoesCollide(Collidable a, Collidable b)
        {
            int radius = a.GetHitRadius() + b.GetHitRadius();
            int dX = (int)b.GetPosition().X - (int)a.GetPosition().X;
            int dY = (int)b.GetPosition().Y - (int)a.GetPosition().Y;
            if (radius * radius < dX * dX + dY * dY)
                return false;
            else
                return true;
        }



        //This resets tracking data
        private void ResetData()
        {
            CollisionChecks = 0;
        }

        //makes the tiles for drawing
        public Texture2D CreateZoneSprite()
        {
            Texture2D rect = new Texture2D(Device, ZoneWidth, ZoneHeight);

            Color[] data = new Color[ZoneWidth * ZoneHeight];
            for (int i = 0; i < data.Length; ++i) data[i] = (Color.White * 0.2f);
            rect.SetData(data);

            return rect;
        }

        //draws collision detection information.
        //not for use in regular gameplay.
        public void Draw(SpriteBatch sb)
        {
            DrawHitboxes(sb);
            DrawAllZones(sb);
            DrawData(sb);
        }

        //draws the collision detection zones
        public void DrawAllZones(SpriteBatch sb)
        {
            for (int i = 1; i < GRID_WIDTH-1; i++)
            {
                for (int j = 1; j < GRID_HEIGHT-1; j++)
                {
                    if (FriendlyCollidables[i, j].Any())
                        DrawZone(sb, i, j, Color.Blue);
                    if (EnemyCollidables[i, j].Any())
                        DrawZone(sb, i, j, Color.Red);
                }
            }
        }

        //draws the hitboxes of the Collidables
        public void DrawHitboxes(SpriteBatch sb)
        {
            foreach (Collidable a in Manager.FriendlyCollidables)
                a.DrawHitbox(sb);

            foreach (Collidable a in Manager.FriendlyProjectiles)
                a.DrawHitbox(sb);

            foreach (Collidable a in Manager.EnemyCollidables)
                a.DrawHitbox(sb);
            
            foreach (Collidable a in Manager.EnemyProjectiles)
                a.DrawHitbox(sb);
        }

        public void DrawData(SpriteBatch sb)
        {
            List<String> msgList = new List<String>();
            msgList.Add("Friendly Collidables: " + Manager.FriendlyCollidables.Count.ToString());
            msgList.Add("Friendly Projectiles: " + Manager.FriendlyProjectiles.Count.ToString());
            msgList.Add("Enemy Collidables:    " + Manager.EnemyCollidables.Count.ToString());
            msgList.Add("Enemy Projectiles:    " + Manager.EnemyProjectiles.Count.ToString());
            msgList.Add("Collisions Checked:   " + CollisionChecks.ToString());
            for(int i = 0; i < msgList.Count; i++)
            {
                sb.DrawString(Font, msgList[i], new Vector2(20, 20 * (i + 1)), Color.White);
            }
        }

        public void DrawZone(SpriteBatch sb, int row, int column, Color color)
        {
            int width = ZoneWidth;
            int height = ZoneHeight;
            Rectangle sourceRectangle = new Rectangle(0, 0, width, height);
            Rectangle destinationRectangle = new Rectangle((row-1) * width, (column-1)* height, width, height);
            sb.Draw(ZoneSprite, destinationRectangle, sourceRectangle, color);
        }

        //contains zone coordinates. just 2 ints, bounded by the grid size
        public class Coordinate
        {
            public int X,
                        Y;
            public Coordinate(int x, int y)
            {
                this.X = Bound(x, 0, GRID_WIDTH);
                this.Y = Bound(y, 0, GRID_HEIGHT);
            }

            public int Bound(int i, int min, int max)
            {
                if (i < min)
                    return min;
                if (i >= max)
                    return max-1;
                return i;
            }
        }


    }
}
