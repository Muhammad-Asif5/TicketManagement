LoadAllData();
function LoadAllData() {
    $.ajax({
        type: "POST",
        url: "/User/GetAllQueryToTable",
        data: {},
        success: function (data) {

            ///alert(data[0].Subject);
            $("table thead").html("");
            $("table tbody").html("");
            for (var i = 0; i < data.length; i++) {
                $("table tbody").append("<tr id='" + data[i].Q_ID + "' onclick='LoadToTextBox(this.id)'>" +
                    "<td>" + (i + 1) + "</td>" +
                    "<td>" + data[i].NAME + "</td>" +
                    //"<td>" + data[i].ASSIGN_TIME.Hours + ":" + data[i].ASSIGN_TIME.Minutes + "</td>" +
                    "<td>" + data[i].ASSIGN_TIME + "</td>" +
                    //"<td>" + data[i].SOLVE_TIME + "</td>" +
                    "<td>" + data[i].DESCRIPTION + "</td>" +
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
        return "<label style='background:red; border-radius:5px; color:white; padding:2px;'>Panding</label>";
    } else {
        return "<label style='background:green; border-radius:5px; color:white; padding:2px;'>Solved</label>";
    }
}

function LoadToTextBox(id) {
    $.ajax({
        type: "POST",
        url: "/User/LoadToTextBoxData",
        data: {
            IssueID: id
        },
        success: function (data) {

            var IssueNo = $("#IssueNo").text(data[0].Q_ID);
            var Employee = $("#Employee").val(data[0].EMP_ID);
            var Description = $("#Description").val(data[0].COMMENTS);
            var Office = $("#Office").val(data[0].OFFICE);

            //$("#Description").prop('disabled', true);
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

function SaveItem() {
    var IssueNo = $("#IssueNo").text();
    var Employee = $("#Employee").val();
    var Description = $("#Description").val();
    var Office = $("#Office").val();

    if (Employee != "-1" && Description != "" && Office != "") {
        $.ajax({
            type: "POST",
            url: "/User/SaveItem",
            data: {
                IssueNo: IssueNo,
                Description: Description,
            },
            success: function (data) {
                debugger
                if (data == "false") {
                    window.location.reload();
                } else {

                }
                LoadAllData();
                $("#reset").click();
                $("#IssueNo").html("");

                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success"
                });
            }, error: function (er) {
                alert(er);
            }
        });
    } else {
        $.notify("Please Enter All Info", {
            globalPosition: "top center",
            className: "error"
        });
    }


}

function Panging() {

    var IssueNo = $("#IssueNo").text();
    var Description = $("#Description").val();

    if (IssueNo != "") {
        $.ajax({
            type: "POST",
            url: "/User/SaveItem1",
            data: {
                IssueNo: IssueNo,
                Description: Description,
            },
            success: function (data) {
                debugger
                if (data == "false") {
                    window.location.reload();
                } else {

                }
                LoadAllData();
                $("#reset").click();
                $("#IssueNo").html("");

                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success"
                });
            }, error: function (er) {
                alert(er);
            }
        });
    } else {
        $.notify("Please Enter All Info", {
            globalPosition: "top center",
            className: "error"
        });
    }


}