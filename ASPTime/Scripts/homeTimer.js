function msToTime(duration) {
    var milliseconds = parseInt((duration % 1000) / 100),
        seconds = parseInt((duration / 1000) % 60),
        minutes = parseInt((duration / (1000 * 60)) % 60),
        hours = parseInt((duration / (1000 * 60 * 60)) % 24);

    hours = (hours < 10) ? "0" + hours : hours;
    minutes = (minutes < 10) ? "0" + minutes : minutes;
    seconds = (seconds < 10) ? "0" + seconds : seconds;

    return hours + " ч. " + minutes + " м.";
}

var selectedDate = null;

$('#addTimeBtn').click(function (e) {
    var isValid = true;
    var message = '<ul>';
    if ($('#timepicker1').val() == $('#timepicker2').val()) {
        message = message + '<li>Селекторы даты не должны быть равны</li>';
        isValid = false;
    }

    if ($('#selectOption').val() == 'Choose...') {
        message = message + '<li>Должен быть выбран другой пункт в выподающем меню</li>';
        isValid = false;
    }

    if (selectedDate == null) {
        message = message + '<li>Должна быть выбрана дата</li>';
        isValid = false;
    }

    if (isValid) {
        $('div.alert').addClass('d-none');
        $('#addTimeModal').modal('hide');

        $('#selectOption').val('Choose...');

        // инициализация слекторов времени
        $('#timepicker1').timeDropper({
            autoswitch: true,
            format: 'HH:mm',
            mousewheel: true
        });

        $('#timepicker2').timeDropper({
            autoswitch: true,
            format: 'HH:mm',
            mousewheel: true
        });

        $('#timeinterval').text('');

        selectedDate = null;

        var datepicker = $('#datepicker').datepicker().data('datepicker');
        datepicker.destroy();

        picker.datepicker({
            onSelect: function (fd, date) {
                selectedDate = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
            }
        });

    }
    else {
        $('div.alert').children('ul').remove();
        $('div.alert').removeClass('d-none');
        $('div.alert').append(message + '</ul>')
    }

});

$(document).ready(function () {

    $('#timepicker1').timeDropper({
        autoswitch: true,
        format: 'HH:mm',
        mousewheel: true
    });

    $('#timepicker2').timeDropper({
        autoswitch: true,
        format: 'HH:mm',
        mousewheel: true
    });

    picker = $('#datepicker');
    picker.datepicker({
        onSelect: function (fd, date) {
            selectedDate = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
        }
    });

    var timer = new easytimer.Timer();

    timer.addEventListener('secondsUpdated', function (e) {
        timeValue = timer.getTimeValues().toString();
        $('.clock').html(timer.getTimeValues().toString());
    });

    timer.addEventListener('started', function (e) {
        timeValue = timer.getTimeValues().toString();
        $('.clock').html(timer.getTimeValues().toString());
    });

    $('.td-overlay').on('touchend mouseup', function () {
        var time = $('#timepicker1').val();
        var time1 = '2012-01-26T' + time + ':00.000-07:00';
        var date1 = Date.parse(time1);
        var date2 = Date.parse('2012-01-26T' + $('#timepicker2').val() + ':00.000-07:00');
        var result = date2 - date1;
        if (date2 < date1) {
            result = result + 86400000;
        }
        $('#timeinterval').text(msToTime(result));
        console.log('timedropper closed');
    });

    var timeValue;
    var flag = true;
    $('.button-style').click(function (e) {
        if (flag) {
            timer.start();
            flag = false;
            $('.button-style').text('stop');
        } else {
            timer.stop();
            flag = true;
            var timeArray = timeValue.split(':');
            $('#body-text').text('Вы работали: ' + timeArray[0] + ' ч. ' + timeArray[1] + ' м. ' + timeArray[2] + ' с. ');
            $('#saveModal').modal('show');
            $('.button-style').text('start');
            $('.clock').text('00:00:00');
        }
    });
});