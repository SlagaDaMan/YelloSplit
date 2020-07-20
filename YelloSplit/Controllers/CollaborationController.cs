using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YelloSplit.Helpers;
using YelloSplit.Models;

namespace YelloSplit.Controllers
{
    public class CollaborationController : Controller
    {
        public IActionResult Index()
        {
            var EntityID = Request.Cookies["ID"].ToString();
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Select * from App_Users Where ID = " + EntityID);
            var CategoryID = varUser.Rows[0][1].ToString();

            DataTable varCollab = new DataTable();
            varCollab = ex.ExecuteQueryFunction("Select CT.Description as Category,SC.Description as SubCategory,U.FirstName + ' ' + U.LastName as Name, C.FileDirectoryID as Audio,C.ShortDescription as Description  from App_Collaborations C " +
                                                "Left Join Lk_CategoryTypes CT ON  CT.ID = C.CategoryID" +
                                                " Left Join Lk_SubCategoryTypes SC ON SC.ID = C.SubCategoryID" +
                                                " LEFT JOIN App_Users U ON U.ID = C.UserID" +
                                                " Where C.APPTypeID = " + CategoryID + ";");

            UserDetails userDetails = new UserDetails();
            Collaborations collabs = new Collaborations();

            userDetails = (from DataRow row in varUser.Rows
                           select new UserDetails
                           {
                            Name = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Credits = row["Credits"].ToString(),
                            varCollaborations = (from DataRow row2 in varCollab.Rows
                                                 select new Collaborations
                                                 {
                                                     Category = row2["Category"].ToString(),
                                                     SubCategory = row2["SubCategory"].ToString(),
                                                     Name = row2["Name"].ToString(),
                                                     Audio = row2["Audio"].ToString(),
                                                     Description = row2["Description"].ToString(),
                                                 }).ToList(),
                           }).FirstOrDefault();
            return View(new List<UserDetails> { userDetails });
        }

        public IActionResult UserCollaborations()
        {
            var EntityID = Request.Cookies["ID"].ToString();
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Select * from App_Users Where ID = " + EntityID);
            var CategoryID = varUser.Rows[0][1].ToString();

            DataTable varCollab = new DataTable();
            varCollab = ex.ExecuteQueryFunction("Select C.ID, CT.Description as Category,SC.Description as SubCategory,U.FirstName + ' ' + U.LastName as Name, C.FileDirectoryID as Audio,C.ShortDescription as Description  from App_Collaborations C " +
                                                "Left Join Lk_CategoryTypes CT ON  CT.ID = C.CategoryID" +
                                                " Left Join Lk_SubCategoryTypes SC ON SC.ID = C.SubCategoryID" +
                                                " LEFT JOIN App_Users U ON U.ID = C.UserID" +
                                                " Where C.APPTypeID = " + CategoryID + " AND UserID = " + EntityID + ";");

            UserDetails userDetails = new UserDetails();
            Collaborations collabs = new Collaborations();

            userDetails = (from DataRow row in varUser.Rows
                           select new UserDetails
                           {
                               Name = row["FirstName"].ToString(),
                               LastName = row["LastName"].ToString(),
                               Credits = row["Credits"].ToString(),
                               varCollaborations = (from DataRow row2 in varCollab.Rows
                                                    select new Collaborations
                                                    {
                                                        ID = Convert.ToInt32(row2["ID"].ToString()),
                                                        Category = row2["Category"].ToString(),
                                                        SubCategory = row2["SubCategory"].ToString(),
                                                        Name = row2["Name"].ToString(),
                                                        Audio = row2["Audio"].ToString(),
                                                        Description = row2["Description"].ToString(),

                                                    }).ToList(),
                           }).FirstOrDefault();
            return View(new List<UserDetails> { userDetails });
          
        }

        public IActionResult Create()
        {
            var EntityID = Request.Cookies["ID"].ToString();
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Select * from App_Users Where ID = " + EntityID);
            var CategoryID = varUser.Rows[0][1].ToString();

            DataTable Category = new DataTable();
            Category = ex.ExecuteQueryFunction("Select * from Lk_CategoryTypes Where APPID = " + CategoryID + ";");
            //var varCategoryID
            DataTable SubCategory = new DataTable();
            SubCategory = ex.ExecuteQueryFunction("Select * from Lk_SubCategoryTypes Where APPID = " + CategoryID + ";");
            Upload UploadData = new Upload();
            Collaborations collabs = new Collaborations();

            UploadData = (from DataRow row in varUser.Rows
                           select new Upload
                           {
                               Name = row["FirstName"].ToString(),
                               LastName = row["LastName"].ToString(),
                               Credits = row["Credits"].ToString(),
                               varCategory = (from DataRow row2 in Category.Rows
                                                    select new Category
                                                    {
                                                        Description = row2["Description"].ToString(),
                                                    }).ToList(),
                               varSubCategory = (from DataRow row2 in SubCategory.Rows
                                              select new SubCategory
                                              {
                                                  Description = row2["Description"].ToString(),
                                              }).ToList(),
                           }).FirstOrDefault();

            return View(new List<Upload> { UploadData });
        }

        public IActionResult Add(string Category, string SubCategory, string Name, string Description, string Audio)
        {
            var EntityID = Request.Cookies["ID"].ToString();
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Select * from App_Users Where ID = " + EntityID);
            var AppCategoryID = varUser.Rows[0][1].ToString();

            DataTable Categorydata = new DataTable();
            Categorydata = ex.ExecuteQueryFunction("Select * from Lk_CategoryTypes Where Description = '" + Category + "';");
            var varCategoryID = Categorydata.Rows[0][0].ToString();

            DataTable SubCategorydata = new DataTable();
            SubCategorydata = ex.ExecuteQueryFunction("Select * from Lk_SubCategoryTypes Where Description = '" + SubCategory + "';");
            var varSubCategoryID = SubCategorydata.Rows[0][0].ToString();
            var CreatedDate = DateTime.Now.ToString();

            ex.ExecuteQueryFunction("Insert into App_Collaborations (CategoryID,SubCategoryID,ShortDescription,FileDirectoryID,UserID,StatusID,APPTypeID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) Values " +
                                    $"({varCategoryID},{varSubCategoryID},'{Description}','{Audio}',{EntityID},1,{AppCategoryID},'{EntityID}','{CreatedDate}','{EntityID}','{CreatedDate}')");
           
            return RedirectToAction("Index");

        }

        public IActionResult Collabo(string ID)
        {
            var EntityID = Request.Cookies["ID"].ToString();
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Select * from App_Users Where ID = " + EntityID);
            var CategoryID = varUser.Rows[0][1].ToString();

            DataTable CoLlaboDetails = new DataTable();
                  
            var CreatedDate = DateTime.Now.ToString();
            ex.ExecuteQueryFunction("Insert into App_Collaborations_Linked (CollaboID,ShortDescription,StatusID,LinkedUserID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) Values " +
                                 $"({ID},{""},{1},'{EntityID}',{EntityID},{LinkedUserID},1,{AppCategoryID},'{EntityID}','{CreatedDate}','{EntityID}','{CreatedDate}')");



            return RedirectToAction("Index");
        }


    }
}
