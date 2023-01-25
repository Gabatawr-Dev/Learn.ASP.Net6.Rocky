var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/inquiry/GetInquiryList"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "fullName", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "email", "width": "15%" },
            {
                "data": "id", "width": "5%",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/inquiry/details/${data}"
                               class="btn btn-success text-white"
                               style="cursor: pointer">
                                <i class="fa-regular fa-pen-to-square"></i>
                            </a>
                        </div>
                    `;
                }, 
            }
        ]
    });
}