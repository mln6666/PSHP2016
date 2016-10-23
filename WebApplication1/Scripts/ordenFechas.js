function ordenFechas() {
    if( ($('#from').val()!="") &($('#to').val()!="") ){
        if ($('#from').val().trim() > $('#to').val().trim() ) {
            $('#spanOrdenFechas').text("Rango de fechas incorrecto, la fecha de inicio supera a la de fin.");
            event.preventDefault();
        }else{
            $('#spanOrdenFechas').text("");
        }
    }
    }