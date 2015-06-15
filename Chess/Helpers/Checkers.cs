using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chess.Helpers
{
    public class Checkers
    {
    }

    public abstract class Piece
    {
        private properties p = new properties();
        public string name { get; set; }
        public string color { get; set; }

        public abstract bool isLegitMovement(Move move);


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
        public name getName { get { return n; } }

        public String name { get; set; }
        public String color { get; set; }
        public Int32 row { get; set; }
        public Int32 col { get; set; }
        public String id { get; set; }
        public Boolean isStart { get; set; }

        name n = new name();

        //public properties()
        //{

        //}

        public properties(string color)
        {
            this.color = color;
        }

    }


    public class name
    {
        public String crown { get { return "crown"; } }
        public String pawn { get { return "pawn"; } }
    }


    public class Move
    {
        public String id { get; set; }
        public Int32 row { get; set; }
        public Int32 col { get; set; }
        public Piece piece { get; set; }
        
        public Move()
        {

        }

        public Move(int row, int column, Piece piece)
        {
            this.row = row;
            this.col = column;
            this.piece = piece;
        }

    }
}