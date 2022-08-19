using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HaberlesmeGulsah.Modellerim;

namespace HaberlesmeGulsah
{
    internal class PilClass
    {
        private uint _currentPilYuzdesi = 101;
        private uint _targetPilYuzdesi = 85;

        PilModel model = new PilModel();

        private void PilAyarla()
        {
            if (_currentPilYuzdesi != _targetPilYuzdesi)
            {
                _currentPilYuzdesi = _targetPilYuzdesi;
            }
            model.Deger = _currentPilYuzdesi;
        }

        public void PilYuzdesiDegistir(uint yeniPilDeger)
        {
            _targetPilYuzdesi = yeniPilDeger;
            PilAyarla();
        }
    }
}
