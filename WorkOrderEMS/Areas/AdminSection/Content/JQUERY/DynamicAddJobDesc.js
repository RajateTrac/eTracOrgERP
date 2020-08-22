

$(document).ready(function () {
    //Collaps menu and show only icons ..
    $('#main-wrapper').toggleClass("menu-toggle");
    GetDropdownForChar();
    $(".hamburger").hide();
    $(".nav-user img").css({ "width": "30px", "height": "30px" });
    function GetDropdownForChar() {
        $.ajax({
            type: "GET",
            url: "../AdminSection/AdminDashboard/GetList",
            success: function (data) {
                if (data != null) {

                    $('#Department').html('');
                    $('#Superior').html('');
                    var optionsSuperior = '';
                    var optionsDepartment = '';
                    optionsSuperior += '<option value="0">-Select Superior--</option>';
                    optionsDepartment += '<option value="0">-Select Department--</option>';
                    for (var i = 0; i < data.listSuperiour.length; i++) {
                        optionsSuperior += '<option value="' + data.listSuperiour[i].parentId + '">' + data.listSuperiour[i].SeatingName + '</option>';
                    }
                    for (var i = 0; i < data.listDepartment.length; i++) {
                        optionsDepartment += '<option value="' + data.listDepartment[i].DeptId + '">' + data.listDepartment[i].DepartmentName + '</option>';
                    }
                    $('#Superior').append(optionsSuperior);
                    $('#Department').append(optionsDepartment);
                }
            }
        })
    }
    //Add and remove text box of JD for VSC
    $('.addrows').click(function (e) {
        var divID = $('#routeDiv div.dymanicAdd').length;
        $('#routeDiv').append('<div class="form-group row dymanicAdd d' + divID + '"><div class="col-sm-11 getJobDesc"><input type="text" class="form-control input-rounded required" placeholder="Job Description" value="" /></div><div class="col-sm-1"><a class="addrows minusSign" id=d' + divID + '><i class="fa fa-minus-circle addColorPlusMinus fa-2x" style="cursor:pointer;" aria-hidden="true"></i></a></div></div>');
        $('#routeDiv').append('<script>jQuery("a.minusSign#d' + divID + '").click(function (){$("div.d' + divID + '").remove();  });</script>');
    });
    $('.addjobtitlerows').click(function (e) {       
        var divID = $('#jobTitleDiv div.dymanicAdd').length;
        $('#jobTitleDiv').append('<div class="form-group row dymanicAdd d' + divID + '"><div class="col-sm-8 getJobTitleDesc"><input type="text" class="form-control input-rounded required removeContent" placeholder="Job Title" value="" style="width: 441px;" /></div><div class="col-sm-3 getJobCount"><input type="text" class="form-control input-rounded required removeContent" placeholder="Job Count" value="" style="width: 130px;" /></div><div class="col-sm-1"><a class="addjobtitlerows minusSign" id=d' + divID + '><i class="fa fa-minus-circle addColorPlusMinus fa-2x" style="cursor:pointer;margin-left: 30px;" aria-hidden="true"></i></a></div></div>');
        $('#jobTitleDiv').append('<script>jQuery("a.minusSign#d' + divID + '").click(function (){$("div.d' + divID + '").remove();  });</script>');
    });
    //$('.addjobtitlerows').click(function (e) {
    //    debugger
    //    var divID = $('#jobTitleDiv div.dymanicAdd').length;
    //    $('#jobTitleDiv').append('<div class="form-group row dymanicAdd d' + divID + '"><div class="col-sm-8 getJobTitleDesc"><input type="text" class="form-control input-rounded required" placeholder="Job Title" value="" style="width: 441px;" /></div> <div class="col-sm-3 getJobCount"><input type="text" class="form-control input-rounded required" placeholder="Job Count" value="" style="width: 130px;" /></div><a class="addjobtitlerows minusSign col-sm-1" id=d' + divID + '><i class="fa fa-minus-circle addColorPlusMinus fa-2x" style="cursor:pointer;margin-left: 30px;" aria-hidden="true"></i></a></div>');
    //    $('#jobTitleDiv').append('<script>jQuery("a.minusSign#d' + divID + '").click(function (){$("div.d' + divID + '").remove();  });</script>');
    //});
    $("#AddChart").click(function () {
        $("#routeDiv").html("");
        $("#SeatingName").val("");
        $("#JobDescription").val("");
        $("#VCSId").val("");
        $("#EmploymentStatus").val("");
        $("#EmploymentClassification").val("");
        $("#RateOfPay").val("");
        tinymce.activeEditor.setContent("");
        tinymce.editors.RolesAndResponsibility.setContent("");
        GetDropdownForChar();
        $("#myModalForChart").modal("show");
        $("#SaveVSC").val("Save");
    });
    ///To save Form Data 
    $('#SaveVSC').click(function (e) {
        debugger
        if ($("#SaveVCSChartForm").valid({
            rules: {
             'SeatingName': { required: true },
            'Superior': { required: true},
            'Department': { required: true },
            'JobDescription': { required: true },
            'RolesAndResponsibility': { required: true },
        },
            messages: {
           'SeatingName': { required: "Please enter Seating Name" },
            'Superior': { required: "Please select Superior" },
            'Department': { required: "Please select department" },
            'JobDescription': { required: "Please add job description" },
            'RolesAndResponsibility': { required: "Please add Roles and responsibility" },
           }
        })) {
            createAddJobDescArray();
            var content = tinymce.activeEditor.getContent({ format: 'raw' });
            $('#RolesAndResponsibility').val(content);
            var dataObject = $("#SaveVCSChartForm").serialize();
            debugger
            $.ajax({
                type: "POST",
                url: '../AdminSection/AdminDashboard/SaveVCS', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
                data: dataObject,

                success: function (Data) {
                    GetDropdownForChar();
                    $("#routeDiv").html("");
                    $("#SeatingName").val("");
                    $("#JobDescription").val("");
                    $("#VCSId").val("");
                    $("#myModalForChart").modal("hide");
                    tinymce.activeEditor.setContent("");
                    $("chart-container").html("");
                    var peopleElement = document.getElementById("chart-container");
                    $.ajax({
                        type: "POST",
                        url: '../AdminSection/AdminDashboard/GetChartDisplayData',
                        success: function (listData) {
                            debugger
                            var tabledata = [];

                            if (listData != null) {
                                for (var i = 0; i < listData.length; i++) {
                                    var data = {};
                                    if (i == 0) {
                                        listData[i].parentId = null;
                                    }
                                    data.id = listData[i].Id;
                                    data.pid = listData[i].parentId;
                                    data.SeatingName = listData[i].SeatingName;
                                    data.JobDescription = listData[i].JobDescription;
                                    data.DepartmentName = listData[i].DepartmentName;
                                    tabledata.push(data);
                                }
                                var chart = new OrgChart(peopleElement, {

                                    scaleInitial: BALKANGraph.match.boundary,
                                    //enableDragDrop: true,
                                    //tags: {
                                    //    "assistant": {
                                    //        template: "ula"
                                    //    }
                                    //},
                                    nodeMenu: {
                                        //add: { text: "Add" },
                                        add: { text: "Job Title" },
                                        edit: { text: "Edit" },
                                    },
                                    nodeBinding: {
                                        field_0: "SeatingName",
                                        field_1: "JobDescription",
                                        field_2: "DepartmentName",
                                        //field_2: "DepartmentName"
                                        //img_0: "img"
                                    },
                                    nodes: tabledata,
                                });

                                $(".field_0").attr({ "style": "font-size:16px;" });
                                $("tspan").attr("x", "140");
                                $(".field_0").attr("x", "100"); $(".field_0").attr("x", "65");
                                $(".field_0").attr("y", "25");
                            }
                        },
                        error: function (err) {
                        }
                    });
                    //var addNewUrl ='../AdminSection/OrgChart/Index';// "@Url.Action("Index", "OrgChart", new { area = "AdminSection" })";
                    //$('#RenderPageId').load(addNewUrl);
                },
                error: function (err) {
                }
            });
        }
    })

    ///This is use to add job description seperated by line
    function createAddJobDescArray() {
        JobDescFormat = $('#JobDescription').val() + '|';
        $("#routeDiv div.dymanicAdd .getJobDesc").each(function () {
            var myObjJson = {};
            $this = $(this)
            var job = $this.find("input").val();
            JobDescFormat = JobDescFormat + job + '| ';
        });
        $('#JobDesc').val(JobDescFormat);
    }

    //This is to save Job Title for Chart
    $('#SaveJobTitle').click(function (e) {
        debugger
        var getJobTitle = $("#JobTitle").val();
        var getJobCount = $("#JobCount").val();
        if (getJobCount != null && getJobCount != "" && getJobCount != undefined && getJobTitle != null && getJobTitle != "" && getJobTitle != undefined) {
            $("#showerrorjobtitle").hide();
            createAddJobTitleArray();
            var getId = $("#parentIdForJobTitle").val();
            var dataJobTitleObject = $("#SaveJobTitleForm").serialize();
            $.ajax({
                type: "POST",
                url: '../AdminSection/AdminDashboard/SaveJobTitle', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
                data: dataJobTitleObject,

                success: function (Data) {
                    $("#jobTitleDiv").html("");
                    $("#parentIdForJobTitle").val("");
                    $("#JobTitle").val("");
                    $("#JobCount").val("");
                    $("#myModalForAddingJobTitle").modal("hide");
                },
                error: function (err) {
                }
            });

        }
        else {
            $("#showerrorjobtitle").show();
            return false;
        }
    })
    function createAddJobTitleArray() {
        JobTitleFormat = $('#JobTitle').val() + '|';
        JobTitleCountFormat = $("#JobCount").val() + '|';
        $("#jobTitleDiv div.dymanicAdd .getJobTitleDesc").each(function () {
            var myObjJson = {};
            $this = $(this)
            var jobTitle = $this.find("input").val();
            JobTitleFormat = JobTitleFormat + jobTitle + '| ';
        });
        $('#JobTitleDesc').val(JobTitleFormat);

        $(".getJobCount").each(function () {
            var myObjJson = {};
            $this = $(this)
            var jobTitleCount = $this.find("input").val();
            JobTitleCountFormat = JobTitleCountFormat + jobTitleCount + '| ';
        });
        $('#JobTitleCountDesc').val(JobTitleCountFormat);
    }
    //function SaveJobPost(){
    //    debugger
    //}
    //$('#SaveJobPost').click(function (e) {
    //    debugger
    //    if ($("#SaveJobPostingForm").valid()) {
    //        //var dataObject = $("#SaveJobPostingForm").serialize();
    //        var dataJobTitleObject = $("#SaveJobPostingForm").serialize();
    //        $.ajax({
    //            type: "POST",
    //            url: '../AdminSection/OrgChart/SaveJobPosting', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
    //            data: dataJobTitleObject,

    //            success: function (Data) {
    //                debugger
                    
    //            },
    //            error: function (err) {
    //            }
    //        });
    //    }
    //    });
})