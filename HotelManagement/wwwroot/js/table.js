$(document).ready(function () {

    $("#tableList").DataTable({
            searching: false,
            order: [],
            processing: true,
            serverSide: true,
            autoWidth: false,
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            ajax: {
                url: "/Table/TableList",
                type: "POST",
                contentType: "application/json",
                data: function (d) {
                    return JSON.stringify({
                        draw: d.draw,
                        start: d.start,
                        length: d.length,
                        //search: null,
                        columns: d.columns.map((col, index) => ({
                            field: col.data,
                            isSortable: col.orderable,
                            sort: d.order.find(o => o.column === index)
                                ? { direction: d.order.find(o => o.column === index).dir === "asc" ? 0 : 1 }
                                : null
                        })),
                        //filters: {
                        //    field1: $("#searchName").val(),
                        //    field2: $("#searchPhone").val(),
                        //    field3: $("#searchEmail").val(),
                        //    field4: $("#searchProject").val() 
                        //}
                    });
                }
            },
            columns: [
                { title: "Table Id", data: "tableId", visible: false, searchable: false },
                { title: "Table Number", data: "tableNumber", searchable: false, orderable: false },
                { title: "Table Name", data: "tableName", searchable: false, orderable: false },
                { title: "Display Name", data: "displayTableName", searchable: false, orderable: false },
                { title: "IsActive", data: "isActive" , searchable: false, orderable: false, width: "10%",
                    render: function (data,a,othercoldata) {
                        var html = '<div class="form-check form-switch">'+
                                    `<input class="form-check-input isActiveTable" type="checkbox" data-id="${othercoldata.tableId}"`;
                                    if(data == true)
                                    {
                                        html += 'checked';
                                    }
                                    html += '></div>';
                        return html;
                    }
                },
                {
                    title: "Action",
                    data: "tableId",
                    orderable: false,
                    searchable: false,
                    width: "10%",
                    render: function (data) {
                        return `
                            <div class="btn-group" role="group">
                                <button type="button" class="btn btn-sm btn-primary dropdown-toggle" data-bs-toggle="dropdown">Action</button>
                                <ul class="dropdown-menu">
                                    <li><a class="dropdown-item  text-primary" href="/Table/CreateTable?tableId=${data}" ><i class="fa fa-edit"></i>&nbsp;Edit</a></li>
                                    <li><a class="dropdown-item  text-danger" style="cursor: pointer;" onclick="deleteTable(${data})" ><i class="fa fa-trash"></i>&nbsp;Delete</a></li>
                                </ul>
                            </div>`;
                    }
                }
            ]
    });
    $(document).on('change', '.isActiveTable', function() {
        var tableId = $(this).data('id');
        var isActive = $(this).is(':checked');
        $.ajax({
        url: `/Table/ChangeTableStatus?tableId=${tableId}&isActive=${isActive}`,
        type: "POST",
        success: function (data) {
            if(data.success != true){
                Swal.fire({
                  icon: "error",
                  text: "status not changed !!!"
                });
            }
        }
        });
    });

    $("#createTableForm").validate({
        rules: {
            TableNumber: { required: true},
            TableName: { required: true}
        },
        messages: {
            TableNumber: {
                required: "TableNumber is Required."
            },
            TableName: {
                required: "TableName is Required."
            }
        },
        errorElement: "div",
        errorClass: "text-danger small mt-1",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        },
        submitHandler: function (form) {
            $('#submitBtnCreateTable').prop('disabled',true);
            var formData = $(form);
            $.ajax({
                url: "/Table/CreateTable",
                type: "POST",
                data: formData.serialize(),
                success: function (response) {
                    $('#submitBtnCreateTable').prop('disabled',false);
                    if(response.success == true){
                    Swal.fire({
                        icon: "success",
                        text: response.message
                          }).then((result) => {
                              if (result.isConfirmed) {
                                window.location.href = "/Table/TableList";
                              }
                          });
                    }
                    if(response.success == false){
                        Swal.fire({
                          icon: "error",
                          text: response.message
                        });
                    }
                },
                error: function (res) {
                    Swal.fire({
                        icon: "error",
                        text: "Something went wrong: " + res.statusText
                    });
                }
            });
        }
    });


});

function deleteTable(tableId) {
    Swal.fire({
      text: "Are you sure want to delete Table ?",
      icon: "question",
      showCancelButton: true,
    }).then((result) => {
        if (result.isConfirmed) {
          $.ajax({
            url: `/Table/DeleteTable?tableId=${tableId}`,
            type: "POST",
            success: function (response) {
                if(response.success == true){
                    Swal.fire({
                        icon: "success",
                        text: response.message
                          }).then((result) => {
                              if (result.isConfirmed) {
                                window.location.reload();
                              }
                          });
                }
                if(response.success == false){
                    Swal.fire({
                      icon: "error",
                      text: response.message
                    });
                }
            }
          });
        }
    });
}

