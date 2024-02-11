
//$("#VireReportPop").click(function () {
//    //cmbCourse = "EE3040001000011F201803A"
//    debugger
//    var a = $('#cmbSemID option:selected').text();
//    if (a == "--Select Course--") {
//        alert("Select Course");
//    } else {
//        var CourseID = "0";

//        if (CourseID != null) {
//            var url = "http://localhost:46339/WebForm/CLOReports.aspx?c_id=" + CourseID + "&comid=" + CompID + "&semid=" + SemID + "&SectionID=" + SectionID;
//            var url = "http://localhost:46339/WebForm/CLOReports.aspx?c_id=" + CourseID;

//            var windowName = "Change Logo";

//            newwindow = window.open(url, windowName, 'width=1000');

//            if (window.focus) { newwindow.focus() }

//            return false;
//        } else {
//            toast("Please select a course first");
//        }
//    }
//});

LoadAllData();
function LoadAllData() {
    $.ajax({
        type: "POST",
        url: "/Default/GetAllQueryToTable",
        data: {},
        success: function (data) {

            ///alert(data[0].Subject);
            // $("table thead").html("");
            $("table tbody").html("");
            for (var i = 0; i < data.length; i++) {
                $("table tbody").append("<tr id='" + data[i].Q_ID + "' onclick='LoadToTextBox(this.id)'>" +
                    "<td>" + (i + 1) + "</td>" +
                    //"<td>" + data[i].ASSIGN_TIME.Hours + ":" + data[i].ASSIGN_TIME.Minutes + "</td>" +
                    "<td>" + data[i].NAME + "</td>" +
                    "<td>" + EmptyStatus(data[i].ASSIGN_TIME) + "</td>" +
                     "<td>" + EmptyStatus(data[i].SOLVE_TIME) + "</td>" +
                    //"<td>" + data[i].SOLVE_TIME + "</td>" +
                    "<td>" + data[i].DESCRIPTION + "</td>" +
                     "<td>" + EmptyStatus(data[i].COMMENTS) + "</td>" +
                    "<td>" + data[i].OFFICE + "</td>" +
                    "<td>" + ConvertStatus(data[i].STATUS) + "</td>" +
                  "</tr>");
            }
        },
        error: function (er) {
            alert(er);
        }
    });
}

function ConvertStatus(Status) {
    if (Status == false) {
        return "<label style='background:red; border-radius:5px; color:white; padding:2px;'>Pending</label>";
    } else {
        return "<label style='background:green; border-radius:5px; color:white; padding:2px;'>Solved</label>";
    }
}

function ConvertDate(value) {
    if (value == null) {
        return "";
    }
    else {
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        var completedate = dt.getDate() + "-" + monthNames[dt.getMonth()] + "-" + dt.getFullYear();
        //"/" + dt.getFullYear().toString().substr(2,2));
        return completedate;
    }
}

function EmptyStatus(id) {
    if (id == null) {
        return "";
    } else {
        return id;
    }
}