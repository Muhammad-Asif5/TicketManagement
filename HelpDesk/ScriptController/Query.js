document.getElementById('Office').disabled = true;
document.getElementById('Description').disabled = true;

$("#VireReportPop").click(function () {
    //cmbCourse = "EE3040001000011F201803A"

    var a = $('#cmbSemID option:selected').text();
    if (a == "--Select Course--") {
        alert("Select Course");
    } else {
        //var cmbCourse = $("#cmbSemID").val();
        var CourseID = "0";
        //var CompID = cmbCourse.substring(8, 14);
        //var SemID = cmbCourse.substring(14, 22);
        //var SectionID = cmbCourse.substring(22);
        if (CourseID != null) {
            //var url = "http://localhost:46339/WebForm/CLOReports.aspx?c_id=" + CourseID + "&comid=" + CompID + "&semid=" + SemID + "&SectionID=" + SectionID;
            var url = "http://localhost:46339/WebForm/CLOReports.aspx?c_id=" + CourseID;

            var windowName = "Change Logo";

            newwindow = window.open(url, windowName, 'width=1000');

            if (window.focus) { newwindow.focus() }

            return false;
        } else {
            toast("Please select a course first");
        }
    }
});

LoadAllData();
function LoadAllData() {

    $.ajax({
        type: "POST",
        url: "/Query/GetAllQueryToTable",
        data: {},
        success: function (data) {

            ///alert(data[0].Subject);
            //$("table thead").html("");
            $("table tbody").html("");
            for (var i = 0; i < data.length; i++) {
                $("table tbody").append("<tr id='" + data[i].STATUS + data[i].Q_ID + "' onclick='LoadToTextBox(this.id)'>" +
                    "<td>" + (i + 1) + "</td>" +
                    //"<td>" + data[i].ASSIGN_TIME.Hours + ":" + data[i].ASSIGN_TIME.Minutes + "</td>" +
                    "<td>" + data[i].USER_ID + "</td>" +

                    //"<td>" + data[i].SOLVE_TIME + "</td>" +
                    "<td>" + data[i].DESCRIPTION.substring(0, 10) + "</td>" +
                    "<td>" + data[i].OFFICE.substring(0, 10) + "</td>" +
                    "<td>" + EmptyStatus(data[i].ASSIGN_TIME) + "</td>" +
                    "<td>" + ConvertStatus(data[i].STATUS) + "</td>" +
                    "<td><button class='btn btn-primary btn-sm'>Select</button></td>" +
                    "<td><button onclick='SolveQuery(this.id)' id='sol" + data[i].STATUS + data[i].Q_ID + "' class='btn btn-success btn-sm'>Solve Query</button></td>" +
                  "</tr>");
                if (data[i].STATUS == "1") {
                    $("#sol" + data[i].STATUS + data[i].Q_ID).prop('disabled', true);
                }
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

function LoadToTextBox(id) {


    var Status = id.substring(0, 1);
    var qId = id.substring(1);

    if (Status == 1) {
        $("#Employee").prop('disabled', true);
        $("#AssignBtn").prop('disabled', true);
        $("#reset").prop('disabled', true);
        //alert(Status);
    }
    else {
        $("#Employee").prop('disabled', false);
        $("#AssignBtn").prop('disabled', false);
        $("#reset").prop('disabled', false);
    }
    $.ajax({
        type: "POST",
        url: "/Query/LoadToTextBoxData",
        data: {
            IssueID: qId
        },
        success: function (data) {
            document.getElementById('Office').disabled = true;
            document.getElementById('Description').disabled = true;
            var IssueNo = $("#IssueNo").text(data[0].Q_ID);
            var Employee = $("#Employee").val(data[0].EMP_ID);
            var Description = $("#Description").val(data[0].DESCRIPTION);
            var Office = $("#Office").val(data[0].OFFICE);

        },
        error: function (er) {
            alert(er);
        }
    });
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
document.getElementById('circle').style.display = 'none'; //Not Visible
function SaveItem() {

    var IssueNo = $("#IssueNo").text();
    var Employee = $("#Employee").val();
    var Description = $("#Description").val();
    var Office = $("#Office").val();

    if (Employee != "" && Description != "" && Office != "") {
        document.getElementById('AssignBtn').disabled = true;

        document.getElementById('circle').style.display = 'block';//Visible
        $.ajax({
            type: "POST",
            url: "/Query/SaveItem",
            data: {
                IssueNo: IssueNo,
                EmployeeNo: Employee,
                Description: Description,
                Office: Office
            },
            success: function (data) {

                if (data == "false") {
                    window.location.reload();
                } else {
                    $("#Employee").prop('disabled', true);
                    document.getElementById('AssignBtn').disabled = false;
                    LoadAllData();
                    $("#reset").click();
                    $("#IssueNo").html("");
                    document.getElementById('circle').style.display = 'none'; //Not Visible
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    });
                }

            }, error: function (er) {
                alert(er);
            }
        });
    } else {
        $.notify("Please Select Employee", {
            globalPosition: "top center",
            className: "error"
        });
    }


}

function EmptyStatus(id) {
    if (id == null) {
        return "";
    } else {
        return id;
    }
}


function SolveQuery(id) {

    var Status = id.substring(0, 4);
    var qId = id.substring(4);


    var Employee = $("#Employee").val();
    if (Employee != "") {
        if (Status != 1) {

            var r = confirm("Are you sure you want to solve this query?");
            if (r == true) {
                $.ajax({
                    type: "POST",
                    url: "/Query/SolveQuery",
                    data: {
                        Issueno: qId
                    },
                    success: function (data) {
                        if (data == "false") {
                            window.location.reload();
                        } else {
                            $("#Employee").prop('disabled', true);
                            document.getElementById('AssignBtn').disabled = false;
                            LoadAllData();
                            $("#reset").click();
                            $("#IssueNo").html("");
                            document.getElementById('circle').style.display = 'none'; //Not Visible
                            $.notify(data.message, {
                                globalPosition: "top center",
                                className: "success"
                            });
                        }
                    },
                    error: function (er) {
                        alert("Something Went Wrong");
                    }
                })
            }
        } else {
            alert("Query already Solved");
        }

    } else {
        alert("Query has not been assigned. Please assign Query before solve");
    }

}