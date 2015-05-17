using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chess.Helpers;
using Chess.Models;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace Chess.Controllers
{
    public class HomeController : Controller
    {
        public static Board board = new Board();


        public ActionResult Index()
        {
            //ilk önce siyah taşları diz
            board.pieces.Clear();


            int x1 = 1;//black pieces
            String color = "Black";
            bool control = false;
            for (int j = 0; j < 6; j++)
            {
                control = true;
                if (j % 2 == 0)
                {
                    control = false;
                }
                if (j == 3)// ikinci olarak beyaz taşları diz
                {
                    x1 = 6;//7.satıra taşları diz
                    color = "White";
                }

                for (int i = 1; i <= 8; i++)
                {
                    if (control)
                    {
                        pieces Pieces = new pieces();//pawn
                        Pieces.pawn.col = i;
                        Pieces.pawn.id = j.ToString() + i.ToString() + color + Pieces.pawn.name;
                        Pieces.pawn.row = x1;
                        Pieces.pawn.color = color;
                        board.pieces.Add(Pieces.pawn);
                        control = false;
                    }
                    else
                    {
                        control = true;
                    }
                }
                x1++;

            }
            return View(board);
        }

        public String OControl(moveC move, properties piece, Int32? stepSize, String Direction)
        {
            board.currentColorTurn = piece.color;

            String removeID = "";
            String crownID = "";
            Boolean isMoveable = false;

            Boolean legitMove = false;
            
            bool SelectedCellEmpty = board.emptyCell(move.row, move.col);
            bool jumpAvailable = board.hasJump();
            
            int x = piece.row;
            int y = piece.col;

            int rowStepSize = Math.Abs(piece.row - move.row);
            int colStepSize = Math.Abs(piece.col - move.col);
            
            switch (Direction)
            {
                case "cross":
                    {
                        if (SelectedCellEmpty)
                        {
                            if (jumpAvailable)
                            {
                                //Try to eat. 
                                if (rowStepSize == 2 || colStepSize == 2)
                                {
                                    properties pieceToEat = board.getEatenPiece(piece, move);
                                    if ((pieceToEat.color != board.currentColorTurn) && pieceToEat.color != "Blank")
                                    {
                                        removeID = pieceToEat.id;
                                        board.removePiece(removeID);
                                        legitMove = true;
                                    }
                                }
                            }
                            else if (rowStepSize == 1 && colStepSize == 1)
                            {
                                legitMove = true;
                            }

                        }

                        break;
                    }
                case "crown":
                    {
                        if (rowStepSize == colStepSize)
                        {

                        }
                        isMoveable = true;
                        break;
                    }
            }

            if (legitMove)
            {
                isMoveable = true;
                piece.col = move.col;
                piece.row = move.row;
                piece.isStart = true;
            }
            return isMoveable + ";" + removeID + ";" + crownID;
        }



        public JsonResult moveControl(moveC move)
        {
            var piece = (from pc in board.pieces where pc.id == move.id select pc).FirstOrDefault();
            string result = "", Direction = "";

            Direction = (piece.row == move.row || piece.col == move.col) ? "direct" : "cross";

            switch (piece.name)
            {
                case "pawn":
                    {
                        int stepSize = piece.isStart == true ? 1 : 2;
                        Boolean backControl = piece.color == piece.getColor.black ? (piece.row - move.row > 0 ? false : true)
                                                                                  : (piece.row - move.row < 0 ? false : true); //geriye gidebilirmi?
                        result = backControl == true ? OControl(move, piece, stepSize, Direction) : "false;";//geriye gidebilirmi?                                               
                        break;
                    }
                //WIP, working on the moves of the crown
                case "crown":
                    {
                        result = true ? OControl(move, piece, 0, "crown") : "false";
                        break;
                    }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}