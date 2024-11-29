// Search form
$('#btnSearch').on('click', function () {
    if (document.getElementById("hfPageNo") !== null) {
        document.getElementById("hfPageNo").value = "1";
    }

    $(this).closest('form').submit();
});

// Reset form
$('#btnReset').on('click', function () {
    $(this).closest('form').find('input[type=search], input[type=text], input[type=hidden], textarea').val('');
    $(this).closest('form').find('.select2').val('').trigger('change');
    $(this).closest('form').find('input[type=radio]:checked').prop('checked', false);

    var limitNumberItem = document.getElementById("ddlPageLength");
    if (limitNumberItem !== null) limitNumberItem.selectedIndex = 0;
    document.getElementById("PageNo").value = 1;

    $(this).closest('form').submit();
});

// Change page length
function changePageLength() {
    document.getElementById("PageNo").value = 1;
    document.getElementById("PageLength").value = $("#ddlPageLength").val();

    $("#frmSearch").submit();
}

// Change by page no
function changePageNo(pageNo) {
    document.getElementById("PageNo").value = pageNo;
    $("#frmSearch").submit();
}

// Sort table
$(".table-sort").click(function (element) {
    var asc = 'table-sort-asc';
    var desc = 'table-sort-desc';
    var orderByValue = "";

    if (element.target.classList.contains(asc)) {
        $(`.${asc}, .${desc}`).removeClass(`${asc} ${desc}`);
        element.target.classList.add(desc);

        orderByValue = element.target.getAttribute("data-sort-by") + "Desc";
    }
    else {
        $(`.${asc}, .${desc}`).removeClass(`${asc} ${desc}`);
        element.target.classList.add(asc);

        orderByValue = element.target.getAttribute("data-sort-by") + "Asc";
    }

    document.getElementById("OrderBy").value = orderByValue;
    $("#frmSearch").submit();
});

// Load pagination
function loadPagination() {
    $('#dvPagination').load("LoadPagination");
}