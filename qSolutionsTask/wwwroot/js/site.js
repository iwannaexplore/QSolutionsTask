let app;
$(document).ready(function () {
    app = new App();
});

function SelectTrElement(selectedElem) {
    let previousSelection = document.getElementsByClassName("tr-selected")[0];
    previousSelection?.classList.remove("tr-selected");
    selectedElem.classList.add("tr-selected");

    let trIndex = selectedElem.getAttribute("formIndex")
    app.selectedTrIndex = +trIndex;
}

function UseSelectedAddress() {
    debugger;
    app.mainForm.setValuesForElements(app.similarAddresses[app.selectedTrIndex].Address, true);
}

class MainForm {
    form;
    elementsHtml;

    validateButton;
    submitButton;


    constructor() {
        this.form = document.getElementById("addressForm");
        this.elementsHtml = this.form.getElementsByTagName("input");

        this._defineButtons()
        this.form.onsubmit = () => {
            let values = this.getValuesOfElements();
            app.messageAreaBlock.value =values.toString();
        }
    }

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

    setValuesForElements(address) {
        for (let element of this.elementsHtml) {
            let name = element.name;
            for (let prop in address) {
                if (prop === name) {
                    if (element.value !== address[prop]) {
                        element.value = address[prop];
                        element.classList.add("input-green-border");
                    }
                }
            }
        }
        return address;
    }


    async validateForm() {
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

    _enableButtons() {
        app.selectAddressButton.removeAttribute("disabled");
        this.submitButton.removeAttribute("disabled");
    }

    _parseContainer(containerJson) {
        let container = JSON.parse(containerJson);
        app.errorMessage = container.ErrorMessage;
        app.resultStatus = container.ResultStatus;
        app.similarAddresses = container.SimilarAddresses;
        app.resultAddress = container.resultAddress;
        app.messageAreaBlock.value = app.errorMessage;
    }

    async _getResultStatusAndDoChanges(message) {
        switch (+message) {
            case -2:
            case -1:
            case 0:
                this._addRedBorderToInputs();
                break;
            case 1:
                this._enableButtons();
                break;
            case 2:
                this.setValuesForElements(app.resultAddress);
                this._enableButtons();

                break;
            case 3:
                await app._showModal();
                this._enableButtons();

                break;
        }
    }

    _addRedBorderToInputs() {
        [...document.getElementsByTagName("input")]
            .forEach(e => e.classList.add("red-border"));
    }

    _defineButtons() {
        this.validateButton = document.getElementById("validateButton");
        this.validateButton.onclick = async (e) => {
            this._removeBorders();
            e.preventDefault();
            await this.validateForm();
            await this._getResultStatusAndDoChanges(app.resultStatus);
        }
        this.submitButton = document.getElementById("submitButton");
        this.submitButton.onclick = () => {
            debugger;
            this.form.setAttribute('action', "/Home/SubmitForm");
            this.form.submit();
        };
    }

    _removeBorders() {
        [...document.getElementsByClassName("green-border")]
            .forEach(el => el.classList.remove("green-border"));
        [...document.getElementsByClassName("red-border")]
            .forEach(el => el.classList.remove("red-border"));
    }
}

class App {
    mainForm;
    errorMessage;
    similarAddresses;
    selectAddressButton;
    resultStatus;
    modalBlock;
    resultAddress;
    messageAreaBlock;

    selectedTrIndex;


    constructor() {
        this.modalBlock = document.getElementById("modal");
        this.mainForm = new MainForm();
        this.selectAddressButton = document.getElementById("selectAddress");
        this.selectAddressButton.onclick = this._showModal;
        this.messageAreaBlock = document.getElementById("messageArea");
    }

    async _showModal() {
        let modalTable = await this._getModalTable()
        this.modalBlock.innerHTML = modalTable;
        $("#AddressesModal")?.modal('show');
    }

    async _getModalTable() {
        let response = await fetch("https://localhost:7151/Home/MultipleResultTable", {
            method: 'POST',
            headers: {
                'Accept': 'text/html',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.similarAddresses)
        });
        return await response.text();

    }
}

class ClQACAddress {
    m_sCountry;
    m_sZIP;
    m_sCity;
    m_sStreet;
    m_sDistrict;
    m_iHouseNo;
    toString() {
        return `Country is: ${this.m_sCountry}, Postal Code: ${this.m_sZIP},
         City: ${this.m_sCity}, Street: ${this.m_sStreet}, District: ${this.m_sDistrict},
         House Number: ${this.m_iHouseNo}`;
    }
}