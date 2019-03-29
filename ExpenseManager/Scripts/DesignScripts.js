// JScript File
//Scripts in this file are used for interface designs only

    
    
    /* Open / Close Menu Divs
       //Javascript controls the div
       //Seyi Adegboyega
       //2007-09-12 8:25AM
    */


    function OpenCloseMenuDiv(div){
        if(document.getElementsByName("MenuControl")[0].value !="" && document.getElementsByName("MenuControl")[0].value.length >0 && document.getElementsByName("MenuControl")[0].value !=div){
                document.getElementById(document.getElementsByName("MenuControl")[0].value).style.display="none";
                document.getElementsByName("MenuControl")[0].value=div;
        }else{
            document.getElementsByName("MenuControl")[0].value=div;
        } 
        //fire event
        if(document.getElementById(div) != window.undefined){
                if (document.getElementById(div).style.display=='block')
                   {document.getElementById(div).style.display='none'
                }else{document.getElementById(div).style.display='block'}
          }
}


    /* Menu control Center
       //Javascript controls the mouse over and mouse out events
       //Seyi Adegboyega
       //2007-09-12 8:25AM
    */
    
    var agt=navigator.userAgent.toLowerCase();
	var is_safari = agt.indexOf("safari")!=-1;
	
   
	function bi(link,div,name){
		var s = document.write ("<tr><td onClick=\"lnk('"+link+"');\" onMouseOver=\"menuovr('"+div+"','"+pn(name)+"',this);\" onMouseOut=\"menuout('"+div+"',this);\" class=\"nav2\">"+name+"</td></tr>");
	}
	function pn(name){
		var s=name;if(s.indexOf("<")>0) {return(s.slice(0,s.indexOf("<")-1));}return(s);
	}
	function lnk(link){
		internal_clicked = true;
		window.open(link,'_self');
	}
	function mainovr(div,status){
		if(is_safari){
			bHover=status;window.status=status;}
		else{
			bHover=status;window.status=status;}
	}
	function mainout(div){
		bHover='';window.status='';
	}
	function menuovr(link, item){
		window.status=link;item.style.backgroundColor='#CCCCCC';
	}
	function imgovr(div){
	}
	function menuout(item){
		item.style.backgroundColor='#F0F0F0';
	}
	function findDiv(n, d) { 
		var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
	    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
	  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
	  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=findDiv(n,d.layers[i].document);
	  if(!x && document.getElementById) x=document.getElementById(n); return x;
	}
	function tNav() { 	
		var i,p,v,obj,args=tNav.arguments;
	  for (i=0; i<(args.length-2); i+=3) if ((obj=findDiv(args[i]))!=null) { v=args[i+2];
	    if (obj.style) { obj=obj.style; v=(v=='show')?'visible':(v='hide')?'hidden':v; }
	    obj.visibility=v; }    
	}
	function hideDDs(sect) {
		tNav('AbtUs','','hide');tNav('trans','','hide');tNav('Resv','','hide');tNav('Auto','','hide');tNav('spsr','','hide');tNav('prdt','','hide');tNav('bplc','','hide');tNav('wwdd','','hide');showElement('SELECT');showElement('OBJECT');showElement('EMBED');
	}
	function hideDD(sect) {
		tNav(sect,'','hide');hideDDs(sect);
	}
	var secSet = null;
	var activeTimer = null;
	var bHover = '';
	var img = '';
	function setDD(sect, windowStatusVal, bshow) {
		hideDDs(sect);
		if ((bHover==windowStatusVal)||(bshow)){
			tNav(sect,'','show');
			if (secSet != null) window.clearTimeout(secSet);		
			hideElement('SELECT', document.all[sect]); 
			hideElement('OBJECT', document.all[sect]); 
			hideElement('EMBED', document.all[sect]); 
		}
	}
	function setDDTimeout(sect){
		if (secSet != null) window.clearTimeout(secSet);
		secSet = window.setTimeout('hideDD("' + sect + '")',500);
	}

	function hideElement( elmID, overDiv ) {
	  if(document.all) {
	    for(i = 0; i < document.all.tags( elmID ).length; i++) {
	      obj = document.all.tags( elmID )[i];
	      if(!obj || !obj.offsetParent) continue;
	      // Find the element's offsetTop and offsetLeft relative to the BODY tag.
	      objLeft   = obj.offsetLeft - overDiv.offsetParent.offsetLeft;
	      objTop    = obj.offsetTop;
	      objParent = obj.offsetParent;
	      while(objParent.tagName.toUpperCase() != 'BODY') {
	        objLeft  += objParent.offsetLeft;
	        objTop   += objParent.offsetTop;
	        objParent = objParent.offsetParent;}
	      objHeight = obj.offsetHeight;
	      objWidth  = obj.offsetWidth;

    //alert(objLeft + "\n" + objTop + "\n" + objParent + "\n" + objHeight+ "\n" + objWidth);	      
    //alert(overDiv.offsetLeft + "\n" + overDiv.offsetWidth + "\n" + overDiv.offsetParent.offsetTop + "\n" + overDiv.offsetHeight + "\n" + overDiv.offsetTop + "\n" + overDiv.offsetLeft);	      

	      if((overDiv.offsetLeft + overDiv.offsetWidth) <= objLeft);
	      else if((overDiv.offsetParent.offsetTop + overDiv.offsetHeight + 20) <= objTop);
	      else if(overDiv.offsetParent.offsetTop >= eval(objTop + objHeight));
	      else if(overDiv.offsetLeft >= eval(objLeft + objWidth));
	      else {
	        obj.style.visibility = 'hidden';
	      }

	    }
	  }
	}

	function showElement(elmID) {
	  if(document.all) {
	    for(i = 0; i < document.all.tags( elmID ).length; i++) {
	      obj = document.all.tags(elmID)[i];
	      if(!obj || !obj.offsetParent) continue;
	      obj.style.visibility = '';
	    }
	  }
	}
//End Of Menu Control Center
