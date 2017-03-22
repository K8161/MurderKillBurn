using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harjoitustyo
{
    class Character
    {
        public string Name { get; set; }
        public int Hitpoints { get; set; }
        public bool HasWeapon { get; set; }
        public bool UsesAmmo { get; set; }

        public void Move()
        {

        }

        public void Fight()
        {

        }

        public void Die()
        {

        }

        public void AlterHitPoints()
        {

        }
        
    }

    class Enemy : Character
    {
        public int ScoreValue { get; set; }
        public int Accuracy { get; set; }

        public void AttackPlayer()
        {

        }
    }

    class Player : Character
    {
        List<Weapon> WeaponList { get; set; }
        public int Ammo { get; set; }
        public int Score { get; set; }

     /*   public Player (string name)
          {
              Name = name;
          } */

        public Player player = new Player();

        public void GainScore()
        {

        }

        public void ExitWhenDead()
        {

        }
    }
}
