using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harjoitustyo
{
    class Weapon
    {
        public string Name { get; set; }
        public int FireRate { get; set; }
        public int Damage { get; set; }
        public bool IsRanged { get; set; }
        public double MaxRange { get; set; }
        public int ClipSize { get; set; }

        public void Reload()
        {

        }
    }
}
