let app;
$(document).ready(function () {
    app = new App();
});

function SelectTrElement(selectedElem) {
    let previousSelection = document.getElementsByClassName("tr-selected")[0];
    previousSelection?.classList.remove("tr-selected");
    selectedElem.classList.add("tr-selected");
    let saveSelectedButton = document.getElementById("saveSelected");

    let formIndex = selectedElem.getAttribute("formIndex")
    saveSelectedButton.setAttribute("form", "addressForm" + formIndex);
}


class MainForm {
    constructor() {
        this.form = document.getElementById("addressForm");
        this.elementsHtml = this.form.getElementsByTagName("input");
        this.validateButton = document.getElementById("validateButton");
        this.form.onsubmit = async (e) => {
            e.preventDefault();
            await this.submitForm();
        }
    }

    form;
    elementsHtml;
    validateButton;

    getValuesOfElements() {
        let address = new ClQACAddress();
        for (let element of this.elementsHtml) {
            let name = element.name;
            let value = element.value;
            for (let prop in address) {
                if (prop === name) {
                    address[prop] = value;
                }
            }
        }
        return address;
    }

    setValuesOfElements(address) {
        for (let element of this.elementsHtml) {
            let name = element.name;
            for (let prop in address) {
                if (prop === name) {
                    element.value = address[prop];
                }
            }
        }
        return address;
    }

    async submitForm() {
        let values = this.getValuesOfElements();
        let response = await fetch(this.form.getAttribute("action"), {
            mode: 'cors',
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(values)
        });
        let containerJson = await response.json();
        this._parseContainer(containerJson);
    }

    _parseContainer(containerJson) {
        let container = JSON.parse(containerJson);
        app.errorMessage = container.ErrorMessage;
        app.similarAddresses = container.SimilarAddresses;
        this.setValuesOfElements(container.ResultAddress);
    }
}

class App {
    constructor() {
        this.mainForm = new MainForm();

        let submitButton = document.getElementById("submitButton");
        submitButton.onclick = () => {
            this.mainForm.setAttribute('action', "/Home/SubmitForm");
            this.mainForm.submit();
        };
        $("#AddressesModal")?.modal('show');
    }

    mainForm;
    errorMessage;
    similarAddresses;
}

class ClQACAddress {
    m_sCountry;
    m_sZIP;
    m_sCity;
    m_sStreet;
    m_sDistrict;
    m_iHouseNo;
}