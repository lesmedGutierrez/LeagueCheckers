using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chess.Helpers;

namespace Chess.Models
{
    public class chessModel
    {
        public List<properties> pieces = new List<properties>();



        public void removePiece(string id)
        {

            foreach (properties item in pieces)
            {
                if (item.id == id)
                {
                    pieces.Remove(item);
                    break;
                }
            }


        }
    }
}