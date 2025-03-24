function filterTable() {
    var input = document.getElementById("filterInput");
    var filter = input.value.toUpperCase();
    var table = document.getElementById("Timesheets") || document.getElementById("Available-Shifts");
    
    if (!table) return;
    
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
            "recruiter": 6,
            "student id": 2,
            "student name": 3,
            "visa restriction": 4,
            "start": 5,
            "end": 6,
            "approved hours": 7,
            "duration": 8
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
                if (td[j] && (td[j].innerText || td[j].textContent).toUpperCase().indexOf(filter) > -1) {
                    match = true;
                    break;
                }
            }

            tr[i].style.display = match ? "" : "none";
        }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    // Handle notification clicks
    const notifications = document.querySelectorAll('.notification-item');
    notifications.forEach(function (notification) {
        notification.addEventListener('click', function () {
            notification.classList.add('read');
            notification.classList.remove('unread');
        });
    });

    // Initialize tab functionality
    const tabToggles = document.querySelectorAll('.tab-toggle');
    tabToggles.forEach(toggle => {
        toggle.addEventListener('change', function() {
            const tabContainer = this.closest('.box');
            const tabContents = tabContainer.querySelectorAll('.tab-content');
            tabContents.forEach(content => content.style.display = 'none');
            
            const activeContent = tabContainer.querySelector(`.tab-content:nth-child(${this.id.replace('tab', '')})`);
            if (activeContent) {
                activeContent.style.display = 'block';
            }
        });
    });

    // Trigger first tab to show initially
    const firstTabs = document.querySelectorAll('.tab-toggle:first-child');
    firstTabs.forEach(tab => {
        const tabContainer = tab.closest('.box');
        const firstContent = tabContainer.querySelector('.tab-content:first-child');
        if (firstContent) {
            firstContent.style.display = 'block';
        }
    });
});
