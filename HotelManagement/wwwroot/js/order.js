$(document).ready(function () {

    $("#applyFilter").click(function () {
        var start = $("#startDate").val();
        var end = $("#endDate").val();
        if(start == "" && end == ""){
            Swal.fire({
              text: "Please select any date.",
              allowOutsideClick: false,
              allowEscapeKey: false
            });
            return false;
        }
        $("#orderList").DataTable().ajax.reload();
    });

    $("#orderList").DataTable({
            searching: false,
            order: [],
            processing: true,
            serverSide: true,
            autoWidth: false,
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            ajax: {
                url: "/Order/OrderList",
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
                        filters: {
                                field1: new Date($("#startDate").val()),
                                field2: new Date($("#endDate").val())
                            }
                    });
                }
            },
            columns: [
                { title: "Order Id", data: "orderId", visible: false, searchable: false },
                { title: "Customer Name", data: "customerName", searchable: false, orderable: true },
                { title: "Table Name", data: "displayTableName" , searchable: false, orderable: true },
                { title: "Created Date", data: "createdDate" , searchable: false, orderable: true },
                { title: "Bill Amount", data: "billAmount" , searchable: false, orderable: true },
                { title: "Bill Payed", data: "billPayed" , searchable: false, orderable: true, width: "10%",
                    render: function (data,a,othercoldata) {
                        var html = '<div class="form-check form-switch">'+
                                    `<input class="form-check-input isBillPayed" type="checkbox" data-id="${othercoldata.orderId}"`;
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
                    data: "orderId",
                    orderable: false,
                    searchable: false,
                    width: "10%",
                    render: function (data) {
                        return `
                            <div class="btn-group" role="group">
                                <button type="button" class="btn btn-sm btn-primary dropdown-toggle" data-bs-toggle="dropdown">Action</button>
                                <ul class="dropdown-menu">
                                    <li><a class="dropdown-item  text-primary" href="/Order/CreateOrder?orderId=${data}" ><i class="fa fa-edit"></i>&nbsp;Edit</a></li>
                                    <li><a class="dropdown-item  text-danger" style="cursor: pointer;" onclick="deleteOrder(${data})" ><i class="fa fa-trash"></i>&nbsp;Delete</a></li>
                                    <li>
                                        <a class="dropdown-item text-secondary" style="cursor:pointer;" onclick="generatePDF(${data})">
                                            <i class="fa fa-file-pdf"></i>&nbsp;Download Bill PDF
                                        </a>
                                    </li>
                                </ul>
                            </div>`;
                    }
                }
            ]
    });

    $(document).on('change', '.isBillPayed', function() {
        var orderId = $(this).data('id');
        var billpayed = $(this).is(':checked');
        $.ajax({
        url: `/Order/IsBillPayed?orderId=${orderId}&billpayed=${billpayed}`,
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

    $("#createOrderForm").validate({
        rules: {
            CustomerName: { required: true},
            TableId: { required: true}
        },
        messages: {
            CustomerName: {
                required: "CustomerName is Required."
            },
            TableId: {
                required: "Table is Required."
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
            $('#submitBtnCreateOrder').prop('disabled',true);
            var formData = $(form);
            $.ajax({
                url: "/Order/CreateOrder",
                type: "POST",
                data: formData.serialize(),
                success: function (response) {
                    $('#submitBtnCreateOrder').prop('disabled',false);
                    if(response.success == true){
                    Swal.fire({
                        icon: "success",
                        text: response.message
                          }).then((result) => {
                              if (result.isConfirmed) {
                                window.location.href = "/Order/OrderList";
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

    $(document).on("input", ".itemPrice, .itemQty", function () {
        let row = $(this).closest("tr");
        let price = parseFloat(row.find(".itemPrice").val()) || 0;
        let qty = parseInt(row.find(".itemQty").val()) || 0;
        row.find(".itemTotal").val((price * qty).toFixed(2));
        calculateBillAmount();
    });

    $(document).on("click", ".removeRow", function () {
        $(this).closest("tr").remove();
        calculateBillAmount();
        refreshDisabledItems();
        reindexOrderItems();
    });

    $(document).on("click", "#additeminorderbtn", function () {
        let allowAdd = true;

        $(".itemNameD").each(function () {
            if (!$(this).val()) {
                allowAdd = false;
            }
        });

        if (!allowAdd) {
            Swal.fire({
                icon: "warning",
                text: "Please select items in all existing rows before adding a new one!"
            });
            return;
        }
        let index = $("#orderItemsTable tbody tr").length;
        let newRow = `
        <tr>
            <td>
                <input type="hidden" name="OrderItems[${index}].OrderItemId" value="0" />
                <select name="OrderItems[${index}].ItemId" class="form-select itemNameD"></select>
            </td>
            <td><input type="number" step="any" name="OrderItems[${index}].Price" class="form-control itemPrice" min="1" /></td>
            <td><input type="number" name="OrderItems[${index}].Quantity" class="form-control itemQty" min="1" /></td>
            <td><input type="number" name="OrderItems[${index}].FinalPrice" class="form-control itemTotal" readonly /></td>
            <td class="text-center"><button type="button" class="btn btn-danger btn-sm removeRow">X</button></td>
        </tr>`;
        $("#orderItemsTable tbody").append(newRow);
        loadItemDropdown();
    });

    $(document).on("change", ".itemNameD", function () {
        let price = $(this).find(":selected").data("price") || 0;
        let row = $(this).closest("tr");
        row.find(".itemPrice").val(price);
        row.find(".itemQty").val(1);
        row.find(".itemTotal").val(price);
        calculateBillAmount();
        refreshDisabledItems();
    });

    refreshDisabledItems();

});

function refreshDisabledItems() {
    let selectedItems = [];
    $(".itemNameD").each(function () {
        let val = $(this).val();
        if (val && val !== "") selectedItems.push(val);
    });

    $(".itemNameD").each(function () {
        let ddl = $(this);
        ddl.find("option").each(function () {
            let optVal = $(this).val();
            if (selectedItems.includes(optVal) && ddl.val() !== optVal) {
                $(this).prop("disabled", true);
            } else {
                $(this).prop("disabled", false);
            }
        });
    });
}

function calculateBillAmount() {
    let bill = 0;
    $(".itemTotal").each(function () {
        let val = parseFloat($(this).val()) || 0;
        bill += val;
    });
    $("#BillAmount").val(bill.toFixed(2));
}

function loadItemDropdown() {
    $.ajax({
        url: "/Order/GetItemList",
        type: "GET",
        success: function(items) {
            $(".itemNameD").each(function () {
                let ddl = $(this);
                if (ddl.children().length === 0) {
                    ddl.append(`<option value="">--Select Item--</option>`);
                    $.each(items, function(i, item) {
                        ddl.append(`<option value="${item.itemId}" data-price="${item.price}">${item.itemName}</option>`);
                    });
                }
            });
            refreshDisabledItems();
        }
    });
}
function reindexOrderItems() {
    $("#orderItemsTable tbody tr").each(function (rowIndex) {
        $(this).find("input, select").each(function () {
            let name = $(this).attr("name");
            if (name) {
                name = name.replace(/OrderItems\[\d+\]/, `OrderItems[${rowIndex}]`);
                $(this).attr("name", name);
            }
        });
    });
}


function generatePDF(orderId) {
    //window.open(`/Order/PrintOrderPDF?orderId=${orderId}`, "_blank");
    window.location.href = `/Order/PrintOrderPDF?orderId=${orderId}`;
}

function deleteOrder(orderId) {
    Swal.fire({
      text: "Are you sure want to delete Order ?",
      icon: "question",
      showCancelButton: true,
    }).then((result) => {
        if (result.isConfirmed) {
          $.ajax({
            url: `/Order/DeleteOrder?orderId=${orderId}`,
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

function FilterOrReset() {
    $("#startDate").val(0);
    $("#endDate").val(0);
    $("#orderList").DataTable().ajax.reload();
}