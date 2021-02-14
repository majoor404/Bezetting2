using System;

namespace Bezetting2.Data
{
    [Serializable]
    public class LooptExtraDienst : IEquatable<LooptExtraDienst>
    {
        public DateTime _datum { get; set; }
        public string _naam { get; set; }
        public string _metcode { get; set; }

        public bool Equals(LooptExtraDienst other)
        {
            // Would still want to check for null etc. first.
            return this._datum == other._datum &&
                   this._naam == other._naam;// &&
                   //this._metcode == other._metcode;
        }
    }
}