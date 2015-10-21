using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    public class Actor: Collidable, IDrawable
    {
        private GraphicsDevice Graphics;
        protected Texture2D Sprite;

        private Hull hull;
        private Engine engine;
        private List<Weapon> WeaponList;

        private bool Shooting = false;

        Random random = new Random();

        Dictionary<int,Effect> ActiveEffects;

        //standard constructor. location, hitbox radius, and the relevant graphics device.
        public Actor(Vector2 _position, int _hitRadius, Engine _engine, Hull _hull, GraphicsDevice _Graphics)
        {
            this.Position = _position;
            this.Delete = false;
            this.Graphics = _Graphics;
            /* NOTE: this constructor is used
             * to create base instances of an actor. Copy
             * constructors are needed when we create new concrete instances of 
             * actors on the gamespace. For this however we can use shared references
             */
            this.hull = _hull;
            this.engine = _engine;
            this.WeaponList = new List<Weapon>();
            this.CollisionSetting = 0;

            this.Box = new Hitbox(_hitRadius, this.CollisionSetting, _position, _Graphics);
            this.CollisionEffects = new Dictionary<int, Effect>();
            this.ActiveEffects = new Dictionary<int,Effect>();
        }




        //COPY CONSTRUCTOR
        //for use when creating new instances of an actor
        public Actor(Actor a, Vector2 _position)
        {
            this.Position = _position;
            this.Graphics = a.Graphics;
            this.engine = new Engine(a.engine);
            this.hull = a.hull.Copy();
            SetHeading(a.engine.GetHeading());
            this.WeaponList = new List<Weapon>();
            foreach (Weapon w in a.WeaponList)
                AddWeapon(w);

            this.Box = new Hitbox(a.Box, Position);
            SetCollision(a.GetCollisionSetting());
            this.CollisionEffects = new Dictionary<int, Effect>();
            foreach (int e in a.CollisionEffects.Keys)
                AddEffect(a.CollisionEffects[e]);
            /* Active effects refer to things currently happening 
             * to the actor, and thus are not copied when we create
             * a new actor
             */
            this.ActiveEffects = new Dictionary<int, Effect>();
        }

        //PROJECTILE COPY CONSTRUCTOR
        /* For this, we do copy active effects.
         * projectiles might have all sorts of timers, etc.
         * that are required for proper behavior, so we copy those.
         */
        public Actor(Actor a, Vector2 _position, double _heading)
            : this(a, _position)
        {
            SetHeading(_heading);
        }

        public void AddWeapon(Weapon w)
        {
            this.WeaponList.Add(new Weapon(w));
        }

        public void SetHeading(Double v)
        {
            engine.SetHeading(v);
        }


        public void Draw(SpriteBatch sb)
        {
            //throw new NotImplementedException();
            int width = Sprite.Width;
            int height = Sprite.Height;

            //need to adjust draw location by sprite size
            int xpos = (int)Position.X - width / 2;
            int ypos = (int)Position.Y - height / 2;

            Rectangle sourceRectangle = new Rectangle(0,0, width, height);
            Rectangle destinationRectangle = new Rectangle(xpos, ypos, width, height);
            sb.Draw(Sprite, destinationRectangle, sourceRectangle, Color.White);
        }

        public void LoadSprite(Texture2D sprite)
        {
            this.Sprite = sprite;
        }
        public void LoadSprite(ContentManager content, String spriteName)
        {
            this.Sprite = content.Load<Texture2D>(spriteName);
        }

        public override void Update(GameTime gt)
        {
            UpdateWeapons(gt);
            UpdateEffects(gt);
            this.Move(gt);
            if (!HullIsAlive())
                this.Destroy();
            //to do: add update effects list
            //to do: 
        }

        private void UpdateWeapons(GameTime gt)
        {
            foreach (Weapon w in WeaponList)
                w.Update(gt);
        }

        private void UpdateEffects(GameTime gt)
        {
            List<int> removalList = new List<int>();
            foreach(int key in ActiveEffects.Keys)
            {
                var e = ActiveEffects[key];
                if (e.HasTime())
                    ActiveEffects[key].UpdateEffect(this, gt);
                else
                {
                    e.OnEnd(this);  
                    removalList.Add(key);
                }
            }
            foreach (int i in removalList)
            {
                ActiveEffects.Remove(i);
            }
        }

        public void Move(GameTime gt)
        {
            engine.Update(gt);
            Position = Vector2.Add(Position, engine.GetOffset());
            Box.UpdatePosition(this.Position);
        }

        public double GetHeading()
        {
            return engine.GetHeading();
        }

        public void Go()
        {
            engine.Go();
        }

        public void Stop()
        {
            engine.Stop();
        }

        public void SetCollision(int i)
        {
            this.CollisionSetting = i;
            Hitbox tempBox = new Hitbox(this.GetHitRadius(), this.CollisionSetting, this.GetPosition(), this.Graphics);
            this.Box = tempBox;
        }

        //supes important, yo
        /* This method takes the effects from c
         * and applies it to THIS. it also does 
         * type based collision resolution.
         */
        public override void Collide(Collidable c)
        {
            Dictionary<int,Effect> effects = c.GetCollisionEffects();
            foreach(int i in effects.Keys)
            {
                var e = effects[i];
                e.OnAttach(this);
                float time = e.GetCurrTime();
                if (time <= 0) 
                    e.OnEnd(this);
                else
                    AddActiveEffect(e);
            }

            if (c is Actor)
            {
                Actor a = (Actor)c;
                int mt = this.GetHullType();
                int ot = a.GetHullType();
                // if I'm a projectile, self destruct on impact
                if (mt == 0)
                    this.Destroy();
                // if I'm a missile, self destruct if i hit a solid
                else if (mt == 1)
                {
                    if (ot == 2)
                    {
                        this.Destroy();
                    }
                }
                // if I'm a solid, take damage if the enemy is a solid.
                // the enemy will do the same. there might be a survivor.
                else if (mt == 2)
                {
                    if (ot == 2)
                    {
                        //DamagePierce(50);
                        var dmg = new EffectDamagePierce(a.GetCurrHP());
                        AddActiveEffect(dmg);
                    }
                }
            }
        }

        public void ResetHP()
        {
            hull.ResetHP();
        }

        public bool HullIsAlive()
        {
            if (GetCurrHP() > 0)
                return true;
            return false;
        }

        public float GetMaxHP()
        {
            return this.hull.GetMaxHP();
        }

        public float GetCurrHP()
        {
            return this.hull.GetCurrHP();
        }

        public void Damage(float d)
        {
            this.hull.Damage(d);
        }

        public void DamagePierce(float d)
        {
            this.hull.DamagePierce(d);
        }

        public int GetHullType()
        {
            return this.hull.GetHullType();
        }

        private void AddActiveEffect(Effect e)
        {
            int id = e.GetID();
            if (ActiveEffects.ContainsKey(id))
                ActiveEffects[id] = e.ResolveDuplicate(ActiveEffects[id]);
            else
                ActiveEffects[id] = e;
        }

        public void ClearActiveEffects()
        {
            this.ActiveEffects = new Dictionary<int, Effect>();
        }

        public List<Weapon> GetWeapons()
        {
            return this.WeaponList;
        }

        public void SetWeaponHeadings(double d)
        {
            foreach (Weapon w in WeaponList)
                w.SetHeading(d);
        }

        public void ResetWeaponCollision()
        {
            int setting = this.GetCollisionSetting();
            foreach (Weapon w in WeaponList)
            {
                switch(setting)
                {
                    case 0:
                    case 1:
                        w.SetCollision(1);
                        break;
                    case 2:
                    case 3:
                        w.SetCollision(3);
                        break;
                }
            }
        }

        public List<Actor> GetShots()
        {
            List<Actor> retList = new List<Actor>();
            foreach (var weapon in WeaponList)
            {
                if (weapon.GetCurrentCooldown() <= 0)
                {
                    var projectile = weapon.GetProjectile();
                    double projHeading = weapon.GetHeading();
                    double deviation = weapon.GetAccOffset();
                    foreach (var h in weapon.GetHeadingOffsets())
                    {
                        Actor tempProj = new Actor(projectile, this.GetPosition());
                        double offset = deviation * (random.NextDouble() *  2 - 1);
                        double newHeading = projHeading + offset + h;
                        tempProj.SetHeading(newHeading);
                        tempProj.Go();
                        retList.Add(tempProj);
                    }
                    weapon.Reload();
                }
            }
            return retList;
        }

        public bool IsShooting()
        {
            return Shooting;
        }

        public void Shoot()
        {
            Shooting = true;
        }

        public void HoldFire()
        {
            Shooting = false;
        }

        public void Special(bool b)
        {
            //implement
        }

        public override void Destroy()
        {
            base.Destroy();
            ClearActiveEffects();
        }

        public override void Revive()
        {
            base.Revive();
            ResetHP();
        }
     }
}
