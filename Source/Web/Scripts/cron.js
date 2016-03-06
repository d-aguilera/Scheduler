function CronControlHandler() {

    function updateCronControl() {
        var $cronExpression = $("#CronExpression");
        var parts = $cronExpression.val().split(' ');
        updatePart('Minute', parts[0]);
        updatePart('Hour', parts[1]);
        updatePart('Day', parts[2], 1);
        updatePart('Month', parts[3], 1);
        updatePart('Weekday', parts[4]);
    }

    function updatePart(partName, value, minValue) {

        minValue = parseInt(minValue || 0);

        var numbers = [];
        var ranges = value.split(',');
        for (var i = 0; i < ranges.length; i++) {
            var values = ranges[i].split('-');
            if (values.length > 1) {
                var min = parseInt(values[0]);
                var max = parseInt(values[1]);
                for (var j = min; j <= max; j++) {
                    numbers.push(j);
                }
            }
            else if (values.length > 0) {
                numbers.push(parseInt(values[0]));
            }
        }

        var $divs = $("#CronExpression" + partName + " > div > div");
        $divs.each(function (index, div) {
            if (numbers.indexOf(index + minValue) < 0)
                $(div).removeClass('bg-info');
            else
                $(div).addClass('bg-info');
        });
    }

    function updateExpression() {
        var expr =
            getPartExpression('Minute') + ' ' +
            getPartExpression('Hour') + ' ' +
            getPartExpression('Day', 1) + ' ' +
            getPartExpression('Month', 1) + ' ' +
            getPartExpression('Weekday');

        $("#CronExpression").val(expr);
        $("#CronExpressionDisplay").text(expr);

        updateCronControl();
    }

    function getPartExpression(partName, minValue) {
        // Expression results in a comma-separated list of ranges (no whitespace).
        // Range format is minValue-maxValue, but if maxValue equals minValue, range is just minValue
        // Example:
        //      Numbers:    10, 15, 16, 17, 18, 19, 20, 21, 23, 24, 25, 28
        //      Expression: 10,15-21,23-25,28

        minValue = parseInt(minValue || 0);

        var numbers = [];
        var allSelected = true;
        $("#CronExpression" + partName + " > div > div").each(function (index, div) {
            var $div = $(div);
            if ($div.hasClass('bg-info'))
                numbers.push(index + minValue);
            else
                allSelected = false;
        });
        var noneSelected = numbers.length === 0;

        if (noneSelected || allSelected) {
            return '*';
        }

        var list = [];
        var range = [numbers[0], numbers[0]];
        for (var i = 1; i < numbers.length; i++) {
            if (numbers[i] === range[1] + 1) {
                range[1] = numbers[i];
            }
            else {
                list.push(range);
                range = [numbers[i], numbers[i]];
            }
        }
        list.push(range);

        var expr = '';
        for (var i = 0; i < list.length; i++) {
            var range = list[i];
            expr += ',' + range[0];
            if (range[1] > range[0]) {
                expr += '-' + range[1];
            }
        }
        return expr.substr(1);
    }

    function bindClick(partName) {
        $("#CronExpression" + partName + " > div > div").click(function (eventObject) {
            var $div = $(this);
            if ($div.hasClass('bg-info'))
                $div.removeClass('bg-info');
            else
                $div.addClass('bg-info');
            updateExpression();
        });
    }

    $(function () {
        bindClick('Minute');
        bindClick('Hour');
        bindClick('Day');
        bindClick('Month');
        bindClick('Weekday');

        updateCronControl();
    });
}
