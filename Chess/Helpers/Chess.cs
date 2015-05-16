using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chess.Helpers
{
    public class Chess
    {
    }

    public class pieces
    {
        properties p = new properties();

        public properties crown
        {
            get
            {
                p.name = p.getName.crown;
                return p;
            }
        }

        public properties pawn
        {
            get
            {
                p.name = p.getName.pawn;
                return p;
            }
        }
    }

    public class properties
    {
        name n = new name();
        color c = new color();

        public properties()
        {

        }


        public properties(string color)
        {
            this.color = color;

        }




        public name getName { get { return n; } }
        public String name { get; set; }
        public color getColor { get { return c; } }
        public String color { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public String id { get; set; }
        public Boolean isStart { get; set; }
    }

    public class color
    {
        public String black { get { return "Black"; } }
        public String white { get { return "White"; } }
    }

    public class name
    {
        public String crown { get { return "crown"; } }
        public String pawn { get { return "pawn"; } }
    }


    public class moveC
    {
        public String id { get; set; }
        public String target { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public moveC()
        {

        }

        public moveC(int fila, int columna)
        {
            row = fila;
            col = columna;
        }

    }
}