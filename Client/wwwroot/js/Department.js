$(document).ready(function () {
    //debugger;
    $('#TB_Department').DataTable({
        //ordering: false,

        "responsive": true,

        "ajax": {
            url: "http://localhost:8042/api/Departments",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('tokenJWT')
            },
        },

        "columns": [
            //Render digunakan untuk menampilkan atau memodifikasi isi sel (cell) pada kolom

            {

                orderable: false, // menonaktifkan order hanya pada kolom tertentu
                "data": null,
                "width": "10%",
                "render": function (data, type, row, meta) {
                    return meta.row + 1 + ".";
                }
            },

            {
                "data": "name",

            },

            {
                "data": null,
                orderable: false, // menonaktifkan order hanya pada kolom name
                "render": function (data, type, row) {

                    return '<a href="#" class="btn btn-warning text-light edit-btn" data-bs-toggle="modal" data-bs-target="#modal-add" onclick = "return GetById(' + row.id + ')"><i class="fas fa-pen"></i></a>' +
                        '<a href="#" class="btn btn-danger ml-2 " data-bs-toggle="modal"  onclick = "return Delete(' + row.id + ', \'' + row.name + '\')"><i class="fas fa-trash" ></i></a>';
                }
            }

        ],
        "order": [], // menonaktifkan order pada semua kolom
        /*   "fnDrawCallback": function (oSettings) {
                 // mengatur nomor urut berdasarkan halaman dan pengurutan yang aktif, menetapkan nomor urut menjadi 1
                 var table = $('#TB_Department').DataTable();
                 var startIndex = table.context[0]._iDisplayStart;
                 table.column(0, { order: 'applied' }).nodes().each(function (cell, i) {
                     cell.innerHTML = startIndex + i + 1;
                 });
             }*/

        /*   "fndrawCallback": function (settings) {
               var api = this.api();
               var rows = api.rows({ page: 'current' }).nodes();
               api.column(1, { page: 'current' }).data().each(function (group, i) {
   
                   $(rows).eq(i).find('td:first').html(i + 1);
               });
           }*/

    });


});


function ClearScreen() {
    $('#departmentId').val('');
    $('#departmentName').val('');
    $('#Update').hide();
    $('#Add').show();
}

function Save() {
    if ($('#departmentName').val() == '') {
        $('#alert-department').show();
        setTimeout(function () {
            $('#alert-department').hide();
        }, 4000);
    } else {
        var Department = new Object  //object baru
        Department.Name = $('#departmentName').val(); //value insert dari id pada input
        $.ajax({
            type: 'POST',
            url: 'http://localhost:8042/api/Departments',
            data: JSON.stringify(Department),
            contentType: "application/json; charset=utf-8"
        }).then((result) => {
            $('#modal-add').modal('hide');
            $('#modal-add').on('hidden.bs.modal', function () {
                $(this).data('bs.modal', null);
            });
            //debugger;
            if (result.status == result.status == 201 || result.status == 204 || result.status == 200) {
                //$('#modal-add').modal('hide'); // hanya hide modal tetapi tidak menutup DOM nya
                Swal.fire({
                    title: "Success!",
                    text: "Data Berhasil Dimasukkan",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    $('#TB_Department').DataTable().ajax.reload();
                });
            }
            else {
                alert("Data gagal dimasukkan");
            }

        })
    }
}



/*function GetById(id) {
    var departmentId = $(this).data('id');
    var departmentName = $(this).data('name');
    $('#departmentId').val(departmentId); //#departmentId id pada modal form input edit department
    $('#departmentName').val(departmentName); //#departmentName id pada modal form input name pada edit department
});
*/


function Delete(id, name) {
    $('#deleteDepartmentName').text(name); 
    //$('#confirm-delete').on('click', '.btn-ok', function () { //Jika menggunakan modal bootstrap
    Swal.fire({
        title: 'Are you sure?',
        text: "Want to delete "+name+" depaertment",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "http://localhost:8042/api/Departments/Id?id=" + id,
                type: "DELETE",
                dataType: "json",
                success: function (result) {
                    if (result.status == 200) {
                        //$('#confirm-delete').off('click'); //Jika menggunakan modal bootrsap
                        //$('#confirm-delete').modal('hide'); Hanya untuk hide tidak menutup pada DOM
                        $('#TB_Department').DataTable().ajax.reload(); //menambahkan ajax.reload() pada DataTable
                        Swal.fire({
                            title: "Success!",
                            text: "Data Berhasil Dihapus",
                            icon: "success",
                            showConfirmButton: false,
                            timer: 1600
                        });
                    }
                },
                error: function () {
                    $('#confirm-delete').off('click');
                    $('#confirm-delete').modal('hide');
                    Swal.fire({
                        icon: 'error',
                        title: 'Failed...',
                        text: 'Department masih digunakan dalam data Employee!',
                        footer: '<a href="">Why do I have this issue?</a>'
                    })
                }
            });
        }
    })
    
}


/*function Edit(id, name) { //debugger; 
    $('#departmentId').val(id);
    $('#departmentName').val(name);
    $('#confirm-delete').on('click', '.btn-ok', function () {
        $.ajax({
            url: "http://localhost:8042/api/Departments/Id?id=" + id,
            type: "PUT",
            dataType: "json",
        }).then((result) => {
            debugger;
            if (result.status == 200) {
                alert(result.message);
            }
            else { alert(result.message); }
        });
    });
}
*/

function GetById(id) {
    //debugger;
    $.ajax({
        url: "http://localhost:8042/api/Departments/" + id,
        type: "GET", contentType: "application/json; charset=utf-8",
        dataType: "json",
        success:
            function (result) {
            //debugger;
                //console.log(result); 
                var obj = result.data; //data yg didapat dari api
                $('#departmentId').val(obj.id);
                $('#departmentName').val(obj.name);
                $('#myModal').modal('show');
                $('#Add').hide();
                $('#Update').show();
            },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}


function Update() {
    debugger;
    var Department = new Object();
    Department.Id = $('#departmentId').val();
    Department.Name = $('#departmentName').val();
    $.ajax({
        url: 'http://localhost:8042/api/Departments',
        type: 'PUT',
        data: JSON.stringify(Department),
        contentType: "application/json; charset=utf-8",
    }).then((result) => {
        debugger;
        if (result.status == 200) {
            Swal.fire({
                title: "Success!",
                text: "Data Berhasil Di Update",
                icon: "success",
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                $('#TB_Department').DataTable().ajax.reload();
            });
        } else {
            alert("Data gagal Diperbaharui"); table.ajax.reload();
        }
    });
}