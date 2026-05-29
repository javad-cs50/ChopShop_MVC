//$(document).ready(function () {
//    loadDataTable();
//})

//function LoadDataTable() {
//    dataTable = $('#myTable').DataTable(
//        { ajax: "/admin/product/getall" },
//        columns: [
//            { data: '' },
//            { data: '' },
//            { data: '' },
//            { data: '' }
//    ]
//    )

//}
function Delete (url){

    Swal.fire({
        title: "Delete Product",
        text: "Are You Sure?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    toastr.options = {
                        "onHidden": function () {
                            location.reload();
                        },
                        "onCloseClick": function () {
                            location.reload();
                        }
                        // سایر تنظیمات toastr
                    };
                    toastr.success(data.message);
                    
                }
            })
           

        }
    });

}
