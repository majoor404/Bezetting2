﻿using System;

namespace Bezetting2.Data
{
    [Serializable]
    public class SnipperAanvraag
    {
        public string _naam { get; set; }
        public string _persnr { get; set; }
        public DateTime _datum { get; set; }
        public string _hoe { get; set; }
        public string _rede { get; set; }
        public string _dienst { get; set; }
        public string _kleur { get; set; }
        public string _Coorcinator { get; set; }
        public string _rede_coordinator { get; set; }
    }
}