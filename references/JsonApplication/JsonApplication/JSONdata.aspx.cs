using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;


namespace JsonApplication
{
    public partial class JSONdata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Option"] == null)
                return;

            string cmd = Request["Option"].ToString();
            if (cmd == "GetListOfCustomers")
            {
                Response.Clear();
                Response.Write(GetListOfCustomers());
                Response.End();
            }
            if (cmd == "GetOrdersForCustomers")
            {
                string customerID = Request["CustomerID"].ToString();
                Response.Clear();
                Response.Write(GetListOfOrdersForCustomer(customerID));
                Response.End();
            }
        }
        public string GetListOfCustomers()
        {
            // Load the list of Customer IDs and Names from the SQL Server database from the [Customer] table.
            //
            NorthwindDataContext dc = new NorthwindDataContext();

            // First, we load the records from the database.
            var listOfCustomers = dc.Customers.OrderBy(s => s.CompanyName).ToList();

            // Then, we build a generic variable containing the parts of the database table which we're interested in.
            var results = new
            {
                NumberOfCustomers = listOfCustomers.Count(),
                Customers = from cust in listOfCustomers
                            select new
                            {
                                CustomerID = cust.CustomerID,
                                CompanyName = cust.CompanyName
                            }
            };

            // And finally, we get JSON.Net to convert it into a JSON string for us.
            string json = JsonConvert.SerializeObject(results);
            return json;
        }

        public string GetListOfOrdersForCustomer(string customerID)
        {
            // Load the list of Orders for a particular [Customer] ID from the SQL Server database
            //  
            NorthwindDataContext dc = new NorthwindDataContext();

            // First, we'll load the hierarchical data (Orders \ Order_Details and Products) from the database..

            var listOfOrders = from order in dc.Orders
                          where (order.CustomerID == customerID)
                          orderby order.OrderDate
                          select new
                          {
                              OrderID = order.OrderID,
                              OrderDate = order.OrderDate,
                              Basket = from od in order.Order_Details
                                       join product in dc.Products
                                       on od.ProductID equals product.ProductID
                                       where od.OrderID == order.OrderID
                                       select new
                                       {
                                           ProductID = od.ProductID,
                                           ProductName = product.ProductName,
                                           Quantity = od.Quantity
                                       }
                          };

            var results = new
            {
                NumberOfOrders = listOfOrders.Count(),
                Orders = listOfOrders
            };
            // Then we'll get JSON.Net to convert it into a JSON string for us.
            string json = JsonConvert.SerializeObject(results);
            return json;
        }
    }
}