var categoryName;



function editAjaxSuccess(data) {
    alert('success');
    return false;
}

function editAjaxError(jqXHR, exception) {
    alert('error');
}

$('#categoryAction').on('show.bs.modal', function (e) {
    $('#Action').text(categoryName);
})

$('#category-list').on('click', '[data-type="list-btn"]', function (e) {
    categoryName = $('#' + e.target.id).text();
});

$('button#editActionBtn').click(function (e) {
    $('input#editInputHidden').val(categoryName);
});

$('#categoryEdit').on('show.bs.modal', function (e) {
    $('#EditInput').val(categoryName);
    $('#categoryAction').modal('hide');
});

$('#categoryDelete').on('show.bs.modal', function (e) {
    $('#Delete').text('Удалить категорию ' + categoryName + '?');
    $('#categoryAction').modal('hide');
});

