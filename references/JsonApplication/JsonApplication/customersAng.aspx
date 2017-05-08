<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customersAng.aspx.cs" Inherits="JsonApplication.customersAng" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app='myApp'>
<head runat="server">
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <script src="Scripts/angular.min.js"></script>
    <script src="Scripts/MasterDetailCtrl.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divMasterDetailWrapper" ng-controller='MasterDetailCtrl' style="position:relative;">
        <span style="width: 200px">Please select a customer:</span>
        <select ng-model="selectedCustomer" ng-change="loadOrders();" ng-options="customer.CustomerID as customer.CompanyName for customer in listOfCustomers" style="width:350px;"></select>
        <br />
        <span style="width: 200px">ID of selected customer: </span>
        <span style="width:80px;border:1px solid #B0B0B0;">{{selectedCustomer}}</span>
        <br />
        <br />
        <pre>listOfCustomers: {{ listOfCustomers | json }}</pre>

        This customer has {{listOfOrders.length}} orders.
        <br />
        <ul class="cssListOfOrders">
            <li ng-repeat='order in listOfOrders'>
                <span class="cssOrderHeader">Order # {{order.OrderID}} ( {{order.OrderDate}} )</span>
                <ul>
                        <li ng-repeat='product in order.ProductsInBasket'>
                            <span style="width:25px"> {{product.Quantity}}</span>
                            <span style="width:15px">x</span>
                            <span style="width:230px"> {{product.ProductName}} </span>
                            <span style="width:30px">@</span>
                            <span style="width:60px"> {{product.UnitPrice | currency}} </span>
                            <span style="width:30px">=</span>
                            <span style="width:100px"> {{product.Quantity * product.UnitPrice | currency}} </span>

                        </li>
                </ul>
            </li>
        </ul>
        <span style="color:Red;">{{errorMessage}}</span>
            
    </div>
    </form>
</body>
</html>
