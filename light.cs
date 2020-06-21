using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2019Tmpl8
{
    public class light
    {
        public Vector3 lightLoc;
        public int lightID = 0;

        public light (Vector3 lLocIn, int lIDIn)
        {
            lightID = lIDIn;
            lightLoc = lLocIn;
        }
    }
}
