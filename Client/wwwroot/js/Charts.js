
$(function () {
    /* ChartJS
    * -------
    * Here we will create a few charts using ChartJS
    */

    //--------------
    //- AREA CHART -
    //--------------
    TotalEmployeeBasedOnDept(); //Total Employe By Department

    $.ajax({
        url: "http://localhost:8042/api/Employees/",
        type: "GET", contentType: "application/json; charset=utf-8",
        dataType: "json",

        success:
            function (result) {

                TotalGender(result);
                genderCountByDepartment(result);

            },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
})


function TotalEmployeeBasedOnDept() {
    // Get context with jQuery - using jQuery's .get() method.
    $.ajax({
        url: "http://localhost:8042/api/Employees/FullNameWithDepartment",
        type: "GET", contentType: "application/json; charset=utf-8",
        dataType: "json",

        success:
            function (result) {

                var departmentCounts = {}; //Definisikan Objek
                // Iterasi melalui data
                for (var i = 0; i < result.data.length; i++) {
                    var employee = result.data[i];
                    var departmentName = employee.departmentName;

                    // Periksa apakah departmentName sudah ada dalam departmentCounts
                    if (departmentCounts.hasOwnProperty(departmentName)) {
                        // Jika sudah ada, tambahkan 1 ke jumlahnya
                        departmentCounts[departmentName]++;
                    } else {
                        // Jika belum ada, inisialisasi dengan 1
                        departmentCounts[departmentName] = 1;
                    }
                }

                var departmentNames = Object.keys(departmentCounts).map(function (key) {
                    return key; //Object terdiri dari Key dan Values (Ambil Key dari Object DepartmentCounts)
                });
                var totalEmpBasedOnDept = Object.values(departmentCounts).map(function (value) {
                    return value; //Object terdiri dari Key dan Values (Ambil Value dari Object DepartmentCounts)
                });

                departmentNames.forEach(function (department) {

                });


                //-------------
                //- CHART SETTINGS DEPARTMENT JUMLAH EMPLOYEE  -
                //-------------
                var areaChartData = {
                    labels: departmentNames,
                    datasets: [
                        {
                            label: 'Jumlah Karyawan',
                            backgroundColor: 'rgba(60,141,188,0.9)',
                            borderColor: 'rgba(60,141,188,0.8)',
                            pointRadius: false,
                            pointColor: '#3b8bba',
                            pointStrokeColor: 'rgba(60,141,188,1)',
                            pointHighlightFill: '#fff',
                            pointHighlightStroke: 'rgba(60,141,188,1)',
                            data: totalEmpBasedOnDept
                        },
                        /*{
                            label               : 'Electronics',
                        backgroundColor     : 'rgba(210, 214, 222, 1)',
                        borderColor         : 'rgba(210, 214, 222, 1)',
                        pointRadius         : false,
                        pointColor          : 'rgba(210, 214, 222, 1)',
                        pointStrokeColor    : '#c1c7d1',
                        pointHighlightFill  : '#fff',
                        pointHighlightStroke: 'rgba(220,220,220,1)',
                        data                : [65, 59, 80, 81, 56, 55, 40]
                            },*/
                    ]
                }


                //-------------
                //- BAR CHART -
                //-------------
                var barChartCanvas = $('#barChart').get(0).getContext('2d')
                var barChartData = $.extend(true, {}, areaChartData)
                var temp0 = areaChartData.datasets[0]
                //var temp1 = areaChartData.datasets[1]
                //barChartData.datasets[0] = temp1
                //barChartData.datasets[1] = temp0

                var barChartOptions = {
                    responsive: true,
                    maintainAspectRatio: false,
                    datasetFill: false
                }

                new Chart(barChartCanvas, {
                    type: 'bar',
                    data: barChartData,
                    options: barChartOptions
                })

                //GetTotalEmployee(labels);
            },
        error: function (errormessage) { alert(errormessage.responseText); }
    });

}


//Function Total Gender
function TotalGender(result) {
    // Get context with jQuery - using jQuery's .get() method.

    var getEmployee = result.data;
    var male = 0;
    var female = 0;

    getEmployee.forEach(function (employee) {

        var gender = employee.gender;

        if (gender === 0) {
            male += 1;
        } else if (gender === 1) {
            female += 1;
        }
    });


    //-------------
    //- CHART SETTINGS JUMLAH GENDER -
    //-------------

    var donutData = {
        labels: [
            'Male',
            'Female'
        ],
        datasets: [
            {
                data: [male, female],
                backgroundColor: ['#D2EFFB', '#F7ABD5'],
            }
        ]
    }


    //-------------
    //- PIE CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
    var pieChartCanvas = $('#genderChart').get(0).getContext('2d')
    var pieData = donutData;
    var pieOptions = {
        maintainAspectRatio: false,
        responsive: true,
    }
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    new Chart(pieChartCanvas, {
        type: 'pie',
        data: pieData,
        options: pieOptions
    })
}



//Function genderCountByDepartmrnt
function genderCountByDepartment(result) {

    var genderCountByDepartment = {};
    var dataEmployee = result.data;
    
    dataEmployee.forEach(function (employee) {
        var departmentName = employee.department.name;
        var gender = employee.gender;

        if (!genderCountByDepartment[departmentName]) { //Cek kalo  gender belum ada ini akan mendefinisikan nilai gender 0
            genderCountByDepartment[departmentName] = {
                male: 0,
                female: 0
            };
        }

        if (gender === 0) {
            genderCountByDepartment[departmentName].male++;
        } else if (gender === 1) {
            genderCountByDepartment[departmentName].female++;
        }
    });

    var departmentMaleCounts = Object.values(genderCountByDepartment).map(function (departmentCount) {
        return departmentCount.male;
    });

    var departmentFemaleCounts = Object.values(genderCountByDepartment).map(function (departmentCount) {
        return departmentCount.female;
    });



    //-------------
    //- CHART SETTINGS Gender By Department -
    //-------------
    var areaChartData = {
        labels: Object.keys(genderCountByDepartment),
        datasets: [
            {
                label: 'Female',
                backgroundColor: '#F7ABD5',
                borderColor: 'rgba(60,141,188,0.8)',
                pointRadius: false,
                pointColor: '#3b8bba',
                pointStrokeColor: 'rgba(60,141,188,1)',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                data: departmentFemaleCounts
            },
            {
                label: 'Male',
                backgroundColor: '#D2EFFB',
                borderColor: 'rgba(210, 214, 222, 1)',
                pointRadius: false,
                pointColor: 'rgba(210, 214, 222, 1)',
                pointStrokeColor: '#c1c7d1',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(220,220,220,1)',
                data: departmentMaleCounts
            },
        ]
    }


    //-------------
    //- BAR CHART -
    //-------------
    var barChartCanvas = $('#genderByDepartment').get(0).getContext('2d')
    var barChartData = $.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    var temp1 = areaChartData.datasets[1]
    barChartData.datasets[0] = temp1
    barChartData.datasets[1] = temp0

    var barChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        datasetFill: false
    }

    new Chart(barChartCanvas, {
        type: 'bar',
        data: barChartData,
        options: barChartOptions
    })


}



