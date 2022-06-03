// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    let submitButton = document.getElementById("submitButton");
    console.log(submitButton);
    submitButton.onclick = () => {
        let form = document.getElementById("addressForm");
        form.attr('action', "/Home/SubmitForm");
        form.submit();
    };
    $("#AddressesModal").modal('show');
});

function SelectTrElement(selectedElem){
    let previousSelection = document.getElementsByClassName("tr-selected")[0];
    previousSelection?.classList.remove("tr-selected");
    selectedElem.classList.add("tr-selected");
    let saveSelectedButton = document.getElementById("saveSelected");
    
    let formIndex = selectedElem.getAttribute("formIndex")
    saveSelectedButton.setAttribute("form", "addressForm"+formIndex);
}

