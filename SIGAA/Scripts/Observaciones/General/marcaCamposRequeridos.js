$(document).ready(function () {
   var $div = $('[type!=\'hidden\'][data-val-required]');//cached
   $div.parent().after('<span style="color:red; font-size: 15px; vertical-align: middle;">*</span>');
});
