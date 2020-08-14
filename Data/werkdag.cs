using System;

namespace Bezetting2.Data
{
    [Serializable]
    public class werkdag
    {
        public string  _naam { get; set; }
        public string _standaarddienst { get; set; }
        public string _afwijkingdienst { get; set; }
        public string _werkplek { get; set; }
        public int _dagnummer { get; set; }
    }
}
