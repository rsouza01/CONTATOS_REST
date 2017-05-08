﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace JSONWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public List<wsCustomer> GetAllCustomers()
        {
            try
            {
                NorthwindDataContext dc = new NorthwindDataContext();

                List<wsCustomer> results = new List<wsCustomer>();

                foreach (Customer cust in dc.Customers)
                {
                    results.Add(new wsCustomer()
                    {
                        CustomerID = cust.CustomerID,
                        CompanyName = cust.CompanyName,
                        City = cust.City
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        public List<wsOrder> GetOrdersForCustomer(string customerID)
        {
            try
            {
                NorthwindDataContext dc = new NorthwindDataContext();
                List<wsOrder> results = new List<wsOrder>();
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                foreach (Order order in dc.Orders.Where(s => s.CustomerID == customerID))
                {
                    results.Add(new wsOrder()
                    {
                        OrderID = order.OrderID,
                        OrderDate = (order.OrderDate == null) ? "" : order.OrderDate.Value.ToString("d", ci),
                        ShipAddress = order.ShipAddress,
                        ShipCity = order.ShipCity,
                        ShipName = order.ShipName,
                        ShipPostcode = order.ShipPostalCode,
                        ShippedDate = (order.ShippedDate == null) ? "" : order.ShippedDate.Value.ToString("d", ci)
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        public List<wsOrder> GetOrderDetails(string orderID)
        {
            try
            {
                int orderIDnumber = int.Parse(orderID);

                NorthwindDataContext dc = new NorthwindDataContext();
                List<wsOrder> results = new List<wsOrder>();
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                
                foreach (Order order in dc.Orders.Where(s => s.OrderID == orderIDnumber))
                {
                    results.Add(new wsOrder()
                    {
                        OrderID = order.OrderID,
                        OrderDate = (order.OrderDate == null) ? "" : order.OrderDate.Value.ToString("d", ci),
                        ShipAddress = order.ShipAddress,
                        ShipCity = order.ShipCity,
                        ShipName = order.ShipName,
                        ShipPostcode = order.ShipPostalCode,
                        ShippedDate = (order.ShippedDate == null) ? "" : order.ShippedDate.Value.ToString("d", ci)
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        public List<CustomerOrderHistory> GetCustomerOrderHistory(string customerID)
        {
            try
            {
                List<CustomerOrderHistory> results = new List<CustomerOrderHistory>();
                NorthwindDataContext dc = new NorthwindDataContext();
                foreach (CustOrderHistResult oneOrder in dc.CustOrderHist(customerID))
                {
                    results.Add(new CustomerOrderHistory()
                    {
                        ProductName = oneOrder.ProductName,
                        Total = oneOrder.Total ?? 0
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        public int UpdateOrderAddress(Stream JSONdataStream)
        {
            try
            {
                // Read in our Stream into a string...
                StreamReader reader = new StreamReader(JSONdataStream);
                string JSONdata = reader.ReadToEnd();

                // ..then convert the string into a single "wsOrder" record.
                JavaScriptSerializer jss = new JavaScriptSerializer();
                wsOrder order = jss.Deserialize<wsOrder>(JSONdata);
                if (order == null)
                {
                    // Error: Couldn't deserialize our JSON string into a "wsOrder" object.
                    return -2;
                }

                NorthwindDataContext dc = new NorthwindDataContext();
                Order currentOrder = dc.Orders.Where(o => o.OrderID == order.OrderID).FirstOrDefault();
                if (currentOrder == null)
                {
                    // Couldn't find an [Order] record with this ID
                    return -3;
                }

                // Update our SQL Server [Order] record, with our new Shipping Details (send from whatever
                // app is calling this web service)
                currentOrder.ShipName = order.ShipName;
                currentOrder.ShipAddress = order.ShipAddress;
                currentOrder.ShipCity = order.ShipCity;
                currentOrder.ShipPostalCode = order.ShipPostcode;

                dc.SubmitChanges();

                return 0;     // Success !
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public wsSQLResult DeleteCustomer(string customerID)
        {
            wsSQLResult result = new wsSQLResult();

            try
            {
                NorthwindDataContext dc = new NorthwindDataContext();
                Customer cust = dc.Customers.Where(s => s.CustomerID == customerID).FirstOrDefault();
                if (cust == null)
                {
                    // We couldn't find a [Customer] record with this ID.
                    result.WasSuccessful = -3;
                    result.Exception = "Could not find a [Customer] record with ID: " + customerID.ToString();
                    return result;
                }

                dc.Customers.DeleteOnSubmit(cust);
                dc.SubmitChanges();

    
                result.WasSuccessful = 1;
                result.Exception = "";
                return result;     // Success !
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);

                result.WasSuccessful = -1;
                result.Exception = "An exception occurred: " + ex.Message;
                return result;     // Failed.            
            }
        }

        public wsSQLResult CreateCustomer(Stream JSONdataStream)
        {
            wsSQLResult result = new wsSQLResult();
            try
            {
                // Read in our Stream into a string...
                StreamReader reader = new StreamReader(JSONdataStream);
                string JSONdata = reader.ReadToEnd();

                // ..then convert the string into a single "wsCustomer" record.
                JavaScriptSerializer jss = new JavaScriptSerializer();
                wsCustomer cust = jss.Deserialize<wsCustomer>(JSONdata);
                if (cust == null)
                {
                    // Error: Couldn't deserialize our JSON string into a "wsCustomer" object.
                    result.WasSuccessful = 0;
                    result.Exception = "Unable to deserialize the JSON data.";
                    return result;
                }

                NorthwindDataContext dc = new NorthwindDataContext();
                Customer newCustomer = new Customer()
                {
                    CustomerID = cust.CustomerID,
                    CompanyName = cust.CompanyName,
                    City = cust.City
                };

                dc.Customers.InsertOnSubmit(newCustomer);
                dc.SubmitChanges();

                result.WasSuccessful = 1;
                result.Exception = "";
                return result;
            }
            catch (Exception ex)
            {
                result.WasSuccessful = 0;
                result.Exception = ex.Message;
                return result;
            }
        }
    }
}
