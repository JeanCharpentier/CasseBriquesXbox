using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class UIManager
    {
        public UIManager()
        {

        }
    }

    public class UIText:Entity
    {
        public UIText(string pTexString):base(pTexString)
        {
            pos.X = bounds.X / 2;
            pos.Y = bounds.Y / 2;
        }
    }
}
