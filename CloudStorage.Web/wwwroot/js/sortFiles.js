function onSortingFiles(element) {
        const sortMethod = element.textContent;

    document.getElementById("sortWayButton").textContent = sortMethod;

    const files = document
        .getElementById("filesContainer")
        .querySelectorAll("div[class=fileItemBox]");

    const divs = [];

    for (let i = 0; i < files.length; i++) {
        divs.push(files[i]);
    }

    if (sortMethod === "Name") {
        sortFilesByName(divs);
    } else if (sortMethod === "Size") {
        sortFilesBySize(divs);
    } else if (sortMethod === "Date") {
        sortFilesByDate(divs);
    }

    const container = document.getElementById("filesContainer");

    container.childNodes.forEach(node => node.remove());

    for (let i = 0; i < divs.length; i++) {
        container.appendChild(divs[i]);
    }
}

function sortFilesByName(divs) {
    divs.sort((a, b) =>
        a
        .querySelector("div[id=fileDescription]")
        .getAttribute("aria-label")
        .localeCompare(b
            .querySelector("div[id=fileDescription]")
            .getAttribute("aria-label"))
    );
}

function sortFilesBySize(divs) {
    divs.sort((a, b) => {
            const firstValue = parseInt(a.querySelector("div[id=fileDescription]").querySelector("div[id=fileSize]").textContent);
            const secondValue = parseInt(b.querySelector("div[id=fileDescription]").querySelector("div[id=fileSize]")
                .textContent);

            if (firstValue > secondValue) {
                return 1;
            }

            if (firstValue < secondValue) {
                return -1;
            }

            return 0;
        }
    );
}

function sortFilesByDate(divs) {
    divs.sort((a, b) => {
        const firstDate = new Date(Date.parse(a.querySelector("div[id=fileDescription]").querySelector("div[id=fileCreatedDate]").textContent));
        const secondDate = new Date(Date.parse(b.querySelector("div[id=fileDescription]").querySelector("div[id=fileCreatedDate]").textContent));

        if (firstDate > secondDate) {
            return 1;
        }

        if (firstDate < secondDate) {
            return -1;
        }

        return 0;
    });
}