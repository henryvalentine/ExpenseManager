
function validateDate(dateFrom, dateTo)
{
    if (dateFrom === "" || dateFrom === null)
    {
        alert("Please enter valid dates in this format (dd/mm/yyyy)");
        return false;
    }

    if (dateTo === "" || dateTo === null)
    {
        alert("Please enter valid dates in this format (dd/mm/yyyy)");
        return false;
    }

    if (dateFrom !== '' && dateTo !== '') 
    {
      
        try {
            var datefrom = dateFrom.split('/');
            var dateto = dateTo.split('/');
            if (datefrom[0].length != 2 || datefrom[1].length != 2 || datefrom[2].length != 4 || dateto[0].length != 2 || dateto[1].length != 2 || dateto[2].length != 4) 
            {
                alert("Please enter valid dates in this format (dd/mm/yyyy)");
                return false;
            }

            for (var i = 0; i < datefrom.length; i++) 
            {
                if (/Invalid|NaN/.test(parseInt(datefrom[i]))) 
                {
                    alert("The 'FROM' date is invalid!");
                    return false;
                }
            }

            for (var j = 0; j < dateto.length; j++)
            {

                if (/Invalid|NaN/.test(parseInt(dateto[j]))) 
                {
                    alert("The 'TO' date is invalid!");
                    return false;
                }
            }
        }
        catch (e) {

            alert("Please supply valid dates!");
            return false;
        }
    }
    return true;
}

function validateDate(date) 
{

    if (date === '' || date === null) 
    {
        alert("Please enter a valid date in this format (dd/mm/yyyy)");
        return false;
    }
    
    if (date !== '')
    {
        try {
            var datefrom = date.split('/');
            if (datefrom[0].length != 2 || datefrom[1].length != 2 || datefrom[2].length != 4)
            {
                alert("Please enter valid dates in this format (dd/mm/yyyy)");
                return false;
            }

            for (var i = 0; i < datefrom.length; i++) 
            {
                if (/Invalid|NaN/.test(parseInt(datefrom[i]))) 
                {
                    alert("Please supply a valid date!");
                    return false;
                }
            }
        }
        catch (e) {

            alert("Invalid date supplied!");
            return false;
        }
    }
    return true;
}