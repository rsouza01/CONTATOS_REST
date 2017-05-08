<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JsonApplication.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>

    <style type="text/css">
      ul ul li 
      {
        color: Blue;
        font-size: 12px;
      }
      li, p, span, div, select
      {
        font-family: Arial;
        font-size: 14px;
      }
    </style>
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {

            // We're about to populate the "listOfCustomers" control with a list of Customers (using JSON).
            // But first, let's make sure it is empty.
            $("#listOfCustomers").empty();

            $.getJSON("http://localhost:21130/JSONdata.aspx?Option=GetListOfCustomers", function (data) {

                // Success !
                // We managed to load the JSON, now, let's iterate through the "Customers" records, and add a 
                // drop down list item for each.
                $.each(data.Customers, function () {
                    $("#listOfCustomers").append($("<option />").val(this.CustomerID).text(this.CompanyName));
                });
            });

            $("#listOfCustomers").change(function () {
                // Using JQuery, find the text and the value of which dropdown list item was selected
                var chosenCompanyName = $("#listOfCustomers option:selected").text();
                var chosenCustomerID = $("#listOfCustomers option:selected").val();

                $("#listOfOrders").empty();


                var URL = "http://localhost:21130/JSONdata.aspx?Option=GetOrdersForCustomers&CustomerID=" + chosenCustomerID;

                $.getJSON(URL, function (data) {

                    $("#divNumberOfOrders").text("This customer has placed " + data.NumberOfOrders + " orders.");

                    // Iterate through the list of Orders in the JSON data.
                    $.each(data.Orders, function () {
                        var divListOfProducts = "Div_" + this.OrderID;
                        $("#listOfOrders").append($("<li />").val(this.OrderID).text("Order date: " + this.OrderDate));
                        $("#listOfOrders").append($("<ul />").attr("id", divListOfProducts));

                        // Iterate through the list of Basket records in this Order record.
                        $.each(this.Basket, function () {
                            var productString = this.Quantity + " x " + this.ProductName;
                            $("#" + divListOfProducts).append($("<li />").val(this.ProductID).text(productString));
                        });

                    });
                });
            });

        });
</script>
</head>
<body>
  <form id="form1" runat="server">
  <div>
    <p>Please choose a customer:</p>
    <select id="listOfCustomers">
    </select>
    <br />
    <br />
    <div id="divNumberOfOrders"></div>
    <ul id="listOfOrders">
    </ul>
  </div>
  </form>
</body>
</html>
