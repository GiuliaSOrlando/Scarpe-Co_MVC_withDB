using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scarpe_Co_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString;
        List<Prodotto> listaprodotti = new List<Prodotto>();
        public HomeController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ScarpeAndCoDB"].ConnectionString;
        }
        public ActionResult Index()
        {
            listaprodotti.Clear();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryVisualizzaProd = new SqlCommand("SELECT * FROM Prodotti WHERE Disponibile = 1", conn);
                SqlDataReader readerProdotti = queryVisualizzaProd.ExecuteReader();
                while (readerProdotti.Read())
                {
                    Prodotto prodotto = new Prodotto()
                    {
                        IdProdotto = (int)readerProdotti["IdProdotto"],
                        Nome = readerProdotti["Nome"].ToString(),
                        Prezzo = Convert.ToDecimal(readerProdotti["Prezzo"]),
                        Descrizione = readerProdotti["Descrizione"].ToString(),
                        CoverImg = readerProdotti["CoverImg"].ToString(),
                        Img1 = readerProdotti["Img1"].ToString(),
                        Img2 = readerProdotti["Img2"].ToString(),
                        Disponibile = Convert.ToBoolean(readerProdotti["Disponibile"])
                    };
                    listaprodotti.Add(prodotto);
                }
                readerProdotti.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();

            }
            return View(listaprodotti);
        }

        public ActionResult Details(int id)
        {
            Prodotto prodottodamodificare = null;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryProdID = new SqlCommand("SELECT * FROM Prodotti WHERE IdProdotto = @Id", conn);
                queryProdID.Parameters.AddWithValue("@Id", id);
                SqlDataReader readerProdotto = queryProdID.ExecuteReader();
                if (readerProdotto.Read())
                {
                    prodottodamodificare = new Prodotto()
                    {
                        IdProdotto = (int)readerProdotto["IdProdotto"],
                        Nome = readerProdotto["Nome"].ToString(),
                        Prezzo = Convert.ToDecimal(readerProdotto["Prezzo"]),
                        Descrizione = readerProdotto["Descrizione"].ToString(),
                        CoverImg = readerProdotto["CoverImg"].ToString(),
                        Img1 = readerProdotto["Img1"].ToString(),
                        Img2 = readerProdotto["Img2"].ToString(),
                        Disponibile = Convert.ToBoolean(readerProdotto["Disponibile"])
                    };
                }
            }
            finally
            {
                conn.Close();
            }

            if (prodottodamodificare == null)
            {
                return HttpNotFound();
            }
            return View("Details", prodottodamodificare);
        }
        [Authorize(Users = "admin")]

        public ActionResult AdminIndex()
        {
            listaprodotti.Clear();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand queryVisualizzaProd = new SqlCommand("SELECT * FROM Prodotti", conn);
                SqlDataReader readerProdotti = queryVisualizzaProd.ExecuteReader();
                while (readerProdotti.Read())
                {
                    Prodotto prodotto = new Prodotto()
                    {
                        IdProdotto = (int)readerProdotti["IdProdotto"],
                        Nome = readerProdotti["Nome"].ToString(),
                        Prezzo = Convert.ToDecimal(readerProdotti["Prezzo"]),
                        Descrizione = readerProdotti["Descrizione"].ToString(),
                        CoverImg = readerProdotti["CoverImg"].ToString(),
                        Img1 = readerProdotti["Img1"].ToString(),
                        Img2 = readerProdotti["Img2"].ToString(),
                        Disponibile = Convert.ToBoolean(readerProdotti["Disponibile"])
                    };
                    listaprodotti.Add(prodotto);
                }
                readerProdotti.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();

            }
            return View(listaprodotti);
        }
        

        private string SaveImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(Server.MapPath("/Content/FileCaricati/"), fileName);

                file.SaveAs(filePath);
                return "/Content/FileCaricati/" + fileName;
            }
            return null;
        }

        [HttpPost]
        public ActionResult Edit(Prodotto prodotto, HttpPostedFileBase coverImg, HttpPostedFileBase img1, HttpPostedFileBase img2)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                try {
                    conn.Open();
                    string coverImgPath = SaveImage(coverImg);
                    string img1Path = SaveImage(img1);
                    string img2Path = SaveImage(img2);

                    SqlCommand queryUpdateProd = new SqlCommand("UPDATE Prodotti SET Nome = @Nome, Prezzo = @Prezzo, Descrizione = @Descrizione, CoverImg = @CoverImg, Img1 = @Img1, Img2 = @Img2, Disponibile = @Disponibile WHERE IdProdotto = @IdProdotto", conn);
                    queryUpdateProd.Parameters.AddWithValue("@IdProdotto", prodotto.IdProdotto);
                    queryUpdateProd.Parameters.AddWithValue("@Nome", prodotto.Nome);
                    queryUpdateProd.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);
                    queryUpdateProd.Parameters.AddWithValue("@Descrizione", prodotto.Descrizione);
                    queryUpdateProd.Parameters.AddWithValue("@CoverImg", string.IsNullOrEmpty(coverImgPath) ? prodotto.CoverImg : coverImgPath);
                    queryUpdateProd.Parameters.AddWithValue("@Img1", string.IsNullOrEmpty(img1Path) ? prodotto.Img1 : img1Path);
                    queryUpdateProd.Parameters.AddWithValue("@Img2", string.IsNullOrEmpty(img2Path) ? prodotto.Img2 : img2Path);
                    queryUpdateProd.Parameters.AddWithValue("@Disponibile", prodotto.Disponibile);

                    queryUpdateProd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }

                return RedirectToAction("Index");
            }

            return View(prodotto);


            }

        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

    public ActionResult Create(Prodotto prodotto, HttpPostedFileBase coverImg, HttpPostedFileBase img1, HttpPostedFileBase img2)
    {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                string coverImgPath = SaveImage(coverImg);
                string img1Path = SaveImage(img1);
                string img2Path = SaveImage(img2);
                {
                    conn.Open();
                    SqlCommand queryInsertProd = new SqlCommand("INSERT INTO Prodotti (Nome, Prezzo, Descrizione, CoverImg, Img1, Img2) VALUES (@Nome, @Prezzo, @Descrizione, @CoverImg, @Img1, @Img2)", conn);
                    {
                        queryInsertProd.Parameters.AddWithValue("@Nome", prodotto.Nome);
                        queryInsertProd.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);
                        queryInsertProd.Parameters.AddWithValue("@Descrizione", prodotto.Descrizione);
                        queryInsertProd.Parameters.AddWithValue("@CoverImg", coverImgPath);
                        queryInsertProd.Parameters.AddWithValue("@Img1", img1Path);
                        queryInsertProd.Parameters.AddWithValue("@Img2", img2Path);

                        queryInsertProd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }
            

        return View(prodotto);
    }
}
}