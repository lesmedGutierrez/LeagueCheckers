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

        public class cor
        {
            public int col { get; set; }
            public int row { get; set; }
        }

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
            List<cor> se = new List<cor>();
            List<properties> ps = new List<properties>();
            var item = new cor();
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
                                        string id = pieceToEat.id;
                                        removeID = id;
                                        legitMove = true;
                                    }
                                }
                            }
                            else if (rowStepSize == 1 && colStepSize == 1)
                            {
                                legitMove = true;
                            }

                        }
                        /*if(piece.color == "White" && move.row == 1 && piece.name == "pawn")
                        {
                            pieces Pieces = new pieces();//newCrown
                            Pieces.crown.col = move.col;
                            Pieces.crown.id = piece.id;
                            Pieces.crown.row = move.col;
                            Pieces.crown.color = "White";
                            board.removePiece(piece.id);
                            board.pieces.Add(Pieces.crown);
                            crownID = piece.id;
                        }
                        else if (piece.color == "Black" && move.row == 8 && piece.name == "pawn")
                        {
                            pieces Pieces = new pieces();//newCrown
                            Pieces.crown.col = move.col;
                            Pieces.crown.id = piece.id;
                            Pieces.crown.row = move.col;
                            Pieces.crown.color = "Black";
                            board.removePiece(piece.id);
                            board.pieces.Add(Pieces.crown);
                            crownID = piece.id;
                        }*/
                        /*pieces Pieces = new pieces();//newCrown
                        Pieces.crown.col = move.col;
                        Pieces.crown.id = piece.id;
                        Pieces.crown.row = move.row;
                        Pieces.crown.color = piece.color;
                        board.removePiece(piece.id);
                        board.pieces.Add(Pieces.crown);
                        crownID = piece.id;*/

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
                for (int i = 1; i <= stepSize; i++)
                {
                    if (move.row > piece.row && move.col > piece.col && x + i <= 8 && y + i <= 8)//x+ y+
                    {
                        se.Add(new cor() { col = y + i, row = x + i });
                    }
                    if (move.row < piece.row && move.col < piece.col && x - i >= 1 && y - i >= 1)//x- y-
                    {
                        se.Add(new cor() { col = y - i, row = x - i });
                    }
                    if (move.row > piece.row && move.col < piece.col && x + i <= 8 && y - i >= 1)//x+ y-
                    {
                        se.Add(new cor() { col = y - i, row = x + i });
                    }
                    if (move.row < piece.row && move.col > piece.col && x - i >= 1 && y + i <= 8)//x- y+
                    {
                        se.Add(new cor() { col = y + i, row = x - i });
                    }
                }
                isMoveable = true;
            }

            foreach (var direct in se)
            {
                foreach (var pc in board.pieces)
                {
                    if (direct.row == pc.row && direct.col == pc.col)
                    {
                        ps.Add(pc);
                    }
                }
            }

            if (se.Count > 0)
            {
                if (ps.Count == 0)//önünde taş yoksa hareket edebilir
                {
                    if (!(Direction == "cross" && piece.name == piece.getName.pawn))
                    {
                        isMoveable = true;
                    }
                }

                if (isMoveable == true)//hareket kabul edildiyse
                {
                    piece.col = move.col;//kalenin yeni sutun değerini güncelle
                    piece.row = move.row;//kalenin yeni satır değeri güncelle
                    piece.isStart = true;// ilk hareket aktif et
                }
            }
            board.removePiece(removeID);


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