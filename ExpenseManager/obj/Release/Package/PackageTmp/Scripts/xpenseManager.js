
       function getFormatedAmountInWords(totalPrice, wrdLbl, naira) 
       {

           if (totalPrice.indexOf("Naira") != -1) 
           {
               $('#' + wrdLbl).text(totalPrice);
           }
           else 
           {
               $('#' + wrdLbl).text(totalPrice + "  " + naira);
           }

       }


       $(document).ready(function () 
       {
           $('#ddlTransactions').append($("<option />").val(0).text("List Is Empty"));
           $('#btnVoucherDateFilter').bind('click', FilterByDate);
           $('#ddlTransactions').on("change", GetVoucher);
       });


       function FilterByDate() 
       {
           var dateFrom = $('#txtVoucherStartDate').val();
           var dateTo = $('#txtVoucherEndDate').val();
           var dpt = $('#ddlDepartmentVoucher').val();
           var transactionDropdown = $('#ddlTransactions');
           
           if (parseInt($('#ddlVoucherFilterOption').val()) < 0) 
           {
               transactionDropdown.empty();
               transactionDropdown.append($("<option />").val(0).text("List Is Empty"));
               transactionDropdown.prop('selectedIndex', 0);
               $('#hiddenDiv').fadeOut("fast");
               alert("Please select a FILTER OPTION!");
               return false;

           }
           if (parseInt(dpt) < 1)
           {
               transactionDropdown.empty();
               transactionDropdown.append($("<option />").val(0).text("List Is Empty"));
               transactionDropdown.prop('selectedIndex', 0);
               $('#hiddenDiv').fadeOut("fast");
               alert("Please select a Department");
               
               return false;

           }
           
           
           if (dateTo === null || dateTo === "undefined" || dateFrom === null || dateFrom === "undefined") 
           {
               transactionDropdown.empty();
               transactionDropdown.append($("<option />").val(0).text("List Is Empty"));
               transactionDropdown.prop('selectedIndex', 0);
               $('#hiddenDiv').fadeOut("fast");
               alert("Please supply a date range.");
               return false;

           }

           if (!validateDate(dateFrom, dateTo)) 
           {
               transactionDropdown.empty();
               transactionDropdown.append($("<option />").val(0).text("List Is Empty"));
               transactionDropdown.prop('selectedIndex', 0);
               $('#hiddenDiv').fadeOut("fast");
               return false;
           }

//           $('#ddlTransactions').fadeOut("fast");
           $('#hiddenDiv').fadeOut("fast");

           var dataToSend = JSON.stringify({ "startDate": dateFrom, "endDate": dateTo, "dept": dpt });
           
           if (parseInt($('#ddlVoucherFilterOption').val()) === 0) 
           {
               $.ajax
                    ({
                        url: "expenseManagerStructuredServices.asmx/GetTransactionPaymentsByDateRange",
                        data: dataToSend,
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: 'POST',
                        success: populateDropdown
                    });
            }

                if (parseInt($('#ddlVoucherFilterOption').val()) === 1) 
                {
                    $.ajax
                    ({
                        url: "expenseManagerStructuredServices.asmx/GetTransactionPaymentsByDateRange",
                        data: dataToSend,
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: 'POST',
                        success: populateDropdown
                    });
                }

                if (parseInt($('#ddlVoucherFilterOption').val()) === 2) 
                {
                    $.ajax
                    ({
                        url: "expenseManagerStructuredServices.asmx/GetVoidedTransactionPaymentsByDateRange",
                        data: dataToSend,
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: 'POST',
                        success: populateDropdown
                    });
                }
                
           
           return false;
       }


       function populateDropdown(xVal)
       {
           var transactionDropdown = $('#ddlTransactions');

           var retData = $.parseJSON(xVal.d);

           if (retData == null) 
           {
               alert('No record found!');
               return;
           }

           if (retData.length === 0)
           {
               alert('No record found!');
               return;
           }

           transactionDropdown.empty();
           transactionDropdown.append($("<option />").val(0).text("Select a Transaction"));
           $.each(retData, function () 
           {
               transactionDropdown.append($("<option />").val(this.ExpenseTransactionPaymentHistoryId).text(this.ExpenseTransaction.ExpenseTitle));

           });
           transactionDropdown.prop('selectedIndex', 0);
           $('#ddlTransactions').fadeIn("slow");
       }

       function GetVoucher() 
       {
           var selection = parseInt($('#ddlTransactions').val());
           if (selection < 1) 
           {
               alert("Please select a Transaction");
               return false;

           }

           var xVal = JSON.stringify({ "paymentHistoryId": selection });

           $.ajax
                    ({
                        url: "expenseManagerStructuredServices.asmx/GetVoucher",
                        data: xVal,
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: 'POST',
                        success: createVoucher
                    });
           return true;
       }

       function SetPaymentHistoryId(voucherId) 
       {
           var xVal = JSON.stringify({ "paymentHistoryId": voucherId.split('V')[1] });
           $.ajax
                ({
                    url: "expenseManagerStructuredServices.asmx/SetPaymentHistoryId",
                    data: xVal,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: 'POST',
                    success: printVoucher
                });

          }


          function printVoucher(data) 
        {
            var retInt = parseInt($.parseJSON(data.d));
            if (retInt < 1)
            {
               alert('An unknown error was encountered. Report cannot be printed.' + '\n Please try again or contact the Admin.');
               return;
           }

           window.open('ExpenseMgt/Voucher/VoucherManager.aspx?data=' + retInt, "Transaction Voucher", "fullscreen=no,toolbar=no,status=yes, " +
                                        "menubar=no,scrollbars=yes,resizable=yes,directories=no,location=no, " +
                                        "width=1800,height=900,left=100,top=100,screenX=100,screenY=100");
              
       }

       function createVoucher(data) 
       {

           var dataRows = $.parseJSON(data.d);
           if (dataRows == null)
           {
               alert('No record found!');
               return;
           }

           if (dataRows.length === 0) 
           {
               alert('No record found!');
               return;
           }

           $('#hiddenDiv').empty();

           $('#hiddenDiv').append($('<div id="parentDv"><table style="width: 100%; margin-top: 10%;" id="containerTbl"><tr><td colspan="2"><div style=" font-weight: bolder; width: 22%; font-size: 1.3em"><label  class="customLabel2" style="width: 100%;"> Transaction Details::</label><br/>'
                    + '</div></td></tr><tr><td colspan="2"></td></tr><tr><td colspan="2">'
                    + '</td></tr><tr><td colspan="2"><table style="width: 100%">'
                   + '<tr><td id="imgDV" colspan="2"><br/><div id="logoDv" style="margin-left: 38%; width: 11%; border: 1px solid black; margin-bottom: -2%"><img src="" runat="server" id="imgLogo"  alt="" style="width: 100%; height: 50%" />'
                   + '</div></td></tr><tr><td colspan="2"><div style=" font-weight: bolder; width: 25%;  margin-left: 38%; font-size: 1.3em"><label  class="customLabel2" style="width: 100%;"></label></div></td></tr>'
                   + '<tr><td colspan="2"></td></tr><tr><td colspan="2"></td></tr><tr id"trCheque" style="border-top: 1px solid black"><td colspan="2" class="divBackground2"><table style="width: 100%;"><tr id="chequeRow">'
                   + '<td style="width: 70%"></td><td style="width: 10%"><label style="width: 100%" class="customLabel">Cheque No:</label></td><td style="width: 20%"><label class="customLabel2" id="lblChequeNo" style="width: 100%" >' + dataRows.ChequeNo + '</label>'
                   + '</td></tr><tr><td style="width: 70%"></td><td style="width: 10%"><label style="width: 100%;" class="customLabel">PCV No:</label></td><td style="width: 20%">'
                   + '<label class="customLabel2" id="lblPCV" style="width: 100%">' + dataRows.PcvNo + '</label></td></tr><tr><td style="width: 70%"><label class="customLabel">Transaction: </label>&nbsp;<label  class="customLabel2" style="width: 100%;" id="lblTransactionTitle" runat="server">' + dataRows.TransactionTitle + '</label></td><td style="width: 10%">'
                   + '<label style="width: 100%" class="customLabel">Date: </label></td><td style="width: 20%"><label class="customLabel2" id="lblPaymentDate">' + dataRows.DatePaid + '</label>'
                   + '</td></tr><tr><td colspan="3"><div style="padding-top: 1%"><label class="customLabel">Requested by: </label><label id="lblRequestedBy" style="width: 100%" class="customLabel2">' + dataRows.RequestedBy + '</label>'
                   + '</div></td></tr></table></td></tr><tr><td colspan="2" class="divBackground2"></td></tr></table></td></tr></table><table id="tblDateVouchers" class="xPlugTextAll_x" cellspacing="0" style="width:100%;border-collapse:collapse; border: 1px solid black" rules="cols">'
                   + '<tr class="gridHeader" style="border-bottom: 1.5px solid" align="left"><th scope="col" style=" width: 2%">'
                           + "S/No" + '</th><th scope="col" style=" width: 7%">' + "Code" + '</th><th scope="col" style=" width: 20%">' + "Item" + '</th><th scope="col" style=" width: 20%">' + "Details" + '</th><th scope="col" style=" width: 8%">'
                           + "Quantity" + '</th><th scope="col" style=" width: 13%">' + "Unit Price(N)" + '</th><th scope="col" style=" width: 13%">' + "Total Price(N)" + '</th></tr></table></div>'));
                
               var deptId = parseInt(dataRows.DepartmentId);
               if (deptId === 6)
               {
                   var src = "App_Themes/Default/Images/xPlug.gif";
                   $('#imgLogo').attr("src", src);
               }
               else {
                   if (deptId === 5)
                   {
                       var lrSrc = "App_Themes/Default/Images/Lr.png";
                       $('#imgLogo').attr("src", lrSrc);
                   }
                   else {
                       if (deptId !== 5 && deptId !== 6) 
                       {
                           var generalSrc = "App_Themes/Default/Images/xPlug.gif";
                           $('#imgLogo').attr("src", generalSrc);
                       }
                   }
               }
               if (dataRows.ChequeNo === "" || dataRows.ChequeNo === null || dataRows.ChequeNo === "undefined" || dataRows.ChequeNo == '')
               {
                   $('#chequeRow').hide();
               }
               else 
               {
                   $('#chequeRow').show();
               }
               
               for (var i = 0; i < dataRows.TransactionItems.length; i++) 
               {

                   $($("#tblDateVouchers tbody:last").last().append($('<tr class="gridRowItem"><td align="left" style="width:4%; border-left: 1px solid black; border-right: 1px solid black"><label class="xPlugTextAll_x">' + [i + 1] + '</label></td><td align="left" style="width:7%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].ExpenseItem.Code
                        + '</td><td align="left" style="width:20%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].ExpenseItem.Title + '</td><td align="left" style="width:20%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].Description + '</td><td align="left" style="width:8%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">'
                        + dataRows.TransactionItems[i].ApprovedQuantity + '</td><td align="left" style="width:13%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x"><label id ="PriceLabel" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].ApprovedUnitPrice
                        + '</label></td><td align="left" style="width:13%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x"><label id ="TotalPriceLabel" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].ApprovedTotalPrice + '</label></td></tr></table>')));

                   $('#PriceLabel').attr("id", "PriceLabel" + i);
                   $('#' + "PriceLabel" +  i).formatCurrency({ symbol: '', roundToDecimalPlace: 1 });
                   $('#TotalPriceLabel').attr("id", "TotalPriceLabel" + i);
                   $('#' + "TotalPriceLabel" + i).formatCurrency({ symbol: '', roundToDecimalPlace: 1 });
               }

               $($('#tblDateVouchers').after($('<br><table id="tblApprovedBy" class="xPlugTextAll_x" cellspacing="0" style="width: 100%; border-collapse:collapse; border-spacing:0 0;"><tr><td></td></tr><tr><td style="width:4%"></td><td style="width:12%"></td><td style="width:40%"></td><td style="width:5%"></td><td style="width:15%"><label style="color: black; font-weight: bold; margin-left: 20%; width: 60%">TOTAL:</label></td>'
                   + '<td style="border-top: solid 1px black; width: 13%; border-bottom: solid 1px black; border-top: solid 1px black; border-left: none; border-right: none;"><div><label id="lblTotal" style="width: 100%; color: black;">' + dataRows.TotalApprovedAmmount + '</label></div></td></tr></table>'
                   + '<tr><td style="width: 100%"><table style="width: 100%"><tr><td colspan="3"></td></tr><tr><td colspan="3"><table style="width: 100%"><tr><td style="width: 17%">'
                   + '<label style="color: black">Amount in words:</label><td style="width: 70%"><label id="lblAmountInWords" style="width: 100%" class="customLabel3"></label>'
                   + '</td></tr></table></td></tr><tr><td colspan="3"><table style="width:  100%"><tr><td style="width: 30%"><label style="width: 100%; color: black">Approved By:</label>'
                   + '</td><td style="width: 40%"><label style="width: 100%; color: black">Received By:</label></td><td style="width: 30%"><label style="width: 100%; color: black">Amount Received:</label>'
                   + '</td></tr><tr><td style="width: 30%"><label style="width: 100%; color: black" id="lblApprover" class="customLabel2">' + dataRows.Approver + '</label></td><td style="width: 40%">'
                   + '<label style="width: 100%" id="lblReceiver" class="customLabel2">' + dataRows.ReceivedBy + '</label></td><td style="width: 30%">'
                   + '<label style="width: 100%" id="lblAmountReceived" class="customLabel3">' + dataRows.AmmountPaid + '</label></td></tr></table></td></tr></table>')));

               $('#parentDv').after($('<table style="width: 100%;"><tr><td colspan="3"><input class="customButton" type="button" value="Print" style="width: 115px; margin-left: 87%; margin-top: 3%" onclick="SetPaymentHistoryId(this.id)" id="btnPrintV' + dataRows.TransactionpaymentHistoryId + '"></input></td></tr></table>'));

               var wrdLbl = "lblAmountInWords";
               $('#lblAmountReceived').formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
               $('#lblTotal').formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
               var naira = "Naira";
               var kobo = "Kobo";
               getFormatedAmountInWords(numbersToWord(dataRows.TotalApprovedAmmount, naira, kobo), wrdLbl, naira);
           SetWordValue(dataRows.TotalApprovedAmmount);
           $('#hiddenDiv').fadeIn("slow");
       }
       
       function SetWordValue(totalPrice) 
       {
           var naira = "Naira";
           var kobo = "Kobo";
           
           var xVal = numbersToWord(totalPrice, naira, kobo);

           if (xVal.indexOf('Naira') != -1) 
           {
               xVal = xVal;
           }
           else 
           {
               xVal = xVal + ' ' + naira;
           }

           var sendVal = JSON.stringify({ "wordString": xVal });

           $.ajax
          ({
              url: "expenseManagerStructuredServices.asmx/SetWordEquiv",
              data: sendVal,
              contentType: "application/json; charset=utf-8",
              dataType: 'json', type: 'POST'
          });
      }
      

      