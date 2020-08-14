using System;

namespace Bezetting2.Data
{
    [Serializable]
    public class veranderingen //: IEquatable<veranderingen>
    {
        public string _naam { get; set; }
        public string _datumafwijking { get; set; }
        public string _afwijking { get; set; }
        public string _datuminvoer { get; set; }
        public string _invoerdoor { get; set; }
        public string _rede { get; set; }
/*
        public bool Equals(veranderingen other)
        {
            return this._naam == other._naam &&
                   this._datumafwijking == other._datumafwijking &&
                   this._afwijking == other._afwijking &&
                   this._datuminvoer == other._datuminvoer &&
                   this._invoerdoor == other._invoerdoor &&
                   this._rede == other._rede;
        }
*/
    }
}
