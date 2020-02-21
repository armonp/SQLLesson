using System;
using System.Collections.Generic;
using System.Text;

namespace SqlLibrary {
    class Instructor {
        public int Id  { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int YearsExp { get; set; }
        public bool isTenured { get; set; }

        public override string ToString() {
            return $"{Id} | {Firstname} {Lastname} | {YearsExp} | {isTenured}";
        }
    }
}
