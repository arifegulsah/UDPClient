using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberlesmeGulsah
{
    internal class Modellerim
    {
        public class PilModel
        {
            private static int _ID = 0; //statik olmak zorundaki farklı bir yerlerde değişim yapıldığı zaman hep oyle kalsın
            private static uint _deger;
            public int ID { get { return _ID; } }
            public uint Deger { get { return _deger; } set { _deger = value; } }

        }

        public class IrtifaModel
        {
            private static int _ID = 1; //statik olmak zorundaki farklı bir yerlerde değişim yapıldığı zaman hep oyle kalsın
            private static uint _deger;
            public int ID { get { return _ID; } }
            public uint Deger { get { return _deger; } set { _deger = value; } }

        }
    }
}
