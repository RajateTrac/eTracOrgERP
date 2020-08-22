var HOBurl = '/NewAdmin/GetListOf306090ForJSGrid';
var QExpectationsUrl = '../NewAdmin/GetListOfQExpectationsForJSGrid';
var QEvaluationsUrl = '../NewAdmin/GetListOfQEvaluationsForJSGrid';
var MeetingUrl = '../NewAdmin/GetMeetingList';
var base_Url = window.location.origin;

var clients, RefreshAllGrid, emp_id, qrtFin, RMS_Id;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
(function ($) {
    'use strict'
    var data;
    $("#List306090").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: base_Url + HOBurl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
            //RefreshAllGrid = 
        },
        //      rowRenderer: function (item) {
        //         var user = item;
        //          //<tr > <td ><img src="http://localhost:57572/Content/Images/ProfilePic/4_636464971121344334.png" style="height: 50px; width: 50px; border-radius: 50px;"></td>
        //          //<td class="jsgrid-cell" style="width: 80px;">George  T  Smith</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;"></td>
        //          //    <td class="jsgrid-cell" style="width: 60px;">Review Submitted</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;">Q4</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;">2019</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;"><div class="btn-toolbar"><span style="background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Assessment" id="btn-profile-undefined"><span><i class="fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;"></i></span></span><span style="background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Notification" id="btn-status-undefined"><span><i class="fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;"></i></span></span></div></td></tr>

        //         // var $photo = $("<div>").addClass("client-photo").append($("<img>").attr("src", user.picture.large));
        //          var $info = $('<tr class="jsgrid-row">')
        //              .append($("<img>").attr("src", user.EMP_Photo).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {
        //              })).append()
        //              .append('<td class="jsgrid-cell">' + user.EmployeeName)
        //              .append('<td class="jsgrid-cell">' + user.JBT_JobTitle)
        //              .append('<td class="jsgrid-cell">' + user.Status)
        //              .append('<td class="jsgrid-cell">' + user.Assesment)
        //.append('<td class="jsgrid-cell" style="width: 60px;"><div class="btn-toolbar"><span style="background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Assessment" id="btn-profile-undefined"><span><i class="fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;"></i></span></span><span style="background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Notification" id="btn-status-undefined"><span><i class="fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;"></i></span></span></div></td>');



        //              //.append($("<p>").text("Location: " + user.location.city.capitalize() + ", " + user.location.street))
        //              //.append($("<p>").text("Email: " + user.email))
        //              //.append($("<p>").text("Phone: " + user.phone))
        //              //.append($("<p>").text("Cell: " + user.cell));

        //          return $info;
        //      },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },
            {
                name: 'Assesment', width: 60, title: "30-60-90"
            },
            {
                name: "UserTask", title: "User Task", width: 60, itemTemplate: function (value, item) {
                    var $iconUserView,$iconScheduleMeeting, $IconFinalSubmit,$IconDisputeHR;
                    if (item.Days > 3 & item.Status == "Assessment Lock") {
                        $iconUserView = $("<span style='cursor: not-allowed;none;' title='Assessment lock'>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    }
                    else {
                        $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');
                    }
                    if (item.Status == "Evaluation Done") {
                        $iconScheduleMeeting = $("<span>").append('<i class= "fa fa-clock-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');
                    }
                    var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    if (item.Days > 3 & item.Status == "Evaluation Lock") {
                        var $evaluationText = $("<span style='cursor: not-allowed;none;' title=''Evaluation lock>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    }
                    if (item.Status == "Final Submit") {
                        $IconFinalSubmit = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');
                    }
                    if (item.Status == "Dispute Appriasal") {
                        $IconDisputeHR = $("<span>").append('<i class= "fa fa-check fa-2x" style="color:green;margin-left: 6px;margin-top: 4px;" ></i>');
                    }
                    else{
                        var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');
                    }
                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Assessment" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                            if (item.Days > 3 & item.Status == "Assessment Lock") {
                            } else { 
                            $.ajax({
                                type: "POST",
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment},
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                url: '../NewAdmin/userAssessmentView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });

                        }
                        }).append($iconUserView);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Notification" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $customScheduleMeetingButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Schedule Meeting" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                            qrtFin = item.Assesment;
                            emp_id = item.EMP_EmployeeID;
                            RMS_Id = ite.RMS_Id;
                            $("#empName").html("");
                            $("#empName").html(item.EmployeeName);
                            $("#MeetingScheduleModal").modal('show');

                        }).append($iconScheduleMeeting);
                   
                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                            if (item.Days > 3 & item.Status == "Evaluation Lock") { } else {
                                $.ajax({
                                    type: "POST",
                                    //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment },
                                    data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'Department': item.DepartmentName, 'LocationName': item.LocationName, 'Status': item.Status },
                                    url: '../NewAdmin/userEvaluationView/',
                                    error: function (xhr, status, error) {
                                    },
                                    success: function (result) {
                                        if (result != null) {
                                            $("#gridArea").hide();
                                            $('#profileArea').show();
                                            $('#profileArea').html(result);
                                        }
                                    }
                                });
                            }
                        }).append($evaluationText);


                    var $customDisputeButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Assessment" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                            emp_id = item.EMP_EmployeeID;
                            $.ajax({
                                type: "POST",
                                url: '../NewAdmin/GetEmployeeDisputeComment?EmployeeId=' + emp_id,
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {
                                    $("#AcceptDisputeBody").html(result);
                                    $("#DisputeByHRModel").modal('show'); 
                                }
                            });
                            
                        }).append($IconDisputeHR);

                   if (item.Status == "Assessment Submitted") {
                    //if (item.Status == "Evaluation Pending") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton).append($evaluationTextButton);
                   }
                    if (item.Status == "Evaluation Pending") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton);
                    }
                    if (item.Status == "Meeting Scheduled") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton).append($evaluationTextButton);
                    }
                    if (item.Status == "Meeting Done")
                    {
                          return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton);
                    }
                    if (item.Status == "Meeting Start") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton);
                    }
                    if (item.Status == "Final Submit") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton).append($evaluationTextButton);
                    }
                    if (item.Status == "Dispute Appriasal") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton).append($customDisputeButton);
                    }
                    if (item.Status == "Dispute") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton).append($evaluationTextButton);
                    }
                     if (item.Status == "Evaluation Done" && item.IsCurrentEmployee == false) {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).
                            append($customUserViewButton).
                            append($customTextButton).
                            append($customTextButton).
                            append($evaluationTextButton).
                            append($customScheduleMeetingButton);
                    }
                    if (item.Status == "Evaluation Done" && item.IsCurrentEmployee == true) {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).
                            append($customUserViewButton).
                            append($customTextButton).
                            append($customTextButton)
                    }
                    else {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton)
                    }
                }
            },
        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }

    });
    $("#ListQExpectations").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: QExpectationsUrl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },

            {
                name: 'Expectation', width: 60, title: "Expectation"
            },
            {
                name: 'FinYear', width: 60, title: "Financial Year"
            }
            ,

            {
                name: "UserTask", title: "User Task", width: 60, itemTemplate: function (value, item) {
                    var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });

                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Assessment" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType ,'FinYear':item.FinYear,'FinQuarter':item.Expectation},
                                url: '../NewAdmin/userExpectationsView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });


                        }).append($iconUserView);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Notification" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment },
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'Department': item.DepartmentName, 'LocationName': item.LocationName },

                                url: '../NewAdmin/userEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });
                        }).append($evaluationText);
                    if (item.Status == "Expectations Submitted" || item.EMP_EmployeeID == $("#LoggedInUser").val()) {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customUserViewButton).append($customTextButton).append($customTextButton);
                    } else {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customUserViewButton).append($customTextButton).append($customTextButton)
                    }
                }
            },


        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
    $("#ListQEvaluations").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: QEvaluationsUrl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },

            {
                name: 'Expectation', width: 60, title: "Expectation"
            },
            {
                name: 'FinYear', width: 60, title: "Financial Year"
            }
            ,
            {
                name: 'MeetingDateTime', width: 60, title: "Meeting Date TIme", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.MeetingDate + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.MeetingTime + "</span></div>");
                }
            },

            {
                name: "UserTask", title: "User Task", width: 70, itemTemplate: function (value, item) {
                    var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconMeeting = $("<span>").append('<i class="fa fa-clock-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconPiP = $("<span>").append('<i class="fa fa-user-circle-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconMeetingComplete = $("<span>").append('<i class="fa fa-clock-o fa-2x " style="color:green;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: '../NewAdmin/QEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });


                        }).append($evaluationText);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Notification" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $customMeetingButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Setup Meeting" })
                        .attr({ id: "btn-status" }).click(function (e) {
                            $("#EmailTo").val(item.EmployeeName);
                            $("#ReceipientEmailId").val(item.EMP_EmployeeID);
                            $("#SetUpMeetingModal").modal('show');
                            $("#FinYear").val(item.FinYear);
                            $("#FinQrtr").val(item.Expectation);

                        }).append($iconMeeting);

                    var $customMeetingSuccessButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Meeting Completed" })
                        .attr({ id: "btn-status" }).click(function (e) {
                        }).append($iconMeetingComplete);

                    var $customPiPButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "PIP" })
                        .attr({ id: "btn-status" }).click(function (e) {

                        }).append($iconPiP);


                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                            $("#EmailTo").val(item.EmployeeName);
                            $("#ReceipientEmailId").val(item.EMP_EmployeeID);
                            $("#FinYear").val(item.FinYear);
                            $("#FinQrtr").val(item.Expectation);
                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: base_Url+'/NewAdmin/GetMeetingDetail/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {
                                    if (result != null) {
                                        $.ajax({
                                            type: "POST",
                                            data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                            url: base_Url+'/NewAdmin/QEvaluationView/',
                                            error: function (xhr, status, error) {
                                            },
                                            success: function (data) {
                                                if (data != null) {
                                                    $("#gridArea").hide();
                                                    $('#profileArea').show();
                                                    $('#profileArea').html(data);
                                                }
                                                if (result == "MEETINGNOTFOUND") {
                                                    $("#selfAssessmentTable :input").attr("disabled", true);
                                                    $("#labelAddCalendar").html('Please complete the meeting before submitting the Evaluation <span title="Setup meeting"><i onclick=showMeetingPopUp() class="fa fa-calendar-check-o fa-2x" style="margin-left:6px; margin-top:4px;"></i ></span>');
                                                    $("#MeetingNotDoneModal").modal('show');
                                                } else if (result == "MEETINGNOTCOMPLETED") {
                                                    $("#selfAssessmentTable :input").attr("disabled", true);
                                                    $("#labelAddCalendar").html('Meeting is scheduled but not completed');
                                                    $("#MeetingNotDoneModal").modal('show');
                                                }




                                            }
                                        });
                                    }
                                    //else {
                                    //    $("#MeetingNotDoneModal").modal('show');
                                    //}
                                }
                            });
                        }).append($evaluationText);
                    if (item.Status == "Expectations Submitted" || item.Status == "Evaluation Submitted" && item.PRMeetingStatus != "MEETINGCOMPLETED") {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($evaluationTextButton).append($customTextButton).append($customMeetingButton).append($customPiPButton).append($evaluationTextButton);
                    } else if (item.Status == "Expectations Submitted" || item.Status == "Evaluation Submitted" && item.PRMeetingStatus == "MEETINGCOMPLETED" || item.PRMeetingStatus == "MEETINGNOTCOMPLETED") {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($evaluationTextButton).append($customTextButton).append($customMeetingSuccessButton).append($customPiPButton).append($evaluationTextButton);
                    } else {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customTextButton);
                    }
                }
            },
        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
    $("#ListCorrectiveAction").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: QEvaluationsUrl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },

            {
                name: 'Expectation', width: 60, title: "Expectation"
            },
            {
                name: 'FinYear', width: 60, title: "Financial Year"
            }
            ,

            {
                name: "UserTask", title: "User Task", width: 70, itemTemplate: function (value, item) {
                    var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconText = $("<span>").append('<i class= "fa fa-plus-square fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconMeeting = $("<span>").append('<i class="fa fa-clock-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconPiP = $("<span>").append('<i class="fa fa-user-circle-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });

                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: '../NewAdmin/QEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });


                        }).append($evaluationText);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Issue Corrective" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $customMeetingButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Setup Meeting" })
                        .attr({ id: "btn-status" }).click(function (e) {
                            $("#EmailTo").val(item.EmployeeName);
                            $("#ReceipientEmailId").val(item.EMP_EmployeeID);
                            $("#SetUpMeetingModal").modal('show');
                            $("#FinYear").val(item.FinYear);
                            $("#FinQrtr").val(item.Expectation);

                        }).append($iconMeeting);

                    var $customPiPButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "PIP" })
                        .attr({ id: "btn-status" }).click(function (e) {

                        }).append($iconPiP);


                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: '../NewAdmin/GetMeetingDetail/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null && result == true) {
                                        $.ajax({
                                            type: "POST",
                                            data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                            url: '../NewAdmin/QEvaluationView/',
                                            error: function (xhr, status, error) {
                                            },
                                            success: function (result) {

                                                if (result != null) {
                                                    $("#gridArea").hide();
                                                    $('#profileArea').show();
                                                    $('#profileArea').html(result);
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        $("#MeetingNotDoneModal").modal('show');
                                    }
                                }
                            });


                        }).append($evaluationText);
                    //if (item.Status == "Expectations Submitted" || item.Status == "Evaluation Submitted") {
                    //    return $("<div>").attr({ class: "btn-toolbar" }).append($evaluationTextButton).append($customTextButton).append($customMeetingButton).append($customPiPButton).append($evaluationTextButton);
                    //} else {
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customTextButton);
                    //}
                }
            },


        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
    $("#ListScheduledMeeting").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: MeetingUrl,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "ManagerPhoto", title: "Manager Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'ManagerName', width: 80, title: "Manager Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.ManagerName + "<br><span style='font-size:10px;color:black;font-style:italic;'></span></div>");
                }
            },
            {
                name: "EmployeePhoto", title: "Employee Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'></span></div>");
                }
            },
            {
                name: 'MeetingDateTime', width: 60, title: "Meeting Date TIme", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.MeetingDate + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.MeetingTime + "</span></div>");
                }
            },

        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
    $("#ListHREvalutionProcess").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: base_Url + "/HR/GetListOfPerformanceForHR" + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            {name: 'Status', width: 60, title: "Status"},
            {name: 'Assesment', width: 60, title: "Assessmnet Status"},
            {
                name: "UserTask", title: "User Task", width: 60, itemTemplate: function (value, item) {
                    var $iconUserView;
                    $iconUserView = $("<span  title='View Evaluation'>").append('<i class= "fa fa-eye fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');               
                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Notification" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment },
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'Department': item.DepartmentName, 'LocationName': item.LocationName },

                                url: '../NewAdmin/userEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {
                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });
                        }).append($iconUserView);
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customTextButton);
                }
            },
        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }

    });
})(jQuery);
$(document).ready(function () {
    $("#drp_MasterLocation").change(function () {
        $_LocationId = $("#drp_MasterLocation option:selected").val();
        $("#ListQRC").jsGrid("loadData");
    })

});
function showMeetingPopUp() {
    $("#MeetingNotDoneModal").modal('hide');
    $("#SetUpMeetingModal").modal('show');

}
function SelfAssessment() {
    $.ajax({
        type: "GET",
        url: '../NewAdmin/GetManagerAssessmentDetails/',
        error: function (xhr, status, error) {
        },
        success: function (item) {

            $.ajax({
                type: "POST",
                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                url: '../NewAdmin/SelfAssessmentView/',
                error: function (xhr, status, error) {
                },
                success: function (result) {

                    if (result != null) {
                        $("#gridArea").hide();
                        $('#profileArea').show();
                        $('#profileArea').html(result);
                    }
                }
            });
        }
    });
}

function RefreshPerformanceGrid() {
    debugger
    $("#" + RefreshAllGrid).jsGrid('loadData');
}
function getTabIdEvent($_this) {
    debugger
    RefreshAllGrid = $($_this).attr("id");
}

function ScheduleAppraisalMeeting() {
    debugger
    $.ajax({
        type: "POST",
        url: base_Url + '/Notification/SendMeetingNotificationToEmployee?EmployeeId=' + emp_id + '&Assessment=' + qrtFin,      
        success: function (item) {   
            $("#MeetingScheduleModal").modal('hide');
            $("#List306090").jsGrid("loadData");
        },
         error: function (err) {
        },
    });
}

function UpdatePerformanceStatus(employeeId, Status) {
    $.ajax({
        type: "POST",
        url: base_Url + '/NewAdmin/UpdatePerformanceStatus?EmployeeId=' + employeeId + "&Status=" + Status + "&RMS_Id" + RMS_Id + '&Assessment=' + qrtFin,
        success: function (item) {
            debugger
            if (Status == "S") {
                $(".hidemeetingStart, .hidemeetingFinal").hide();
                $(".hidemeetingStop").show();
                toastr.Success("Meeting started.")
            }
            else if (Status == "D") {
                $(".hidemeetingStart, .hidemeetingStop").hide();
                $(".hidemeetingFinal").show();
                toastr.Success("Meeting End.")
            }
            else {
                toastr.Success("You submitted final evaluation.")
                $("#gridArea").show();
                $('#profileArea').hide();
                $("#List306090").jsGrid("loadData");
            }
        },
        error: function (err) {
        },
    });
}



function SaveCommentEMP(employeeId, Status) {
    debugger
    var comment = $("#addComment").val();
    var url = window.location.origin;
    var fileUploadLicense = $("#uploadDisputeAttachment").get(0);
    var filesLicense = fileUploadLicense.files;
    var fileData = new FormData(); 
    for (var i = 0; i < filesLicense.length; i++) {
        fileData.append(filesLicense[i].name, filesLicense[i]);
    }
    $.ajax({
        url: url + '/NewAdmin/UploadFilesAppriasalDispute?EmployeeId=' + employeeId + '&Status=' + Status + '&Comment=' + comment,
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            debugger
            var fileUploadSSN = $("#mySSNfileUpload").get(0);
            var filesSSN = fileUploadSSN.files;
            fileData = new FormData();
            for (var i = 0; i < filesSSN.length; i++) {
                fileData.append(filesSSN[i].name, filesSSN[i]);
            }
            $("#AcceptDisputeMeeting").modal('hide');
            //alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
    $.ajax({
        type: "POST",
        url: base_Url + '/NewAdmin/SaveCommentEmployee?EmployeeId=' + employeeId + "&Status=" + Status,
        success: function (item) {          
            if (Status == "A") {
                //toastr.Success("Save ");
            }
            else {
                //toastr.Success("You ");
            }
            $("#gridArea").show();
            $('#profileArea').hide();
            $("#List306090").jsGrid("loadData");
        },
        error: function (err) {
        },
    });
}

function AcceptDisputeByHR($_Status) {
    if ($_Status == "U") {
        $("#UpdateByHRbtn").show();
        $("#AcceptByHRbtn").show();
        $("#SelectRational").show();
    }
    else {
        if ($_Status == "S") {
            $_Status = "U";
        }
        var selectComment = $("#SelectRational").val();
        $.ajax({
            type: "POST",
            url: base_Url + '/NewAdmin/AcceptDisputeByHR?EmployeeId=' + employeeId + "&Status=" + $_Status + "&Remedy=" + selectComment,
            success: function (item) {
                if (Status == "U") {
                    toastr.Success("Saved successfully");                    
                }
                $("#ListHREvalutionProcess").jsGrid("loadData");
            },
            error: function (err) {
            },
        });
    }
}