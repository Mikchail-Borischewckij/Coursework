using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MathСalculator.WebUI.Models;
using MathСalculator.Domain.Methods;
using MathСalculator.Domain.Abstract;
using MathСalculator.Domain.Entities;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.IO;
using System.Security;
using System.Web.UI.WebControls;
using MathСalculator.WebUI.Properties;
using Novacode;

namespace MathСalculator.WebUI.Controllers
{
    public class MethodsController : Controller
    {

        private IUserRepository _userRepos;

        //
        // GET: /Methods/
        [HttpGet, ValidateInput(false)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Error404()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MathCalc()
        {
            return View();
        }


        [HttpGet]
        public ViewResult Presentation(string path)
        {

            ViewBag.Presentat = "http://player.myshared.ru/" + path + "/";
            return View();
        }
        public FileResult GetFile(string path)
        {

            string file_path = Server.MapPath("~/Content/" + path);
            string file_type = "application/msword";
            return File(file_path, file_type, path);
        }

        public FileResult GetFilePPT(string path)
        {

            string file_path = Server.MapPath("~/Content/files/" + path);
            string file_type = "application/ppt";
            return File(file_path, file_type, path);
        }

        public FileResult GetFilePDF(string path)
        {

            string file_path = Server.MapPath("~/Content/files/" + path);
            string file_type = "application/pdf";
            return File(file_path, file_type, path);
        }
        public FileResult Download(string path)
        {
            string actualPath = Server.MapPath("~/Content/" + path);
            return File(actualPath, "application/pdf", Server.UrlEncode(path));
        }

        #region Методы решения СЛАУ

        [HttpGet]
        public ActionResult Gaus()
        {
            ViewBag.flag1 = true;
            return View();
        }

        [HttpPost]
        public ActionResult Gaus(GausModel model1, IList<double> ints)
        {

            try
            {
                if (model1.myArray == null)
                {
                    ViewBag.flag = true;
                    ViewBag.flag1 = false;
                    return View(model1);
                }
                if (ints != null)
                {
                    double[] Matr = {};
                    double[,] Matrix = new double[model1.countRows, model1.countVariable];
                    var count = ints.Count()/model1.countRows;
                    for (int i = 0; i < model1.countRows; i++)
                    {
                        Matr = ints.Skip(i*Matr.Length).Take(count).ToArray();
                        for (int j = 0; j < model1.countVariable; j++)
                        {

                            Matrix[i, j] = Matr[j];
                        }

                    }
                    model1.Matrix = Matrix;
                }
                ViewBag.flag = false;
                ViewBag.answer = true;
                model1.answer = GausResult(model1.Matrix, model1.myArray, model1.countRows);
            }
            catch (Exception)
            {
                    ModelState.AddModelError("",",kf-,kf");
                return View(model1);
            }
            return View(model1);
        }

        [NonAction]
        private double[] GausResult(double[,] matrix, double[] mas, int n)
        {
            var row = (uint) n;
            var answer = new double[row];
            var solution = new GausMethod(row, row);
            for (var i = 0; i < row; i++)
                solution.RightPart[i] = mas[i];

            for (var i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                    solution.Matrix[i][j] = matrix[i, j];
            }
            solution.SolveMatrix();
            for (int i = 0; i < row; i++)
                answer[i] = solution.Answer[i];
            return answer;
        }

        #endregion


        [HttpGet]
        public ViewResult Book()
        {
            return View();
        }


        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Graph()
        {
            return View();
        }
    }
}