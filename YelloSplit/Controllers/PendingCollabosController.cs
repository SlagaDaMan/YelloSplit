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
    public class PendingCollabosController : Controller
    {
        public IActionResult Index()
        {
            var EntityID = Request.Cookies["ID"].ToString();
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Select * from App_Users Where ID = " + EntityID);
            var CategoryID = varUser.Rows[0][1].ToString();

            DataTable varCollab = new DataTable();
            varCollab = ex.ExecuteQueryFunction("Select ACL.ID, AC.FileDirectoryID as Audio,CT.Description as Category,SCT.Description as SubCategory,U.FirstName + ' ' + U.LastName as MasterOwner, AC.ShortDescription,(SELECT FirstName + ' ' + LastName as CollaboName  from App_Users  Where ID = ACL.LinkedUserID ) CollaboName  from [dbo].[App_Collaborations_Linked] ACL" +
                                                " LEFT JOIN[dbo].[App_Collaborations] AC ON AC.ID = ACL.CollaboID" +
                                                " LEFT JOIN Lk_SubCategoryTypes SCT ON SCT.ID = AC.SubCategoryID" +
                                                " Left Join Lk_CategoryTypes CT ON  CT.ID = AC.CategoryID" +
                                                " LEFT JOIN App_Users U ON U.ID = AC.UserID" +
                                                " Where AC.APPTypeID = " + CategoryID + " AND ACL.LinkedUserID = " + EntityID + "  AND ACL.StatusID <> 3 ");

            DataTable varCollabMessages = new DataTable();
            varCollabMessages = ex.ExecuteQueryFunction("Select AC.FileDirectoryID as Audio," +
                                                       " CT.Description as Category, " +
                                                       " SCT.Description as SubCategory, U.FirstName + ' ' + U.LastName as MasterOwner, AC.ShortDescription," +
                                                       " (Select FirstName + ' ' + LastName from App_Users Where ID = ACL.LinkedUserID ) as CollaboName , " +
                                                       " ACL.NewUpload as ResponseAudio" +
                                                       " from[dbo].[App_Collaborations_Linked] ACL" +
                                                       " LEFT JOIN[dbo].[App_Collaborations] AC ON AC.ID = ACL.CollaboID" +
                                                       " LEFT JOIN Lk_SubCategoryTypes SCT ON SCT.ID = AC.SubCategoryID" +
                                                       " Left Join Lk_CategoryTypes CT ON CT.ID = AC.CategoryID" +
                                                       " LEFT JOIN App_Users U ON U.ID = AC.UserID Where AC.APPTypeID = " + CategoryID + " AND AC.UserID = " + EntityID + "  AND ACL.StatusID = 2 ");
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
                               Messages= varMessages.ToString(),
                               varCollaborations = (from DataRow row2 in varCollab.Rows
                                                    select new Collaborations
                                                    {
                                                        ID = Convert.ToInt32(row2["ID"].ToString()),
                                                        Category = row2["Category"].ToString(),
                                                        SubCategory = row2["SubCategory"].ToString(),
                                                        Name = row2["MasterOwner"].ToString(),
                                                        Audio = row2["Audio"].ToString(),
                                                        Description = row2["ShortDescription"].ToString(),
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

        public IActionResult Completed()
        {
            var EntityID = Request.Cookies["ID"].ToString();
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Select * from App_Users Where ID = " + EntityID);
            var CategoryID = varUser.Rows[0][1].ToString();

            DataTable varCollab = new DataTable();
            varCollab = ex.ExecuteQueryFunction("Select ACL.ID, AC.FileDirectoryID as Audio,CT.Description as Category,SCT.Description as SubCategory,U.FirstName + ' ' + U.LastName as MasterOwner, AC.ShortDescription,(SELECT FirstName + ' ' + LastName as CollaboName  from App_Users  Where ID = ACL.LinkedUserID ) CollaboName, ACL.NewUpload as ResponseAudio  from [dbo].[App_Collaborations_Linked] ACL" +
                                                " LEFT JOIN[dbo].[App_Collaborations] AC ON AC.ID = ACL.CollaboID" +
                                                " LEFT JOIN Lk_SubCategoryTypes SCT ON SCT.ID = AC.SubCategoryID" +
                                                " Left Join Lk_CategoryTypes CT ON  CT.ID = AC.CategoryID" +
                                                " LEFT JOIN App_Users U ON U.ID = AC.UserID" +
                                                " Where AC.APPTypeID = " + CategoryID + " AND AC.UserID = " + EntityID + "  AND ACL.StatusID = 3 ");
            var varMessages = varCollab.Rows.Count;
            DataTable varCategories = new DataTable();
            varCategories = ex.ExecuteQueryFunction("Select Description from Lk_CategoryTypes Where APPID =  " + CategoryID);

            DataTable varPending = new DataTable();
            varPending = ex.ExecuteQueryFunction("Select Count(CollaboID) as Pending from [dbo].[App_Collaborations_Linked] Where LinkedUserID  = " + EntityID + " AND StatusID = 3");

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
                                                        Name = row2["MasterOwner"].ToString(),
                                                        Audio = row2["Audio"].ToString(),
                                                        Description = row2["ShortDescription"].ToString(),
                                                        ResposeAudio = row2["ResponseAudio"].ToString(),
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

        public IActionResult submit(string ID, string audio)
        {
            UsersQueries ex = new UsersQueries();
            DataTable varUser = new DataTable();
            varUser = ex.ExecuteQueryFunction("Update [dbo].[App_Collaborations_Linked] SET [NewUpload] = '" + audio + "', StatusID = 3 Where ID = " + ID);
            return RedirectToAction("Index", "Collaboration");
        }

        }
}
