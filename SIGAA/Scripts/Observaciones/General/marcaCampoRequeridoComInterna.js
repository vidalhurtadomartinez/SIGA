$(document).ready(function () {
    var $div = $('[type!=\'hidden\'][data-val-required]');//cached
    $div.parent().parent().after('<span style="color:red; font-size: 20px; vertical-align: middle;">*</span>');
});
