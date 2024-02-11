function getUserId(val) {
    $("#LoginID").val("");
    //alert(val);
    if (val != "") {
        $.ajax({
            type: "POST",
            url: "/Account/GetAllEmployeeById",
            data: {
                empId: val
            },
            success: function (data) {

                $("#LoginID").val(data[0].USER_ID);
            },
            error: function (er) {
                alert(er);
            }
        });
    }
    else {
        alert("Select User");

    }


}

function UpdateEmp() {

    var SuperUser = $("#SuperUser").val();
    var Password = $("#Password").val();
    var LoginID = $("#LoginID").val();



    if (SuperUser != "" && Password != "" && LoginID != "") {

        $.ajax({
            type: "POST",
            url: "/Account/UpdateUser",
            data: {
                empId: SuperUser,
                Password: Password,
                LoginID: LoginID
            },
            success: function (data) {
                $("#SuperUser").val("");
                $("#Password").val("");
                $("#LoginID").val("");
                alert(data);
            },
            error: function (er) {
                alert(er);
            }
        });
    } else {
        alert("Please Select User & Enter Password");
    }

}