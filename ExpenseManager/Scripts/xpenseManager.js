
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

//*********************************************************************** SINGLE VOUCHER LOADING & PREVIEWING *************************************************************
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


       function clearForm()
       {
           $('#txtVoucherStartDate').val('');
           $('#txtVoucherEndDate').val('');
           $('#ddlTransactions').hide('fast');
           $('#hiddenDiv').hide('fast');
         
       }


       function populateDropdown(xVal)
       {
           var transactionDropdown = $('#ddlTransactions');

           var retData = $.parseJSON(xVal.d);

           if (retData == null || retData.length < 1)
           {
               $('#hiddenDiv').hide('fast');
               transactionDropdown.empty();
               transactionDropdown.append($("<option />").val(0).text("-- List is empty --"));
               alert('No record found!');
               return;
           }

           transactionDropdown.empty();
           transactionDropdown.append($("<option />").val(0).text("-- Select a Transaction --"));
           $.each(retData, function (sz, cdk) 
           {
               transactionDropdown.append($("<option />").val(cdk.ExpenseTransactionPaymentHistoryId).text(cdk.ExpenseTransaction.ExpenseTitle));

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

       function SetSingleVoucherPaymentHistoryId(voucherId) 
       {
           var xVal = JSON.stringify({ "paymentHistoryId": voucherId.split('V')[1] });
           $.ajax
                ({
                    url: "expenseManagerStructuredServices.asmx/SetPaymentHistoryId",
                    data: xVal,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: 'POST',
                    success: printSingleVoucher
                });

          }


          function printSingleVoucher(data) 
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

           var dst = '';

           var deptId = parseInt(dataRows.DepartmentId);

           if (deptId === 6)

            {
                dst = '<tr><td style="width: 630px; vertical-align: bottom" colspan="2">'
                + '<img src="App_Themes/Default/Images/xPlug.gif" id="imgDeptLogo" style="width: 37%; height: 71px; margin-left: 2%"/></td>'
                + '<td style="width: 30%; overflow: hidden"><table border="0" cellpadding="0" cellspacing="0" style="width: 632px; margin-left: 0px">'
                + '<tr><td ><div style="width: 50%; height: 33px; margin-left: 50%">'
                + '<img src="../../Images/Report/xPlugCaligraphy.gif" style="width: 99%; height: 29px;"/></div></td></tr>'
                + '<tr><td align="right"><span style="margin-left: 400px; font-weight: bold;color: black">2nd&nbsp;Floor,&nbsp;Block&nbsp;B,&nbsp;Adebowale&nbsp;House,&nbsp;</span></td></tr>'
                + '<tr><td align="right"><span style="margin-left: 400px; font-weight: bold;color: black">150,&nbsp;Ikorodu&nbsp;Road,&nbsp;Onipanu,&nbsp;Lagos</span></td></tr>'
                + '<tr><td><div style="top:39px; width:236px; height:15px; margin-left: 400px; font-weight: bold;color: black"><span >Telephone:&nbsp;01-7368212,&nbsp;01-6290010</span></div></td></tr>'
                + '<tr><td align="right" ><span style="margin-left: 400px; width:200px;height:15px; top: 54px;font-weight: bold;color: black">info@xplugng.org&nbsp;|&nbsp;www.xplugng.com</span></td></tr>';
            }
            else {
                if (deptId === 5) {
                    dst = '<tr><td style="width: 630px; vertical-align: bottom" colspan="2">'
                    + '<img src="App_Themes/Default/Images/Lrlogo.gif" id="imgDeptLogo" style="width: 37%; height: 71px; margin-left: 2%"/></td>'
                    + '<td style="width: 30%; overflow: hidden"><table border="0" cellpadding="0" cellspacing="0" style="width: 632px; margin-left: 0px">'
                    + '<tr><td style="vertical-align: bottom"><div style="width: 50%; height: 25px; margin-left: 55%">'
                    + '<img src="App_Themes/Default/Images/LrCaligraphy.gif" style="width: 90%; height: 100%"/></div></td></tr>'
                    + '<tr><td align="right"><span style="margin-left: 400px; font-weight: bold;color: black">1st&nbsp;Floor,&nbsp;Block&nbsp;B,&nbsp;Adebowale&nbsp;House,&nbsp;</span></td></tr>'
                    + '<tr><td align="right"><span style="margin-left: 400px; font-weight: bold;color: black">150,&nbsp;Ikorodu&nbsp;Road,&nbsp;Onipanu,&nbsp;Lagos</span></td></tr>'
                    + '<tr><td><div style="top:39px; width:236px; height:15px; margin-left: 400px; font-weight: bold;color: black"><span >Telephone:&nbsp;01-7368212,&nbsp;01-6290010</span></div></td></tr>'
                    + '<tr><td align="right" ><span style="margin-left: 400px; width:200px;height:15px; top: 54px;font-weight: bold;color: black">info@lrcard.com&nbsp;|&nbsp; www.lrcard.com</span></td></tr>';
                }
                else
                {
                    if (deptId !== 5 && deptId !== 6) 
                    {
                        dst = '<tr><td style="width: 630px; vertical-align: bottom" colspan="2">'
                + '<img src="App_Themes/Default/Images/xPlug.gif" id="imgDeptLogo" style="width: 37%; height: 71px; margin-left: 2%"/></td>'
                + '<td style="width: 30%; overflow: hidden"><table border="0" cellpadding="0" cellspacing="0" style="width: 632px; margin-left: 0px">'
                + '<tr><td ><div style="width: 50%; height: 33px; margin-left: 50%">'
                + '<img src="../../Images/Report/xPlugCaligraphy.gif" style="width: 99%; height: 29px;"/></div></td></tr>'
                + '<tr><td align="right"><span style="margin-left: 400px; font-weight: bold;color: black">2nd&nbsp;Floor,&nbsp;Block&nbsp;B,&nbsp;Adebowale&nbsp;House,&nbsp;</span></td></tr>'
                + '<tr><td align="right"><span style="margin-left: 400px; font-weight: bold;color: black">150,&nbsp;Ikorodu&nbsp;Road,&nbsp;Onipanu,&nbsp;Lagos</span></td></tr>'
                + '<tr><td><div style="top:39px; width:236px; height:15px; margin-left: 400px; font-weight: bold;color: black"><span >Telephone:&nbsp;01-7368212,&nbsp;01-6290010</span></div></td></tr>'
                + '<tr><td align="right" ><span style="margin-left: 400px; width:200px;height:15px; top: 54px;font-weight: bold;color: black">info@xplugng.org&nbsp;|&nbsp;www.xplugng.com</span></td></tr>';
                    }
                }
            }
               
           $('#hiddenDiv').empty();

           $('#hiddenDiv').append($('<table style="width: 100%" id="tbMasterContent">'
                       + dst
                       + '<tr><td></td></tr></table></td></tr>'
                        + '<tr ><td colspan="3" style="border-top: 1px solid black"><table style="width: 100%"><tr><td style="width: 7%"><label style="margin-top: 5px; margin-left: 2%;color: black">Cheque #:</label></td>'
                         + '<td style="width: 71%"><label style="margin-top: 5px;font-weight: bold;color: black" id="lblChequeMultiple">' + dataRows.ChequeNo + '</label></td><td style="width: 5%"><label style="margin-top: 5px; margin-left:5%; color: black">PVC #:</label></td>'
                          + '<td style="width: 10%; text-align: right"><label style="margin-top: -1%;font-weight: bold;color: black" id="lblPVCMultiple">' + dataRows.PcvNo + '</label></td></tr>'
                           + '<tr><td colspan="2"></td><td ><label style="margin-top: 5px; margin-left:5%;color: black">Date: </label></td><td style="text-align: right"><label style="margin-top: 5px; font-weight: bold;color: black" id="lblDateMultiple">' + dataRows.DatePaid + '</label></td></tr></table>'
                            + '<div style="border: 1px solid black; width: 25%; background-color: white; margin-left: 36%; height: 68px; margin-top: -7%">'
                             + '<img src="../../Images/Report/voucherLogo.gif" style="width: 100%; height: 100%" /></div></td></tr>'
                              + '<tr><td colspan="3"><div style="width:100%"></div></td></tr><tr><td colspan="3"><div style="width:100%"></div></td></tr><tr><td colspan="3"><div style="width:100%"></div></td></tr><tr ><td colspan="3" ><table style="width: 100%; margin-top: 3%; border-bottom: 1px solid #008080" id="tblItemsGrp"><tr style=" border-color:#008080; border: solid 1px;background-color:#008080">'
                              + '<th scope="col" style=" width: 2%; color: white">S/No</th><th scope="col" style=" width: 7%; color: white">Code</th><th scope="col" style=" width: 20%; color: white">Item</th>'
                              + '<th scope="col" style=" width: 20%; color: white">Detail</th><th scope="col" style=" width: 8%; color: white">Quantity</th>'
                             + '<th scope="col" style=" width: 13%; color: white">Unit Price(N)</th><th scope="col" style=" width: 13%; color: white">Total Price(N)</th>'
                          + '</tr> </table></td></tr><tr><td colspan="3"></td></tr><tr><td colspan="3"><table style="width: 100%; margin-top: 2%"><tr><td style=" width: 68%; font-weight: bold;"><label style=" color: black">Amount in Words:</label></td>'
                          + '<td style=" width: 12%; font-weight: bold; border-bottom: 1px solid #008080"><label style=" color: black">Total Amount:</label></td><td style=" width: 20%; font-weight: bold; text-align: right; color: #008080; border-bottom: 1px solid #008080"><label id="lblTotalAmountMultiple">' + dataRows.TotalApprovedAmmount + '</label></td></tr></table></td></tr>'
                          + '<tr><td colspan="3"><table style="width: 100%"><tr><td style=" width: 68%; font-weight: bold;color: black"><label id="lblAmountInWordsMultiple" style="font-weight: normal; width: auto; color: black">' + numbersToWord(dataRows.TotalApprovedAmmount, naira, kobo) + '</label></td>'
                          + '<td style=" width: 12%; font-weight: bold; border-bottom: 1px solid #008080"><label style="font-weight: bold; color : black">Amount Received:</label></td><td style=" width: 32%; font-weight: bold; text-align: right; color: #008080; border-bottom: 1px solid #008080"><label style="font-weight: bold; border-bottom: 1px solid" id="lblAmountReceivedMultiple">' + dataRows.AmmountPaid + '</label></td></tr></table></td></tr>'
                          + '<tr><td colspan="3"><table style="100%; margin-top: 2%"><tr><td style=" width: 78%"><label style="font-weight: bold; width: auto; border-bottom: 1px solid black; margin-left: 5%; font-style: italic; color: black" id="lblApproverMultiple">' + dataRows.Approver + '</label></td><td style=" width: 12%">'
                          + '<div style="margin-left: 10%; width: 507px; text-align: center"><label style="font-weight: bold; border-bottom: 1px solid black; width: auto; font-style: italic; color: black" id="lblReceiverMultiple">' + dataRows.ReceivedBy + '</label></div></td></tr>'
                          + '<tr><td style=" width: 78%"><label style=" margin-left: 10%; color: black">Approver</label></td style=" width: 12%"><td><label style="margin-left: 54%; color: black">Receiver</label></td> </tr></table></td></tr><tr><td colspan = "3"><table style="width:100%; margin-top: 3%"><tr><td style="width:100%; border-bottom: 1px dashed black"></td></tr></table></td></tr></table>'));

               
               
               for (var i = 0; i < dataRows.TransactionItems.length; i++) 
               {

                   $($("#tblItemsGrp tbody:last").append($('<tr class="gridRowItem"><td align="left" style="width:4%; border-left: 1px solid #008080; border-right: 1px solid #008080"><label class="xPlugTextAll_x" style="color: black">' + [i + 1] + '</label></td><td align="left" style="width:7%; border-left: 1px solid #008080;color: black; border-right: 1px solid #008080;" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].ExpenseItem.Code
                        + '</td><td align="left" style="width:20%; border-left: 1px solid #008080; border-right: 1px solid #008080;color: black" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].ExpenseItem.Title + '</td><td align="left" style="width:20%; border-left: 1px solid #008080;color: black; border-right: 1px solid #008080;" class="xPlugTextAll_x">' + dataRows.TransactionItems[i].Description + '</td><td align="center" style="width:8%; border-left: 1px solid #008080; border-right: 1px solid #008080;color: black; text-align: center" class="xPlugTextAll_x">'
                        + dataRows.TransactionItems[i].ApprovedQuantity + '</td><td align="left" style="width:13%; border-left: 1px solid #008080;color: black; border-right: 1px solid #008080;text-align: right"><label id ="PriceLabel" class="xPlugTextAll_x" style="color: black">' + dataRows.TransactionItems[i].ApprovedUnitPrice
                        + '</label></td><td align="left" style="width:13%; border-left: 1px solid #008080; border-right: 1px solid #008080; text-align: right" class="xPlugTextAll_x"><label id ="TotalPriceLabel" class="xPlugTextAll_x" style="color: black">' + dataRows.TransactionItems[i].ApprovedTotalPrice + '</label></td></tr>')));

                   $('#PriceLabel').attr("id", "PriceLabel" + i);
                   $('#' + "PriceLabel" +  i).formatCurrency({ symbol: '', roundToDecimalPlace: 1 });
                   $('#TotalPriceLabel').attr("id", "TotalPriceLabel" + i);
                   $('#' + "TotalPriceLabel" + i).formatCurrency({ symbol: '', roundToDecimalPlace: 1 });
               }

               $('#tbMasterContent').after($('<table style="width: 100%;"><tr><td colspan="3"><input class="customButton" type="button" value="Print" style="width: 115px; margin-left: 87%; margin-top: 3%" onclick="SetSingleVoucherPaymentHistoryId(this.id)" id="btnPrintV' + dataRows.TransactionpaymentHistoryId + '"></input></td></tr></table>'));

               var wrdLbl = "lblAmountInWordsMultiple";
               $('#lblAmountReceivedMultiple').formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
               $('#lblTotalAmountMultiple').formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
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

      //END OF SINGLE VOUCHER LOADING & PREVIEWING


      //******************************************************************************************* EXPENSE TRANSACTION APPROVAL ************************************


      var idCollection = "";
      var idArray = [];

      var rdVoidItd = $('#ctl00_MainContent_ctl00_radApprove');
      var rejectTransRd = $('#ctl00_MainContent_ctl00_radReject');
      var voidTransRd = $('#ctl00_MainContent_ctl00_radVoid'); ;
      var selctListC = '';

      function CheckChanged(id) 
      {
          if ($('#' + id).is(':checked'))
          {
              rejectTransRd.prop('checked', false);

              voidTransRd.prop('checked', false);

            var gt = parseInt(id.replace('chkSelect',''));
              if (gt > 0)
              {
                  idArray.push(gt);
                  SendIds();
              }
          }
          else 
          {
              $('#' + id).prop('checked', false);
              var dt = id.replace('chkSelect', '');
              idArray.splice($.inArray(dt, idArray), 1);
              SendIds();
          }
      }

      function CheckAllChanged(id) 
      {
        var selctListC =  $('#dgExpenseItem [id^="chkSelect"]');
          if ($('#' + id).is(':checked'))
          {
              $('#ctl00_MainContent_ctl00_radReject').prop('checked', false);

              $('#ctl00_MainContent_ctl00_radVoid').prop('checked', false);
              
              idCollection = "";
              idArray = [];
              selctListC.each(function (r, c) 
              {
                  $(c).prop('checked', true);
                  var tf = parseInt($(c).attr('id').replace('chkSelect', ''));
                  if (tf > 0)
                  {
                      idArray.push(tf);

                  }
              });
              
              SendIds();
          }

          else {
                   selctListC.each(function (y, l) 
                    {
                        $(l).prop('checked', false);
                    });
                    idArray = [];
                    idCollection = "";

              }
          }


          function SendIds()
          {
          idCollection = '';
          if ($('#ctl00_MainContent_ctl00_radApprove').is(':checked') && idArray.length > 0)
          {
              for (var x = 0; x < idArray.length; x++)
              {
                  if (parseInt(idArray[x]) > 0) 
                  {
                      idCollection = idCollection + idArray[x] + ",";

                  }

              }

              if (idCollection.length > 0) 
              {
                  var dataToSend = JSON.stringify({ "transCollection": idCollection });

                  $.ajax({
                      url: "expenseManagerStructuredServices.asmx/GetTransCollection",
                      data: dataToSend,
                      contentType: "application/json; charset=utf-8",
                      dataType: 'json',
                      type: 'POST',
                      success: nullIdCollection
                  });
              }
          }
      }

      function nullIdCollection()
      {
          idCollection = "";
      }

      function UncheckSlctns(id)
      {
          var slctListC = $('#dgExpenseItem [id^="chkSelect"]');
          var slctAll = $('#chkAll');
          
          if ($('#' + id).is(':checked')) 
          {

              if (slctAll.is(':checked')) 
              {
                  slctAll.prop('checked', false);
              }
              idCollection = "";
              idArray = [];

              slctListC.each(function (i, v) 
              {
                  $(v).prop('checked', false);
              });

          }
      }

      function groupDigits()
      {
          $('#textApprovedUnitPrice').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
          calculateTotalPrice();
          return true;
      }

      function calculateTotalPrice()
      {

          $('#txtTransTotalPrice').val('');
          var approvedQuantityString = $('#ctl00_MainContent_ctl00_textApprouvedQuantity').val();
          var approvedUnitPriceString = $('#textApprovedUnitPrice').val();
          if (approvedQuantityString === null || approvedQuantityString == "" || approvedQuantityString === undefined || approvedQuantityString.length < 1 || approvedUnitPriceString === null || approvedUnitPriceString.length < 1 || approvedUnitPriceString === undefined || approvedUnitPriceString == "") {
              return false;
          }
          var approvedQuantity = parseInt(approvedQuantityString);
          var approvedUnitPrice = parseFloat(approvedUnitPriceString.replace(',', ''));
          if (approvedQuantity === null || approvedQuantity === "NaN" || approvedQuantity == '' || approvedQuantity === undefined || approvedQuantity < 1 || approvedUnitPrice === null || approvedUnitPrice < 1 || approvedUnitPrice === undefined || approvedUnitPrice == '' || approvedUnitPrice == "NaN") {
              return false;
          }
          var totalPrice = approvedQuantity * approvedUnitPrice;
          if (totalPrice === "NaN" || totalPrice < 1) {
              return false;
          }
          $('#txtTransTotalPrice').val(totalPrice);
          $('#txtTransTotalPrice').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
          return true;
      }

      //************************************** Get Item Payment History **************************

      var lblId = "";
      var leftPos = "";
      var topPos = "";
      function GetTransactionsWithSimilarItem()
      {
          var itemIdString = lblId.split('m')[1];
          var itemId = parseInt(itemIdString);
          if (itemId > 0) {
              var dataToSend = JSON.stringify({ "expenseItemId": itemId });
              $.ajax({
                  url: "expenseManagerStructuredServices.asmx/GetTransObject",
                  data: dataToSend,
                  contentType: "application/json; charset=utf-8",
                  dataType: 'json',
                  type: 'POST',
                  success: showItems
              });

          }
      }

      function showItems(retData)
      {
          var retItemData = retData.d;

          if (retItemData === null || retData.length < 1) 
          {
              var dv = $('<div id="parentDiv" class="tipDiv shadow" style="z-index:9999;left:' + leftPos + "px;top: " + topPos + "px" + '"><span id="backgroundSpan" style="position: absolute; top: -10px; left:14px; width:21px; height:10px; z-index:41"></span><div>No cost has been approved for this item before.</div></div>');
              //dv.css({ 'z-index': 9999, left: leftPos + 'px', top: topPos + 'px' });
              $('#tdGrid').append(dv);
              $('#backgroundSpan').css(
                   { background: "url(App_Themes/Default/Images/top.png)  no-repeat"
                   });
              $('#parentDiv').show();
              return;
          }

          var content = ($('<div id="parentDiv" class="tipDiv shadow" style="width: 40%; z-index:9999;left:' + leftPos + "px;top: " + topPos + "px" + '" ><span id="backgroundSpan" style="position: absolute; top: -10px; left:14px; width:21px; height:10px; z-index:41"></span>'
                   + '<fieldset><legend><label style="font-size: 0.8em;">Most Recent Approved cost(s) of:</label> <label id="lblItemTitle"></label></legend><table id="tblItems"  cellspacing="0" style="width:100%;border-collapse:collapse; border: 1px solid black" rules="cols">'
                   + '<tr class="gridHeaderPop"  style="border-bottom: 1.5px solid" align="left"><th scope="col" style=" width: 30%">' + "Expense Transaction" + '</th><th scope="col" style=" width: 7%">'
                   + "Quantity" + '</th>'
                   + '<th scope="col" style=" width: 13%">' + "Unit Price(N)" + '</th><th scope="col" style=" width: 13%">' + "Date Approved" + '</th></tr></table></fieldset></div></div>'));
          //            content.css({ 'z-index': 9999, left: leftPos +'px', top: topPos +'px' });
          $('#tdGrid').append(content);

          var tblId = "tblItems";
          $('#lblItemTitle').text(retItemData[0].TransactionItem);
          for (var i = 0; i < retItemData.length; i++) {
              $($('#' + tblId + " " + "tbody:last").last().append($('<tr ><td align="left" style="width:30%">' + retItemData[i].TransactionTitle
                        + '</td><td align="left" style="width:7%" >'
                        + retItemData[i].Quantity + '</td><td align="left" style="width:10%" ><label id ="PriceLabel" class="xPlugTextAll_x" style="width:13%">' + retItemData[i].UnitPrice
                        + '</label></td><td align="left" style="width:13%" ><label id ="TotalPriceLabel" class="xPlugTextAll_x">' + retItemData[i].TransactionApprovalDate + '</label></td></tr></table>')));

          }

          $('#backgroundSpan').css({ background: "url(App_Themes/Default/Images/top.png)  no-repeat"
          });

          $('#parentDiv').show();
          //            var popShow = $find('mpeSimile');
          //            popShow.show();

      }

      $(function() {
          $('.xPlugTextLabel').on('mouseover', function ()
          {
              var id = $(this).attr('id');
              var pos = $(this).offset();
              leftPos = pos.left;
              topPos = pos.top + 25;
              lblId = id;
              GetTransactionsWithSimilarItem();

          });

          $('.xPlugTextLabel').on('mouseout', function ()
          {
              $('#parentDiv').hide('slow');
              $('#parentDiv').remove();


          });
      });

      // END OF EXPENSE TRANSACTION APPROVAL