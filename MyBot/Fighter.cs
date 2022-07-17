using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot
{
    abstract internal class Fighter
    {
        public int hp;
        public int damage;
        public int weaponDistance;
        public int distance = 2;
        public bool isBlock = false;

        public bool IsAlive => hp > 0;

        public void TakeDamage(Fighter fighter)
        {
            if (!isBlock)
            {
                hp -= fighter.damage;
            }
            else
            {
                isBlock = false;
            }
        }

        public void Block()
        {
            isBlock = true;
        }

        public void Move(Fighter fighter, int offset)
        {
            distance += offset;
            fighter.distance += offset;
        }

        public void UseSword()
        {
            damage = 20;
            weaponDistance = 2;
        }

        public void UseBowl()
        {
            damage = 10;
            weaponDistance = 4;
        }

        public void UseSpear()
        {
            damage = 15;
            weaponDistance = 3;
        }
        
        public void RecoverHealth()
        {
            hp = 100;
        }
    }
}
