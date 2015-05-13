using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chess.Helpers;

namespace Chess.Models
{
    public class Board
    {
        public List<properties> pieces = new List<properties>();
        public string currentColorTurn = "";
        public const int SIZE = 8;



        public bool canJump(properties piece)
        {
            return piece.name == "pawn" ? canPawnJump(piece) : canCrownJump(piece);
        }



        private bool canPawnJump(properties piece)
        {   
            int row = piece.row;
            int col = piece.col;

            if (currentColorTurn == "White")
            {

                if (piece.row > 1 && piece.col > 1 &&
                   getPiece(row - 1, col - 1).color == "Black" && getPiece(row - 2, col - 2).color == "Blank") // check jump to the left
                {
                    return true;
                }
                if (col < 6 && row > 1 &&
                   getPiece(row - 1, col + 1).color == "Black" && getPiece(row - 2, col + 2).color == "Blank") // check jump to the right
                {
                    return true;
                }
            }
            else if (currentColorTurn == "Black")
            {
                if (col > 1 && row < 6 &&
                   getPiece(row + 1, col - 1).color == "White" && getPiece(row + 2, col - 2).color == "Blank") // check jump to the left
                {
                    return true;
                }
                if (col < 6 && row < 6 &&
                   getPiece(row + 1, col + 1).color == "White" && getPiece(row + 2, col + 2).color == "Blank") // check jump to the right
                {
                    return true;
                }
            }
            return false;
        }


        private bool canCrownJump(properties piece)
        {
            int col = piece.col;
            int row = piece.row;

            string opponent;
            if (currentColorTurn == "White")
            {
                opponent = "Black";
            }
            else 
            {
                opponent = "White";
            }
            if (col > 1 && row > 1 &&
               getPiece(row - 1, col - 1).color == opponent && getPiece(row - 2, col - 2).color == "Blank") // check jump to the left
            {
                return true;
            }
            if (col < 6 && row > 1 &&
               getPiece(row - 1, col + 1).color == opponent && getPiece(row - 2, col + 2).color == "Blank") // check jump to the right
            {
                return true;
            }
            if (col > 1 && row < 6 &&
               getPiece(row + 1, col - 1).color == "White" && getPiece(row + 2, col - 2).color == "Bank") // check jump to the left
            {
                return true;
            }
            if (col < 6 && row < 6 &&
               getPiece(row + 1, col + 1).color == "White" && getPiece(row + 2, col + 2).color == "Blank") // check jump to the right
            {
                return true;
            }
            return false;
        }


        public bool hasJump()
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                string pieceColor = pieces[i].color;

                if (pieceColor == currentColorTurn)
                {
                    if (canJump(pieces[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }



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

        public properties getPiece(int row, int col)
        {
            if (row < 8 && col < 8)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i].row == row && pieces[i].col == col)
                    {
                        return pieces[i];
                    }
                }
                return new properties("Blank");
            }
            return new properties("");
        }




        public properties getEatenPiece(properties piece, moveC move)
        {
            //Going to top. 
            if (move.row < piece.row)
            {
                //If Diagonal to the right do.. else...
                return move.col > piece.col ? getPiece(move.row+1,move.col-1):getPiece(move.row+1,move.col+1);
            }
            //Going to bottom.
            else
            {
                //If Diagonal to the right  do.. else.. 
                return move.col > piece.col ? getPiece(move.row -1, move.col -1) : getPiece(move.row -1, move.col + 1);

            }
        }

        public bool emptyCell(int row, int col)
        {
            string p = getPiece(row, col).color;

            return getPiece(row, col).color == "Blank" ? true : false;
        }
    }
}