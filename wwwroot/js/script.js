function filterTable() {
    var input = document.getElementById("filterInput");
    var filter = input.value.toUpperCase();
    var table = document.getElementById("Timesheets");
    var tr = table.getElementsByTagName("tr");

    if (filter === '') {
        for (var i = 1; i < tr.length; i++) {
            tr[i].style.display = "";
        }
        return;
    }

    var filterParts = filter.split(":").map(function (part) {
        return part.trim();
    });

    if (filterParts.length === 2) {
        var column = filterParts[0];
        var value = filterParts[1];

        var columnIndexMap = {
            "date": 0,
            "shift id": 1,
            "available shifts": 2,
            "start time": 3,
            "end time": 4,
            "total hours": 5,
            "recruiter": 6
        };

        var columnIndex = columnIndexMap[column.toLowerCase()];

        if (columnIndex !== undefined) {
            for (var i = 1; i < tr.length; i++) {
                var td = tr[i].getElementsByTagName("td");

                if (td && td[columnIndex]) {
                    var cellValue = td[columnIndex].innerText || td[columnIndex].textContent;
                    if (cellValue.toUpperCase().indexOf(value.toUpperCase()) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        } else {
            for (var i = 1; i < tr.length; i++) {
                tr[i].style.display = "none";
            }
        }
    } else {
        for (var i = 1; i < tr.length; i++) {
            var td = tr[i].getElementsByTagName("td");
            var match = false;

            for (var j = 0; j < td.length; j++) {
                if (td[j] && td[j].innerText.toUpperCase().indexOf(filter) > -1) {
                    match = true;
                    break;
                }
            }

            tr[i].style.display = match ? "" : "none";
        }
    }
}


function handleAccept(button) {
    var row = button.closest("tr");
    row.classList.remove("declined");
    row.classList.add("accepted");

    button.disabled = true;
    var declineButton = row.querySelector(".decline-button");
    declineButton.disabled = true;
}

function handleDecline(button) {
    var row = button.closest("tr");
    row.classList.remove("accepted");
    row.classList.add("declined");

    button.disabled = true;
    var acceptButton = row.querySelector(".accept-button");
    acceptButton.disabled = true;
}

document.addEventListener('DOMContentLoaded', function () {
    const notifications = document.querySelectorAll('.notification-item');
    notifications.forEach(function (notification) {
        notification.addEventListener('click', function () {
            notification.classList.add('read');
            notification.classList.remove('unread');
        });
    });
});