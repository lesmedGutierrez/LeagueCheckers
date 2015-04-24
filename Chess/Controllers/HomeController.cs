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
        public static chessModel listPieces = new chessModel();
        public static List<ColRowHistory> coorhistory = new List<ColRowHistory>();

        public class cor
        {
            public int col { get; set; }
            public int row { get; set; }
        }

        public ActionResult Index()
        {
            //ilk önce siyah taşları diz
            listPieces.pieces.Clear();

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
                        Pieces.pawn.id = j.ToString()+i.ToString()+ color + Pieces.pawn.name;
                        Pieces.pawn.row = x1;
                        Pieces.pawn.color = color;
                        listPieces.pieces.Add(Pieces.pawn);
                        control = false;
                    }
                    else
                    {
                        control = true;
                    }
                }
                x1++;
                
            }
            /*
            foreach (var pieces in listPieces.pieces)
            {
                coorhistory.Add(new ColRowHistory
                {
                    id = pieces.id,
                    col = pieces.col,
                    row = pieces.row
                });
            }
            */
            return View(listPieces);
        }
        
        public String OControl(moveC move, properties piece, Int32? stepSize, String Direction)
        {

            String removeID = "";//silinecek taşın id si
            String crownID = "";
            Boolean isMoveable = false;//taş hareket edebilirmi
            List<cor> se = new List<cor>();
            List<properties> ps = new List<properties>();
            var item = new cor();
            int x = piece.row;
            int y = piece.col;
            switch (Direction)
            {
                case "cross":
                    {
                        if (stepSize == null)
                        {
                            stepSize = Math.Abs(move.col - piece.col);
                        }
                        else
                        {
                            if (Math.Abs(piece.row - move.row) == 2 || Math.Abs(piece.col - move.col) == 2)
                            {
                                properties eaten = isEatable(move, piece);
                                if (eaten==null){
                                    //removeID = eaten.id;
                                    break;
                                }
                                else
                                {
                                    if (eaten.color != piece.color)
                                    {
                                        string id = eaten.id;
                                        removeID = id;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else if (Math.Abs(piece.row - move.row) != 1 || Math.Abs(piece.col - move.col) != 1)
                            {
                                break;
                            }
                        }
                        
                        if (isEmpty(move)!=null) 
                        {
                            removeID = "";
                            break;
                        }

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
                        
                        /*if(piece.color == "White" && move.row == 1 && piece.name == "pawn")
                        {
                            pieces Pieces = new pieces();//newCrown
                            Pieces.crown.col = move.col;
                            Pieces.crown.id = piece.id;
                            Pieces.crown.row = move.col;
                            Pieces.crown.color = "White";
                            listPieces.removePiece(piece.id);
                            listPieces.pieces.Add(Pieces.crown);
                            crownID = piece.id;
                        }
                        else if (piece.color == "Black" && move.row == 8 && piece.name == "pawn")
                        {
                            pieces Pieces = new pieces();//newCrown
                            Pieces.crown.col = move.col;
                            Pieces.crown.id = piece.id;
                            Pieces.crown.row = move.col;
                            Pieces.crown.color = "Black";
                            listPieces.removePiece(piece.id);
                            listPieces.pieces.Add(Pieces.crown);
                            crownID = piece.id;
                        }*/
                        pieces Pieces = new pieces();//newCrown
                        Pieces.crown.col = move.col;
                        Pieces.crown.id = piece.id;
                        Pieces.crown.row = move.row;
                        Pieces.crown.color = piece.color;
                        listPieces.removePiece(piece.id);
                        listPieces.pieces.Add(Pieces.crown);
                        crownID = piece.id;
                        break;
                    }

                case "crown":
                    {
                        int xDif = Math.Abs(piece.row - move.row);
                        int yDif = Math.Abs(piece.col - move.col);
                        if (Math.Abs(piece.row - move.row) == Math.Abs(piece.col - move.col))
                        {
                            isMoveable = true;
                        }
                        break;
                    }
               
            }
            
            foreach (var direct in se)//hareket ettiği eksende hangi taşlar var
            {
                foreach (var pc in listPieces.pieces)
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
                    { isMoveable = true;}
                    else
                    {
                        
                    }
                }

                if (isMoveable == true)//hareket kabul edildiyse
                {
                    piece.col = move.col;//kalenin yeni sutun değerini güncelle
                    piece.row = move.row;//kalenin yeni satır değeri güncelle
                    piece.isStart = true;// ilk hareket aktif et
                }
            }
            listPieces.removePiece(removeID);
            
            return isMoveable + ";" + removeID + ";" + crownID;
        }

        
        public JsonResult moveControl(moveC move)
        {
            var piece = (from pc in listPieces.pieces where pc.id == move.id select pc).FirstOrDefault();
            string result = "", Direction = "";

            Direction = (piece.row == move.row || piece.col == move.col) ? "direct" : "cross";

            switch (piece.name)
            {
                    case "pawn"://
                    {
                        int stepSize = piece.isStart == true ? 1 : 2;//ilk hareketinde 2 tane adım atabilir
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

        properties isEmpty(moveC move)
        {
            foreach (properties pc in listPieces.pieces)
            {
                if (move.col == pc.col && move.row == pc.row)
                {
                    return pc;
                }
            }
            return null;
            
        }
        properties isEatable(moveC move, properties piece)
        {
            int rowTemp, colTemp;
            rowTemp = eatable_index(piece.row, move.row);
            colTemp = eatable_index(piece.col, move.col);
            moveC move_piece = new moveC(rowTemp, colTemp);
            properties isEmp = isEmpty(move_piece);
            if (isEmp != null)
            {
                if (isEmp.color != piece.color)
                {
                    //isEmp = null;
                    return isEmp;
                }
            }
            return isEmp ;

            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="dir">1 arriba-derecha</param>
        /// <param name="campo"></param>
        /// <returns></returns>
        bool valIndexces(int row, int col, int dir, int campo)
        {
            if (row - 2 < 1 || col - 2 < 1 || row + 2 > 8 || col + 2 > 8)
            {
                return false;
            }
            return true;
        }
        int eatable_index(int v, int m)
        {
            if (m > v)
            {
                return m - 1;
            }
            else if (m < v)
            {
                return m + 1;

            }
            return 0;

        }
    }
}