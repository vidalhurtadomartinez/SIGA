$(document).ready(function () {
       $(function () {
           $('input[type="date"]').datetimepicker({
                    locale: 'es',
                    format: 'L'
                    //format: 'L' //solo muestra calendario
                    //format: 'LTS' mustra hora minuto segunfo
                    //format: 'LT' //muestra hora
                });
            });

       $(function () {
           $('input[type="time"]').datetimepicker({
               format: 'LT' //muestra hora
           });
       });
});
