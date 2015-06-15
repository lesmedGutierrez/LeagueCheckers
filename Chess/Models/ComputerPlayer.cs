using Chess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chess.Models
{
    public class ComputerPlayer
    {
        public moveC moveToDo;
        public properties pieceToMove;
        public properties pieceToRemove;

        public ComputerPlayer()
        {

        }

        public ComputerPlayer(properties piece, moveC move )
        {
            this.pieceToMove = piece;
            this.moveToDo = move;

        }


        public ComputerPlayer(properties piece)
        {
            this.pieceToMove = piece;

        }


        internal void jump(properties pieceToMove)
        {
            throw new NotImplementedException();
        }

        internal void moveRandomPiece(List<properties> list)
        {
            throw new NotImplementedException();
        }
    }
}