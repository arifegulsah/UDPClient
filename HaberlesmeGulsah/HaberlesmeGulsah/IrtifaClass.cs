using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HaberlesmeGulsah.Modellerim;

namespace HaberlesmeGulsah
{
    internal class IrtifaClass
    {
        private uint _currentIrtifaDegeri = 101;
        private uint _targetIrtifaDegeri = 85;

        IrtifaModel model = new IrtifaModel();

        private void IrtifaAyarla()
        {
            if (_currentIrtifaDegeri != _targetIrtifaDegeri)
            {
                _currentIrtifaDegeri = _targetIrtifaDegeri;
            }
            model.Deger = _currentIrtifaDegeri;
        }

        public void IrtifaDegeriDegistir(uint yeniIrtifaDegeri)
        {
            _targetIrtifaDegeri=yeniIrtifaDegeri;
            IrtifaAyarla();
        }
    }
}
