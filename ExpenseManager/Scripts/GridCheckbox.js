function CheckAllDataGridCheckBoxes(aspCheckBoxID, checkVal, dgGrid) {
       
		 //re = new RegExp(':' + aspCheckBoxID + '$')  //generated control name starts with a colon

        for(i = 0; i < document.forms[0].elements.length; i++) {

            elm = document.forms[0].elements[i]
            
            if ((elm.type == 'checkbox') && (elm.name.indexOf(dgGrid) > -1)){

                //if (re.test(elm.name)) {

                    elm.checked = checkVal;

               // }
            }
        }
    }

    function CheckAllDataGridCheckBoxes2(aspCheckBoxID, checkVal, dgGrid) {

        //re = new RegExp(':' + aspCheckBoxID + '$')  //generated control name starts with a colon
               
        for (i = 0; i < document.forms[0].elements.length; i++) {

            elm = document.forms[0].elements[i]

            if ((elm.type == 'checkbox') && (elm.name.indexOf('$dgTableDescriptor') > -1)) {

                //if (re.test(elm.name)) {

                elm.checked = checkVal;

                // }
            }
        }
    }
    function toggleSideDiv(divName){
			 //alert(divName);
			if(document.getElementById(divName) != window.undefined){
				if(document.getElementById(divName).style.display == "none"){
						// alert('check 1');
					document.getElementById(divName).style.display="Block";
					//change clicked Image to Collapsed -collapsed
					if(document.getElementById("img"+divName)!= window.undefined){
					      document.getElementById("img"+divName).src = "Images/expanded.gif"
					   }
				}else { 
                     document.getElementById(divName).style.display="none";
						//change clicked Image to Collapsed -expanded
						if(document.getElementById("img"+divName)!= window.undefined){
							  document.getElementById("img"+divName).src = "Images/collapsed.gif"
					     }
                     
					 }
					 
				
			 }
		
		 }