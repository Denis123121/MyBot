using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot
{
    internal class Enemy:Fighter
    {
        private static Enemy bot = new Enemy();
        private static Player player = new Player();

        public Enemy() 
        {
            RecoverHealth();
        }
    }
}
