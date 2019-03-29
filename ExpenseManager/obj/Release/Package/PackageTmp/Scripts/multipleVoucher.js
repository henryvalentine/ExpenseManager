     
      var idString = "";
      var idArrayList = [];
      function CheckPrintChanged(id) 
      {
          if ($('#' + id).is(':checked')) 
          {
              var gtt = id.split('w');
              if (parseInt(gtt[1]) > 0)
              {
                  idArrayList.push(gtt[1]);
              }
          }
          else 
          {
//              $('#' + id).prop('checked', false);
              var dt = id.split('w');
              idArrayList.splice($.inArray(dt[1], idArrayList), 1);
           
          }
      }

      function CheckAllprintOptionIdChanged(id)
      {
          var slctListC = document.getElementsByName("printOptionSlct");

          if ($('#' + id).is(':checked')) 
          {
           
              idString = "";
              idArrayList = [];
              for (var i = 0; i < slctListC.length; i++) 
              {
                  slctListC[i].checked = true;
                  var tTf = slctListC[i].id.split('w');
                  if (parseInt(tTf[1]) > 0) 
                  {
                      idArrayList.push(tTf[1]);

                  }

              }
              
          }
          else
          {
              for (var j = 0; j < slctListC.length; j++) 
              {
                  slctListC[j].prop('checked', false);
                  idArrayList = [];
                  idString = "";
              }

          }
      }

      function SendOptionIds() 
      {
          idString = '';
          if (idArrayList.length > 0) 
          {
              for (var x = 0; x < idArrayList.length; x++)
                    {
                        if (parseInt(idArrayList[x]) > 0)
                        {
                            idString = idString + idArrayList[x] + ",";

                        }

                    }

                    if (idString.length > 0) 
                    {
                        var dataToSend = JSON.stringify({ "paymentHistoryIds": idString });

                        $.ajax({
                            url: "expenseManagerStructuredServices.asmx/GetTransactionPaymentVouchers",
                            data: dataToSend,
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            type: 'POST',
                            success: DesignVouchers 
                        });
                    }

              return true;
          }
          else
          {
                alert("Please Select at least one Item from the List!");
                return false;
          }
                
      }
        
      function nullIdString() 
      {
          idString = "";
      }

      function HideDivs()
      {
//          $('#dvPreviewMultiple').hide();
//          $('#dvMultiVouchers').show();
          $('#dvPreviewMultiple').hide("slow");
          $('#dvMultiVouchers').fadeIn("slow");
      }



      function DesignVouchers(data)
      {

          var dataRows = $.parseJSON(data.d);
          if (dataRows == null) {
              alert('No record found!');
              $('#dvItemsView').empty();
              $('#dvPreviewMultiple').hide();
              return;
          }

          if (dataRows.length === 0)
          {
              alert('No record found!');
              $('#dvItemsView').empty();
              $('#dvPreviewMultiple').hide();
              return;
          }

          $('#dvItemsView').empty();
          var genericCollection = "";
          for (var k = 0; k < dataRows.length; k++) 
          {
              var tblId = "tblDateVouchersC" + k;
              var contentTbl = "containerTblC" + k;
              var pDiv = "mainDv" + k;

              $('#dvItemsView').append($('<div id="mainDv"><table style="width: 100%; margin-top: 10%;" id="containerTblC"><tr><td colspan="2"><table style="width: 100%">'
                   + '<tr><td id="imgDV" colspan="2"><br/><div id="logoDv" style="margin-left: 38%; width: 11%; border: 1px solid black; margin-bottom: -2%"><img src="" runat="server" id="imgLogoC"  alt="" style="width: 100%; height: 50%" />'
                   + '</div></td></tr>'
                   + '<tr><td colspan="2"></td></tr><tr><td colspan="2"></td></tr><tr id"trCheque" style="border-top: 1px solid black"><td colspan="2" class="divBackground2"><table style="width: 100%;"><tr id="chequeRowC">'
                   + '<td style="width: 70%"></td><td style="width: 10%"><label style="width: 100%" class="customLabel">Cheque No:</label></td><td style="width: 20%"><label class="customLabel2" id="lblChequeNo" style="width: 100%" >' + dataRows[k].ChequeNo + '</label>'
                   + '</td></tr><tr><td style="width: 70%"></td><td style="width: 10%"><label style="width: 100%;" class="customLabel">PCV No:</label></td><td style="width: 20%">'
                   + '<label class="customLabel2" id="lblPCVC" style="width: 100%">' + dataRows[k].PcvNo + '</label></td></tr><tr><td style="width: 70%"><label class="customLabel">Transaction: </label>&nbsp;<label  class="customLabel2" style="width: 100%;" id="lblTransactionTitle" runat="server">' + dataRows[k].TransactionTitle + '</label></td><td style="width: 10%">'
                   + '<label style="width: 100%" class="customLabel">Date: </label></td><td style="width: 20%"><label class="customLabel2" id="lblPaymentDate">' + dataRows[k].DatePaid + '</label>'
                   + '</td></tr><tr><td colspan="3"><div style="padding-top: 1%"><label class="customLabel">Requested by: </label><label id="lblRequestedByC" style="width: 100%" class="customLabel2">' + dataRows[k].RequestedBy + '</label>'
                   + '</div></td></tr></table></td></tr><tr><td colspan="2" class="divBackground2"></td></tr></table></td></tr></table><table id="tblC" class="xPlugTextAll_x" cellspacing="0" style="width:100%;border-collapse:collapse; border: 1px solid black" rules="cols">'
                   + '<tr class="gridHeader" style="border-bottom: 1.5px solid" align="left"><th scope="col" style=" width: 2%">'
                           + "S/No" + '</th><th scope="col" style=" width: 7%">' + "Code" + '</th><th scope="col" style=" width: 20%">' + "Item" + '</th><th scope="col" style=" width: 20%">' + "Details" + '</th><th scope="col" style=" width: 8%">'
                           + "Quantity" + '</th><th scope="col" style=" width: 13%">' + "Unit Price(N)" + '</th><th scope="col" style=" width: 13%">' + "Total Price(N)" + '</th></tr></table></div>'));

              var imgLogoC = "imgLogoC" + [k];
              $('#imgLogoC').attr("id", imgLogoC);

              var deptId = parseInt(dataRows[k].DepartmentId);
              //              var xplug = 0;
              //              var lrGlobal = 0;//              
              //               var dtSend = JSON.stringify({ "refId": deptId });
              //               $.ajax
              //                    ({
              //                        url: "expenseManagerStructuredServices.asmx/GetXplugDepartment",
              //                        data: dtSend,
              //                        contentType: "application/json; charset=utf-8",
              //                        dataType: 'json',
              //                        type: 'POST',
              //                        success: function (retVal) {
              //                            var xValue = $.parseJSON(retVal.d);
              //                            xplug = parseInt(xValue);
              //                           
              //                        }
              //                    });

              //                    $.ajax
              //                    ({
              //                        url: "expenseManagerStructuredServices.asmx/GetLrDepartment",
              //                        data: dtSend,
              //                        contentType: "application/json; charset=utf-8",
              //                        dataType: 'json',
              //                        type: 'POST',
              //                        success: function (retVal) 
              //                        {
              //                            var lValue = $.parseJSON(retVal.d);
              //                            lrGlobal = parseInt(lValue);
              //                           
              //                        }
              //                    });

              if (deptId === 6) {
                  var src = "App_Themes/Default/Images/xPlug.gif";
                  $('#' + imgLogoC).attr("src", src);
              }
              else {
                  if (deptId === 5) {
                      var lrSrc = "App_Themes/Default/Images/Lr.png";
                      $('#' + imgLogoC).attr("src", lrSrc);
                  }
                  else {
                      if (deptId !== 5 && deptId !== 6) {
                          var generalSrc = "App_Themes/Default/Images/xPlug.gif";
                          $('#' + imgLogoC).attr("src", generalSrc);
                      }
                  }
              }

              var chequeRowC = "chequeRowC" + k;
              $('#chequeRowC').attr("id", chequeRowC);
              if (dataRows[k].ChequeNo === "" || dataRows[k].ChequeNo === null || dataRows[k].ChequeNo === "undefined" || dataRows[k].ChequeNo == '') {
                  $('#' + chequeRowC).hide();
              }
              else {
                  $('#' + chequeRowC).show();
              }

              $('#mainDv').attr("id", pDiv);
              $('#tblC').attr("id", tblId);
              $('#containerTblC').attr("id", contentTbl);


              for (var i = 0; i < dataRows[k].TransactionItems.length; i++) 
              {

                  $($('#' + tblId + " " + "tbody:last").last().append($('<tr class="gridRowItem"><td align="left" style="width:4%; border-left: 1px solid black; border-right: 1px solid black"><label class="xPlugTextAll_x">' + [i + 1] + '</label></td><td align="left" style="width:7%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">' + dataRows[k].TransactionItems[i].ExpenseItem.Code
                        + '</td><td align="left" style="width:20%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">' + dataRows[k].TransactionItems[i].ExpenseItem.Title + '</td><td align="left" style="width:20%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">' + dataRows[k].TransactionItems[i].Description + '</td><td align="left" style="width:8%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x">'
                        + dataRows[k].TransactionItems[i].ApprovedQuantity + '</td><td align="left" style="width:13%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x"><label id ="PriceLabel" class="xPlugTextAll_x">' + dataRows[k].TransactionItems[i].ApprovedUnitPrice
                        + '</label></td><td align="left" style="width:13%; border-left: 1px solid black; border-right: 1px solid black" class="xPlugTextAll_x"><label id ="TotalPriceLabel" class="xPlugTextAll_x">' + dataRows[k].TransactionItems[i].ApprovedTotalPrice + '</label></td></tr></table>')));

                  $('#PriceLabel').attr("id", "PriceLabel" + [k] + i);
                  $('#' + "PriceLabel" + [k] + i).formatCurrency({ symbol: '', roundToDecimalPlace: 1 });
                  $('#TotalPriceLabelC').attr("id", "TotalPriceLabelC" + [k] + i);
                  $('#' + "TotalPriceLabelC" + [k] + i).formatCurrency({ symbol: '', roundToDecimalPlace: 1 });
              }

              $($('#' + tblId).after($('<br><table id="tblApprovedBy" class="xPlugTextAll_x" cellspacing="0" style="width: 100%; border-collapse:collapse; border-spacing:0 0;"><tr><td></td></tr><tr><td style="width:4%"></td><td style="width:12%"></td><td style="width:40%"></td><td style="width:5%"></td><td style="width:15%"><label style="color: black; font-weight: bold; margin-left: 20%; width: 60%">TOTAL:</label></td>'
                   + '<td style="border-top: solid 1px black; width: 13%; border-bottom: solid 1px black; border-top: solid 1px black; border-left: none; border-right: none;"><div><label id="lblTotal" style="width: 100%; color: black;">' + dataRows[k].TotalApprovedAmmount + '</label></div></td></tr></table>'
                   + '<tr><td style="width: 100%"><table style="width: 100%"><tr><td colspan="3"></td></tr><tr><td colspan="3"><table style="width: 100%"><tr><td style="width: 17%">'
                   + '<label style="color: black; font-weight: bold">Amount in words:</label><td style="width: 70%"><label id="lblAmountInWordsC" style="width: 100%; font-weight: normal; color: black" class="customLabel2"></label>'
                   + '</td></tr></table></td></tr><tr><td colspan="3"><table style="width:  100%"><tr><td style="width: 30%"><label style="width: 100%; color: black">Approved By:</label>'
                   + '</td><td style="width: 40%"><label style="width: 100%; color: black">Received By:</label></td><td style="width: 30%"><label style="width: 100%; color: black">Amount Received:</label>'
                   + '</td></tr><tr><td style="width: 30%"><label style="width: 100%; color: black" id="lblApprover" class="customLabel2">' + dataRows[k].Approver + '</label></td><td style="width: 40%">'
                   + '<label style="width: 100%" id="lblReceiver" class="customLabel2">' + dataRows[k].ReceivedBy + '</label></td><td style="width: 30%">'
                   + '<label style="width: 100%" id="lblAmountReceivedC" class="customLabel3">' + dataRows[k].AmmountPaid + '</label></td></tr></table></td></tr></table>')));

//              $('#' + pDiv).after($('<table style="width: 100%;"><tr><td colspan="3"><input id="btnPrint" class="customButton" type="button" value="Print" style="width: 115px; margin-left: 87%; margin-top: 3%" onclick="printVoucherC(this.id)"></input></td></tr></table>'));

              var wrdLbl = "lblTotalAmountInWords" + k;
              $('#lblAmountReceivedC').attr("id", "lblAmountReceivedC" + k);
              $('#' + "lblAmountReceivedC" + k).formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
              $('#lblTotal').attr("id", "lblTotal" + k);
              $('#' + "lblTotal" + k).formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
              $('#mainDv').attr("id", pDiv);
              $('#lblAmountInWordsC').attr("id", wrdLbl);
              $('#btnPrint').attr("id", "btnprintVoucherC" + [k]);
              var naira = "Naira";
              var kobo = "Kobo";
              var wordEquiv = numbersToWord(dataRows[k].TotalApprovedAmmount, naira, kobo);
              getFormatedAmountInWords(wordEquiv, wrdLbl, naira);
              genericCollection += dataRows[k].TransactionpaymentHistoryId + "+" + formatAmountInWords(wordEquiv, naira) + ",";
          }

          SetPaymentHistoryIds(genericCollection);
          nullIdString();
          $('#dvPreviewMultiple').fadeIn("slow");
          $('#dvMultiVouchers').hide("slow");
      }

      function SetPaymentHistoryIds(voucherCollection) 
      {
          var xVal = JSON.stringify({ "voucherCollection": voucherCollection });
          $.ajax
                ({
                    url: "expenseManagerStructuredServices.asmx/SetWordEquivs",
                    data: xVal,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: 'POST'
                });

            }
      
       function printSelection() 
      {
        window.open('ExpenseMgt/Voucher/MultiVoucherManager.aspx?data=1', "Transaction Voucher", "fullscreen=no,toolbar=no,status=yes, " +
                                        "menubar=no,scrollbars=yes,resizable=yes,directories=no,location=no, " +
                                        "width=1800,height=900,left=100,top=100,screenX=100,screenY=100");

    }


    function formatAmountInWords(totalPrice, naira) {

        if (totalPrice.indexOf("Naira") != -1) {
            return totalPrice;
        }
        else {
            return totalPrice + "  " + naira;
        }

    }