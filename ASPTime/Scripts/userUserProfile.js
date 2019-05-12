var categoryName;



function editAjaxSuccess(data) {
    alert('success');
    return false;
}

function editAjaxError(jqXHR, exception) {
    alert('error');
}

$('button#editActionBtn').click(function (e) {
    $('input#editInputHidden').val(categoryName);
});

$('[data-type="list-btn"]').click(function (e) {
    categoryName = $('#' + e.target.id).text();
});

$('#categoryAction').on('show.bs.modal', function (e) {
    $('#Action').text(categoryName);
});

$('#categoryEdit').on('show.bs.modal', function (e) {
    $('#EditInput').val(categoryName);
    $('#categoryAction').modal('hide');
});

$('#categoryDelete').on('show.bs.modal', function (e) {
    $('#Delete').text('Deleting category : ' + categoryName);
    $('#categoryAction').modal('hide');
});

var ctx = document.getElementById('test').getContext('2d');
var ctx1 = document.getElementById('test1').getContext('2d');
var ctx2 = document.getElementById('test2').getContext('2d');
var ctx3 = document.getElementById('test3').getContext('2d');

var chType = 'pie';

var dayData = {
    data: [3, 5, 7, 9],
    backgroundColor: ['#F0A0B0', '#284068', '#88B8B0', '#A8E8E0']
}

var chart = new Chart(ctx, {

    type: chType,

    data: {
        labels: ["чтение", "программирование", "workout", "none"],
        datasets: [dayData]
    }
});

var chart1 = new Chart(ctx1, {

    type: chType,

    data: {
        labels: ["чтение", "программирование", "workout", "none"],
        datasets: [dayData]
    }
});
var chart2 = new Chart(ctx2, {

    type: chType,

    data: {
        labels: ["чтение", "программирование", "workout", "none"],
        datasets: [dayData]
    }
});
var chart3 = new Chart(ctx3, {

    type: chType,

    data: {
        labels: ["чтение", "программирование", "workout", "none"],
        datasets: [dayData]
    }
});