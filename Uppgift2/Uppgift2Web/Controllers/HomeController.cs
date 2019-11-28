using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Uppgift2Web.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            User spUser = null;

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.Title);

                    Web myWeb = clientContext.Web;
                    clientContext.Load(myWeb);

                    List myList = myWeb.Lists.GetByTitle("Uppgift2Lista");
                    clientContext.Load(myList);

                    var webListsTitles = myWeb.Lists;
                    clientContext.Load(webListsTitles);

                    CamlQuery query = CamlQuery.CreateAllItemsQuery(100);
                    ListItemCollection items = myList.GetItems(query);
                    clientContext.Load(items);

                    int counter = 0;
                    int counter2 = 0;
                    int counter3 = 0; 
                        
                    List<string> Titles = new List<string>();

                    List<string> WebTitles = new List<string>();
                   

                    clientContext.ExecuteQuery();

                    foreach (var lists in webListsTitles)
                    {
                        WebTitles.Add(lists.Title);
                    }

                    foreach (var item in items)
                    {

                        if (item["Status"].ToString() == "Started" && item["Priority"].ToString() == "Hög")
                        {
                            Titles.Add(item["Title"].ToString());
                        }
                        if (item["Priority"].ToString() == "Hög" && item["Status"].ToString() == "Created")
                        {
                            counter++;
                        }
                        if (item["Priority"].ToString() == "Medel" || item["Priority"].ToString() == "Låg"
                            && (item["Status"].ToString() == "Created" || item["Status"].ToString() == "Started"))
                        {
                            counter2++;
                        }
                        if (item["Priority"].ToString() != "Hög" && item["Status"].ToString() != "Finished")
                        {
                            counter3++;
                        }

                    }

                    ViewBag.UserName = spUser.Title;

                    ViewBag.WebTitle = myWeb.Title;

                    ViewBag.MyList = myList.Title;

                    ViewBag.ItemTitle = Titles;

                    ViewBag.ListTitles = WebTitles;

                    ViewBag.Counter = counter;

                    ViewBag.Counter2 = counter2;
                    ViewBag.Counter3 = counter3;
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
