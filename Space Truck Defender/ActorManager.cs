﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    /* This is the big daddy manager class.
     * it handles updating, taking input, collisions, etc
     * for all actors. so essentially it does the game.
     */
    public class ActorManager
    {
        
        //BEGIN DEFAULTS
        Engine baseEngine1,
                baseEngine2;
        Actor baseFriendlyActor,
                baseEnemyActor;
        Actor baseProjectile1;
        Weapon baseWeapon1;

        Hull ProjectileHull,
            ActorHull,
            PlayerHull;

        EffectDestroy baseDestroy;
        EffectDamage baseDamage;

        //END DEFAULTS





        //"connector" fields. links the actor manager to input
        // and display devices
        public GraphicsDeviceManager Graphics;
        public GraphicsDevice Device;
        public InputManager Input;

        private int BorderWidth,
                    BorderHeight;

        private List<AI> AIList;

        /* A lot of collidables have a "CollisionSettings" field
         * that determines which collidable list it goes into. the 
         * mapping is as follows:
         * 0: FriendlyCollidables (collide with all enemies)
         * 1: Friendly Projectiles (collide only with enemy projectiles)
         * 2: Enemy Collidables
         * 3: Enemy Projectiles
         */

        public List<Collidable> FriendlyCollidables,
                            FriendlyProjectiles,
                            EnemyCollidables,
                            EnemyProjectiles;

        //list of players
        private List<PlayerControls> PlayerControlsList;

        //collision manager. manages collisions.
        private CollisionManager Collisions;

        //
        public SpriteFont Font;


                    

        public ActorManager(GraphicsDeviceManager _graphics, GraphicsDevice _device, InputManager _input)
        {
            this.Graphics = _graphics;
            this.Device = _device;
            this.Input = _input;

            AIList = new List<AI>();

            FriendlyCollidables = new List<Collidable>();
            FriendlyProjectiles = new List<Collidable>();
            EnemyCollidables = new List<Collidable>();
            EnemyProjectiles = new List<Collidable>();


            PlayerControlsList = new List<PlayerControls>();
            Collisions = new CollisionManager(this);

            int gWidth = CollisionManager.GRID_WIDTH;
            int gHeight = CollisionManager.GRID_HEIGHT;

            BorderWidth = (gWidth + 2) / gWidth * Graphics.PreferredBackBufferWidth;
            BorderHeight = (gHeight + 2) / gHeight * Graphics.PreferredBackBufferHeight;

            InitializeBases();
        }

        private void InitializeBases()
        {
            ActorHull = new Hull(75, 5, 2);
            PlayerHull = new Hull(100, 5, 2);
            ProjectileHull = new Hull(1, -1, 0);
            
            baseDestroy = new EffectDestroy();
            baseDamage = new EffectDamage(20);
            
            baseEngine1 = new Engine(5, 50);
            baseEngine2 = new Engine(10, 100);
            Vector2 v = new Vector2(0,0);
            baseFriendlyActor = new Actor(v, 10, baseEngine1, PlayerHull, Device);
            //baseFriendlyActor.AddEffect(baseDestroy);
            baseFriendlyActor.SetCollision(0);
            baseEnemyActor = new Actor(v, 10, baseEngine1, ActorHull, Device);
            baseEnemyActor.SetCollision(2);
            //baseEnemyActor.AddEffect(baseDestroy);
            baseProjectile1 = new Actor(v, 5, baseEngine2, ProjectileHull, Device);
            baseProjectile1.AddEffect(baseDamage);
            baseProjectile1.SetCollision(1);
            //baseProjectile1.AddEffect(baseDestroy);
            List<double> d = new List<double>();
            for (int i = 0; i < 3 ; i++)
                d.Add(0);
            baseWeapon1 = new Weapon(baseProjectile1, d, 0.075f, 0.12);

        }

        public void SetFont(SpriteFont _font)
        {
            Font = _font;
            Collisions.SetFont(_font);
        }

        public void DrawCollisions(SpriteBatch sb)
        {
            Collisions.Draw(sb);
        }

        //
        public void NewPlayer(Vector2 _position)
        {
            Actor a = new Actor(baseFriendlyActor, _position);
            AI ai = new AI(a);
            a.AddWeapon(baseWeapon1);
            PlayerControls p = new PlayerControls(a);
            AIList.Add(ai);
            InsertCollidable(a);
            AddPlayer(p);
        }

        public void NewEnemy()
        {
            Random r = new Random();
            int xpos = r.Next(50, 400);
            int ypos = r.Next(50, 400);
            Vector2 pos = new Vector2(xpos, ypos);
            Actor a = new Actor(baseEnemyActor, pos);
            InsertCollidable(a);
        }

        public void AddPlayer(PlayerControls p)
        {
            PlayerControlsList.Add(p);
        }


        public void Update(GameTime gt)
        {
            //this only updates inpt commands
            UpdateInput(gt);

            UpdateAIList(gt);

            //use the below for syntax help
            //Action<List<Collidable>> x = l => UpdateCollidableList(l, gt);

            UpdateCollidableList(FriendlyCollidables, gt);
            UpdateCollidableList(FriendlyProjectiles, gt);
            UpdateCollidableList(EnemyCollidables, gt);
            UpdateCollidableList(EnemyProjectiles, gt);

            Collisions.CheckCollisions();

            //test code
            if (EnemyCollidables.Count < 100)
                NewEnemy();
        }

        //this updates actor lists and removes
        //any destroyed elements
        private void UpdateCollidableList(List<Collidable> list, GameTime gt)
        {
            int max = list.Count;
            for(int i = max-1; i >= 0; i--)
            {
                
                if (list[i] is Actor)
                    UpdateActor((Actor)list[i], gt);
                else
                    list[i].Update(gt);

                if (list[i].IsDestroyed())
                {
                    ProcessDestruction(list[i]);
                    list.RemoveAt(i);
                }
            }
        }

        private void UpdateAIList(GameTime gt)
        {
            int max = AIList.Count;
            var list = AIList;
            for (int i = max - 1; i >= 0; i--)
            {
                //first, do trigger checks

                //then, instantiate new states/ triggers

                if (!list[i].IsDestroyed())
                    list[i].Update(gt);
                else
                    list.RemoveAt(i);
            }
        }

        /* When a piece is destroyed, it might have death effects.
         * this method checks those statuses and resolves them
         */
        private void ProcessDestruction(Collidable c)
        {

        }

        //this processes each individual actor and extracts weapon data
        private void UpdateActor(Actor a, GameTime gt)
        {
            ProcessShooting(a);
            a.Update(gt);
        }

        private void ProcessShooting(Actor a)
        {
            if (a.IsShooting())
            {
                var shots = a.GetShots();
                if (shots.Count > 0)
                    InsertShots(shots);
            }
        }

        //Manages updates to input/Collidable coupling
        public void UpdateInput(GameTime gt)
        {
            foreach (PlayerControls p in PlayerControlsList)
            {
                p.UpdatePlayer(Input);
                p.Update(gt);
                if (p.GetRespawnTimeCurr() <= 0)
                {
                    p.ResetRespawnTime();
                    Actor a = p.GetPlayer();
                    a.Revive();
                    a.SetPosition(new Vector2(BorderWidth / 2, BorderHeight / 2));
                    InsertCollidable(a);
                }                    
            }
        }

        public void InsertCollidable(Collidable c)
        {
            switch (c.GetCollisionSetting())
            {
                case 0:
                    FriendlyCollidables.Add(c);
                    break;
                case 1:
                    FriendlyProjectiles.Add(c);
                    break;
                case 2:
                    EnemyCollidables.Add(c);
                    break;
                case 3:
                    EnemyProjectiles.Add(c);
                    break;
            }
        }

        //ONLY USE WHEN COLLIDABLES ARE IN THE SAME CATEGORY
        public void InsertShots(List<Actor> cl)
        {
            int setting = cl[0].GetCollisionSetting();
                switch (setting)
                {
                    case 0:
                        FriendlyCollidables.AddRange(cl);
                        break;
                    case 1:
                        FriendlyProjectiles.AddRange(cl);
                        break;
                    case 2:
                        EnemyCollidables.AddRange(cl);
                        break;
                    case 3:
                        EnemyProjectiles.AddRange(cl);
                        break;
                }
        }
        
        public void Draw(SpriteBatch sb)
        {
            /*
            DrawList(FriendlyCollidables, sb);
            DrawList(FriendlyProjectiles, sb);
            DrawList(EnemyCollidables, sb);
            DrawList(FriendlyProjectiles, sb);
             */
        }
        

        public void DrawList(List<Collidable> cl, SpriteBatch sb)
        {
            foreach (var c in cl)
            {
                if (c is IDrawable)
                {
                    var d = (IDrawable)c;
                    d.Draw(sb);
                }
            }
        }

    }
}
