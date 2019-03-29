
$(window).load(function ()
{
    if ($('#ctl00_MainContent_ctl00_dgVouchers [id^="chkPrintPreview"]').length < 1)
    {
       $('#btnPrintSelection').hide();
    }
    else
    {
        $('#btnPrintSelection').show();
    }
});
 
 
 
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
          else {
              //              $('#' + id).prop('checked', false);
              var dt = id.split('w');
              idArrayList.splice($.inArray(dt[1], idArrayList), 1);

          }
      }

      function CheckAllprintOptionIdChanged(id)
      {
          var thx = $('#ctl00_MainContent_ctl00_dgVouchers [id^="chkPrintPreview"]');

          if ($('#' + id).is(':checked')) 
          {

              idString = "";
              idArrayList = [];

              thx.each(function (i, g)
              {
                  $(g).prop('checked', true);
                  var tTf = $(g).attr('id').replace('chkPrintPreview', '');
                  if (parseInt(tTf) > 0)
                  {
                      idArrayList.push(tTf);
                  }

              });
          }
          else
          {

              thx.each(function (i, g)
              {
                  $(g).prop('checked', false);
              });

              idArrayList = [];
              idString = "";
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
                      success: createVouchers
                  });
              }
              return;
          }
          else {
              alert("Please Select at least one Item from the List!");
              return;
          }

      }

      function nullIdString()
      {
          idString = "";
      }

      function HideDivs() 
      {
          $('#btnBack').hide();
          $('#dvPreview').hide("slow");
          $('#dvMultiVouchers').fadeIn("slow");
      }

      var xguid = "";
      function createVouchers(data) 
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

          var tempDiv = $('#dvItemsView');
          var mainContainer = $('#dvPreviewMultiple');
          tempDiv.empty();
          mainContainer.empty();
          mainContainer.append($(''));
          mainContainer.append($('<input id="btnPrint1" class="customButton" type="button" value="Print" style="width: 115px; margin-left: 75%; margin-top: 6%" onclick="printVoucher(this.id)"></input><div style="height: 842px; width: 934px; max-width: 934px; -moz-min-width: 934px; -ms-min-width: 934px; -o-min-width: 934px; -webkit-min-width: 934px; min-width: 934px; border: 0 groove transparent; margin-top: 3%; page-break-after: always" id="content1"></div>'));
          var contentDiv = $('#dvPreviewMultiple [id^="content"]');
          var naira = "Naira";
          var kobo = "Kobo";
          var dvCollections = [];

          for (var y = 0; y < dataRows.length; y++) 
          {
              var xVal = dataRows[y];
              var tx = y + 1;

              var dst = '';

              var deptId = parseInt(xVal.DepartmentId);

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
              else
              {
                  if (deptId === 5)
                  {
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
                  else {
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

              var imgStr = $('<table style="width: 100%;max-width: 100%; -moz-min-width: 100%; -ms-min-width: 100%; -o-min-width: 100%; -webkit-min-width: 100%; min-width: 100%; margin-top: 15px" id="tbMasterContent">'
                       + dst
                       + '<tr><td></td></tr></table></td></tr>'
                        + '<tr ><td colspan="3" style="border-top: 1px solid black"><table style="width: 100%"><tr><td style="width: 11%"><label style="margin-top: 5px; margin-left: 2%;color: black">Cheque #:</label></td>'
                         + '<td style="width: 71%"><label style="margin-top: 5px;font-weight: bold;color: black" id="lblChequeMultiple">' + xVal.ChequeNo + '</label></td><td style="width: 10%"><label style="margin-top: 5px; margin-left:1%; color: black">PVC #:</label></td>'
                          + '<td style="width: 10%; text-align: right"><label style="margin-top: -1%;font-weight: bold;color: black" id="lblPVCMultiple">' + xVal.PcvNo + '</label></td></tr>'
                           + '<tr><td colspan="2"></td><td ><label style="margin-top: 5px; margin-left:1%;color: black">Date: </label></td><td style="text-align: right"><label style="margin-top: 5px; font-weight: bold;color: black" id="lblDateMultiple">' + xVal.DatePaid + '</label></td></tr></table>'
                            + '<div id="dvLogo" style="border: 2px solid black; width: 25%; background-color: white; margin-left: 36%; height: 68px; margin-top: -7%">'
                             + '<img src="Images/Report/voucherLogo.gif" style="width: 100%; height: 100%" id="voucherLogo"/></div></td></tr>'
                              + '<tr><td colspan="3"><div style="width:100%"></div></td></tr><tr><td colspan="3"><div style="width:100%"></div></td></tr><tr><td colspan="3"><div style="width:100%"></div></td></tr><tr ><td colspan="3" ><table style="width: 100%; margin-top: 2%; border-bottom: 1px solid rgb(0, 128, 128); border-collapse:collapse" id="tblItemsGrp' + tx + '"><tr id="trHeader" style=" border-color:#008080; border: solid 1px;background-color:#008080">' 
                              + '<th scope="col" style=" width: 2%; color: white">S/No</th><th scope="col" style=" width: 7%; color: white">Code</th><th scope="col" style=" width: 20%; color: white">Item</th>'
                              + '<th scope="col" style=" width: 20%; color: white">Detail</th><th scope="col" style=" width: 8%; color: white">Quantity</th>'
                             + '<th scope="col" style=" width: 13%; color: white">Unit Price(N)</th><th scope="col" style=" width: 13%; color: white">Total Price(N)</th>'
                          + '</tr> </table></td></tr><tr><td colspan="3"></td></tr><tr><td colspan="3"><table style="width: 100%; margin-top: 2%"><tr><td style=" width: 60%; font-weight: bold;"><label style=" color: black">Amount in Words:</label></td>'
                          + '<td style=" width: 16%; font-weight: bold; border-bottom: 1px solid rgb(0, 128, 128)"><label style=" color: black;font-size:9pt">Total Amount:</label></td><td style=" width: 24%; font-weight: bold; text-align: right; color: rgb(0, 128, 128); border-bottom: 1px solid rgb(0, 128, 128)"><label id="lblTotalAmountMultiple">' + xVal.TotalApprovedAmmount + '</label></td></tr></table></td></tr>'
                          + '<tr><td colspan="3"><table style="width: 100%"><tr><td style=" width: 60%;color: black"><label id="lblAmountInWordsMultiple" style="font-weight: bold; width: auto; color: black; font-size:10pt">' + formatAmountInWords(numbersToWord(xVal.TotalApprovedAmmount, naira, kobo), naira) + '</label></td>'
                          + '<td style=" width: 16%; border-bottom: 1px solid rgb(0, 128, 128)"><label style="font-weight: bold; color : black; font-size:9pt">Amount Received:</label></td><td style=" width: 24%; font-weight: bold; text-align: right; color: rgb(0, 128, 128); border-bottom: 1px solid rgb(0, 128, 128)"><label style="font-weight: bold; font-size:10pt" id="lblAmountReceivedMultiple">' + xVal.AmmountPaid + '</label></td></tr></table></td></tr>'
                          + '<tr><td colspan="3"><table style="width:100%; margin-top: 2%"><tr><td style=" width: 78%"><label style="font-weight: bold; width: auto; border-bottom: 1px solid black; margin-left: 5%; font-style: italic; color: black" id="lblApproverMultiple">' + xVal.Approver + '</label></td><td style=" width: 12%">'
                          + '<div style="margin-left: 10%; width: 507px; text-align: center"><label style="font-weight: bold; border-bottom: 1px solid black; width: auto; font-style: italic; color: black" id="lblReceiverMultiple">' + xVal.ReceivedBy + '</label></div></td></tr>'
                          + '<tr><td style=" width: 78%"><label style=" margin-left: 10%; color: black">Approver</label></td style=" width: 12%"><td><label style="margin-left: 54%; color: black">Receiver</label></td> </tr></table></td></tr><tr><td colspan = "3"><table style="width:100%; margin-top: 3%"><tr><td style="width:100%; border-bottom-style: 1px dashed ;border-bottom-color: black; border-bottom: 1px dashed black;" id="tdLast"></td></tr></table></td></tr></table>');

              var tblId = "tblItemsGrp" + tx;

              //var deptId = parseInt(xVal.DepartmentId); 

              if (dvCollections.length < 1 && contentDiv.is(':empty')) 
              {
                  contentDiv.append(imgStr);

                  imgFunc(xVal.TransactionItems, tblId, tx);

                  //alert($('#tbMasterContent').outerHeight());

                  var gs = $('#dvPreviewMultiple [id^="tbMasterContent"]:last');

                  var gt = parseInt(gs.outerHeight());

                  var tk = parseInt(gs.css("margin-top"));

                  var xz = gt + tk;
                 
                  dvCollections.push(xz);
              }
              else
              {
                  tempDiv.append(imgStr);

                  $('#dvItemsView [id^="tblItemsGrp"]:last').attr("id", tblId);

                  var tblId1 = $('#dvItemsView [id^="tblItemsGrp"]:last').attr("id");

                  imgFunc(xVal.TransactionItems, tblId1, tx);

                  var totalHeight = 0;

                  for (var r = 0; r < dvCollections.length; r++)
                  {
                      totalHeight = totalHeight + dvCollections[r];
                  }

                  contentDiv = $('#dvPreviewMultiple [id^="content"]:last');

                  var tm = $('#' + contentDiv.attr("id") + ' [id^="tbMasterContent"]:last');

                  var h1 = parseInt(contentDiv.outerHeight());

                  var h2 = parseInt((tm).css("margin-top"));

                  var h3 = parseInt(tm.height());

                  var hz = h2 + h3;

                  var ht = totalHeight;

                  var hr = h1 - ht;

                  if (hr > hz)
                  {
                      tm.after(document.getElementById('dvItemsView').innerHTML);

                      $('#dvItemsView').empty();

                      dvCollections.push(hz);
                  }

                  else {

                      var theight = 0;

                      $('#' + contentDiv.attr("id") + ' [id^="tbMasterContent"]').each(function ()
                      {
                          theight += $(this).height();

                      });

                      $('#' + contentDiv.attr("id") + ' [id^="tdLast"]:last').css({'border-bottom': '1px solid black'});

                      contentDiv.css({ 'max-height': '', 'height': '' }); //delete attribute

                      contentDiv.css({ 'max-height': theight + 10 + 'px', 'height': theight + 10 + 'px', 'min-height': theight + 10 + 'px' }); //set max height
                      
                      var last = $('#dvPreviewMultiple [id^="content"]:last').attr("id");

                      $('#' + last).after($('<input id="btnPrint" class="customButton" type="button" value="Print" style="width: 115px; margin-left: 75%; margin-top: 6%" onclick="printVoucher(this.id)"></input><div style="height: 842px; min-height: 842px; width: 934px; max-width: 934px; -moz-min-width: 934px; -ms-min-width: 934px; -o-min-width: 934px; -webkit-min-width: 934px; min-width: 934px; border: 0 groove transparent; margin-top: 3%; page-break-after: always" id="content' + tx + '"></div>'));
                      
                      contentDiv = $('#dvPreviewMultiple [id^="content"]:last');

                      contentDiv.after($(''));

                      var ef = "btnPrint" + (tx);

                      $('#btnPrint').attr("id", ef);

                      contentDiv.append(imgStr);

                      var tblId2 = $('#' + contentDiv.attr("id") + ' [id^="tblItemsGrp"]:last').attr("id");

                      var gj = $('#' + contentDiv.attr("id") + ' [id^="tbMasterContent"]:last');

                      imgFunc(xVal.TransactionItems, tblId2, tx);

                      dvCollections = [];

                      var vtx = parseInt(gj.outerHeight()) + parseInt(gj.css("margin-top"));

                      dvCollections.push(vtx);
                      
                  }


              }

          }

          $('#dvPreview').fadeIn("slow");
          $('#btnBack').fadeIn("slow");
          $('#dvMultiVouchers').hide("slow");
      }

      function setId(zVal)
      {
          xguid = zVal.d;
      }

      function logoFunc(deptId, t)
      {

          if (deptId === 6) {
              var src = "Images/Report/xPlug.gif";
              $('#imgDeptLogo' + t).attr("src", src);
          }
          else {
              if (deptId === 5) {
                  var lrSrc = "Images/Report/Lr.png";
                  $('#imgDeptLogo' + t).attr("src", lrSrc);
              }
              else {
                  if (deptId !== 5 && deptId !== 6) {
                      var generalSrc = "Images/Report/xPlug.gif";
                      $('#imgDeptLogo' + t).attr("src", generalSrc);
                  }
              }
          }     
      }

      function imgFunc(xVal, tblId, t) 
      {
          if ($('#lblTotalAmountMultiple' + t).length < 1) 
          {
              $('#lblTotalAmountMultiple').attr("id", "lblTotalAmountMultiple" + t);
              $('#lblAmountReceivedMultiple').attr("id", "lblAmountReceivedMultiple" + t);
              $('#' + "lblTotalAmountMultiple" + t).formatCurrency({ symbol: 'N', roundToDecimalPlace: 2 });
              $('#' + "lblAmountReceivedMultiple" + t).formatCurrency({ symbol: 'N', roundToDecimalPlace: 2 });
            
              for (var i = 0; i < xVal.length; i++) 
              {

                  $($('#' + tblId + " " + "tbody:last").append($('<tr class="gridRowItem"><td align="left" style="width:4%; border-left: 1px solid rgb(0, 128, 128); border-right: 1px solid rgb(0, 128, 128)"><label class="xPlugTextAll_x" style="color: black">' + [i + 1] + '</label></td><td align="left" style="width:7%; border-left: 1px solid rgb(0, 128, 128);color: black; border-right: 1px solid rgb(0, 128, 128);" class="xPlugTextAll_x">' + xVal[i].ExpenseItem.Code
                        + '</td><td align="left" style="width:20%; border-left: 1px solid rgb(0, 128, 128); border-right: 1px solid rgb(0, 128, 128);color: black" class="xPlugTextAll_x">' + xVal[i].ExpenseItem.Title + '</td><td align="left" style="width:20%; border-left: 1px solid rgb(0, 128, 128);color: black; border-right: 1px solid rgb(0, 128, 128);" class="xPlugTextAll_x">' + xVal[i].Description + '</td><td align="center" style="width:8%; border-left: 1px solid rgb(0, 128, 128); border-right: 1px solid rgb(0, 128, 128);color: black; text-align: center" class="xPlugTextAll_x">'
                        + xVal[i].ApprovedQuantity + '</td><td align="left" style="width:13%; border-left: 1px solid rgb(0, 128, 128);color: black; border-right: 1px solid rgb(0, 128, 128);text-align: right"><label id ="PriceLabel" class="xPlugTextAll_x" style="color: black">' + xVal[i].ApprovedUnitPrice
                        + '</label></td><td align="left" style="width:13%; border-left: 1px solid rgb(0, 128, 128); border-right: 1px solid rgb(0, 128, 128); text-align: right" class="xPlugTextAll_x"><label id ="TotalPriceLabel" class="xPlugTextAll_x" style="color: black">' + xVal[i].ApprovedTotalPrice + '</label></td></tr>')));
                        
                  $('#' + tblId + ' #TotalPriceLabel').attr("id", "TotalPriceLabel" + i);
                  $('#' + tblId + ' #PriceLabel').attr("id", "PriceLabel" + i);
                  $('#' + tblId + ' #TotalPriceLabel' + i).formatCurrency({ symbol: '', roundToDecimalPlace: 2 });
                  $('#' + tblId + ' #PriceLabel' + i).formatCurrency({ symbol: '', roundToDecimalPlace: 2 });
              }
          }

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


      function formatAmountInWords(totalPrice, naira) 
      {

          if (totalPrice.indexOf("Naira") != -1)
          {
              return totalPrice;
          }
          else
          {
              return totalPrice + "  " + naira;
          }

      }

      function printVoucher(id) 
      {
          var idString = id.split('t')[2];
          var containerDv = "content" + idString;
           var tempTarget = $('#dvItemsView');
            tempTarget.empty();
         $('#' + containerDv).clone().appendTo(tempTarget);
          var logoCollection = $('#dvItemsView [id^="dvLogo"]');
        
          $('#dvItemsView [id^="tbMasterContent"]').each(function () 
          {
              $(this).css({ 'margin-left': "auto", 'padding-top': "0" });
          });
          
          $.each(logoCollection, function () 
          {
              $(this).css({ 'margin-top': "-9%" });
          });
      
        var winpops = window.open('', "Transaction Voucher", "fullscreen=no,toolbar=no,status=yes, " +
                                   "menubar=no,scrollbars=yes,resizable=yes,directories=yes,location=no, " +
                                 "width=900,height=500,left=100,top=100,screenX=100,screenY=100");
                                  winpops.document.write(tempTarget.html());
                                  $('#dvItemsView').empty();

      }



      function printMultiVouchers()
      {
          var idString = id.split('t')[2];
          var containerDv = "content" + idString;
          var tempTarget = $('#dvItemsView');
          tempTarget.empty();
          $('#' + containerDv).clone().appendTo(tempTarget);
          var logoCollection = $('#dvItemsView [id^="dvLogo"]');

          $('#dvItemsView [id^="tbMasterContent"]').each(function ()
          {
              $(this).css({ 'margin-left': "auto", 'padding-top': "0" });
          });

          var tyx = $('#dvItemsView [id^="tbMasterContent"]:last');

          $('#' + tyx.attr('id') + ' #tdLast').css({ 'border-bottom-style': '', 'border-bottom': '' });

          $('#' + tyx.attr('id') +' #tdLast').css({ 'border-bottom-style': 'dashed', 'border-bottom-color': 'black', 'border-bottom': ' dashed black' });

          $.each(logoCollection, function ()
          {
              $(this).css({ 'margin-top': "-9%" });
          });

          var winpops = window.open('', "Transaction Voucher", "fullscreen=no,toolbar=no,status=yes, " +
                                   "menubar=no,scrollbars=yes,resizable=yes,directories=yes,location=no, " +
                                 "width=900,height=500,left=100,top=100,screenX=100,screenY=100");
          winpops.document.write(tempTarget.html());
          $('#dvItemsView').empty();

      }