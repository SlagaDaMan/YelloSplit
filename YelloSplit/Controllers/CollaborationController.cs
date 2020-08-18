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
            varCollab = ex.ExecuteQueryFunction("Select C.ID, CT.Description as Category,SC.Description as SubCategory,U.FirstName + ' ' + U.LastName as Name, C.FileDirectoryID as Audio,C.ShortDescription as Description  from App_Collaborations C " +
                                                "Left Join Lk_CategoryTypes CT ON  CT.ID = C.CategoryID" +
                                                " Left Join Lk_SubCategoryTypes SC ON SC.ID = C.SubCategoryID" +
                                                " LEFT JOIN App_Users U ON U.ID = C.UserID" +
                                                " Where C.APPTypeID = " + CategoryID + " AND StatusID = 1 AND C.ID NOT IN (Select CollaboID from [dbo].[App_Collaborations_Linked] Where LinkedUserID  = " + EntityID + ");");

            DataTable varCollabMessages = new DataTable();
            varCollabMessages = ex.ExecuteQueryFunction("Select AC.FileDirectoryID as Audio,CT.Description as Category, SCT.Description as SubCategory,U.FirstName + ' ' + U.LastName as MasterOwner, AC.ShortDescription,(SELECT FirstName + ' ' + LastName as CollaboName  from App_Users  Where ID = ACL.LinkedUserID ) CollaboName, ACL.NewUpload as ResponseAudio  from [dbo].[App_Collaborations_Linked] ACL" +
                                                " LEFT JOIN[dbo].[App_Collaborations] AC ON AC.ID = ACL.CollaboID" +
                                                " LEFT JOIN Lk_SubCategoryTypes SCT ON SCT.ID = AC.SubCategoryID" +
                                                " Left Join Lk_CategoryTypes CT ON  CT.ID = AC.CategoryID" +
                                                " LEFT JOIN App_Users U ON U.ID = AC.UserID" +
                                                " Where AC.APPTypeID = " + CategoryID + " AND AC.UserID = " + EntityID + "  AND ACL.StatusID = 2 ");
            var varMessages = varCollabMessages.Rows.Count;

            DataTable varCategories = new DataTable();
            varCategories = ex.ExecuteQueryFunction("Select Description from Lk_CategoryTypes Where APPID =  " + CategoryID);

            DataTable varPending = new DataTable();
            varPending = ex.ExecuteQueryFunction("Select Count(CollaboID) as Pending from [dbo].[App_Collaborations_Linked] Where LinkedUserID  = " + EntityID + " AND StatusID <> 3");

            DataTable varSubCategories = new DataTable();
            varSubCategories = ex.ExecuteQueryFunction("Select Description from Lk_SubCategoryTypes Where APPID =  " + CategoryID);

            UserDetails userDetails = new UserDetails();
            Collaborations collabs = new Collaborations();

            userDetails = (from DataRow row in varUser.Rows
                           select new UserDetails
                           {
                            Name = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Credits = row["Credits"].ToString(),
                            Pending = varPending.Rows[0][0].ToString(),
                            Messages = varMessages.ToString(),
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
                               varCategory = (from DataRow row2 in varCategories.Rows
                                                    select new Category
                                                    {
                                                        Description = row2["Description"].ToString(),
                                                    }).ToList(),
                               varSubCategory = (from DataRow row2 in varSubCategories.Rows
                                              select new SubCategory
                                              {
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
                                 $"({ID},'{""}',{1},'{EntityID}','{EntityID}','{CreatedDate}','{EntityID}','{CreatedDate}')");



            return RedirectToAction("Index");
        }


    }
}
