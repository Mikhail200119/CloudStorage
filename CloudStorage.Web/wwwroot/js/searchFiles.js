function onSearchInput(element) {
    document
        .querySelectorAll("div[class=fileItemBox]")
        .forEach(fileBox => {
            const fileNameDiv = fileBox.querySelector("div[id=fileDescription]")
                .querySelector("div[id=fileName]");

            const parent = fileBox.parentElement;
            console.log(parent);
            
            if (fileNameDiv.innerText.includes(element.value)) {
                if (fileBox.parentElement.getAttribute("hidden")) {
                    fileBox.parentElement.removeAttribute("hidden");
                }
            } else {
                fileBox.parentElement.setAttribute("hidden", "hidden");
            }
        });
}