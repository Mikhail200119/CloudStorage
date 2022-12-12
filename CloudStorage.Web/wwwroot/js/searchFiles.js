function onSearchInput(element) {
    document
        .querySelectorAll("div[class=fileItemBox]")
        .forEach(fileBox => {
            const fileNameDiv = fileBox.querySelector("div[id=fileDescription]")
                .querySelector("div[id=fileName]");

            if (fileNameDiv.innerText.includes(element.value)) {
                if (fileBox.getAttribute("hidden")) {
                    fileBox.removeAttribute("hidden");
                }
            } else {
                fileBox.setAttribute("hidden", "hidden");
            }
        });
}